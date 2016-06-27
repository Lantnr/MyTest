using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Family;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 申请处理
    /// </summary>
    public class FAMILY_APPLY_PROCESS
    {
        public static FAMILY_APPLY_PROCESS ObjInstance;

        /// <summary>
        /// FAMILY_APPLY_PROCESS单体模式
        /// </summary>
        public static FAMILY_APPLY_PROCESS GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_APPLY_PROCESS());
        }

        /// <summary>申请处理</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_APPLY_PROCESS", "申请处理");
#endif
                var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
                var applyuserid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "userid").Value);
                var family_member = session.Player.Family;
                var family = tg_family.GetEntityById(family_member.fid);

                var basefamilypost = Variable.BASE_FAMILYPOST.FirstOrDefault(m => m.post == family_member.degree);
                if (basefamilypost == null)
                    return ErrorResult((int)ResultType.BASE_TABLE_ERROR);
                if (basefamilypost.adopt == (int)FamilyRightType.NORIGHT)
                    return ErrorResult((int)ResultType.FAMILY_DROIT_LACK);

                if (type == (int)FamilyApplyType.ACCEPT)
                {
                    return AcceptProcess(applyuserid, family);
                }
                if (type != (int)FamilyApplyType.REFUSE)
                {
                    return ErrorResult((int)ResultType.FRONT_DATA_ERROR);
                }
                if (!tg_family_apply.GetDelete(applyuserid, family.id))
                    return ErrorResult((int)ResultType.DATABASE_ERROR);
                return new ASObject(Common.GetInstance().BuilDataApply((int)ResultType.SUCCESS, GetFamilyApplyList(family_member.fid)));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        private List<FamilyApplyVo> GetFamilyApplyList(Int64 fid)
        {
            var list_apply = view_user_role_family_apply.GetEntityByFid(fid);
            var list_flvo = new List<FamilyApplyVo>();

            foreach (var fl in list_apply)
            {
                list_flvo.Add(EntityToVo.ToFamilyApplyVo(fl));
            }
            return list_flvo;
        }

        /// <summary>
        /// 接受处理
        /// </summary>
        /// <param name="applyuser">申请人</param>
        /// <param name="family">家族</param>
        /// <returns></returns>
        public ASObject AcceptProcess(Int64 applyuserid, tg_family family)
        {
            var applyuser = tg_user.GetUsersById(applyuserid);
            if (applyuser == null) return ErrorResult((int)ResultType.DATABASE_ERROR);                
            var apply_member = tg_family_member.GetEntityById(applyuser.id);
            var result = Check(apply_member, family);
            if (result != 0)
                return ErrorResult(result);

            var new_member = (new Share.Family()).CreateMember(applyuser.id, family.id, (int)FamilyPositionType.FAMILY_MEMBER);
            family.number += 1;

            if (!tg_family_member.GetInsert(new_member) || !tg_family.GetUpdate(family) ||
                !tg_family_apply.GetDelete(applyuserid, family.id))
                return ErrorResult((int)ResultType.DATABASE_ERROR);

            MakeSession(applyuserid, new_member);

            var base_log = Variable.BASE_FAMILYLOG.FirstOrDefault(m => m.type == (int)FamilyLogType.FAMILY_JOIN);
            if (base_log == null) return ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            var log = Common.GetInstance().CreateFamilyLog(base_log, new_member);
            (new Share.Family()).FamilyMemberAllPush(new_member, log);//推送日志
            Common.GetInstance().ChatPush(new_member, base_log, applyuser);

            if (!tg_family_log.GetInsert(log)) return ErrorResult((int)ResultType.DATABASE_ERROR);
            return new ASObject(Common.GetInstance().BuilDataApply((int)ResultType.SUCCESS, GetFamilyApplyList(family.id)));
        }

        /// <summary>验证申请人，家族人数，基表数据</summary>
        private int Check(tg_family_member apply_member, tg_family family)
        {
            var base_level = Variable.BASE_FAMILYLEVEL.FirstOrDefault(m => m.level == family.family_level);
            if (base_level == null)
            {
                return (int)ResultType.BASE_TABLE_ERROR;
            }
            if (family.number + 1 > base_level.amount)
            {
                return (int)ResultType.FAMILY_MEMBER_ENOUGH;
            }
            if (apply_member != null)
            {
                return (int)ResultType.FAMILY_MEMBER_EXIST;//该玩家已加入其他家族
            }
            return 0;
        }

        private ASObject ErrorResult(int error)
        {
            return new ASObject(Common.GetInstance().BuilDataApply(error, null));
        }

        private void MakeSession(Int64 applyuserid, tg_family_member new_member)
        {
            if (Variable.OnlinePlayer.ContainsKey(applyuserid))  //申请人是否在线
            {
                var sessionB = Variable.OnlinePlayer.FirstOrDefault(m => m.Key == applyuserid).Value as TGGSession;
                sessionB.Player.Family = new_member;
            }
        }
    }
}
