using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    public class USER_VIP_PUSH
    {
        private static USER_VIP_PUSH _objInstance;

        /// <summary>
        /// USER_VIP_PUSH单体模式
        /// </summary>
        public static USER_VIP_PUSH GetInstance()
        {
            return _objInstance ?? (_objInstance = new USER_VIP_PUSH());
        }

        /// <summary>推送vip信息 </summary>
        /// <param name="userid">玩家id</param>
        public void CommandStart(Int64 userid)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "USER_VIP_PUSH", "推送vip信息");
#endif
            if (!Variable.OnlinePlayer.ContainsKey(userid))
                return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var vip = session.Player.Vip.CloneEntity();
            var dic = new Dictionary<string, object>
            {
                { "vipVo", EntityToVo.ToVipVo(vip) },
            };
            var data = new ASObject(dic);
            var pv = session.InitProtocol((int)ModuleNumber.USER, (int)Core.Enum.Command.UserCommand.USER_VIP_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
