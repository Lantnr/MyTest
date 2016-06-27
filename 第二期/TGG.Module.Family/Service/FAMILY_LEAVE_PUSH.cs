using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 推送玩家离开家族
    /// </summary>
    public class FAMILY_LEAVE_PUSH
    {
        private static FAMILY_LEAVE_PUSH ObjInstance;

        /// <summary> FAMILY_LEAVE_PUSH单体模式 </summary>
        public static FAMILY_LEAVE_PUSH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_LEAVE_PUSH());
        }

        /// <summary> 推送玩家离开家族</summary>
        public void CommandStart(TGGSession session)
        {
            ASObject aso = BuildData();
            LogPush(session, aso);
        }
        /// <summary>组装数据</summary>
        private ASObject BuildData()
        {
            var dic = new Dictionary<string, object> { { "result", (int)TGG.Core.Enum.Type.ResultType.SUCCESS } };
            return new ASObject(dic);
        }

        /// <summary>推送玩家离开家族</summary>
        private static void LogPush(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "FAMILY_LEAVE_PUSH", "推送玩家离开家族");
#endif
            var pv = session.InitProtocol((int)ModuleNumber.FAMILY, (int)TGG.Core.Enum.Command.FamilyCommand.FAMILY_LEAVE_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
