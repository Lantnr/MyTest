using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Family;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 创建家族
    /// </summary>
    public class FAMILY_CREATE : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(Int64 user_id, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "FAMILY_CREATE", "创建家族");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_OFFLINE_ERROR); 
            var name = Convert.ToString(data.FirstOrDefault(m => m.Key == "name").Value);
            var notice = Convert.ToString(data.FirstOrDefault(m => m.Key == "notice").Value);
            return CreateFamily(name, notice, session);
        }

        private int result;
        /// <summary>创建家族</summary>
        public ASObject CreateFamily(string name, string notice, TGGSession session)
        {
            var user = session.Player.User.CloneEntity();
            if (CheckNameAndNotice(session, name, notice) != (int)ResultType.SUCCESS)
                return CommonHelper.ErrorResult(result);
            var baseRule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18002");
            if (baseRule != null)
            {
                int cost = Convert.ToInt32(baseRule.value);
                if (user.coin < cost)
                    return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_COIN_ERROR); 
                return CreateFamily(user.id, name, notice, cost);
            }
            return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR); 

        }

        /// <summary> 验证家族名字、公告、玩家等级 </summary>
        public int CheckNameAndNotice(TGGSession session, string name, string notice)
        {
            var base_level = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18005");
            var r1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18007");
            var r2 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18006");
            var r3 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18008");
            var member = session.Player.Family;
            var familylist = tg_family.GetAllById();
            if (base_level == null || r2 == null || r1 == null || r3 == null)
                return result = (int)ResultType.BASE_TABLE_ERROR;

            //被邀请人等级小于20级 
            if (session.Player.Role.Kind.role_level < Convert.ToInt32(base_level.value))
                return result = (int)ResultType.FAMILY_USERLEVEL_LACK;

            //创建人是否已加入其他家族
            if (member.fid != 0)
                return result = (int)ResultType.FAMILY_MEMBER_EXIST;

            //名字是否为空
            var _name = name.Trim();
            if (name == null || _name.Length == 0)
                return result = (int)ResultType.FAMILY_NAME_NULL;

            if (familylist.Any())
            {
                var fname = familylist.FirstOrDefault(m => m.family_name == name);
                if (fname != null) //是否存在家族名字
                    return result = (int)ResultType.FAMILY_NAME_EXIST;
            }

            //名字字数最小值
            if (name.Length < Convert.ToInt32(r2.value))
                return result = (int)ResultType.FAMILY_NAME_WORDS_LACK;

            //名字字数最大值
            if (name.Length > Convert.ToInt32(r1.value))
                return result = (int)ResultType.FAMILY_NAME_WORDS_ENOUGH;

            //公告字数最大值
            if (name.Length > Convert.ToInt32(r3.value))
                return result = (int)ResultType.FAMILY_NOTICE_WORDS_ENOUGH;
            return (int)ResultType.SUCCESS;
        }

        /// <summary> 创建家族 </summary>
        public ASObject CreateFamily(Int64 userid, string name, string notice, int cost)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid))
                return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            var user = session.Player.User.CloneEntity();

            var family = CreateFamily(name, user, notice);
            if (!tg_family.GetInsert(family)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

            var newfamily = tg_family.GetEntityByChief(user.id);
            if (newfamily == null) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

            var member = (new Family()).CreateMember(user.id, newfamily.id, (int) FamilyPositionType.CHIEF);  //创建族长
            if (!tg_family_member.GetInsert(member)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

            session.Player.Family = member;
            var coin = user.coin;
            user.coin = user.coin - cost;
            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, cost, user.coin);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.FAMILY, (int)FamilyCommand.FAMILY_CREATE, logdata);

            if (!tg_user.GetUserUpdate(user)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
            (new Share.Family()).RewardsToUser(session, user, (int)GoodsType.TYPE_COIN);

            var familylog = Variable.BASE_FAMILYLOG.FirstOrDefault(m => m.type == (int)FamilyLogType.FAMILY_JOIN);
            if (familylog == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            //日志推送
            var log = (new Family()).CreateFamilyLog(familylog, member);
            (new Family()).FamilyMemberAllPush(member, log);
            new Chat().FamilyPush(user.id, member.fid, user.player_name, familylog.prestige);// Common.GetInstance().FamilyTraining(user.id, member.fid, user.player_name, familylog.prestige);

            if (!tg_family_log.GetInsert(log)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
            return (new Family()).SuccessResult(newfamily, member);
        }

        private tg_family CreateFamily(string name, tg_user user, string notice)
        {
            BaseFamilyLevel baseFamily = Variable.BASE_FAMILYLEVEL.FirstOrDefault(m => m.level == 1);
            //创建家族
            var family = new tg_family()
            {
                family_name = name,
                userid = user.id,
                notice = notice,
                family_level = 1,
                clanbadge = 1001,  //族徽默认值
                number = 1,
                renown = 0,
                resource = 0,
                salary = baseFamily.daySalary,
            };
            return family;
        }
        private ASObject ErrorResult(int error)
        {
            return new ASObject((new Family()).BuilData(error, null));
        }

    }
}
