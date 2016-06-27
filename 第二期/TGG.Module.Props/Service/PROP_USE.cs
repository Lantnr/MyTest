using System.Collections.Generic;
using FluorineFx;
using System.Linq;
using System.Data;
using TGG.Core.Base;
using TGG.Core.Enum.Type;
using TGG.Core.Vo;
using TGG.SocketServer;
using NewLife.Log;
using System;
using TGG.Core.Entity;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 道具使用
    /// </summary>
    public class PROP_USE
    {
        private static PROP_USE ObjInstance;

        /// <summary>PROP_USE单体模式</summary>
        public static PROP_USE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new PROP_USE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_PROP, session, data);
        }

        /// <summary>道具使用</summary>
        public ASObject CommandStart(int goodtype, TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine(string.Format("{0}:{1}", "PROP_USE", "道具使用"));
#endif
            var id = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value.ToString()); //解析数据
            var propCount = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "count").Value.ToString());
            var props = tg_bag.GetEntityById(id);
            if (props == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR));
            var base_prop = Common.GetInstance().GetBaseProp(props.base_id);                     //道具基表数据  
            if (base_prop == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.BASE_TABLE_ERROR));
            if (base_prop.useMode != 1) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_UNUSED));                              //玩家增加相关数据          
            if (base_prop.useLevel > session.Player.Role.Kind.role_level)
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.BASE_PLAYER_LEVEL_ERROR));
            var temp = props.count - propCount; 
            //道具资源足够使用
            if (temp < 0) return new ASObject(Common.GetInstance().BuildData((int) ResultType.SUCCESS));
            props.count = temp;
            var resule = (int)Common.GetInstance().BagToUse(session, base_prop.containsResources, propCount, props);
            return new ASObject(Common.GetInstance().BuildData(resule));
        }

    }
}
