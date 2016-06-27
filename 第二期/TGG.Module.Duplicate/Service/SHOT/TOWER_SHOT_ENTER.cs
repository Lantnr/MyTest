using System;
using System.Linq;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using NewLife.Log;
using TGG.Core.Entity;

namespace TGG.Module.Duplicate.Service.SHOT
{
    /// <summary>
    /// 进入名塔
    /// </summary>
    public class TOWER_SHOT_ENTER
    {
        private static TOWER_SHOT_ENTER ObjInstance;

        /// <summary>TOWER_SHOT_ENTER单体模式</summary>
        public static TOWER_SHOT_ENTER GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_SHOT_ENTER());
        }

        /// <summary>进入名塔</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TOWER_SHOT_ENTER", "进入名塔");
#endif
            var shot = tg_duplicate_shot.GetEntityByUserId(session.Player.User.id);
            if (shot == null)
            {
                shot = new tg_duplicate_shot { user_id = session.Player.User.id };
            }
            else
            {
                shot.score_current = 0;
                shot.score_total = 0;
            }
            try
            {
                shot.Save();
            }
            catch
            {
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR, session.Player.UserExtend.shot_count));
            }

            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, session.Player.UserExtend.shot_count));
            }
    }
}
