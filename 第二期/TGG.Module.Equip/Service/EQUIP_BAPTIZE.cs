using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Equip.Service
{
    /// <summary>
    /// 装备洗炼
    /// </summary>
    public class EQUIP_BAPTIZE
    {
        private static EQUIP_BAPTIZE ObjInstance;

        /// <summary> EQUIP_BAPTIZE单体模式 </summary>
        public static EQUIP_BAPTIZE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new EQUIP_BAPTIZE());
        }

        /// <summary> 装备洗炼</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var userid = session.Player.User.id;
            var role = session.Player.Role.Kind;
            var equipid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
            var roleid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "roleId").Value);

            var power = Variable.BASE_RULE.FirstOrDefault(m => m.id == "6002");
            if (power == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var p = Convert.ToInt32(power.value);
            if (role.power < p && role.buff_power < p) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_POWER_ERROR);  //验证体力

            var equip = tg_bag.GetEntityById(equipid);
            if (equip == null) return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);
            var r = tg_role.FindByid(roleid);
            if (r == null) return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);
            var result = RoleByEquip(equip, r);
            if (result != ResultType.SUCCESS) return CommonHelper.ErrorResult(result);

            if (equip.baptize_count >= 5) return CommonHelper.ErrorResult(ResultType.EQUIP_BAPTIZE_MAX);
            var base_equip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == equip.base_id);
            if (base_equip == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            if (base_equip.grade == Convert.ToInt16(GradeType.Green)) return CommonHelper.ErrorResult(ResultType.EQUIP_GRADE_ERROR);

            var newequip = CommonHelper.EquipReset(userid, equip.base_id);
            if (newequip == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            var atts = GetAttribute(equip);
            SetRole(r, atts, false);
            var _atts = GetAttribute(newequip);
            SetRole(r, _atts, true);
            new Share.Role().PowerUpdateAndSend(role, p, userid); //扣主角武将体力

            var logdata = string.Format("{0}_{1}_{2}_{3}", "装备洗炼", "装备Id:" + equip.id, "原属性:" + GetString(atts), "洗炼后属性:" + GetString(_atts));
            (new Share.Log()).WriteLog(userid, (int)LogType.Get, (int)ModuleNumber.EQUIP, (int)EquipCommand.EQUIP_BAPTIZE, logdata);  //日志
            
            Update(equip, newequip);
            r.Update();

            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, equip, r.id));
        }

        private string GetString(List<CommonHelper.EquipItem> list)
        {
            var log = "";
            foreach (var item in list)
            {
                log += item.type + "_" + item.value + "|";
            }
            return log;
        }

        /// <summary> 验证该装备是否是该武将的 </summary>
        private ResultType RoleByEquip(tg_bag equip, tg_role role)
        {
            switch (equip.equip_type)
            {
                case (int)EquipType.ARMOR: { return role.equip_armor == equip.id ? ResultType.SUCCESS : ResultType.EQUIP_TYPE_ERROR; } //铠甲
                case (int)EquipType.ARTWORK: { return role.equip_craft == equip.id ? ResultType.SUCCESS : ResultType.EQUIP_TYPE_ERROR; } //艺术品
                case (int)EquipType.BOOK: { return role.equip_book == equip.id ? ResultType.SUCCESS : ResultType.EQUIP_TYPE_ERROR; }  //书籍
                case (int)EquipType.JEWELRY: { return role.equip_gem == equip.id ? ResultType.SUCCESS : ResultType.EQUIP_TYPE_ERROR; } //珠宝
                case (int)EquipType.MOUNTS: { return role.equip_mounts == equip.id ? ResultType.SUCCESS : ResultType.EQUIP_TYPE_ERROR; }//坐骑
                case (int)EquipType.NANBAN: { return role.equip_barbarian == equip.id ? ResultType.SUCCESS : ResultType.EQUIP_TYPE_ERROR; }//南蛮物
                case (int)EquipType.TEA: { return role.equip_tea == equip.id ? ResultType.SUCCESS : ResultType.EQUIP_TYPE_ERROR; } //茶器
                case (int)EquipType.WEAPON: { return role.equip_weapon == equip.id ? ResultType.SUCCESS : ResultType.EQUIP_TYPE_ERROR; }//武器
                default: { return ResultType.EQUIP_TYPE_ERROR; }
            }
        }

        private void Update(tg_bag equip, tg_bag newequip)
        {
            equip.attribute1_spirit_level = 0;
            equip.attribute1_spirit_lock = (int)SpiritLockType.UNLOCK;//解锁
            equip.attribute1_spirit_value = 0;
            equip.attribute1_type = newequip.attribute1_type;
            equip.attribute1_value = newequip.attribute1_value;
            equip.attribute1_value_spirit = 0;

            equip.attribute2_spirit_level = 0;
            equip.attribute2_spirit_lock = (int)SpiritLockType.UNLOCK;//解锁
            equip.attribute2_spirit_value = 0;
            equip.attribute2_type = newequip.attribute2_type;
            equip.attribute2_value = newequip.attribute2_value;
            equip.attribute2_value_spirit = 0;

            equip.attribute3_spirit_level = 0;
            equip.attribute3_spirit_lock = (int)SpiritLockType.UNLOCK;//解锁
            equip.attribute3_spirit_value = 0;
            equip.attribute3_type = newequip.attribute3_type;
            equip.attribute3_value = newequip.attribute3_value;
            equip.attribute3_value_spirit = 0;

            equip.baptize_count++;
            equip.Update();

        }

        /// <summary> 获取装备的属性值 </summary>
        /// <param name="equip">要获取的装备</param>
        private List<CommonHelper.EquipItem> GetAttribute(tg_bag equip)
        {
            var list = new List<CommonHelper.EquipItem>();
            if (equip.attribute1_type != 0)
            {
                var value = equip.attribute1_value + equip.attribute1_value_spirit;
                list.Add(new CommonHelper.EquipItem() { type = equip.attribute1_type, value = value });
            }
            if (equip.attribute2_type != 0)
            {
                var value = equip.attribute2_value + equip.attribute2_value_spirit;
                list.Add(new CommonHelper.EquipItem() { type = equip.attribute2_type, value = value });
            }
            if (equip.attribute3_type != 0)
            {
                var value = equip.attribute3_value + equip.attribute3_value_spirit;
                list.Add(new CommonHelper.EquipItem() { type = equip.attribute3_type, value = value });
            }
            return list;
        }

        private void SetRole(tg_role role, List<CommonHelper.EquipItem> list, bool flag)
        {
            foreach (var item in list)
            {
                var values = flag ? -item.value : item.value;
                switch (item.type)
                {
                    #region

                    case (int)RoleAttributeType.ROLE_ATTACK:
                        {
                            role.att_attack = role.att_attack - values;
                            break;
                        }
                    case (int)RoleAttributeType.ROLE_BRAINS:
                        {
                            role.base_brains_equip = role.base_brains_equip - values;
                            break;
                        }
                    case (int)RoleAttributeType.ROLE_CAPTAIN:
                        {
                            role.base_captain_equip = role.base_captain_equip - values;
                            break;
                        }
                    case (int)RoleAttributeType.ROLE_CHARM:
                        {
                            role.base_charm_equip = role.base_charm_equip - values;
                            break;
                        }
                    case (int)RoleAttributeType.ROLE_DEFENSE:
                        {
                            role.att_defense = role.att_defense - values;
                            break;
                        }
                    case (int)RoleAttributeType.ROLE_FORCE:
                        {
                            role.base_force_equip = role.base_force_equip - values;
                            break;
                        }
                    case (int)RoleAttributeType.ROLE_GOVERN:
                        {
                            role.base_govern_equip = role.base_govern_equip - values;
                            break;
                        }
                    case (int)RoleAttributeType.ROLE_HURTINCREASE:
                        {
                            role.att_sub_hurtIncrease = role.att_sub_hurtIncrease - values;
                            break;
                        }
                    case (int)RoleAttributeType.ROLE_HURTREDUCE:
                        {
                            role.att_sub_hurtReduce = role.att_sub_hurtReduce - values;
                            break;
                        }
                    case (int)RoleAttributeType.ROLE_LIFE:
                        {
                            role.att_life = role.att_life - Convert.ToInt32(values);
                            break;
                        }
                    #endregion
                }
            }
        }
    }
}
