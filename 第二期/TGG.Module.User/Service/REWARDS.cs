using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.AMF;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 物品更新
    /// </summary>
    public class REWARDS
    {
        private static REWARDS ObjInstance;

        /// <summary>REWARDS单例模式</summary>
        public static REWARDS GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new REWARDS());
        }

        /// <summary>物品更新指令</summary>
        public void CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "REWARDS", "物品更新");
#endif
            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = (int)ModuleNumber.USER,
                commandNumber = (int)UserCommand.REWARDS,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }
   
    }
}
