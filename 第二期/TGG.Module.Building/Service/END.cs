using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Building.Service
{
    /// <summary>
    /// 活动结束
    /// </summary>
    public class END
    {
        private static END _objInstance;

        /// <summary>END 单体模式</summary>
        public static END GetInstance()
        {
            return _objInstance ?? (_objInstance = new END());
        }

        private static readonly object obj = new object();

        /// <summary>筑城完成， 活动结束 </summary>
        /// type:1.东军胜 2.西军胜
        public void CommandStart(int type)
        {
            Variable.Activity.BuildActivity.isover = true;
            SaveAndPush(type);
        }

        /// <summary> 时间到，活动结束 </summary>
        ///    /// type:1 东军胜 2 西军胜 3 平
        public void CommandStart()
        {
            var type = 0;
            if (Variable.Activity.BuildActivity.EastCityBlood > Variable.Activity.BuildActivity.WestCityBlood)
                type = 1;

            else if (Variable.Activity.BuildActivity.EastCityBlood < Variable.Activity.BuildActivity.WestCityBlood)
                type = 2;
            else
                type = 3;
            Variable.Activity.BuildActivity.isover = true;
            SaveAndPush(type);
        }

        /// <summary>
        ///发送系统公告
        /// </summary>
        /// <param name="type"></param>
        private void SendPlayersNoyice(int type)
        {
            var content = "";
            var baseid = 0;
            switch (type)
            {
                case 1:
                    {
                        baseid = 100005;
                        content = "东军";
                    } break;
                case 2:
                    {
                        baseid = 100005;
                        content = "西军";
                    } break;
                case 3:
                    {
                        baseid = 100007;
                        content = "";
                    }
                    break;
            }
            new Share.Notice().TrainingPlayer(baseid, content);
        }

        /// <summary> 获取玩家活动数据 </summary>
        // type:1 东军胜 2 西军胜 3 平
        private tg_activity_building GetEndData(BuildActivity.UserGoods ug, int type)
        {
            var wintype = 0;
            switch (type)
            {
                case 1: wintype = ug.camp == (int)CampType.East ? (int)ActivityRewardType.WIN : (int)ActivityRewardType.LOSE; break;
                case 2: wintype = ug.camp == (int)CampType.West ? (int)ActivityRewardType.WIN : (int)ActivityRewardType.LOSE; break;
                case 3: wintype = (int)ActivityRewardType.DRAW; break;   //平
            }
            var acgoods = new tg_activity_building()
            {
                endtime = DateTime.Now.Ticks,
                fame = ug.fame,
                makebuild = ug.totalbasebuild,
                wood = ug.totalwood,
                torch = ug.totaltorch,
                wintype = type,
                user_id = ug.user_id
            };
            var teamreward = Variable.BASE_BUILDING_REWARD.LastOrDefault(q => q.type == wintype && q.fame <= ug.fame); //活动团队奖励表
            if (teamreward == null) return acgoods;
            acgoods.team_fame = teamreward.fameReward;
            acgoods.team_money = teamreward.money;
            return acgoods;
        }


        /// <summary> 组装奖励</summary>
        private List<RewardVo> BuildReward(int fame)
        {
            var listreward = new List<RewardVo> { new RewardVo() { goodsType = (int)GoodsType.TYPE_FAME, value = fame, } };
            return listreward;
        }

        /// <summary>
        /// 保存结束数据并做结束相关推送
        /// </summary>
        /// <param name="type">胜负类型</param>
        private void SaveAndPush(int type)
        {
            new Share.Activity().PushActivity(0, 15);
            SendPlayersNoyice(type);//发送系统公告
            lock (obj)
            {
                foreach (var item in Variable.Activity.BuildActivity.userGoods.Keys)
                {
                    new Share.DaMing().CheckDaMing(item, (int)DaMingType.墨俣一夜城);
                    var temp = new BuildingEnd
                    {
                        type = type,
                        ug = Variable.Activity.BuildActivity.userGoods[item]
                    };
                    var tokenTest = new CancellationTokenSource(); //开启新线程进行推送
                    Task.Factory.StartNew(m =>
                    {
                        var t = m as BuildingEnd;
                        if (t == null) { return; }
                        var ug = GetEndData(t.ug, t.type);
                        Common.GetInstance().GetEmail(t.ug.user_id, ug);
                        Common.GetInstance().SendPv(t.ug.user_id, BuildData(t.type, t.ug.fame), (int)BuildingCommand.END, (int)ModuleNumber.BUILDING);
                        PushMyEnterScene(t.ug.user_id); //向活动外场景的其他玩家推送进入场景
                        ug.Insert();
                        tokenTest.Cancel();
                    }, temp, tokenTest.Token);
                }
                // tg_activity_building.GetListInsert(listdata);
                Variable.Activity.BuildActivity.userGoods.Clear();
                Variable.Activity.ScenePlayer.Clear();
                (new Share.Building()).EndBulid();
            }

        }

        /// <summary> 组装活动结束数据 </summary>
        private ASObject BuildData(int type, int fame)
        {
            var dic = new Dictionary<string, object>()
                    {
                        {"type",type },
                        {"count", BuildReward(fame) }
                    };
            return new ASObject(dic);
        }


        /// <summary> 推送玩家进入场景并向其他人推送 </summary>
        private void PushMyEnterScene(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var userscene = session.Player.Scene;
            if (userscene == null) return;
            if (userscene.model_number != (int)ModuleNumber.BUILDING) return;
            userscene.model_number = (int)ModuleNumber.SCENE;
            var mykey = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, userid);
            if (!Variable.SCENCE.ContainsKey(mykey)) return;
            Variable.SCENCE[mykey].model_number = (int)ModuleNumber.SCENE;
            var otherplays = Core.Common.Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);
            var aso = new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, userscene.scene_id, userscene.X, userscene.Y, otherplays));
            SendPushCommand(session, aso, (int)SceneCommand.PUSH_ENTER_SCENET, (int)ModuleNumber.SCENE);//推送自己回到场景
            foreach (var item in otherplays)
            {
                var temp = new Common.ScenePush()
                {
                    user_id = userid,
                    other_user_id = item.user_id,
                    user_scene = userscene

                };
                var tokenTest = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    if (!Variable.OnlinePlayer.ContainsKey(temp.other_user_id)) return;
                    var othersession = Variable.OnlinePlayer[temp.other_user_id] as TGGSession;
                    if (othersession == null) return;
                    var aso1 = new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, temp.user_scene));
                    SendPushCommand(othersession, aso1, (int)SceneCommand.PLAYER_ENTER_SCENE, (int)ModuleNumber.SCENE);//推送自己回到场景

                }, temp, tokenTest.Token);

            }
        }


        class BuildingEnd
        {
            /// <summary>胜利类型</summary>
            public Int32 type;

            /// <summary>用户物品 </summary>
            public BuildActivity.UserGoods ug;

        }



        /// <summary>
        /// 发送协议
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        /// <param name="commandNumber"></param>
        /// <param name="mn"></param>
        public void SendPushCommand(TGGSession session, ASObject data, int commandNumber, int mn)
        {
            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = mn,
                commandNumber = commandNumber,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }

    }
}
