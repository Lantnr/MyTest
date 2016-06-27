using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    /// <summary>
    /// 推送玩家拒绝加入家族
    /// </summary>
    public class FAMILY_REFUSE_PUSH
    {
        private static FAMILY_REFUSE_PUSH ObjInstance;

        /// <summary> FAMILY_REFUSE_PUSH单体模式 </summary>
        public static FAMILY_REFUSE_PUSH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_REFUSE_PUSH());
        }

        /// <summary> 推送玩家拒绝加入家族</summary>
        public void CommandStart(TGGSession session)
        {
            ASObject aso = BuildData();
            RefusePush(session, aso);
        }
        /// <summary>组装数据</summary>
        private ASObject BuildData()
        {
            var dic = new Dictionary<string, object> { { "result", (int)ResultType.FAMILY_REFUSE } };
            return new ASObject(dic);
        }

        /// <summary>推送玩家拒绝加入家族</summary>
        private static void RefusePush(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "FAMILY_REFUSE_PUSH", "推送玩家拒绝加入家族");
#endif
            var pv = session.InitProtocol((int)ModuleNumber.FAMILY, (int)Core.Enum.Command.FamilyCommand.FAMILY_REFUSE_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
