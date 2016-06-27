using System;
using System.Linq;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Share
{
    public class Skill : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>消耗体力日志</summary>
        public void PowerLog(tg_role role, int modulenumber, int command)
        {
            var cost = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1010");
            if (cost == null) return;
            var _role = tg_role.GetRoleById(role.id);
            if (role == null) return;

            var power = 0; var _power = 0;
            if (role.role_state == (int)RoleStateType.PROTAGONIST)
                power = tg_role.GetTotalPower(role);
            else
                _power = role.power;

            string logdata = "";
            if (role.role_state == (int)RoleStateType.PROTAGONIST)
            {
                var totalpower = tg_role.GetTotalPower(_role);
                logdata = string.Format("{0}_{1}_{2}_{3}", "Power", power, Convert.ToInt32(cost.value), totalpower);
            }
            else
            {
                logdata = string.Format("{0}_{1}_{2}_{3}", "Power", _power, Convert.ToInt32(cost.value), _role.power);
            }
            (new Share.Log()).WriteLog(role.user_id, (int)LogType.Use, modulenumber, command, logdata);
        }
    }
}
