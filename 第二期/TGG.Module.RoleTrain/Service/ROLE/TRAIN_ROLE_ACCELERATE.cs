using FluorineFx;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 修行加速
    /// </summary>
    public class TRAIN_ROLE_ACCELERATE
    {
        private static TRAIN_ROLE_ACCELERATE ObjInstance;

        /// <summary> TRAIN_ROLE_ACCELERATE单体模式 </summary>
        public static TRAIN_ROLE_ACCELERATE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TRAIN_ROLE_ACCELERATE());
        }

        /// <summary> 修行加速</summary>
        public ASObject CommandStart(int goodsType, TGGSession session, ASObject data)
        {
            return (new Consume.TRAIN_ROLE_ACCELERATE()).Execute(session.Player.User.id, data);
        }
    }
}
