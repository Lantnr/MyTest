using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluorineFx;
using FluorineFx.Context;
using NewLife.Web;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Role;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.User.Service
{
    public partial class Common
    {
        #region  公共方法

        /// <summary>
        /// 推送协议
        /// </summary>
        public void SendPv(TGGSession session, ASObject aso, int commandNumber, int mn, Int64 otheruserid)
        {
            var key = string.Format("{0}_{1}_{2}", mn, commandNumber, otheruserid);
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }

        /// <summary> 加入推送模块 </summary>
        public void SendPv(TGGSession session, ASObject aso)
        {
            var key = string.Format("{0}_{1}", (int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_PUSH);
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
