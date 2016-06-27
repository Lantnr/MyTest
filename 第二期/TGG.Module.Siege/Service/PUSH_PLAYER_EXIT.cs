using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary> 其他玩家离开美浓攻略 </summary>
    public class PUSH_PLAYER_EXIT
    {
        public static PUSH_PLAYER_EXIT ObjInstance;

        /// <summary>PUSH_PLAYER_EXIT单体模式</summary>
        public static PUSH_PLAYER_EXIT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new PUSH_PLAYER_EXIT());
        }

        /// <summary> 推送给其他玩家 该玩家离开美浓攻略</summary>
        public void CommandStart(TGGSession session)
        {
            var user = session.Player.User;
            var list = Common.GetInstance().GetOtherSceneUsers(user.id);      //找出只是美浓活动中的玩家
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                    Common.GetInstance().PushPv(session, new ASObject(BuildData(user.id)),
                    (int)SiegeCommand.PUSH_PLAYER_EXIT, Convert.ToInt64(m)), item.user_id, token.Token);
            }  //对在活动中的其他玩家推送该玩家离开活动
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(Int64 userid)
        {
            var dic = new Dictionary<string, object>
            {
                {"userId", userid},
            };
            return dic;
        }
    }
}
