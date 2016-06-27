using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using TGG.Core.Common;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 武将视图
    /// </summary>
    public partial class view_role
    {
        /// <summary>获取武将数据库数据</summary>
        public static EntityList<view_role> FindViewRole(Int64 userid)
        {
            return FindAll(new String[] { _.user_id, _.role_state }, new Object[] { userid, (int)RoleStateType.PROTAGONIST });
        }

        /// <summary>根据Id获取武将数据</summary>
        public static EntityList<view_role> FindViewRoleById(Int64 id)
        {
            return FindAll(_.id, id);
        }

        /// <summary>获取玩家所有武将数据</summary>
        public static EntityList<view_role> FindViewRoleAll(Int64 userid)
        {
            return FindAll(_.user_id, userid);
        }

        /// <summary>获取主角武将</summary>
        public static RoleItem GetFindPlayer(Int64 userid)
        {

            var list = new EntityList<view_role>();
            list = FindViewRole(userid);
            var role = new RoleItem
            {
                Kind = SetRole(list),
                LifeSkill = SetRoleLifeSkill(list),
                FightSkill = SetRoleFightSkill(list)
            };
            return role;
        }

        /// <summary>根据Id获取武将数据</summary>
        public static RoleItem GetFindRoleById(Int64 id)
        {

            var list = new EntityList<view_role>();
            list = FindViewRoleById(id);
            var role = new RoleItem
            {
                Kind = SetRole(list),
                LifeSkill = SetRoleLifeSkill(list),
                FightSkill = SetRoleFightSkill(list)
            };
            return role;
        }

        /// <summary>获取玩家所有武将</summary>
        public static List<RoleItem> GetFindRoleAll(Int64 userid)
        {
            var result = new List<RoleItem>();
            var temp = FindViewRoleAll(userid).ToList();
            var roles = temp.GroupBy(m => m.role_id);
            foreach (var item in roles)
            {
                var list = temp.Where(m => m.role_id == item.Key);
                result.AddRange(list.Select(entity => new RoleItem
                {
                    Kind = SetRole(list),
                    LifeSkill = SetRoleLifeSkill(list),
                    FightSkill = SetRoleFightSkill(list)
                }));
            }
            return result;
        }


        #region 属性方法
        /// <summary>单项属性点总数</summary>
        /// <param name="type">属性类型</param>
        /// <param name="model">武将信息</param>
        /// <param name="ischeck">是否检查</param>
        public static Double GetSingleTotal(RoleAttributeType type, view_role model, bool ischeck = true)
        {
            return GetSingleFixed(type, model, ischeck) + GetSingleRange(type, model, ischeck);
        }


        /// <summary>单项属性固定值</summary>
        /// <param name="type">属性类型</param>
        /// <param name="model">武将信息</param>
        /// <param name="ischeck">是否检查</param>
        public static Double GetSingleFixed(RoleAttributeType type, view_role model, bool ischeck = true)
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
            result = CheckSingleFixed(result);
            return result;
        }

        /// <summary>检查单个固定属性最大值</summary>
        /// <param name="type">类型</param>
        /// <param name="att">属性值</param>
        /// <param name="ischeck">是否检查最大值</param>
        private static Double CheckSingleFixed(Double att, bool ischeck = true)
        {
            if (!ischeck) return att;
            var max = GetRuleRole("7025");
            return att - max > 0 ? max : att;
        }

        /// <summary>单项属性范围值</summary>
        /// <param name="type">属性类型</param>
        /// <param name="model">武将信息</param>
        /// <param name="ischeck">是否检查</param>
        public static Double GetSingleRange(RoleAttributeType type, view_role model, bool ischeck = true)
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
            result = CheckSingleRange(result);
            return result;
        }

        /// <summary>检查单个范围值最大值</summary>
        /// <param name="type">类型</param>
        /// <param name="att">属性值</param>
        /// <param name="ischeck">是否检查最大值</param>
        private static Double CheckSingleRange(Double att, bool ischeck = true)
        {
            if (!ischeck) return att;
            var max = GetRuleRole("7026");
            return att - max > 0 ? max : att;
        }

        /// <summary>属性检查</summary>
        /// <param name="type">检查类型</param>
        /// <param name="role_state">武将类型</param>
        /// <param name="att">属性值</param>
        /// <param name="ischeck">是否检查</param>
        private static Double CheckValue(RoleCheckType type, int role_state, Double att, bool ischeck = true)
        {
            var max = att;
            if (!ischeck) return att;
            switch (type)
            {
                #region
                case RoleCheckType.LifeSkill: { return att; }
                case RoleCheckType.Vocation:
                    {
                        max = GetRuleRole(role_state == (int)RoleStateType.PROTAGONIST ? "7012" : "7018");
                        return att > max ? max : att;
                    }
                case RoleCheckType.Level:
                    {
                        max = GetRuleRole(role_state == (int)RoleStateType.PROTAGONIST ? "7014" : "0");
                        return att > max ? max : att;
                    }
                case RoleCheckType.Train:
                    {
                        max = GetRuleRole(role_state == (int)RoleStateType.PROTAGONIST ? "7013" : "7019");
                        return att > max ? max : att;
                    }
                case RoleCheckType.Spirit:
                    {
                        max = GetRuleRole("7015");
                        return att > max ? max : att;
                    }
                case RoleCheckType.Equip:
                    {
                        max = GetRuleRole("7016");
                        return att > max ? max : att;
                    }
                case RoleCheckType.Title:
                    {
                        max = GetRuleRole("7017");
                        return att > max ? max : att;
                    }
                #endregion
            }
            return att;
        }

        /// <summary>获取基表对应值</summary>
        private static Double GetRuleRole(string id)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            return rule == null ? 0 : Convert.ToDouble(rule.value);
        }

        #region 获取能转换为战斗属性值

        /// <summary>获取能转换为战斗属性值</summary>
        /// <param name="type">属性</param>
        /// <param name="att">属性值</param>
        /// <param name="model">武将实体</param>
        public static Double GetCanToFight(RoleAttributeType type, Double att, view_role model)
        {
            var _total_false = GetSingleTotal(type, model, false);
            var _total_true = GetSingleTotal(type, model);
            var poor = _total_false - _total_true;
            //大于0 说明超过限制值
            if (!(poor > 0)) return att;
            //待转换值
            var use = att - poor;
            //小于0说明属性无法在转换
            return use < 0 ? 0 : use;
        }

        #endregion

        #region

        /// <summary>获取总称号属性点数</summary>
        /// <param name="model">武将信息</param>
        public static Double GetTitleTotal(view_role model)
        {
            Double title = 0;
            title += model._base_brains_title;
            title += model.base_captain_title;
            title += model.base_charm_title;
            title += model.base_force_title;
            title += model.base_govern_title;
            return title;
        }


        /// <summary>获取总装备属性点数</summary>
        /// <param name="model">武将信息</param>
        public static Double GetEquipTotal(view_role model)
        {
            Double equip = 0;
            equip += model.base_brains_equip;
            equip += model.base_captain_equip;
            equip += model.base_charm_equip;
            equip += model.base_force_equip;
            equip += model.base_govern_equip;
            return equip;
        }


        /// <summary>获取总注魂附加总数属性点数</summary>
        /// <param name="model">武将信息</param>
        public static Double GetSpiritTotal(view_role model)
        {
            Double spirit = 0;
            spirit += model.base_brains_spirit;
            spirit += model.base_captain_spirit;
            spirit += model.base_charm_spirit;
            spirit += model.base_force_spirit;
            spirit += model.base_govern_spirit;
            return spirit;
        }

        /// <summary>获取总锻炼属性点数</summary>
        /// <param name="model">武将信息</param>
        public static Double GetTrainTotal(view_role model)
        {
            Double train = 0;
            train += model.base_brains_train;
            train += model.base_captain_train;
            train += model.base_charm_train;
            train += model.base_force_train;
            train += model.base_govern_train;
            return train;
        }

        /// <summary>获取总基础属性点数</summary>
        /// <param name="model">武将信息</param>
        public static Double GetBaseTotal(view_role model)
        {
            Double _base = 0;
            _base += model.base_brains;
            _base += model.base_captain;
            _base += model.base_charm;
            _base += model.base_force;
            _base += model.base_govern;
            return _base;
        }

        #endregion

        #region 锻炼点数

        /// <summary>获得可以锻炼点数</summary>
        /// <param name="att">待锻炼点数</param>
        /// <param name="model">武将信息</param>
        public static Double GetCanTrain(Double att, view_role model)
        {
            var train = GetTrainTotal(model);
            Double total = 0, max = 0;
            //锻炼后总数
            total = att + train;
            if (model.role_state == (int)RoleStateType.PROTAGONIST)
            {
                max = GetRuleRole("7023");          //主角锻炼加点总数
                return max - total > 0 ? att : (att - (total - max));
            }
            var _base = GetBaseTotal(model);
            max = GetRuleRole("7028");              //家臣总属性加点总数
            var train_max = GetRuleRole("7024");    //家臣锻炼加点总数
            var life = GetRuleRole("7022");         //生活技能加点总数
            var temp = max - life - _base;
            var check = temp < max ? temp : train_max;
            return check - total > 0 ? att : (att - (total - check));
        }

        /// <summary>单个锻炼最大值</summary>
        /// <param name="type">属性类型</param>
        /// <param name="model">武将信息</param>
        public static Double GetSingleTrainMax(RoleAttributeType type, view_role model)
        {
            Double _base = 0, _life = 55, _level = 0;
            var single_max = GetRuleRole("7025");       //单项属性加点总数
            switch (type)
            {
                #region
                case RoleAttributeType.ROLE_CAPTAIN:
                    {
                        _base = model._base_captain;
                        _level = model._base_captain_level;
                        break;
                    }
                case RoleAttributeType.ROLE_FORCE:
                    {
                        _base = model._base_force;
                        _level = model._base_force_level;
                        break;
                    }
                case RoleAttributeType.ROLE_BRAINS:
                    {
                        _base = model._base_brains;
                        _level = model._base_brains_level;
                        break;
                    }
                case RoleAttributeType.ROLE_CHARM:
                    {
                        _base = model._base_charm;
                        _level = model._base_charm_level;
                        break;
                    }
                case RoleAttributeType.ROLE_GOVERN:
                    {
                        _base = model._base_govern;
                        _level = model._base_govern_level;
                        break;
                    }
                #endregion
            }
            var max = single_max - _life - _base - _level;
            return max;
        }

        /// <summary>获取单个能锻炼值</summary>
        /// <param name="type">属性类型</param>
        /// <param name="att">待锻炼点数</param>
        /// <param name="model">武将信息</param>
        public static Double GetSingleCanTrain(RoleAttributeType type, Double att, view_role model)
        {
            var c_t = GetCanTrain(att, model);          //剩余可以锻炼总点数
            if (c_t < 0) return 0;

            Double current = 0;
            switch (type)
            {
                #region
                case RoleAttributeType.ROLE_CAPTAIN:
                    {
                        current = model._base_captain_train;
                        break;
                    }
                case RoleAttributeType.ROLE_FORCE:
                    {
                        current = model._base_force_train;
                        break;
                    }
                case RoleAttributeType.ROLE_BRAINS:
                    {
                        current = model._base_brains_train;
                        break;
                    }
                case RoleAttributeType.ROLE_CHARM:
                    {
                        current = model._base_charm_train;
                        break;
                    }
                case RoleAttributeType.ROLE_GOVERN:
                    {
                        current = model._base_govern_train;
                        break;
                    }
                #endregion
            }

            var c_s = GetSingleTrainMax(type, model);   //单个属性锻炼最大值
            var s = c_s - current;                      //剩余单个锻炼值
            if (s <= 0) return 0;

            var c = s - c_t;
            //c大于等于0表示可以锻炼否则达到单个最大值
            return c >= 0 ? c_t : c;
        }


        #endregion

        #region 获取战斗属性
        /// <summary>获取最终武将攻击力</summary>
        /// <param name="model">武将信息</param>
        public static Double GetTotalAttack(view_role model)
        {
            //武力换算攻击力
            var total_fo = GetSingleTotal(RoleAttributeType.ROLE_FORCE, model);
            var c = RuleConvert.GetConvertAttribute(RoleAttributeType.ROLE_FORCE, total_fo);
            var total = c + model.att_attack;
            return total;
        }

        /// <summary>获取最终武将奥义触发几率</summary>
        /// <param name="model">武将信息</param>
        public static Double GetTotalMysteryProbability(view_role model)
        {
            //统帅换算奥义触发几率  (百分比)
            var total_mp = GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, model);
            var c = RuleConvert.GetConvertAttribute(RoleAttributeType.ROLE_CAPTAIN, total_mp);
            var total = c + model.att_mystery_probability;
            return total;
        }

        /// <summary>获取最终武将闪避几率(百分比)</summary>
        /// <param name="model">武将信息</param>
        public static Double GetTotalDodgeProbability(view_role model)
        {
            //智谋换算闪避几率  (百分比)
            var total_dp = GetSingleTotal(RoleAttributeType.ROLE_BRAINS, model);
            var c = RuleConvert.GetConvertAttribute(RoleAttributeType.ROLE_BRAINS, total_dp);
            var total = c + model.att_dodge_probability;
            return total;
        }

        /// <summary>获取最终武将会心效果 (百分比)</summary>
        /// <param name="model">武将信息</param>
        /// <returns></returns>
        public static Double GetTotalCritAddition(view_role model)
        {
            //政务换算会心效果  (百分比)
            var total_ca = GetSingleTotal(RoleAttributeType.ROLE_GOVERN, model);
            var c = RuleConvert.GetConvertAttribute(RoleAttributeType.ROLE_GOVERN, total_ca);
            var total = c + model.att_crit_addition;
            return total;
        }

        /// <summary>获取最终武将攻击力</summary>
        /// <param name="model">武将信息</param>
        /// <returns></returns>
        public static Double GetTotalCritProbability(view_role model)
        {
            //魅力换算会心几率 (百分比)
            var total_cp = GetSingleTotal(RoleAttributeType.ROLE_CHARM, model);
            var c = RuleConvert.GetConvertAttribute(RoleAttributeType.ROLE_CHARM, total_cp);
            var total = c + model.att_crit_probability;
            return total;
        }

        /// <summary>检查总属性值</summary>
        /// <param name="type"></param>
        /// <param name="surplus"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static Double CheckTotal(RoleAttributeType att_type, RoleCheckType type, Double surplus, view_role model)
        {
            var list = new List<AttributeItem>();
            switch (type)
            {
                #region
                case RoleCheckType.Spirit:
                    {
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_CAPTAIN, value = model.base_captain_spirit });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_FORCE, value = model.base_force_spirit });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_GOVERN, value = model.base_govern_spirit });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_BRAINS, value = model.base_brains_spirit });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_CHARM, value = model.base_charm_spirit });
                        break;
                    }
                case RoleCheckType.Equip:
                    {
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_CAPTAIN, value = model.base_captain_equip });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_FORCE, value = model.base_force_equip });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_GOVERN, value = model.base_govern_equip });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_BRAINS, value = model.base_brains_equip });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_CHARM, value = model.base_charm_equip });
                        break;
                    }
                case RoleCheckType.Title:
                    {
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_CAPTAIN, value = model.base_captain_title });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_FORCE, value = model.base_force_title });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_GOVERN, value = model.base_govern_title });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_BRAINS, value = model.base_brains_title });
                        list.Add(new AttributeItem { type = RoleAttributeType.ROLE_CHARM, value = model.base_charm_title });
                        break;
                    }
                #endregion
            }

            var sur = list.Where(m => m.value < surplus).OrderBy(m => m.value);
            if (sur.Any())
            {
                var _s = surplus;
                foreach (var item in sur)
                {
                    var i = _s - item.value;
                    if (i < 0)
                    {
                        return item.type == att_type ? i : 0;
                    }
                    _s = i;
                }
            }


            return 0;
        }

        #endregion

        #endregion

        #region 私有方法数据

        /// <summary>设置武将基础属性</summary>
        private static tg_role SetRole(IEnumerable<view_role> list)
        {
            var entity = list.ToList().FirstOrDefault();
            if (entity == null) return null;
            var kind = new tg_role();
            kind.CopyFrom(entity);
            return kind;
        }

        /// <summary>设置武将生活技能</summary>
        private static tg_role_life_skill SetRoleLifeSkill(IEnumerable<view_role> list)
        {
            var entity = list.ToList().FirstOrDefault();
            if (entity == null) return null;
            var skill = new tg_role_life_skill();
            skill.CopyFrom(entity);
            skill.id = entity.lid;
            return skill;
        }

        /// <summary>设置武将战斗技能</summary>
        private static EntityList<tg_role_fight_skill> SetRoleFightSkill(IEnumerable<view_role> list)
        {
            var list_skill = new EntityList<tg_role_fight_skill>();
            foreach (var item in list)
            {
                var skill = new tg_role_fight_skill();
                skill.CopyFrom(item);
                skill.id = item.fid;
                list_skill.Add(skill);
            }
            return list_skill;
        }

        #endregion
    }
}
