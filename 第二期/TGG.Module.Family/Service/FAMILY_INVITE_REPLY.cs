using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 响应邀请
    /// </summary>
    public class FAMILY_INVITE_REPLY
    {
        private static FAMILY_INVITE_REPLY ObjInstance;

        /// <summary>
        /// FAMILY_INVITE_REPLY单体模式
        /// </summary>
        public static FAMILY_INVITE_REPLY GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_INVITE_REPLY());
        }
        /// <summary>响应邀请</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_INVITE_REPLY", "响应邀请");
#endif
                var state = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "state").Value);
                var fid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
                var userid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "userid").Value);
                var family = tg_family.GetEntityById(fid);

                if (family == null) return ErrorResult((int)ResultType.FAMILY_NOEXIST);
                if (session.Player.Family.fid != 0) return ErrorResult((int)ResultType.FAMILY_MEMBER_EXIST);
                    
                if (state == (int)FamilyApplyType.ACCEPT)
                {
                    var base_level = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18005");
                    if (base_level == null)
                        return ErrorResult((int)ResultType.FAMILY_USERLEVEL_LACK);
                    if (session.Player.Role.Kind.role_level < Convert.ToInt32(base_level.value))
                        return ErrorResult((int)ResultType.FAMILY_USERLEVEL_LACK);
                    return InviteReplyProcess(family, session.Player.User.id);
                }
                if (state == (int) FamilyApplyType.REFUSE)  //推送拒绝信心给邀请人
                {
                    if (Variable.OnlinePlayer.ContainsKey(userid))
                    {
                        var sessionB = Variable.OnlinePlayer[userid] as TGGSession;
                        if (sessionB != null)
                        { FAMILY_REFUSE_PUSH.GetInstance().CommandStart(sessionB); }
                    } 
                }
                else
                    return ErrorResult((int) ResultType.FRONT_DATA_ERROR);
                return ErrorResult((int)ResultType.SUCCESS);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>响应邀请处理</summary>
        private ASObject InviteReplyProcess(tg_family family, Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) 
                return ErrorResult((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if(session==null)
                return ErrorResult((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);
            var familylevel = Variable.BASE_FAMILYLEVEL.FirstOrDefault(m => m.level == family.family_level);
            if (familylevel == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            if (family.number >= familylevel.amount)  //判断家族人数是否足够
                return ErrorResult((int)ResultType.FAMILY_MEMBER_ENOUGH);

            var _member = (new Share.Family()).CreateMember(session.Player.User.id, family.id, (int)FamilyPositionType.FAMILY_MEMBER);  
            family.number += 1;

            if (!tg_family_member.GetInsert(_member)) return ErrorResult((int)ResultType.DATABASE_ERROR);
            if (!tg_family.GetUpdate(family)) return ErrorResult((int)ResultType.DATABASE_ERROR);
                
            var base_log = Variable.BASE_FAMILYLOG.FirstOrDefault(m => m.type == (int)FamilyLogType.FAMILY_JOIN);
            if (base_log == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                
            session.Player.Family = _member;
            var log = Common.GetInstance().CreateFamilyLog(base_log, _member);
            if (!tg_family_log.GetInsert(log)) return ErrorResult((int)ResultType.DATABASE_ERROR);
                
            Common.GetInstance().FamilyTraining(session.Player.User.id, _member.fid, session.Player.User.player_name, base_log.prestige);
            (new Share.Family()).FamilyMemberAllPush(_member, log);//日志推送
            return Common.GetInstance().SuccessResult(family, _member);
        }

        /// <summary>返回错误结果</summary>
        private ASObject ErrorResult(int error)
        {
            return new ASObject(Common.GetInstance().BuilData(error, null));
        }
    }
}
