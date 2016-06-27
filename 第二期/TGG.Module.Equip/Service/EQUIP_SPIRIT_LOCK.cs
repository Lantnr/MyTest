using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Equip.Service
{
    /// <summary>
    /// 装备铸魂解锁
    /// </summary>
    public class EQUIP_SPIRIT_LOCK
    {
        private static EQUIP_SPIRIT_LOCK ObjInstance;

        /// <summary> EQUIP_SPIRIT_LOCK单体模式 </summary>
        public static EQUIP_SPIRIT_LOCK GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new EQUIP_SPIRIT_LOCK());
        }
        private int result;
        /// <summary> 装备铸魂解锁</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.EQUIP_SPIRIT_LOCK()).Execute(session.Player.User.id, data);
        }
    }
}
