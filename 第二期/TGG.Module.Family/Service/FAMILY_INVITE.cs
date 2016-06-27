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
    /// 邀请
    /// </summary>
    public class FAMILY_INVITE
    {
        public static FAMILY_INVITE ObjInstance;

        /// <summary>
        /// FAMILY_INVITE单体模式
        /// </summary>
        public static FAMILY_INVITE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_INVITE());
        }
        private int result;
        /// <summary>邀请</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_INVITE", "邀请");
#endif
                var playname = Convert.ToString(data.FirstOrDefault(m => m.Key == "palyername").Value);
                var member = session.Player.Family;
                if (member == null)
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR));

                var family = tg_family.GetEntityById(member.fid);
                if (family == null)
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR));

                var list_member = view_user_role_family_member.GetAll();
                if (!list_member.Any())
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.DATABASE_ERROR));
                if (string.IsNullOrEmpty(playname))
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.FRONT_DATA_ERROR));

                var basefamilypost = Variable.BASE_FAMILYPOST.FirstOrDefault(m => m.post == member.degree);
                var invited = tg_user.GetUserByName(playname);
                if (invited == null)
                    return new ASObject(Common.GetInstance().BuilData((int)ResultType.FAMILY_PLAYER_NONENTITY));

                var role = tg_role.GetRoreByUserid(invited.id, invited.role_id);
                if (Check(list_member, family, basefamilypost, playname, session, role) != (int)ResultType.SUCCESS)
                    return new ASObject(Common.GetInstance().BuilData(result));

                return InviteProcess(invited, family, session);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>验证被邀请人名字，等级，基表 </summary>
        public int Check(List<view_user_role_family_member> list, tg_family family, BaseFamilyPost basepost, string playname, TGGSession session, tg_role role)
        {
            var baselevel = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18005");
            var familylevel = Variable.BASE_FAMILYLEVEL.FirstOrDefault(m => m.level == family.family_level);

            if (baselevel == null)
                return result = (int)ResultType.BASE_TABLE_ERROR;

            if (family.number + 1 > familylevel.amount) //验证家族人数
                return result = (int)ResultType.FAMILY_MEMBER_ENOUGH;

            if (basepost == null || baselevel == null)
                return result = (int)ResultType.BASE_TABLE_ERROR;

            if (basepost.invite == (int)FamilyRightType.NORIGHT)
                return result = (int)ResultType.FAMILY_DROIT_LACK;//没权利邀请

            if (playname == session.Player.User.player_name)
                return result = (int)ResultType.FAMILY_NOINVITE_OWN;     //不能邀请自己 

            if (role.role_level < Convert.ToInt32(baselevel.value))
                return result = (int)ResultType.FAMILY_USERLEVEL_LACK; //被邀请人等级小于20级 

            var fm = list.FirstOrDefault(m => m.player_name == playname);
            if (fm != null)
                return result = (int)ResultType.FAMILY_MEMBER_EXIST; //玩家已在其他家族
            return (int) ResultType.SUCCESS;
        }


        /// <summary>邀请处理</summary>
        public ASObject InviteProcess(tg_user invited, tg_family family, TGGSession session)
        {
            if (Variable.OnlinePlayer.ContainsKey(invited.id))  //邀请的好友是否在线
            {
                var sessionB = Variable.OnlinePlayer[invited.id] as TGGSession;
                //推送邀请信息
                FAMILY_INVITE_RECEIVE.GetInstance().CommandStart(sessionB, family, session);
                return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS));
            }
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.BASE_PLAYER_OFFLINE_ERROR));
        }
    }
}
