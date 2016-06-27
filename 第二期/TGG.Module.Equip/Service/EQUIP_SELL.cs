using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.SocketServer;
using TGG.Core.Enum.Type;
using TGG.Core.Entity;

namespace TGG.Module.Equip.Service
{
    /// <summary>
    /// 装备出售
    /// </summary>
    public class EQUIP_SELL
    {
        private static EQUIP_SELL ObjInstance;

        /// <summary> EQUIP_SELL单体模式 </summary>
        public static EQUIP_SELL GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new EQUIP_SELL());
        }

        /// <summary> 装备出售</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "EQUIP_SELL", "装备出售");
#endif
                var equipid = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
                return BagSell(equipid, 1, session);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary> 道具出售验证出售类型 </summary>
        public ASObject BagSell(Int64 propid, int count, TGGSession session)
        {
            var prop = tg_bag.FindByid(propid); //查询道具信息 
            return prop == null ? BuildData((int)ResultType.PROP_UNKNOW) : EquipSell(prop, count, session);
        }

        /// <summary>装备出售</summary>
        private ASObject EquipSell(tg_bag equip, int count, TGGSession session)
        {
            var user = session.Player.User.CloneEntity();
            if (equip == null)
                return BuildData((int)ResultType.EQUIP_UNKNOW); //没有该装备
            if (equip.state == (int)LoadStateType.LOAD)
                return BuildData((int)ResultType.EQUIP_NOSELL);
            var base_equip = Common.GetInstance().GetBaseEquip(equip.base_id);  //查询装备基表信息

            var coin = user.coin;
            if (base_equip == null) return BuildData((int)ResultType.FRONT_DATA_ERROR);
            user.coin = tg_user.IsCoinMax(user.coin, base_equip.sellPrice * count);
            if (user.Update() <= 0) return BuildData((int)ResultType.DATABASE_ERROR); //处理用户的数据

            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", "Coin", "EquipSell",equip.base_id, coin, base_equip.sellPrice * count, user.coin);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.EQUIP, (int)EquipCommand.EQUIP_SELL, logdata);
            CreateLog(user.id, equip.base_id, count);

            Common.GetInstance().RewardsToUser(session, user, (int)GoodsType.TYPE_COIN);
            log.BagInsertLog(equip, (int)ModuleNumber.EQUIP, (int)EquipCommand.EQUIP_SELL, base_equip.sellPrice * count); //记录日志
            if (equip.Delete() == 0) //删除装备
                return BuildData((int)ResultType.DATABASE_ERROR);
            session.Player.Bag.Surplus += 1;
            if (session.Player.Bag.BagIsFull) //背包满状态处理
                session.Player.Bag.BagIsFull = false;
            return BuildData((int)ResultType.SUCCESS);
        }

        /// <summary>购买装备日志</summary>
        private void CreateLog(Int64 userid, int baseid, int count)
        {
            var equips = tg_bag.GetFindBag(userid, baseid);
            var _count = equips.Count - count;
            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}_{4}", "EquipSell", baseid, equips.Count, count, _count);
            (new Share.Log()).WriteLog(userid, (int)LogType.Get, (int)ModuleNumber.EQUIP, (int)EquipCommand.EQUIP_BUY, logdata);
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        public ASObject BuildData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
            };
            return new ASObject(dic);
        }
    }

}
