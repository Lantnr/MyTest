using System;
using System.Linq;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;

namespace TGG.Module.Equip.Service
{
    /// <summary>
    /// 装备公共方法
    /// </summary>
    public partial class Common
    {
        /// <summary>获取掉落装备信息</summary>
        /// <param name="equip">装备基表信息BaseEquip</param>
        /// <param name="userid">userid</param>
        public tg_bag AcquireEquip(BaseEquip equip, Int64 userid)
        {
            var newequip = new tg_bag();
            if (equip.captain != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_CAPTAIN, equip.captain);
            }
            if (equip.force != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_FORCE, equip.force);
            }
            if (equip.brains != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_BRAINS, equip.brains);
            }
            if (equip.govern != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_GOVERN, equip.govern);
            }
            if (equip.charm != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_CHARM, equip.charm);
            }
            if (equip.attack != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_ATTACK, equip.attack);
            }
            if (equip.hurtIncrease != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_HURTINCREASE, Convert.ToDouble(equip.hurtIncrease));
            }
            if (equip.defense != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_DEFENSE, equip.defense);
            }
            if (equip.hurtReduce != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_HURTREDUCE, Convert.ToDouble(equip.hurtReduce));
            }
            if (equip.life != 0)
            {
                newequip = EquipAtt(newequip, (int)RoleAttributeType.ROLE_LIFE, equip.life);
            }
            newequip.user_id = userid;
            newequip.base_id = (int)equip.id;
            newequip.type = (int)GoodsType.TYPE_EQUIP;
            newequip.equip_type = equip.typeSub;
            newequip.state = (int)LoadStateType.UNLOAD;
            newequip.count = 1;
            return newequip;
        }

        /// <summary>获取装备的加成属性值</summary>
        /// <param name="value">加成范围值</param>
        /// <returns>加成值</returns>
        public int AddValue(string value)
        {
            if (!value.Contains("_")) return Convert.ToInt32(value);
            var item = value.Split("_").ToList();
            var add = RNG.Next(Convert.ToInt32(item[0]), Convert.ToInt32(item[1]));
            return Convert.ToInt32(add);
        }

        /// <summary>装备的基础属性</summary>
        /// <param name="equip">装备tg_bag</param>
        /// <param name="type">属性类型</param>
        /// <param name="value">加成属性值</param>
        /// <returns>装备tg_bag</returns>
        public tg_bag EquipAtt(tg_bag equip, int type, double value)
        {
            if (equip.attribute1_type == 0)
            {
                equip.attribute1_type = type;
                equip.attribute1_value = value;
            }
            else if (equip.attribute2_type == 0)
            {
                equip.attribute2_type = type;
                equip.attribute2_value = value;
            }
            else if (equip.attribute3_type == 0)
            {
                equip.attribute3_type = type;
                equip.attribute3_value = value;
            }
            return equip;
        }
    }
}
