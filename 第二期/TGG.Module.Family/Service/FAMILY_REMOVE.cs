using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Reflection;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 踢出家族
    /// </summary>
    public class FAMILY_REMOVE
    {
        public static FAMILY_REMOVE ObjInstance;

        /// <summary>
        /// FAMILY_REMOVE单体模式
        /// </summary>
        public static FAMILY_REMOVE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_REMOVE());
        }

        /// <summary>踢出家族</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_REMOVE", "踢出家族");
#endif
                var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "userid").Value);
                var member = tg_family_member.GetEntityByIdd(id);
                if (member == null) return Error((int)ResultType.FAMILY_MEMBER_NOEXIST);    

                var family_member = session.Player.Family;
                var family = tg_family.GetEntityById(family_member.fid);
                var remover = tg_user.GetUsersById(member.userid);
                if (family == null || remover == null) return Error((int)ResultType.DATABASE_ERROR);                                                

                var basepost = Variable.BASE_FAMILYPOST.FirstOrDefault(m => m.post == family_member.degree);
                var base_log = Variable.BASE_FAMILYLOG.FirstOrDefault(m => m.type == (int)FamilyLogType.FAMILY_SIGN_OUT);
                if (basepost == null || base_log == null) return Error((int)ResultType.BASE_TABLE_ERROR);

                if (basepost.expel == (int)FamilyRightType.NORIGHT) return Error((int)ResultType.FAMILY_DROIT_LACK); //没权利踢出成员  
                                                                          
                var log = Common.GetInstance().CreateFamilyLog(base_log, member);
                family.number = family.number - 1;
                if (!tg_family.GetUpdate(family) || !tg_family_log.GetInsert(log)) return Error((int)ResultType.DATABASE_ERROR);

                Common.GetInstance().ChatPush(member, base_log, remover);
                FamilyLeavePush(member.userid);
                (new Share.Family()).FamilyMemberAllPush(member, log);
                
                if (!tg_family_member.GetDelete(id))
                    return Error((int) ResultType.DATABASE_ERROR);
                return Common.GetInstance().SuccessResult(family, family_member);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        private ASObject Error(int error)
        {
            return Common.GetInstance().Error(error);
        }

        /// <summary>推送成员离开信息</summary>
        private void FamilyLeavePush(Int64 userid)
        {
            if (Variable.OnlinePlayer.ContainsKey(userid))
            {
                var sessionB = Variable.OnlinePlayer[userid] as TGGSession;
                if (sessionB != null)
                { FAMILY_LEAVE_PUSH.GetInstance().CommandStart(sessionB); }
                sessionB.Player.Family = new tg_family_member { fid = 0 };
            }
        }
    }
}
