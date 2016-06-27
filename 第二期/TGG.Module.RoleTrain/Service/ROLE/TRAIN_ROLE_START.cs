using System.Linq.Expressions;
using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 开始修行
    /// </summary>
    public class TRAIN_ROLE_START
    {
        private static TRAIN_ROLE_START ObjInstance;

        /// <summary> TRAIN_ROLE_START单体模式 </summary>
        public static TRAIN_ROLE_START GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TRAIN_ROLE_START());
        }

        /// <summary> 开始修行</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.TRAIN_ROLE_START()).Execute(session.Player.User.id, data);
        }
    }
}
