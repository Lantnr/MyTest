using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 解散家族
    /// </summary>
    public class FAMILY_DISSOLVE
    {
        private static FAMILY_DISSOLVE ObjInstance;

        /// <summary>
        /// FAMILY_DISSOLVE单体模式
        /// </summary>
        public static FAMILY_DISSOLVE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_DISSOLVE());
        }

        /// <summary>解散家族</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_DISSOLVE", "解散家族");
#endif
                var member = session.Player.Family;
                var basepost = Variable.BASE_FAMILYPOST.FirstOrDefault(m => m.post == member.degree);

                if (basepost == null)return ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                if (basepost.dissolve == (int)FamilyRightType.NORIGHT) return ErrorResult((int)ResultType.FAMILY_DROIT_LACK);

                var list = tg_family_member.GetAllById(member.fid);
                var playerids = list.ToList().Select(m => m.userid).ToArray();

                var base_log = Variable.BASE_FAMILYLOG.FirstOrDefault(m => m.type == (int)FamilyLogType.FAMILY_DISMISS);
                if (base_log == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR); 

                //推送家族频道
                Common.GetInstance().FamilyTraining(session.Player.User.id, member.fid, session.Player.User.player_name, base_log.prestige);

                foreach (var uid in playerids)
                {
                    if (!Variable.OnlinePlayer.ContainsKey(uid)) continue;
                    var sessionB = Variable.OnlinePlayer[uid] as TGGSession;
                    sessionB.Player.Family = new tg_family_member { fid = 0 };
                    //推送邀请信息   
                    FAMILY_LEAVE_PUSH.GetInstance().CommandStart(sessionB);
                }
                session.Player.Family = new tg_family_member { fid = 0 };
                if (!tg_family_apply.GetDelete(member.fid)) return ErrorResult((int)ResultType.DATABASE_ERROR);
                if (!tg_family_member.GetMembersDelete(member.fid)) return ErrorResult((int)ResultType.DATABASE_ERROR);
                if (!tg_family_log.GetDelete(member.fid)) return ErrorResult((int)ResultType.DATABASE_ERROR);
                if (!tg_family.GetDelete(member.fid)) return ErrorResult((int)ResultType.DATABASE_ERROR);
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        private ASObject ErrorResult(int error)
        {
            return new ASObject(Common.GetInstance().BuilData(error));
        }
    }
}
