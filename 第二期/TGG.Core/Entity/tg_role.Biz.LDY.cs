using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    public partial class tg_role
    {
        /// <summary>
        /// 根据武将id 和用户id查询武将信息
        /// </summary>
        public static tg_role GetRoreByUserid(Int64 userid)
        {
            return Find(new string[] { _.user_id, _.role_state }, new object[] { userid, RoleStateType.PROTAGONIST });
        }

        /// <summary> 获取参加个人战的武将</summary>
        public static List<tg_role> GetWarRoles(Int64 userid)
        {
            return FindAll(new string[] { _.role_state, _.user_id }, new object[] { (int)RoleStateType.PERSONAL_WAR, userid });
        }

        /// <summary>体力buff更新 </summary>
        public static void GetPowerBuffUpdate(int buff)
        {
            var _set = string.Format("[buff_power] ={0}", buff);
            var _where = string.Format("[role_state]={0} ", (int)RoleStateType.PROTAGONIST);
            Update(_set, _where);

        }

        /// <summary>武将升级更新 </summary>
        public static int GetLevelExpUpdate(int level, int exp, Int64 id, int life, int totalexp)
        {
            var _set = string.Format("role_exp ={0},role_level={1},att_life ={2},total_exp={3}", exp, level, life, totalexp);
            var _where = string.Format("[id]={0} ", id);
            return Update(_set, _where);

        }

        /// <summary>主角升级更新 </summary>
        public static int GetLevelExpUpdate(int level, int exp, Int64 id, int life, int att_points, int totalexp)
        {
            var _set = string.Format("role_exp ={0},role_level={1},att_life ={2},att_points={3},total_exp={4}", exp, level, life, att_points, totalexp);
            var _where = string.Format("[id]={0} ", id);
            return Update(_set, _where);

        }


        /// <summary> 获取主角总体力</summary>
        public static int GetTotalPower(tg_role role)
        {
            return role.power + role.buff_power;
        }

    }
}
