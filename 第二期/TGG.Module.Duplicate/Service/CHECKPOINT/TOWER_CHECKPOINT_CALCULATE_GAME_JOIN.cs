using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 算术游戏进入
    /// </summary>
    public class TOWER_CHECKPOINT_CALCULATE_GAME_JOIN
    {
        private static TOWER_CHECKPOINT_CALCULATE_GAME_JOIN _objInstance;

        /// <summary>TOWER_CHECKPOINT_CALCULATE_GAME_JOIN单体模式</summary>
        public static TOWER_CHECKPOINT_CALCULATE_GAME_JOIN GetInstance()
        {
            return _objInstance ?? (_objInstance = new TOWER_CHECKPOINT_CALCULATE_GAME_JOIN());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_CALCULATE_GAME_JOIN", "算术游戏进入");
#endif
                var user = session.Player.User;
                var tower = tg_duplicate_checkpoint.GetEntityByUserId(user.id);
                if (tower == null) return Error((int)ResultType.DATABASE_ERROR);

                if (tower.blood <= 0) return Error((int)ResultType.TOWER_BOOLD_UNENOUGH);    //验证玩家临时血量
                var sgame = Variable.BASE_TOWERGAME.FirstOrDefault(m => m.pass == tower.site);
                if (sgame == null || sgame.type != (int)SamllGameType.CALCULATE) return Error((int)ResultType.TOWER_SITE_ERROR);  //验证关卡是否为算术游戏

                if (tower.state == (int)DuplicateClearanceType.CLEARANCE_FAIL || tower.state == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
                    return Error((int)ResultType.TOWER_NO_OPEN);

                return tower.state == (int)DuplicateClearanceType.CLEARANCE_FIGHTING ?
                    new ASObject(Common.GetInstance().CalculateBuildData((int)ResultType.SUCCESS, tower.calculate_star)) :
                    ClearanceBegin(tower);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>第一次开始闯关</summary>
        private ASObject ClearanceBegin(tg_duplicate_checkpoint tower)
        {
            if (!string.IsNullOrEmpty(tower.calculate_star))
            {
                tower.calculate_star = "";
            }
            tower.state = (int)DuplicateClearanceType.CLEARANCE_FIGHTING;

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
