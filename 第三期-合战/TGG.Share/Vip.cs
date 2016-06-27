using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Share
{
    public partial class Vip : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>推送vip信息 </summary>
        /// <param name="user_id">玩家id</param>
        public void Upgrade(Int64 user_id)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id))
                return;
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return;
            var vip = session.Player.Vip.CloneEntity();
            var dic = new Dictionary<string, object>
            {
                { "vipVo", EntityToVo.ToVipVo(vip) },
            };
            var data = new ASObject(dic);
            var pv = session.InitProtocol((int)ModuleNumber.USER, (int)UserCommand.USER_VIP_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
