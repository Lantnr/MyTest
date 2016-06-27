using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using TGG.Core.Common.Util;
using TGG.Core.Vo.Fight;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 破坏城门
    /// </summary>
    public class ATTACK_GATE
    {
        private static ATTACK_GATE _objInstance;

        /// <summary>ATTACK_GATE单体模式</summary>
        public static ATTACK_GATE GetInstance()
        {
            return _objInstance ?? (_objInstance = new ATTACK_GATE());
        }

        /// <summary> 破坏城门</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User;

            var playerdata = Variable.Activity.Siege.PlayerData.FirstOrDefault(m => m.user_id == user.id);
            if (playerdata == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.NO_DATA));//活动数据中没有该玩家数据
            if (playerdata.count < 1) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_YUNTI_ERROR)); //验证玩家云梯数

            #region 注释代码 -- 验证坐标
            //var rivalcamp = user.player_camp == (int)CampType.East ? (int)CampType.West : (int)CampType.East;//对手阵营
            //var result = IsCoorPoint(user, rivalcamp);//验证是否在城门附近
            //if (result != ResultType.SUCCESS) return new ASObject(Common.GetInstance().BuildData((int)result));
            #endregion

            var result = IsTime(playerdata);
            if (result != ResultType.SUCCESS) return new ASObject(Common.GetInstance().BuildData((int)result));  //验证破坏城门间隔时间

            if (playerdata.ismatching) return ThreadGateResult(user.id, playerdata);                             //对匹配过的玩家  线程等待直接出破坏结果
            var rivalcamp = user.player_camp == (int)CampType.East ? (int)CampType.West : (int)CampType.East;    //对手阵营
            return PlayerMatching(user.id, rivalcamp, Variable.Activity.Siege.BaseData.GateTime);                //匹配玩家
        }

        /// <summary> 破坏城门时间到后返回破坏城门结果 </summary>
        /// <param name="session">session</param>
        /// <param name="playerdata">玩家活动数据</param>
        private ASObject ThreadGateResult(Int64 userid, SiegePlayer playerdata)
        {
            playerdata.ismatching = false;
            var token = new CancellationTokenSource();
            var task = new Task(() => SpinWait.SpinUntil(() => false, Variable.Activity.Siege.BaseData.GateTime), token.Token);
            task.Start();
            task.ContinueWith(m =>
            {
                if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
                var session = Variable.OnlinePlayer[userid];
                PUSH_GATE_RESULT.GetInstance().CommandStart(session);
                token.Cancel();
            },
            token.Token);
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }

        /// <summary> 验证破坏城门的间隔时间 </summary>
        /// <param name="playerdata">玩家活动数据</param>
        private ResultType IsTime(SiegePlayer playerdata)
        {
            var t = playerdata.gatetime + Variable.Activity.Siege.BaseData.GateTime;
            var time = Convert.ToDouble((DateTime.Now.Ticks - 621355968000000000) / 10000);
            if (t > time) return ResultType.ARENA_TIME_ERROR;
            playerdata.gatetime = time;
            return ResultType.SUCCESS;
        }

        /// <summary> 修改刚战斗完的防守玩家状态 让玩家不被匹配到 </summary>
        /// <param name="userid">玩家Id</param>
        /// <param name="time">间隔时间</param>
        private void RivalStateUpdate(Int64 userid, int time)
        {
            var token = new CancellationTokenSource();
            var task = new Task(() => SpinWait.SpinUntil(() => false, time), token.Token);
            task.Start();
            task.ContinueWith(m =>
            {
                var rival = Variable.Activity.Siege.PlayerData.FirstOrDefault(n => n.user_id == userid);
                if (rival == null) return;
                if (rival.state == (int)SiegePlayerType.REST)
                    rival.state = (int)SiegePlayerType.DEFEND;
                token.Cancel();
            },
            token.Token);
        }

        /// <summary> 获取玩家 </summary>
        /// <param name="userid">当前用户玩家Id</param>
        /// <param name="rivalcamp">对手阵营</param>
        /// <param name="time"></param>
        private ASObject PlayerMatching(Int64 userid, int rivalcamp, int time)
        {
            var flag = true;
            var token = new CancellationTokenSource();
            var task = new Task(() => SpinWait.SpinUntil(() =>
            {
                if (!flag)
                {
                    token.Cancel();
                    return true;
                }
                flag = IsGetRival(userid, rivalcamp);
                return false;
            }, time), token.Token);
            task.Start();
            task.ContinueWith(m =>
            {
                if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
                var session = Variable.OnlinePlayer[userid];
                PUSH_GATE_RESULT.GetInstance().CommandStart(session);
                token.Cancel();
            }, token.Token);
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }

        /// <summary> 获取对手是否失败 </summary>
        /// <param name="userid">当前玩家用户Id</param>
        /// <param name="rivalcamp">对手阵营</param>
        /// <returns>是否失败</returns>
        private bool IsGetRival(Int64 userid, int rivalcamp)
        {
            var rival = GetRival(rivalcamp);
            if (rival == null) return true;
            var f = GetFightResult(userid, rival.user_id);
            if (f.Result != ResultType.SUCCESS) return true;
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return false;
            if (!Variable.OnlinePlayer.ContainsKey(rival.user_id)) return false;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            var rivalsession = Variable.OnlinePlayer[rival.user_id] as TGGSession;
            PUSH_FIGHT.GetInstance().CommandStart(session, f.Ofight);  //发给自己
            PUSH_FIGHT.GetInstance().CommandStart(rivalsession, f.Rfight);//发给对方
            if (f.Iswin)
            {
                var pdata = Variable.Activity.Siege.PlayerData.FirstOrDefault(m => m.user_id == userid);
                if (pdata == null) return false;
                var rivaldata = Variable.Activity.Siege.PlayerData.FirstOrDefault(m => m.user_id == rival.user_id);
                if (rivaldata == null) return false;
                pdata.ismatching = true;
                rivaldata.state = (int)SiegePlayerType.EXIT_DEFEND; //自己赢了
            }
            else
            {
                var time = Variable.Activity.Siege.BaseData.AShotTime;  //守方赢了
                RivalStateUpdate(rival.user_id, ((f.ShotCount * time) + 10000));
            }
            return false;
        }

        /// <summary> 战斗双方推送 </summary>
        /// <param name="session">session</param>
        /// <param name="vo">vo</param>
        private void PushFight(TGGSession session, FightVo vo)
        {
            if (!Variable.OnlinePlayer.ContainsKey(session.Fight.Rival)) return;
            var _session = Variable.OnlinePlayer[session.Fight.Rival] as TGGSession;
            var _vo = vo.CloneEntity();
            int count = _vo.moves.Sum(item => item.Count());
            var user = session.Player.User;
            _vo.isWin = !_vo.isWin;
            if (!_vo.isWin) Common.GetInstance().GetSiegePlayer(user.id, user.player_camp).state = (int)SiegePlayerType.EXIT_DEFEND;
            PUSH_FIGHT.GetInstance().CommandStart(session, vo);  //发给自己
            PUSH_FIGHT.GetInstance().CommandStart(_session, _vo);//发给对方
            var time = Variable.Activity.Siege.BaseData.AShotTime;
            RivalStateUpdate(session.Fight.Rival, (count * time));
        }


        /// <summary> 获取对手数据 </summary>
        /// <param name="rivalcamp">对方阵营</param>
        private SiegePlayer GetRival(int rivalcamp)
        {
            var rival = Variable.Activity.Siege.PlayerData.Where(m =>
                m.player_camp == rivalcamp && m.state == (int)SiegePlayerType.DEFEND).OrderBy(m => Guid.NewGuid()).FirstOrDefault();
            if (rival == null) return null;
            if (!Common.GetInstance().IsActivities(rival.user_id)) return null;
            lock (this) { rival.state = (int)SiegePlayerType.REST; }
            return rival;
        }


        /// <summary> 获取战斗结果 </summary>
        private Fight GetFightResult(Int64 userid, Int64 rivalid)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, rivalid, FightType.BOTH_SIDES, 0, true, false, true);
            new Share.Fight.Fight().Dispose();
            return fight;
        }

        ///// <summary> 验证是否在对方城门附近坐标 </summary>
        ///// <param name="user">用户信息</param>
        ///// <param name="rivalcamp">对方阵营</param>
        //private ResultType IsCoorPoint(tg_user user, int rivalcamp)
        //{
        //    var gate = Variable.BASE_NPCSIEGE.FirstOrDefault(m => m.type == (int)SiegeNpcType.GATE && m.camp == rivalcamp);
        //    if (gate == null) return ResultType.BASE_TABLE_ERROR;

        //    var xy = gate.coorPoint.Split(',');
        //    if (xy.Length != 2) return ResultType.BASE_TABLE_ERROR;

        //    var scene = Variable.Activity.ScenePlayer.FirstOrDefault(m => m.user_id == user.id);
        //    if (scene == null) return ResultType.NO_DATA;

        //    return Common.GetInstance().IsCoorPoint(xy, scene) ? ResultType.POSITION_ERROR : ResultType.SUCCESS;
        //}

    }
}
