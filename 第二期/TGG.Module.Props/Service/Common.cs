using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.AMF;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.SocketServer;
using tg_user = TGG.Core.Entity.tg_user;

namespace TGG.Module.Props.Service
{
    public partial class Common
    {
        private static Common ObjInstance;

        /// <summary>
        /// Common 单体模式
        /// </summary>
        /// <returns></returns>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }


        #region 数据组装

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result)
        {
            var dic = new Dictionary<string, object> { { "result", result } };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, int count)
        {
            var dic = new Dictionary<string, object> { 
            { "result", result } ,
            {"count",count}
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, dynamic props)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, 
                { "prop", props != null ? AMFConvert.ToASObject(EntityToVo.ToPropVo(props)) : null }
            };
            return dic;
        }
        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, dynamic list_props, dynamic list_equip)
        {
            return BuildData(result, list_props, list_equip, true);
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, List<tg_bag> list_props, List<tg_bag> list_equip, int count)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"prop", list_props.Any() ? ConvertListASObject(list_props, "Props") : null},
                {"equip", list_equip.Any() ? ConvertListASObject(list_equip, "Equip") : null},
                {"count", count}
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, dynamic list_update, dynamic list, bool isequip)
        {
            var dic = new Dictionary<string, object> { { "result", result } };
            if (list_update != null && list_update.Count > 0)
                dic.Add("prop", ConvertListASObject(list_update, "Props"));
            else
                dic.Add("prop", null);
            if (isequip)
            {
                if (list != null && list.Count > 0)
                    dic.Add("equip", ConvertListASObject(list, "Equip"));
                else
                    dic.Add("equip", null);
            }
            else
            {
                if (list != null && list.Count > 0)
                    dic.Add("propremove", list);
                else
                    dic.Add("propremove", null);
            }
            return dic;
        }

        #endregion

        /// <summary>向用户推送更新</summary>
        public void RewardsToUser(TGGSession session, tg_user user, int type)
        {
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
        }

        /// <summary>集合转换ASObject集合</summary>
        public List<ASObject> ConvertListASObject(dynamic list, string classname)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model;
                switch (classname)
                {
                    case "Props": model = EntityToVo.ToPropVo(item); break;
                    case "Equip": model = EntityToVo.ToEquipVo(item); break;
                    default: model = null; break;
                }
                list_aso.Add(AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        /// <summary>获取基表道具</summary>
        public BaseProp GetBaseProp(int propid)
        {
            return Variable.BASE_PROP.FirstOrDefault(m => m.id == propid);
        }

        /// <summary>获取基表装备</summary>
        public BaseEquip GetBaseEquip(int equipid)
        {
            return Variable.BASE_EQUIP.FirstOrDefault(m => m.id == equipid);
        }

        /// <summary>获取道具出售的总额</summary>
        /// <param name="coin">单价</param>
        /// <param name="count">数量</param>
        public int GetSellMoney(int coin, int count)
        {
            return coin * count;
        }
    }
}
