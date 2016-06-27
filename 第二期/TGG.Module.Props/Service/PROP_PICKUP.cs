using System.Collections.Generic;
using FluorineFx;
using NewLife.Log;
using System;
using System.Linq;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 拾取道具
    /// </summary>
    public class PROP_PICKUP
    {
        private static PROP_PICKUP _objInstance;

        /// <summary>PROP_PICKUP单体模式</summary>
        public static PROP_PICKUP GetInstance()
        {
            return _objInstance ?? (_objInstance = new PROP_PICKUP());
        }

        /// <summary> 拾取道具 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "PROP_PICKUP", "拾取道具");
#endif
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value); //type:类型 0:丢弃  1:入包
            var userid = session.Player.User.id;
            if (!Variable.TempProp.ContainsKey(userid))
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR));
            if (type == 0)
            {
                var bags = new List<tg_bag>();
                Variable.TempProp.TryRemove(userid, out bags);
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
            }
            if (type != 1)
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR));
            if (session.Player.Bag.Surplus < Variable.TempProp[session.Player.User.id].Count)    //验证背包剩余格子
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_BAG_LACK));
            (new Bag()).BuildReward(userid, Variable.TempProp[session.Player.User.id]);
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }
    }
}
