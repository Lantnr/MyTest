using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;

namespace TGG.Share
{
   public class Equip
    {
        public List<ASObject> ConvertListASObject(dynamic list_equip)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list_equip)
            {
                var model = EntityToVo.ToEquipVo(item);
                list_aso.Add(AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        /// <summary>
        /// 判断装备是否存在前端传递过来的装备属性
        /// </summary>
        /// <param name="type">装备属性</param>
        /// <param name="equip">装备信息</param>
        public bool IsContainAttritute(int type, tg_bag equip)
        {
            if (type == 0) return false;
            var list_att = new List<int> { equip.attribute1_type, equip.attribute2_type, equip.attribute3_type };

            return list_att.Contains(type);
        }

        /// <summary>获取装备属性等级</summary>
        /// <param name="location"></param>
        /// <param name="equip"></param>
        /// <returns></returns>
        public int GetEquipLevel(int location, tg_bag equip)
        {
            switch (location)
            {
                case (int)EquipPositionType.ATT1_LOCATION:
                    return equip.attribute1_spirit_level;
                case (int)EquipPositionType.ATT2_LOCATION:
                    return equip.attribute2_spirit_level;
                case (int)EquipPositionType.ATT3_LOCATION:
                    return equip.attribute3_spirit_level;
            }
            return 0;
        }
    }
}
