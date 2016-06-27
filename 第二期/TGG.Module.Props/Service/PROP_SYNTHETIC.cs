using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Props.Service
{
    /// <summary>
    /// 道具合成
    /// </summary>
    public class PROP_SYNTHETIC
    {
        private static PROP_SYNTHETIC ObjInstance;

        /// <summary>
        /// PROP_SYNTHETIC单体模式
        /// </summary>
        /// <returns></returns>
        public static PROP_SYNTHETIC GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new PROP_SYNTHETIC());
        }

        /// <summary> 道具合成 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "PROP_SYNTHETIC", "道具合成");
#endif
                var propid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "id").Value);//解析数据
                var userid = session.Player.User.id;
                var prop = tg_bag.FindByid(propid); //查询道具信息    
                if (prop == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR));
                if (session.Player.Bag.BagIsFull) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_BAG_LACK)); //验证背包是否满

                var baseprop = Variable.BASE_PROP.FirstOrDefault(m => m.id == prop.base_id);  //道具基表数据
                if (baseprop == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.BASE_TABLE_ERROR));
                if (baseprop.make != 1) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_NO_COMPOSE));

                prop.count -= baseprop.consumecount;
                if (prop.count < 0) return new ASObject(Common.GetInstance().BuildData((int)ResultType.PROP_LACK));
                var newprop = BuildProp(baseprop.targetId, userid); //合成组装的道具

                var reward = new List<RewardVo>();
                if (prop.count == 0)
                {
                    prop.Delete();

                    var model = string.Format("{0}", "删除道具:" + prop.id);
                    (new Log()).WriteLog(prop.user_id, (int)LogType.Delete, (int)ModuleNumber.BAG, (int)PropCommand.PROP_SYNTHETIC, model);

                    reward.Add(new RewardVo { goodsType = (int)GoodsType.TYPE_PROP, deleteArray = new List<double> { prop.id } });
                    (new Bag()).BuildReward(prop.user_id, new List<tg_bag>() { prop, newprop }, reward);//物品更变
                }
                else (new Bag()).BuildReward(prop.user_id, new List<tg_bag>() { prop, newprop });//物品更变
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
            }
            catch (Exception ex)
            {
                XTrace.WriteLine(ex.Message);
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.FAIL, null, null));
            }
        }

        /// <summary> 组装并推送 </summary>
        /// <param name="propid">合成目标的道具基表Id</param>
        /// <param name="userid">玩家id</param>
        private tg_bag BuildProp(decimal propid, Int64 userid)
        {
            var target = Variable.BASE_PROP.FirstOrDefault(m => m.id == propid);
            if (target == null) return null;
            var prop = new tg_bag()
            {
                base_id = target.id,
                type = (int)GoodsType.TYPE_PROP,
                count = 1,
                bind = target.bind,
                user_id = userid,
            };
            return prop;
        }
    }
}
