using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluorineFx.Util;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将公共方法
    /// </summary>
    public partial class Common
    {
        private static Common ObjInstance;

        /// <summary> Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }


        /// <summary>每天刷新家臣30点体力</summary>
        public void RolePowerUpdate()
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7003");
            var rule_max = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7011");
            if (rule == null || rule_max == null) return;
            var power = Convert.ToInt32(rule.value);
            var max = Convert.ToInt32(rule_max.value);
            tg_role.UpdateRolePower(power, max);
        }

  
    }
}
