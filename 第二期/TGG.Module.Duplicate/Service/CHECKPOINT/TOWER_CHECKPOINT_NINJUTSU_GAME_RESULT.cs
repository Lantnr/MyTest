using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;
using TGG.Core.Common.Randoms;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 忍术游戏结束
    /// </summary>
    public class TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT
    {
        private static TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT _objInstance;

        /// <summary>TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT单体模式</summary>
        public static TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT GetInstance()
        {
            return _objInstance ?? (_objInstance = new TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT", "忍术游戏结束");
#endif
                var position = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "position").Value.ToString());  //玩家选中的盅位置                
                var correct = session.Player.Position.position;   //色子的正确位置
                var tower = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);

                if (tower == null || tower.state != (int)DuplicateClearanceType.CLEARANCE_FIGHTING)  //验证关卡信息及闯关状态
                    return Error((int)ResultType.DATABASE_ERROR);

                var clearance = Variable.BASE_TOWERGAME.FirstOrDefault(m => m.pass == tower.site && m.type == (int)SamllGameType.NINJUTSU);
                if (clearance == null) return Error((int)ResultType.BASE_TABLE_ERROR);

                var nxing = clearance.standardHp;       //过关需要星星
                return position == correct ? Success(session, tower, nxing, (int)FightResultType.WIN) :
                    UnSuccess(session, tower, nxing, (int)FightResultType.LOSE);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>当局成功判断</summary>
        public ASObject Success(TGGSession session, tg_duplicate_checkpoint tower, int nxing, int result)
        {
            var position = RNG.Next(0, 3);     //下一局随机位置
            session.Player.Position.position = position;

            tower.ninjutsu_star = Common.GetInstance().StarsUpdate(tower.ninjutsu_star, result);
            if (Common.GetInstance().StarsCount(tower.ninjutsu_star, result) >= nxing)        //通关成功
            {
                tower.state = (int)DuplicateClearanceType.CLEARANCE_SUCCESS;
                if (!tg_duplicate_checkpoint.UpdateSite(tower)) return Error((int)ResultType.DATABASE_ERROR);
                TOWER_CHECKPOINT_DARE_PUSH.GetInstance().CommandStart(session.Player.User.id, tower.site, (int)DuplicateClearanceType.CLEARANCE_SUCCESS);       //推送成功结果

                var reward = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == tower.site);        //领取声望奖励
                if (reward == null || string.IsNullOrEmpty(reward.passAward)) return Error((int)ResultType.BASE_TABLE_ERROR);
                var user = session.Player.User.CloneEntity();
                var fame = user.fame;
                user.fame = tg_user.IsFameMax(user.fame, Convert.ToInt32(reward.passAward));
                user.Update();
                session.Player.User = user;
                (new Share.User()).REWARDS_API((int)GoodsType.TYPE_FAME, user);   //推送声望奖励

                //记录获得魂日志
                var logdata = string.Format("{0}_{1}_{2}_{3}", "Fame", fame, Convert.ToInt32(reward.passAward), user.fame);
                (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.DUPLICATE, (int)DuplicateCommand.TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT, logdata);

                return new ASObject(Common.GetInstance().NinjutsuBuildData((int)ResultType.SUCCESS, position, tower.ninjutsu_star));
            }
            //继续游戏
            return !tg_duplicate_checkpoint.UpdateSite(tower) ? Error((int)ResultType.DATABASE_ERROR) :
                new ASObject(Common.GetInstance().NinjutsuBuildData((int)ResultType.SUCCESS, position, tower.ninjutsu_star));
        }

        /// <summary>当局判断失败</summary>
        public ASObject UnSuccess(TGGSession session, tg_duplicate_checkpoint tower, int nxing, int result)
        {
            var position = RNG.Next(0, 3);     //下一局随机位置
            session.Player.Position.position = position;

            tower.ninjutsu_star = Common.GetInstance().StarsUpdate(tower.ninjutsu_star, result);
            if (Common.GetInstance().StarsCount(tower.ninjutsu_star, result) >= nxing)        //判断是否结束爬塔
            {
                //通关失败
                tower.state = (int)DuplicateClearanceType.CLEARANCE_FAIL;
                if (!tg_duplicate_checkpoint.UpdateSite(tower)) return Error((int)ResultType.DATABASE_ERROR);
                TOWER_CHECKPOINT_DARE_PUSH.GetInstance().CommandStart(tower.user_id, tower.site, (int)DuplicateClearanceType.CLEARANCE_FAIL);       //推送失败结果

                return new ASObject(Common.GetInstance().NinjutsuBuildData((int)ResultType.SUCCESS, position, tower.ninjutsu_star));
            }
            //继续游戏
            return !tg_duplicate_checkpoint.UpdateSite(tower) ? Error((int)ResultType.DATABASE_ERROR) :
                    new ASObject(Common.GetInstance().NinjutsuBuildData((int)ResultType.SUCCESS, position, tower.ninjutsu_star));
        }

        /// <summary>错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }
    }
}
