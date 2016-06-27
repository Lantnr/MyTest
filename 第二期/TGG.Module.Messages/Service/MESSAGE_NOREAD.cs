using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using TGG.Core.Entity;

namespace TGG.Module.Messages.Service
{
    /// <summary>
    /// 未读邮件
    /// Author:arlen xiao
    /// </summary>
    public class MESSAGE_NOREAD
    {
        private static MESSAGE_NOREAD ObjInstance;

        /// <summary>MESSAGE_NOREAD单体模式</summary>
        public static MESSAGE_NOREAD GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new MESSAGE_NOREAD());
        }

        /// <summary>未读邮件</summary>
        public ASObject CommandStart(SocketServer.TGGSession session)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "MESSAGE_NOREAD", "未读邮件");
#endif
            var dic = new Dictionary<string, object>();

            int rc = tg_messages.GetCountByUserId(session.Player.User.id, (int)Core.Enum.Type.MessageIsReadType.UN_READ);
            dic.Add("result", (int)Core.Enum.Type.ResultType.SUCCESS);
            dic.Add("number", rc);
            return new ASObject(dic);
        }
    }
}
