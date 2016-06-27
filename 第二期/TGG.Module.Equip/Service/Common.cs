using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Base;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Module.Equip.Service
{
    public partial class Common
    {
        private static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }

        /// <summary>组装获得装备属性信息</summary>
        public tg_bag GetEquip(BaseEquip equip)
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
            newequip.base_id = (int)equip.id;
            newequip.type = (int)GoodsType.TYPE_EQUIP;
            newequip.equip_type = equip.typeSub;
            newequip.state = 0;
            newequip.count = 1;
            return newequip;
        }

        public tg_bag GetEquip(Int64 equip)
        {
            var base_equip = Variable.BASE_EQUIP.FirstOrDefault(m => m.id == Convert.ToDecimal(equip));
            return base_equip == null ? null : GetEquip(base_equip);
        }

    }
}
