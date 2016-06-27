using FluorineFx;
using NewLife.Log;
using System;
using System.Linq;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;

namespace TGG.Module.Messages.Service
{
    /// <summary>
    /// 邮件读取
    /// Author:arlen xiao
    /// </summary>
    public class MESSAGE_READ
    {
        private static MESSAGE_READ ObjInstance;

        /// <summary>MESSAGE_READ单体模式</summary>
        public static MESSAGE_READ GetInstance()
        {
            if (ObjInstance == null) ObjInstance = new MESSAGE_READ();
            return ObjInstance;
        }

        /// <summary>邮件读取</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "MESSAGE_READ", "邮件读取");
#endif
            var id = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value.ToString());
            tg_messages.GetMessagesIsReadUpdate(id, (int)MessageIsReadType.HAVE_READ);
            return Common.GetInstance().BuildData((int)ResultType.SUCCESS);
        }
    }
}
