using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Equip;
using TGG.Core.Vo.Prop;
using TGG.SocketServer;
using TGG.Core.AMF;
using TGG.Core.Vo.Role;

namespace TGG.Module.Equip.Service
{
    public partial class Common
    {
        #region 数据组装
        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, dynamic equip)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"equip", equip != null ? EntityToVo.ToEquipVo(equip) : null}
            };
            return dic;
        }
        #endregion

        #region  最新的

        /// <summary>数据组装 </summary>
        public Dictionary<String, Object> BuilData(int result, List<tg_bag> list_equip)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"equip", list_equip != null ? ConvertListASObject(list_equip) : null}
            };
            return dic;
        }

        /// <summary>数据组装 </summary>
        public Dictionary<String, Object> BuilData(int result, dynamic equip, int spirit, Int64 rid)//, tg_train_role roletrain)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result },
                { "equip", equip != null ? EntityToVo.ToEquipVo(equip) : null },
                { "spirit", spirit }, 
                { "role", rid == 0 ? null : RoleInfo(rid) }
            };
            return dic;
        }

        /// <summary>数据组装 </summary>
        public Dictionary<String, Object> BuilDataSpiritLock(int result, dynamic equip)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"equip", equip != null ? EntityToVo.ToEquipVo(equip) : null}
            };
            return dic;
        }

        public Dictionary<String, Object> BuildData(int result, tg_bag equip, Int64 rid)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"equip", equip != null ? EntityToVo.ToEquipVo(equip) : null},
                { "role", rid == 0 ? null : RoleInfo(rid) }
            };
            return dic;
        }

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

        /// <summary>反射武将公共方法</summary>
        public RoleInfoVo RoleInfo(Int64 rid)
        {
            return (new Share.Role()).BuildRole(rid);
        }


        //public BaseSpirit SearchBaseSpirit(IEnumerable<BaseSpirit> base_spirit, BaseEquip base_equip)
        //{
        //    var uselevel = base_equip.useLevel;
        //    var value = new UserLevelValue();
        //    foreach (var item in base_spirit)
        //    {
        //        var levels = item.userLv.Split('_');
        //        value.minlevel = Convert.ToInt32(levels[0]);
        //        value.maxlevel = Convert.ToInt32(levels[1]);
        //        if (uselevel >= value.minlevel && uselevel <= value.maxlevel)
        //        {
        //            return item;
        //        }
        //    }
        //    return null;
        //}


        /// <summary>获取基表装备</summary>
        public BaseEquip GetBaseEquip(decimal equipid)
        {
            return Variable.BASE_EQUIP.FirstOrDefault(m => m.id == equipid);
        }

        /// <summary>向用户推送更新</summary>
        public void RewardsToUser(TGGSession session, tg_user user, int type)
        {
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
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
        #endregion
    }
}
