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
    /// 退出家族
    /// </summary>
    public class FAMILY_EXIT
    {
        private static FAMILY_EXIT ObjInstance;

        /// <summary>
        /// FAMILY_EXIT单体模式
        /// </summary>
        public static FAMILY_EXIT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_EXIT());
        }

        /// <summary>退出家族</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_EXIT", "退出家族");
#endif
                var member = session.Player.Family;
                var family = tg_family.GetEntityById(member.fid);    
          
                if(family==null)
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR));

                family.number = family.number - 1;
                if (!tg_family.GetUpdate(family))
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR));

                var baselog = Variable.BASE_FAMILYLOG.FirstOrDefault(m=>m.type==(int)FamilyLogType.FAMILY_SIGN_OUT);
                if (baselog == null)
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.BASE_TABLE_ERROR));
                var log = Common.GetInstance().CreateFamilyLog(baselog, member);

                if (!tg_family_log.GetInsert(log))
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR));

                (new Share.Family()).FamilyMemberAllPush(member, log);
                Common.GetInstance().FamilyTraining(session.Player.User.id, member.fid, session.Player.User.player_name, baselog.prestige);

                if (!tg_family_member.GetDelete(member.id))
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR));
                session.Player.Family=new tg_family_member { fid = 0 };
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }
    }
}
