using System.Security.Cryptography.X509Certificates;
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
using TGG.Core.Vo.Family;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 进入家族
    /// </summary>
    public class FAMILY_JOIN
    {
        public static FAMILY_JOIN ObjInstance;

        /// <summary>
        /// FAMILY_JOIN单体模式
        /// </summary>
        public static FAMILY_JOIN GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_JOIN());
        }

        /// <summary>进入家族</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0} {1}", "FAMILY_JOIN", "进入家族");
#endif
                var list_family = new List<tg_family>();
                var base_level = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18005");
                if (base_level == null)
                    return Error((int)ResultType.BASE_TABLE_ERROR, list_family, session);
                if (session.Player.Role.Kind.role_level < Convert.ToInt32(base_level.value))// 玩家等级小于20级
                {
                    return Error((int)ResultType.FAMILY_USERLEVEL_LACK, list_family, session);
                }
                var family_member = session.Player.Family;
                if (family_member.fid == 0)
                {
                    return FamilyList(session);
                }
                return FamilyMySelf(session, family_member);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>其他家族列表</summary>
        public ASObject FamilyList(TGGSession session)
        {
            var fid_list = new List<Int64>();
            var list_family = new List<tg_family>();
            var list_fm = view_user_role_family_member.GetAllByDegree();  //所有族长 
            var chief = new List<view_user_role_family_member>();
            if (list_fm.Count > 0)
            {
                foreach (var item in list_fm)
                {
                    fid_list.Add(item.fid);
                    chief.Add(item);
                }
            }
            else
                return Error((int)ResultType.NO_DATA, list_family, session);
            if (fid_list.Count > 0) { list_family = tg_family.GetFamilysByIds(fid_list); }

            var list = list_family.OrderByDescending(m => m.family_level).ThenByDescending(m => m.time).ToList();  //所有家族排序
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, null, session.Player.User.id, list, chief));
        }

        /// <summary>玩家家族</summary>
        public ASObject FamilyMySelf(TGGSession session, tg_family_member family_member)
        {
            var list_family = new List<tg_family>();
            list_family = tg_family.GetAllById();
            var listf = list_family.OrderByDescending(m => m.family_level).ThenByDescending(m => m.time).ToList();
            var familymyself = list_family.FirstOrDefault(m => m.id == family_member.fid);
            var rank = listf.IndexOf(familymyself);   //排名
            rank = rank + 1;
            var userextend = tg_user_extend.GetByUserId(family_member.userid);
            var familyvo = Common.GetInstance().FamilyInfo(familymyself, rank, userextend);
            list_family = new List<tg_family>();
            return new ASObject(Common.GetInstance().BuilData((int)ResultType.SUCCESS, familyvo, session.Player.User.id, list_family, null));
        }

        private ASObject Error(int result, List<tg_family> list_family, TGGSession session)
        {
            return new ASObject(Common.GetInstance().BuilData(result, null, session.Player.User.id, list_family, null));
        }
    }
}

