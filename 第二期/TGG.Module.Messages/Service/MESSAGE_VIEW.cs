using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;

namespace TGG.Module.Messages.Service
{
    /// <summary>
    /// 所有邮件信息
    /// Author:arlen xiao
    /// </summary>
    public class MESSAGE_VIEW
    {
        private static MESSAGE_VIEW ObjInstance;

        /// <summary>MESSAGE_VIEW单体模式</summary>
        public static MESSAGE_VIEW GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new MESSAGE_VIEW());
        }

        /// <summary>所有邮件信息</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "MESSAGE_VIEW", "查询所有邮件信息");
#endif
            var list = view_messages.GetEntityListByUserId(session.Player.User.id);
            return Common.GetInstance().BuildData((int)ResultType.SUCCESS, list);
        }
    }
}
