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
    /// 任职
    /// </summary>
    public class FAMILY_OFFICE
    {
        private static FAMILY_OFFICE ObjInstance;

        /// <summary>
        /// FAMILY_OFFICE单体模式
        /// </summary>
        public static FAMILY_OFFICE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_OFFICE());
        }
        private int result;
        /// <summary>任职</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_OFFICE", "任职");
#endif
                var baseid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "baseId").Value);
                var postuserid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "userid").Value);
                var basepost = Variable.BASE_FAMILYPOST.FirstOrDefault(m => m.id == baseid);
                var member = session.Player.Family.CloneEntity();  //任职人             
                var list = tg_family_member.GetAllById(member.fid);
                var officemember = list.FirstOrDefault(m => m.userid == postuserid);  //被任职成员                            
                var family = tg_family.GetEntityById(member.fid);
                var man = new List<tg_family_member>();

                var baselevel = Variable.BASE_FAMILYLEVEL.FirstOrDefault(m => m.level == family.family_level);

                if (Check(postuserid, list, member, baselevel, basepost) != (int)ResultType.SUCCESS)
                    return FamilyData(null, null);
                if (basepost.post == (int)FamilyPositionType.CHIEF) return PostProcess(basepost.post, session, officemember, family);

                if (basepost.post == (int)FamilyPositionType.DEPUTY_CHIEFS)
                {
                    man = list.Where(m => m.degree == (int)FamilyPositionType.DEPUTY_CHIEFS).ToList();
                    return SecondPost(man.Count, baselevel.viceChairman, basepost.post,
                        officemember, member, family);
                }
                if (basepost.post == (int)FamilyPositionType.ELDERS)
                {
                    man = list.Where(m => m.degree == (int)FamilyPositionType.ELDERS).ToList();
                    return SecondPost(man.Count, baselevel.elder, basepost.post,
                       officemember, member, family);
                }
                if (basepost.post == (int)FamilyPositionType.FAMILY_MEMBER)
                {
                    return MemberPost(basepost.post, officemember, member, family);
                }
                return Common.GetInstance().Error((int)ResultType.FRONT_DATA_ERROR); //return new ASObject(Common.GetInstance().BuilData((int)ResultType.FRONT_DATA_ERROR, null));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }

        }

        /// <summary>组装数据 </summary>
        public ASObject FamilyData(tg_family family, tg_family_member family_member)
        {
            if (result != (int)ResultType.SUCCESS)
            {
                return Common.GetInstance().Error(result);
            }

            return Common.GetInstance().SuccessResult(family, family_member);
        }

        /// <summary>验证家族成员和基表数据是否为空</summary>
        private int Check(Int64 postuserid, List<tg_family_member> list, tg_family_member member, BaseFamilyLevel baselevel, BaseFamilyPost type)
        {
            var basepost = Variable.BASE_FAMILYPOST.FirstOrDefault(m => m.post == member.degree);
            if (list.Count < 0)
                return result = (int)ResultType.NO_DATA;

            var officemember = list.FirstOrDefault(m => m.userid == postuserid);  //被任职成员 
            if (member == null || officemember == null)
                return result = (int)ResultType.FAMILY_MEMBER_NOEXIST;

            if (baselevel == null || basepost == null || type == null)
                return result = (int)ResultType.BASE_TABLE_ERROR;

            if (basepost.setPost == (int)FamilyRightType.NORIGHT)
                return result = (int)ResultType.FAMILY_DROIT_LACK;
            return (int)ResultType.SUCCESS;
        }

        /// <summary>任职副族长和长老</summary>
        private ASObject SecondPost(int number, int limit, int type, tg_family_member officemember, tg_family_member family_member, tg_family family)
        {
            if (number < limit)
            {
                officemember.degree = type;
            }
            else
            {
                result = (int)ResultType.FAMILY_ELDER_ENOUGH;
                return FamilyData(null, null);
            }
            officemember.Update();
            if (Variable.OnlinePlayer.ContainsKey(officemember.userid))  //被任职人是否在线
            {
                var sessionB = Variable.OnlinePlayer.FirstOrDefault(m => m.Key == officemember.userid).Value as TGGSession;
                sessionB.Player.Family = officemember;
            }
            return FamilyData(family, family_member);
        }

        /// <summary>任职族长</summary>
        private ASObject PostProcess(int type, TGGSession session, tg_family_member officemember, tg_family family)
        {
            var member = session.Player.Family.CloneEntity();  //任职人   
            if (member.degree != (int)FamilyPositionType.CHIEF)
            {
                result = (int)ResultType.FAMILY_CHIEF_NOPOST;
                return FamilyData(null, null);
            }
            family.userid = officemember.userid;
            officemember.degree = type;
            member.degree = (int)FamilyPositionType.FAMILY_MEMBER;
            member.Update();
            officemember.Update();
            family.Update();
            session.Player.Family = member;
            if (Variable.OnlinePlayer.ContainsKey(officemember.userid))  //被任职人是否在线
            {
                var sessionB = Variable.OnlinePlayer.FirstOrDefault(m => m.Key == officemember.userid).Value as TGGSession;
                sessionB.Player.Family = officemember;
            }

            return FamilyData(family, member);
        }

        /// <summary>任职为族员</summary>
        public ASObject MemberPost(int type, tg_family_member officemember, tg_family_member member, tg_family family)
        {
            officemember.degree = type;
            officemember.Update();
            if (Variable.OnlinePlayer.ContainsKey(officemember.userid))  //被任职人是否在线
            {
                var sessionB = Variable.OnlinePlayer.FirstOrDefault(m => m.Key == officemember.userid).Value as TGGSession;
                sessionB.Player.Family = officemember;
            }
            return FamilyData(family, member);
        }
    }
}
