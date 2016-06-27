using System;
using System.Collections.Generic;
using FluorineFx;
using TGG.Core.Enum.Command;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 美浓攻略其他玩家移动
    /// </summary>
    public class PUSH_PLAYER_MOVING
    {
        public static PUSH_PLAYER_MOVING ObjInstance;

        /// <summary>PUSH_PLAYER_MOVING单体模式</summary>
        public static PUSH_PLAYER_MOVING GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new PUSH_PLAYER_MOVING());
        }

        /// <summary>推送美浓攻略玩家移动</summary>
        public void CommandStart(TGGSession session, Int64 otheruserid, int x, int y)
        {
            var user = session.Player.User;
            var aso = new ASObject(BuildData(user.id, x, y));
            Common.GetInstance().PushPv(session, aso, (int)SiegeCommand.PUSH_PLAYER_MOVING, otheruserid);
        }

        /// <summary>推送美浓攻略玩家移动</summary>
        public void SendCommandStart(TGGSession session, Int64 userid, int x, int y)
        {
            // var user = session.Player.User;
            var aso = new ASObject(BuildData(userid, x, y));
            Common.GetInstance().TrainingSiegeEndSend(session, aso, (int)SiegeCommand.PUSH_PLAYER_MOVING);
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(Int64 userid, int x, int y)
        {
            var dic = new Dictionary<string, object>
            {
                {"userId", userid},
                {"x", x},
                {"y", y},
            };
            return dic;
        }
    }
}
