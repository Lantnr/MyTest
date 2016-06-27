using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 推送玩家位置跳转
    /// </summary>
    public class PUSH_PLAYER_POS
    {
        public static PUSH_PLAYER_POS ObjInstance;

        /// <summary>PUSH_PLAYER_POS单体模式</summary>
        public static PUSH_PLAYER_POS GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new PUSH_PLAYER_POS());
        }

        /// <summary> 推送玩家位置跳转</summary>
        public void CommandStart(TGGSession session, Int64 otheruserid, int type, int entryid)
        {
            var user = session.Player.User;
            var aso = new ASObject(BuildData(user.id, type, entryid));
            Common.GetInstance().PushPv(session, aso, (int)SiegeCommand.PUSH_PLAYER_POS, otheruserid);
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(Int64 userid, int type, int entryid)
        {
            var dic = new Dictionary<string, object>
            {
                {"userId", userid},
                {"type", type},
                {"entryId", entryid},
            };
            return dic;
        }
    }
}
