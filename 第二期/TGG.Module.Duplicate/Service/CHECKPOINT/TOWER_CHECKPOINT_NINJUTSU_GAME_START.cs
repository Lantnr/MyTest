using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Common.Randoms;
using TGG.Core.Global;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 忍术游戏开始
    /// </summary>
    public class TOWER_CHECKPOINT_NINJUTSU_GAME_START
    {
        private static TOWER_CHECKPOINT_NINJUTSU_GAME_START _objInstance;

        /// <summary>TOWER_CHECKPOINT_NINJUTSU_GAME_START单体模式</summary>
        public static TOWER_CHECKPOINT_NINJUTSU_GAME_START GetInstance()
        {
            return _objInstance ?? (_objInstance = new TOWER_CHECKPOINT_NINJUTSU_GAME_START());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_NINJUTSU_GAME_START", "忍术游戏开始");
#endif
                var tower = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);
                if (tower == null) return Error((int)ResultType.DATABASE_ERROR);   
                if (tower.blood <= 0) return Error((int)ResultType.TOWER_BOOLD_UNENOUGH);    //验证玩家临时血量

                var sgame = Variable.BASE_TOWERGAME.FirstOrDefault(m => m.pass == tower.site);
                if (sgame == null || sgame.type != (int)SamllGameType.NINJUTSU) return Error((int)ResultType.TOWER_SITE_ERROR);  //验证关卡是否为忍术游戏

                if (tower.state == (int)DuplicateClearanceType.CLEARANCE_FAIL || tower.state == (int)DuplicateClearanceType.CLEARANCE_SUCCESS)
                    return Error((int)ResultType.TOWER_NO_OPEN);       //验证闯关状态

                var position = RNG.Next(0, 3);
                session.Player.Position.position = position;    //获得色子位置

                return tower.state == (int)DuplicateClearanceType.CLEARANCE_FIGHTING ?
                    new ASObject(Common.GetInstance().NinjutsuBuildData((int)ResultType.SUCCESS, position, tower.ninjutsu_star)) :
                    ClearanceBegin(tower, position);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>第一次开始游戏</summary>
        private ASObject ClearanceBegin(tg_duplicate_checkpoint tower, int position)
        {
            if (!string.IsNullOrEmpty(tower.ninjutsu_star))
            {
                tower.ninjutsu_star = "";
            }
            tower.state = (int)DuplicateClearanceType.CLEARANCE_FIGHTING;

            return !tg_duplicate_checkpoint.UpdateSite(tower) ?
                Error((int)ResultType.DATABASE_ERROR) :
                new ASObject(Common.GetInstance().NinjutsuBuildData((int)ResultType.SUCCESS, position, tower.ninjutsu_star));
        }

        /// <summary>错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }
    }
}
