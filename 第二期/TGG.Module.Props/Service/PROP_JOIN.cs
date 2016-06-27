using FluorineFx;
using NewLife.Log;
using System;
using System.Linq;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Entity;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 背包数据
    /// </summary>
    public class PROP_JOIN
    {
        private static PROP_JOIN ObjInstance;

        /// <summary>
        /// PROP_JOIN单体模式
        /// </summary>
        /// <returns></returns>
        public static PROP_JOIN GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new PROP_JOIN());
        }

        /// <summary>
        /// 加载道具数据
        /// </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "PROP_JOIN", "加载道具");
#endif
                var userid = session.Player.User.id;
                var bagcount = session.Player.UserExtend.bag_count;                           //背包总格子数
                var list = tg_bag.GetFindByUserId(userid).ToList();
                var listprops = list.Where(m => m.type == (int)GoodsType.TYPE_PROP).ToList(); //获取道具信息
                var listequip = list.Where(m => m.type == (int)GoodsType.TYPE_EQUIP).ToList();//获取装备信息

                return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS, listprops, listequip, bagcount));
            }
            catch (Exception ex)
            {
                XTrace.WriteLine(ex.Message);
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.FAIL, null, null));
            }
        }
    }
}
