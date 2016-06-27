using System;
using System.Collections.Generic;
using FluorineFx.Messaging.Rtmp.SO;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo;
using TGG.Core.Vo.Equip;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.Props.Service
{
    public partial class Common
    {
        /// <summary>数据组装 </summary>
        public Dictionary<String, Object> BuilData(int result, Int64 rid)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "role", RoleInfo(rid) } };
            return dic;
        }

        /// <summary>反射武将公共方法</summary>
        public RoleInfoVo RoleInfo(Int64 rid)
        {
            if (rid == 0) return null;
            return (new Share.Role()).BuildRole(rid);
        }
    }
}
