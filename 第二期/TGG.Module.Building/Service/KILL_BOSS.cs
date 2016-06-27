using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Module.Building.Service
{
    /// <summary>
    /// 一夜墨俣--击杀boss
    /// 开发者：李德雁
    /// </summary>
    public class KILL_BOSS
    {
        private static KILL_BOSS _objInstance;

        /// <summary>KILL_BOSS 单体模式</summary>
        public static KILL_BOSS GetInstance()
        {
            return _objInstance ?? (_objInstance = new KILL_BOSS());
        }

        private static readonly object obj = new object();

        private ConcurrentDictionary<Int64, bool> dic = new ConcurrentDictionary<long, bool>();


        /// <summary> 一夜墨俣--击杀boss</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("一夜墨俣：击杀BOSS -{0}——{1}", session.Player.User.player_name, "KILL_BOSS");
#endif
            var userid = session.Player.User.id;
            var shotcount = 0;
            bool iswin;

            var camp = session.Player.User.player_camp;
            if (!CheckPoint(camp, userid)) return BuildData((int)ResultType.BUILDING_POINT_OUT, null); //验证坐标

            if (!Variable.Activity.BuildActivity.userGoods.ContainsKey(userid))
                return BuildData((int)ResultType.BUILDING_NOT_IN, null);
            var ac = Variable.Activity.BuildActivity.userGoods[userid];
            var fighttime = ac.FightTime;
            if (DateTime.Now < fighttime)
                return BuildData((int)ResultType.BUILDING_TIME_OUT, null);
            var bossid = GetBoosId(camp);

            var bossblood = camp == (int)CampType.East ? Variable.Activity.BuildActivity.WestBoosBlood : Variable.Activity.BuildActivity.EastBoosBlood;
            if (bossblood <= 0) return BuildData((int)ResultType.SUCCESS, null);
            if (!CheckQueue(userid)) return BuildData((int)ResultType.BUILDING_TIME_OUT, null);

            var fightvo = new FightVo();
            lock (obj)
            {
                //  Thread.Sleep(5000); //测试数据
                var fight = new Share.Fight.Fight().GeFight(userid, bossid, FightType.BUILDING, bossblood, false, true);
                new Share.Fight.Fight().Dispose();
                if (fight.Result != ResultType.SUCCESS)
                    return BuildData((int)ResultType.FIGHT_ERROR, null);
                var blood = fight.BoosHp;
                bossblood = blood;
                shotcount = fight.ShotCount;
                iswin = fight.Iswin;
                fightvo = fight.Ofight;
                GetBossBlood(camp, bossblood);
                var fightcost = shotcount * 0.5;
                ac.FightTime = DateTime.Now.AddSeconds(fightcost); //下一次挑战时间
                if (iswin) new Common().AddFame(Variable.Activity.BuildActivity.KillAddFame, ac); //打赢boss
                PushBossBlood(userid, camp == (int)CampType.East ? 2 : 1); //1.东军返回西军Boss血量 2.西军返回东军boss血量
            }
            RemoveQueue(userid);
            return BuildData((int)ResultType.SUCCESS, fightvo);

        }

        private bool CheckQueue(Int64 userid)
        {
            if (dic.ContainsKey(userid)) return false;
            dic.TryAdd(userid, true);
            return true;
        }

        /// <summary>
        /// 移除该用户
        /// </summary>
        /// <param name="userid">用户id</param>
        private void RemoveQueue(Int64 userid)
        {
            bool isremove;
            if (!dic.ContainsKey(userid)) return;
            if (dic.TryRemove(userid, out isremove)) return;
            if (!dic.ContainsKey(userid)) return;
            RemoveQueue(userid);
        }

        private void GetBossBlood(int camp, Int64 bossblood)
        {
            if (camp == (int)CampType.East)
            {
                Variable.Activity.BuildActivity.WestBoosBlood = bossblood;
                if (Variable.Activity.BuildActivity.WestBoosBlood < 0)
                    Variable.Activity.BuildActivity.WestBoosBlood = 0;
            }
            else
            {
                Variable.Activity.BuildActivity.EastBoosBlood = bossblood;
                if (Variable.Activity.BuildActivity.EastBoosBlood < 0)
                    Variable.Activity.BuildActivity.EastBoosBlood = 0;
            }
        }


        /// <summary> 组装返回数据 </summary>
        private ASObject BuildData(int result, FightVo fight)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"fight", fight}
            };
            return new ASObject(dic);
        }


        /// <summary> 推送Boss血量变化更新 </summary>
        private void PushBossBlood(Int64 userid, int type)
        {
            //type:1.西军 2 东军
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            //boss血量更新
            foreach (var item in Variable.Activity.ScenePlayer.Keys)
            {
                var temp = new BloodPush()
                {
                    type = type,
                    blood = type == 2 ? Variable.Activity.BuildActivity.WestBoosBlood : Variable.Activity.BuildActivity.EastBoosBlood,
                    user_id = userid,
                    ohter_user_id = Variable.Activity.ScenePlayer[item].user_id
                };
                var tokenTest = new CancellationTokenSource(); //开启新线程进行推送
                Task.Factory.StartNew(m =>
                {
                    var t = m as BloodPush;
                    if (t == null) return;
                    var dic = new Dictionary<string, object>()
                    {
                        {"type",t.type },
                        {"blood",t.blood }
                    };
                    new Common().SendPv(t.ohter_user_id, new ASObject(dic), (int)BuildingCommand.BOOS_BLOOD, (int)ModuleNumber.BUILDING);
                    tokenTest.Cancel();
                }, temp, tokenTest.Token);

            }
        }

        /// <summary> 验证坐标 </summary>
        private bool CheckPoint(int camp, Int64 userid)
        {
            //阵营取对面的阵营
            if (camp == (int)CampType.East) camp = (int)CampType.West;
            else
                camp = (int)CampType.East;
            var baseinfo = Variable.BASE_NPC_BUILD.FirstOrDefault(q => q.type == (int)ActivityBuildType.BOSS && q.camp == camp);
            if (baseinfo == null) return false;
            if (!baseinfo.coorPoint.Contains(",")) return false;
            var point = baseinfo.coorPoint.Split(',');
            return Common.GetInstance().CheckPoint(Convert.ToInt32(point[0]), Convert.ToInt32(point[1]), userid);
        }

        /// <summary>
        /// 获取bossid
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        private int GetBoosId(int camp)
        {
            return camp == (int)CampType.East
                ? Variable.Activity.BuildActivity.WestBossId
                : Variable.Activity.BuildActivity.EastBossId;
        }

        class BloodPush
        {
            /// <summary> 类型 </summary>
            public int type { get; set; }

            /// <summary>血量 </summary>
            public Int64 blood { get; set; }

            /// <summary>用户id </summary>
            public Int64 user_id { get; set; }

            /// <summary> 其他用户id </summary>
            public Int64 ohter_user_id { get; set; }

        }
    }
}
