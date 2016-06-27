using System;
using System.Collections.Generic;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    public partial class tg_role
    {
        /// <summary>查询主角武将信息</summary>
        public static tg_role GetRoleByUserId(Int64 userId)
        {
            return Find(new String[] { _.user_id, _.role_state }, new Object[] { userId, 1 });
        }

        /// <summary>根据userid 查询玩家所有家臣信息</summary>
        public static List<tg_role> QueryRolesByUserId(Int64 userId)
        {
            var where = string.Format("user_id={0} and role_state !={1}", userId, 1);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>查询所有玩家主角武将信息</summary>
        public static List<tg_role> GetAllMainRoles()
        {
            return FindAll(new String[] { _.role_state }, new Object[] { 1 });
        }

        #region 武将属性
        /// <summary>单项属性点总数</summary>
        /// <param name="type">属性类型</param>
        /// <param name="model">武将信息</param>
        /// <param name="ischeck">是否检查</param>
        public static Double GetSingleTotal(RoleAttributeType type, tg_role model, bool ischeck = true)
        {
            return GetSingleFixed(type, model, ischeck) + GetSingleRange(type, model, ischeck);
        }

        /// <summary>单项属性固定值</summary>
        /// <param name="type">属性类型</param>
        /// <param name="model">武将信息</param>
        /// <param name="ischeck">是否检查</param>
        public static Double GetSingleFixed(RoleAttributeType type, tg_role model, bool ischeck = true)
        {
            Double result = 0;
            switch (type)
            {
                #region
                case RoleAttributeType.ROLE_CAPTAIN:
                    {
                        result += model.base_captain;
                        result += model.base_captain_life;
                        result += model.base_captain_level;
                        result += model.base_captain_train;
                        break;
                    }
                case RoleAttributeType.ROLE_FORCE:
                    {
                        result += model.base_force;
                        result += model.base_force_life;
                        result += model.base_force_level;
                        result += model.base_force_train;
                        break;
                    }
                case RoleAttributeType.ROLE_BRAINS:
                    {
                        result += model.base_brains;
                        result += model.base_brains_life;
                        result += model.base_brains_level;
                        result += model.base_brains_train;
                        break;
                    }
                case RoleAttributeType.ROLE_CHARM:
                    {
                        result += model.base_charm;
                        result += model.base_charm_life;
                        result += model.base_charm_level;
                        result += model.base_charm_train;
                        break;
                    }
                case RoleAttributeType.ROLE_GOVERN:
                    {
                        result += model.base_govern;
                        result += model.base_govern_life;
                        result += model.base_govern_level;
                        result += model.base_govern_train;
                        break;
                    }
                #endregion
            }
            result = CheckSingleFixed(result, ischeck);
            return result;
        }

        /// <summary>检查单个固定属性最大值</summary>
        /// <param name="att">属性值</param>
        /// <param name="ischeck">是否检查最大值</param>
        private static Double CheckSingleFixed(Double att, bool ischeck = true)
        {
            if (!ischeck) return att;
            const int max = 280; //固定规则表  7025
            return att - max > 0 ? max : att;
        }

        /// <summary>单项属性范围值</summary>
        /// <param name="type">属性类型</param>
        /// <param name="model">武将信息</param>
        /// <param name="ischeck">是否检查</param>
        public static Double GetSingleRange(RoleAttributeType type, tg_role model, bool ischeck = true)
        {
            Double result = 0;
            switch (type)
            {
                #region
                case RoleAttributeType.ROLE_CAPTAIN:
                    {
                        result += model.base_captain_equip;
                        result += model.base_captain_spirit;
                        result += model.base_captain_title;
                        break;
                    }
                case RoleAttributeType.ROLE_FORCE:
                    {
                        result += model.base_force_equip;
                        result += model.base_force_spirit;
                        result += model.base_force_title;
                        break;
                    }
                case RoleAttributeType.ROLE_BRAINS:
                    {
                        result += model.base_brains_equip;
                        result += model.base_brains_spirit;
                        result += model.base_brains_title;
                        break;
                    }
                case RoleAttributeType.ROLE_CHARM:
                    {
                        result += model.base_charm_equip;
                        result += model.base_charm_spirit;
                        result += model.base_charm_title;
                        break;
                    }
                case RoleAttributeType.ROLE_GOVERN:
                    {
                        result += model.base_govern_equip;
                        result += model.base_govern_spirit;
                        result += model.base_govern_title;
                        break;
                    }
                #endregion
            }
            result = CheckSingleRange(result, ischeck);
            return result;
        }

        /// <summary>检查单个范围值最大值</summary>
        /// <param name="att">属性值</param>
        /// <param name="ischeck">是否检查最大值</param>
        private static Double CheckSingleRange(Double att, bool ischeck = true)
        {
            if (!ischeck) return att;
            const int max = 300;         //固定规则表  7026
            return att - max > 0 ? max : att;
        }

        #endregion
    }
}
