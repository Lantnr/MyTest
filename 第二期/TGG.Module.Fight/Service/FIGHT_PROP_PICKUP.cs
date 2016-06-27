using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Fight.Service
{
    public class FIGHT_PROP_PICKUP
    {
        private static FIGHT_PROP_PICKUP _objInstance;

        /// <summary>FIGHT_PROP_PICKUP单体模式</summary>
        public static FIGHT_PROP_PICKUP GetInstance()
        {
            return _objInstance ?? (_objInstance = new FIGHT_PROP_PICKUP());
        }

        /// <summary> 拾取道具 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "FIGHT_PROP_PICKUP", "拾取战斗道具");
#endif
            var userid = session.Player.User.id;
            var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value); //type:类型 0:丢弃  1:入包
            var bags = new List<tg_bag>();
            if (!Variable.TempProp.ContainsKey(userid))
                return BuildData((int)ResultType.FRONT_DATA_ERROR);
            if (type == 0)
            {
                Variable.TempProp.TryRemove(userid, out bags);
                return BuildData((int)ResultType.SUCCESS);
            }
            if (type != 1)
                return BuildData((int)ResultType.FRONT_DATA_ERROR);
            if (session.Player.Bag.Surplus < Variable.TempProp[userid].Count)    //验证背包剩余格子
                return BuildData((int)ResultType.PROP_BAG_LACK);
            (new Bag()).BuildReward(userid, Variable.TempProp[userid]); //入包整理并推送
            Variable.TempProp.TryRemove(userid, out bags);
            return BuildData((int)ResultType.SUCCESS);
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(int result)
        {
            var dic = new Dictionary<string, object> { { "result", result } };
            return new ASObject(dic);
        }
    }
}
