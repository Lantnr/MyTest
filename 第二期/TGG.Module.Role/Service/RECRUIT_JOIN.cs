using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 进入酒馆
    /// </summary>
    public class RECRUIT_JOIN
    {
        private static RECRUIT_JOIN _objInstance;

        /// <summary>
        /// RECRUIT_JOIN单体模式
        /// </summary>
        public static RECRUIT_JOIN GetInstance()
        {
            return _objInstance ?? (_objInstance = new RECRUIT_JOIN());
        }

        /// <summary> 进入酒馆 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User;
            var list = tg_role_recruit.GetFindAllByUserId(user.id).ToList();     //查询数据库数据
            var recruit_time = session.Player.UserExtend.recruit_time;
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (recruit_time > time || recruit_time == 0)
                return new ASObject(Common.GetInstance()
                        .BuildData((int)ResultType.SUCCESS, session.Player.UserExtend.recruit_time, list));
            tg_user_extend.UpdateRecruit(session.Player.UserExtend.id, 0);
            session.Player.UserExtend.recruit_time = 0;
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, session.Player.UserExtend.recruit_time, list));
        }


    }
}
