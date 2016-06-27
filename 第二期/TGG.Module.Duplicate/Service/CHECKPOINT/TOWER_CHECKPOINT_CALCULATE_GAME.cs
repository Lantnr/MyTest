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

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 算术游戏
    /// </summary>
    public class TOWER_CHECKPOINT_CALCULATE_GAME
    {
        private static TOWER_CHECKPOINT_CALCULATE_GAME _objInstance;

        /// <summary>TOWER_CHECKPOINT_CALCULATE_GAME单体模式</summary>
        public static TOWER_CHECKPOINT_CALCULATE_GAME GetInstance()
        {
            return _objInstance ?? (_objInstance = new TOWER_CHECKPOINT_CALCULATE_GAME());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_CALCULATE_GAME", "算术游戏");
#endif
                var list = data.FirstOrDefault(m => m.Key == "values").Value as object[];        //玩家摇出来的数字集合
                if (list == null) return Error((int)ResultType.FRONT_DATA_ERROR);

                var tower = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);
                if (tower == null || tower.state != (int)DuplicateClearanceType.CLEARANCE_FIGHTING)              // 验证关卡信息
                    return Error((int)ResultType.DATABASE_ERROR);

                var clearance = Variable.BASE_TOWERGAME.FirstOrDefault(m => m.pass == tower.site && m.type == 2);
                if (clearance == null) return Error((int)ResultType.BASE_TABLE_ERROR);

                var nxing = clearance.standardHp;     //通关需要星星
                if (Convert.ToInt32(list[0]) == Convert.ToInt32(list[1]) && Convert.ToInt32(list[0]) == Convert.ToInt32(list[2]))          //摇出来的三个数相同
                {
                    return Success(session, tower, nxing, (int)FightResultType.WIN);   //本局胜利
                }
                return UnSuccess(tower, nxing, (int)FightResultType.LOSE);   //本局失败
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>摇出相同的数字</summary>
        public ASObject Success(TGGSession session, tg_duplicate_checkpoint tower, int nxing, int result)
        {
            tower.calculate_star = Common.GetInstance().StarsUpdate(tower.calculate_star, result);                //更新星星位置及数量
            if (Common.GetInstance().StarsCount(tower.calculate_star, result) >= nxing)        //通关成功
            {
                //通关成功
                tower.state = (int)DuplicateClearanceType.CLEARANCE_SUCCESS;
                if (!tg_duplicate_checkpoint.UpdateSite(tower))
                    return Error((int)ResultType.DATABASE_ERROR);
                TOWER_CHECKPOINT_DARE_PUSH.GetInstance().CommandStart(session.Player.User.id, tower.site, (int)DuplicateClearanceType.CLEARANCE_SUCCESS);       //推送成功结果

                var reward = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == tower.site);           //领取声望奖励
                if (reward == null || string.IsNullOrEmpty(reward.passAward)) return Error((int)ResultType.BASE_TABLE_ERROR);

                var user = session.Player.User.CloneEntity();
                var fame = user.fame;
                user.fame = tg_user.IsFameMax(user.fame, Convert.ToInt32(reward.passAward));
                user.Update();
                session.Player.User = user;
                (new Share.User()).REWARDS_API((int)GoodsType.TYPE_FAME, user); //推送声望奖励

                //记录获得声望日志
                var logdata = string.Format("{0}_{1}_{2}_{3}", "Fame", fame, Convert.ToInt32(reward.passAward), user.fame);
                (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.DUPLICATE, (int)DuplicateCommand.TOWER_CHECKPOINT_CALCULATE_GAME, logdata);

                return new ASObject(Common.GetInstance().CalculateBuildData((int)ResultType.SUCCESS, tower.calculate_star));
            }
            //继续游戏
            return !tg_duplicate_checkpoint.UpdateSite(tower) ? Error((int)ResultType.DATABASE_ERROR) :
                new ASObject(Common.GetInstance().CalculateBuildData((int)ResultType.SUCCESS, tower.calculate_star));
        }

        /// <summary>摇出来的数字不相同</summary>
        public ASObject UnSuccess(tg_duplicate_checkpoint tower, int nxing, int result)
        {
            tower.calculate_star = Common.GetInstance().StarsUpdate(tower.calculate_star, result);                //更新星星位置及数量
            if (Common.GetInstance().StarsCount(tower.calculate_star, result) >= nxing)        //判断是否结束爬塔
            {
                //通关失败
                tower.state = (int)DuplicateClearanceType.CLEARANCE_FAIL;
                if (!tg_duplicate_checkpoint.UpdateSite(tower)) return Error((int)ResultType.DATABASE_ERROR);

                TOWER_CHECKPOINT_DARE_PUSH.GetInstance().CommandStart(tower.user_id, tower.site, (int)DuplicateClearanceType.CLEARANCE_FAIL);
                return new ASObject(Common.GetInstance().CalculateBuildData((int)ResultType.SUCCESS, tower.calculate_star));
            }
            //继续游戏
            return !tg_duplicate_checkpoint.UpdateSite(tower) ? Error((int)ResultType.DATABASE_ERROR) :
                new ASObject(Common.GetInstance().CalculateBuildData((int)ResultType.SUCCESS, tower.calculate_star));
        }

        /// <summary>错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }
    }
}
