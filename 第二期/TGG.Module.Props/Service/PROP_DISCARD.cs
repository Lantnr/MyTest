using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 道具丢弃
    /// </summary>
    public class PROP_DISCARD
    {
        private static PROP_DISCARD _objInstance;

        /// <summary>
        /// PROP_DISCARD单体模式
        /// </summary>
        /// <returns></returns>
        public static PROP_DISCARD GetInstance()
        {
            return _objInstance ?? (_objInstance = new PROP_DISCARD());
        }

        /// <summary> 道具丢弃 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "PROP_DISCARD", "道具丢弃");
#endif
            var propid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value.ToString());//解析数据

            var prop = tg_bag.FindByid(propid); //查询道具信息 
            if (prop == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_UNKNOW));

            var result = IsDelte(prop);         //验证道具信息
            if (result != ResultType.SUCCESS) return new ASObject(Common.GetInstance().BuildData((int)result));

            try { prop.Delete(); }
            catch { return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR)); }

            log.BagInsertLog(prop, (int)ModuleNumber.BAG, (int)PropCommand.PROP_DISCARD, 0); //记录日志
            var temp = string.Format("{0}_{1}", "丢弃道具ID", prop.id);
            (new Share.Log()).WriteLog(prop.user_id, (int)LogType.Delete, (int)ModuleNumber.BAG, (int)PropCommand.PROP_DISCARD, temp);
            
            session.Player.Bag.Surplus += 1;
            if (session.Player.Bag.BagIsFull) session.Player.Bag.BagIsFull = false;

            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }

        /// <summary> 验证是否可以删除 </summary>
        /// <param name="prop">要验证的实体</param>
        private ResultType IsDelte(tg_bag prop)
        {
            return prop.type == (int)GoodsType.TYPE_EQUIP ? IsDeleteEquip(prop) : IsDeleteProp(prop);
        }

        /// <summary> 验证装备是否可以删除 </summary>
        /// <param name="prop">装备</param>
        private ResultType IsDeleteEquip(tg_bag prop)
        {
            if (prop.state == (int)LoadStateType.LOAD) return ResultType.EQUIP_NOSELL; //验证装备状态
            var base_equip = Common.GetInstance().GetBaseEquip(prop.base_id);                     //查询装备基表信息
            return base_equip == null ? ResultType.BASE_TABLE_ERROR : ResultType.SUCCESS;
        }

        /// <summary> 验证道具是否可以删除 </summary>
        /// <param name="prop">道具</param>
        private ResultType IsDeleteProp(tg_bag prop)
        {
            var base_prop = Common.GetInstance().GetBaseProp(prop.base_id); //查询道具的基表数据
            if (base_prop == null) return ResultType.BASE_TABLE_ERROR;
            return base_prop.destroy == 0 ? ResultType.EQUIP_NO_DISCARD : ResultType.SUCCESS;
        }

    }
}
