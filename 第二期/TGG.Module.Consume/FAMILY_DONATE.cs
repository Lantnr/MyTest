using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 捐献
    /// </summary>
    public class FAMILY_DONATE : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(Int64 user_id, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "FAMILY_DONATE", "捐献");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);
            var count = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "count").Value);
            return CommandStart(count, session);
        }

        private int result;
        /// <summary>捐献</summary>
        public ASObject CommandStart(int count,TGGSession session)
        {
            try
            {
                var user = session.Player.User.CloneEntity();
                var member = session.Player.Family.CloneEntity();
                var family = tg_family.GetEntityById(member.fid);
                var userextend = session.Player.UserExtend.CloneEntity();
                if (user.coin < count)
                    return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_COIN_ERROR);

                var coin = user.coin;
                user.coin = user.coin - count;
                //日志
                var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, count, user.coin);
                (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.FAMILY, (int)FamilyCommand.FAMILY_DONATE, logdata);

                if (!CheckDonate(count, userextend.donate)) return CommonHelper.ErrorResult(result);
                var devote = RuleDataDonate(count);  //捐献所得贡献值
                member.devote += devote;
                family = DevoteProcess(devote, family);
                if (family == null) return CommonHelper.ErrorResult(result);

                session.Player.Family = member;
                family.resource = family.resource + count;
                userextend.donate += count;

                if (!tg_family.GetUpdate(family)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                if (!tg_family_member.GetUpdate(member)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                if (!tg_user.GetUserUpdate(user)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                if (!tg_user_extend.GetUpdate(userextend)) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

                session.Player.UserExtend = userextend;
                (new Share.Family()).RewardsToUser(session, user, (int)GoodsType.TYPE_COIN);

                var rank = (new Share.Family()) .GetRanking(family);
                var familyvo = (new Share.Family()).FamilyInfo(family, rank, userextend);
                return new ASObject((new Family()).BuilData((int)ResultType.SUCCESS, familyvo));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>验证个人捐献资源</summary>
        private bool CheckDonate(int count, int donate)
        {
            var donate_max = Variable.BASE_RULE.FirstOrDefault(m => m.id == "18001");
            if (donate_max == null)
            {
                result = (int)ResultType.BASE_TABLE_ERROR;
                return false;
            }
            //userextend.donate += count;
            donate += count;
            if (donate > Convert.ToInt32(donate_max.value))
            {
                result = (int)ResultType.FAMILY_RESOURCE_ENOUGH;
                return false;
            } //捐献金钱+家族资源>捐献资源最大值
            return true;
        }

        /// <summary>
        /// 家族升级处理
        /// </summary>
        /// <param name="count">捐献值</param>
        /// <param name="family">家族</param>
        /// <param name="family_member">家族成员</param>
        /// <returns></returns>
        private tg_family DevoteProcess(int devote, tg_family family)
        {
            int renown_value = RuleDataDevote(devote); //名望值
            if (renown_value > 0)
            {
                var level = family.family_level;
                var base_level = Variable.BASE_FAMILYLEVEL.FirstOrDefault(m => m.level == level);
                if (base_level == null)
                {
                    result = (int)ResultType.BASE_TABLE_ERROR; return null;
                }
                if (family.renown >= base_level.prestige)
                {
                    result = (int)ResultType.FAMILY_RENOWN_ENOUGH;
                    return null;
                }
                family.renown += renown_value;
                if (family.renown >= base_level.prestige) //升级处理
                {
                    if (base_level.nextId == 0)
                    {
                        family.renown = base_level.prestige;
                        return null;
                    }
                    var next_level = Variable.BASE_FAMILYLEVEL.FirstOrDefault(m => m.id == base_level.nextId);//升级 
                    if (next_level == null)
                    {
                        result = (int)ResultType.BASE_TABLE_ERROR; return null;
                    }
                    family.family_level = next_level.level;
                    family.salary = next_level.daySalary;
                    family.time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
                    if (family.renown > base_level.prestige)
                        family.renown = family.renown - base_level.prestige;
                    else
                        family.renown = 0;
                }
            }
            return family;
        }

        /// <summary>返回错误结果</summary>
        private ASObject ErrorResult(int error)
        {
            return new ASObject((new Share.Family()).BuilData(result, null));
        }


        /// <summary> 家族捐献所得贡献值</summary>
        private int RuleDataDonate(int donate)
        {
            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "18003");
            if (baserule == null) return 0;
            var temp = baserule.value;
            temp = temp.Replace("donate", donate.ToString("0.00"));
            var _devote = CommonHelper.EvalExpress(temp);
            var devote = Convert.ToInt32(_devote);//Math.Ceiling(Convert.ToDecimal(_devote));
            return devote;
        }

        /// <summary>家族贡献所得名望值 </summary>
        private int RuleDataDevote(int devote)
        {
            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "18004");
            if (baserule == null) return 0;
            var temp = baserule.value;
            temp = temp.Replace("devote", devote.ToString("0.00"));
            var _renown = CommonHelper.EvalExpress(temp);
            var renown = Convert.ToInt32(_renown);//Math.Ceiling(Convert.ToDecimal(_renown));
            return renown;
        }
    }
}
