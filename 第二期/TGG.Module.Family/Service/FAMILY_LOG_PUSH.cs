using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 日志推送
    /// </summary>
    public class FAMILY_LOG_PUSH
    {
        private static FAMILY_LOG_PUSH ObjInstance;

        /// <summary> FAMILY_LOG_PUSH单体模式 </summary>
        public static FAMILY_LOG_PUSH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_LOG_PUSH());
        }

        /// <summary> 日志推送</summary>
        public void CommandStart(TGGSession session,tg_family_log log, string name)
        {
            if(session==null)return;
            var aso = BuildData(log, name);
            LogPush(session, aso);
        }
        /// <summary>组装数据</summary>
        private ASObject BuildData(tg_family_log log, string name)
        {
            var dic = new Dictionary<string, object> { { "log", EntityToVo.ToFamilyLogVo(log,name) } };
            return new ASObject(dic);
        }

        /// <summary>日志推送</summary>
        private static void LogPush(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "FAMILY_LOG_PUSH", "日志推送");
#endif
            var pv = session.InitProtocol((int)ModuleNumber.FAMILY, (int)TGG.Core.Enum.Command.FamilyCommand.FAMILY_LOG_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

    }
}
