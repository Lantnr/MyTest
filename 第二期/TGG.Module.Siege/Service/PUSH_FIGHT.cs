using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Command;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary> 推送玩家匹配战斗 </summary>
    public class PUSH_FIGHT
    {
        public static PUSH_FIGHT ObjInstance;

        /// <summary>PUSH_FIGHT单体模式</summary>
        public static PUSH_FIGHT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new PUSH_FIGHT());
        }

        /// <summary> 推送玩家匹配战斗</summary>
        public void CommandStart(TGGSession session, FightVo vo)
        {
            //Common.GetInstance().PushPv(session, new ASObject(BuildData(vo)), (int)SiegeCommand.PUSH_FIGHT);
            Common.GetInstance().TrainingSiegeEndSend(session, new ASObject(BuildData(vo)), (int)SiegeCommand.PUSH_FIGHT);
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(FightVo vo)
        {
            var dic = new Dictionary<string, object>
            {
                {"fight", vo},
            };
            return dic;
        }
    }
}
