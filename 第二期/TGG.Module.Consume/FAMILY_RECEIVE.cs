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
    /// 领取
    /// </summary>
    public class FAMILY_RECEIVE : IConsume
    {
        /// <summary>执行方法</summary>
        public ASObject Execute(Int64 user_id, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "FAMILY_RECEIVE", "领取");
#endif
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return new ASObject((new Family()).BuilData((int)ResultType.BASE_PLAYER_OFFLINE_ERROR));
            return CommandStart(session);
        }

        /// <summary>领取</summary>
        public ASObject CommandStart(TGGSession session)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_RECEIVE", "领取");
#endif
                var member = session.Player.Family;
                var family = tg_family.GetEntityById(member.fid);
                var user = session.Player.User.CloneEntity();
                var userextend = session.Player.UserExtend.CloneEntity();
                if (userextend.daySalary != (int) FamilySalaryReceiveType.NOTRECEIVE)
                    return CommonHelper.ErrorResult((int) ResultType.FAMILY_SALARY_RECEIVE);
                var coin = user.coin;
                user.coin = tg_user.IsCoinMax(user.coin, family.salary);
                //日志
                var logdata = string.Format("{0}_{1}_{2}_{3}", "Coin", coin, family.salary, user.coin);
                (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.FAMILY, (int)FamilyCommand.FAMILY_RECEIVE, logdata);

                userextend.daySalary = (int)FamilySalaryReceiveType.RECEIVE;
                if (!tg_family_member.GetUpdate(member))
                    return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                if (!tg_user.GetUserUpdate(user))
                    return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                if (!tg_user_extend.GetUpdate(userextend))
                    return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);
                session.Player.UserExtend = userextend;
                (new Family()).RewardsToUser(session, user, (int)GoodsType.TYPE_COIN);
                return new ASObject((new Family()).BuilData((int)ResultType.SUCCESS));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }
    }
}
