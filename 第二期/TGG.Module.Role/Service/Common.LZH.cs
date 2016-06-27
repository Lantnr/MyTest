using System;
using System.Collections.Generic;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Equip;
using TGG.Core.Vo.Role;

namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将公共方法
    /// </summary>
    public partial class Common
    {
        #region 组装数据
        /// <summary>武将加载数据组装</summary>
        public Dictionary<String, Object> RoleJoinData(int result, int bar, List<ASObject> listroles, HomeHireVo model, int powerbuy)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"bar",bar},
                {"roles", listroles.Count > 0 ? listroles : null},
                {"homeHireVo", model},
                {"powerCount",powerbuy},
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> RoleLoadData(int result, RoleInfoVo rolevo)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "role", rolevo } };
            return dic;
        }

        /// <summary>装备穿戴组装数据</summary>
        public Dictionary<String, Object> RoleLoadEquipData(int result, RoleInfoVo rolevo, EquipVo equipvo)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "role", rolevo }, { "equip", equipvo }, };
            return dic;
        }

        /// <summary>武将属性保存组装数据</summary>
        public Dictionary<String, Object> RoleAttData(int result, int count, RoleInfoVo rolevo)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "growAddCount", count }, { "role", rolevo } };
            return dic;
        }
        #endregion

        /// <summary>武将加成信息判断</summary>
        public tg_role RoleInfoCheck(tg_role role, tg_bag equip, int type)
        {
            if (equip.attribute1_type != 0)
            {
                role = RoleInfoUpdate(role, equip.attribute1_type, equip.attribute1_value, type, equip.attribute1_value_spirit);
            }
            if (equip.attribute2_type != 0)
            {
                role = RoleInfoUpdate(role, equip.attribute2_type, equip.attribute2_value, type, equip.attribute2_value_spirit);
            }
            if (equip.attribute3_type != 0)
            {
                role = RoleInfoUpdate(role, equip.attribute3_type, equip.attribute3_value, type, equip.attribute3_value_spirit);
            }
            return role;
        }

        /// <summary>更新武将信息</summary>
        public tg_role RoleInfoUpdate(tg_role role, int attType, double value, int type, double spirit)
        {
            if (type == (int)RoleDatatype.ROLEDATA_ADD)
            {
                switch (attType)        //加成属性值
                {
                    case (int)RoleAttributeType.ROLE_CAPTAIN:
                        {
                            role.base_captain_equip += value;
                            role.base_captain_spirit += spirit; break;  //统率
                        }
                    case (int)RoleAttributeType.ROLE_FORCE:
                        {
                            role.base_force_equip += value;
                            role.base_force_spirit += spirit; break;      //武力                           
                        }
                    case (int)RoleAttributeType.ROLE_BRAINS:
                        {
                            role.base_brains_equip += value;
                            role.base_brains_spirit += spirit; break;    //智谋
                        }
                    case (int)RoleAttributeType.ROLE_GOVERN:
                        {
                            role.base_govern_equip += value;
                            role.base_govern_spirit += spirit; break;    //政务
                        }
                    case (int)RoleAttributeType.ROLE_CHARM:
                        {
                            role.base_charm_equip += value;
                            role.base_charm_spirit += spirit; break;    //魅力       
                        }
                    case (int)RoleAttributeType.ROLE_ATTACK: role.att_attack = role.att_attack + value + spirit; break;                              //攻击力
                    case (int)RoleAttributeType.ROLE_HURTINCREASE: role.att_sub_hurtIncrease = role.att_sub_hurtIncrease + value + spirit; break; //增伤
                    case (int)RoleAttributeType.ROLE_DEFENSE: role.att_defense = role.att_defense + value + spirit; break;                       //防御力
                    case (int)RoleAttributeType.ROLE_HURTREDUCE: role.att_sub_hurtReduce = role.att_sub_hurtReduce + value + spirit; break;       //减伤
                    case (int)RoleAttributeType.ROLE_LIFE: role.att_life = role.att_life + Convert.ToInt32(value) + Convert.ToInt32(spirit); break;         //生命值
                }
            }
            else
            {
                switch (attType)         //削减属性值
                {
                    case (int)RoleAttributeType.ROLE_CAPTAIN:
                        role.base_captain_equip -= value;
                        role.base_captain_spirit -= spirit; break;  //统率
                    case (int)RoleAttributeType.ROLE_FORCE:
                        role.base_force_equip -= value;
                        role.base_force_spirit -= spirit; break;    //武力
                    case (int)RoleAttributeType.ROLE_BRAINS:
                        role.base_brains_equip -= value;
                        role.base_brains_spirit -= spirit; break;   //智谋
                    case (int)RoleAttributeType.ROLE_GOVERN:
                        role.base_govern_equip -= value;
                        role.base_govern_spirit -= spirit; break;  //政务
                    case (int)RoleAttributeType.ROLE_CHARM:
                        role.base_charm_equip -= value;
                        role.base_charm_spirit -= spirit; break;  //魅力
                    case (int)RoleAttributeType.ROLE_ATTACK: role.att_attack = role.att_attack - value - spirit; break;               //攻击力
                    case (int)RoleAttributeType.ROLE_HURTINCREASE: role.att_sub_hurtIncrease = role.att_sub_hurtIncrease - value - spirit; break; //增伤
                    case (int)RoleAttributeType.ROLE_DEFENSE: role.att_defense = role.att_defense - value - spirit; break;        //防御力
                    case (int)RoleAttributeType.ROLE_HURTREDUCE: role.att_sub_hurtReduce = role.att_sub_hurtReduce - value - spirit; break;       //减伤
                    case (int)RoleAttributeType.ROLE_LIFE: role.att_life = role.att_life - Convert.ToInt32(value) - Convert.ToInt32(spirit); break;    //生命值
                }
                role = CheckRoleAtt(role);
            }
            return role;
        }

        /// <summary>卸载装备向下验证武将属性</summary>
        public tg_role CheckRoleAtt(tg_role role)
        {
            if (role.base_captain_equip < 0) role.base_captain_equip = 0;
            if (role.base_captain_spirit < 0) role.base_captain_spirit = 0;
            if (role.base_force_equip < 0) role.base_force_equip = 0;
            if (role.base_force_spirit < 0) role.base_force_spirit = 0;
            if (role.base_brains_equip < 0) role.base_brains_equip = 0;
            if (role.base_brains_spirit < 0) role.base_brains_spirit = 0;
            if (role.base_govern_equip < 0) role.base_govern_equip = 0;
            if (role.base_brains_spirit < 0) role.base_brains_spirit = 0;
            if (role.base_charm_equip < 0) role.base_charm_equip = 0;
            if (role.base_charm_spirit < 0) role.base_charm_spirit = 0;
            if (role.att_sub_hurtIncrease < 0) role.att_sub_hurtIncrease = 0;
            if (role.att_sub_hurtReduce < 0) role.att_sub_hurtReduce = 0;
            if (role.att_attack < 0) role.att_attack = 0;
            if (role.att_defense < 0) role.att_defense = 0;
            if (role.att_life < 0) role.att_life = 0;
            if (role.att_crit_addition < 0) role.att_crit_addition = 0;
            if (role.att_crit_probability < 0) role.att_crit_probability = 0;
            if (role.att_dodge_probability < 0) role.att_dodge_probability = 0;
            if (role.att_mystery_probability < 0) role.att_mystery_probability = 0;
            return role;
        }
    }
}
