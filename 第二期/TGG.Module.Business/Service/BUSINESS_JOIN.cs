using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Business;
using TGG.SocketServer;
using NewLife.Log;


namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 跑商进入指令
    /// </summary>
    public class BUSINESS_JOIN
    {
        private static BUSINESS_JOIN ObjInstance;

        /// <summary>BUSINESS_JOIN单体模式</summary>
        public static BUSINESS_JOIN GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_JOIN());
        }

        /// <summary>跑商进入指令</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_JOIN", "跑商进入指令");
#endif
            try
            {
                var listvo = new List<BusinessCarVo>();
                var businesslist = new List<ASObject>();

                var player = session.Player.User.CloneEntity();              
                var list_car = tg_car.GetEntityByUserId(player.id);
                if (list_car.Count <= 0) return new ASObject(BuildData((int) ResultType.BUSINESS_CAR_LACK, businesslist));
                var goods = tg_goods_business.GetEntity(list_car);//.ToList().Where(m => ids.IndexOf(m.cid) >= 0).ToList();
                foreach (var item in list_car) //遍历马车
                {
                    var gd = goods.Where(m => m.cid == item.id).ToList();//该马车的货物
                    listvo.Add(EntityToVo.ToBusinessCarVo(item, Common.GetInstance().ConverBusinessGoodsVos(gd)));
                }
                businesslist = ConvertListASObject(listvo);
                return new ASObject(BuildData((int)ResultType.SUCCESS, businesslist));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }


        /// <summary>玩家所有马车组装数据</summary>
        private Dictionary<String, Object> BuildData(object result, List<ASObject> list)
        {
            var dic = new Dictionary<string, object> {{"result", result}, {"car", list.Count > 0 ? list : null}};
            return dic;
        }

        private List<ASObject> ConvertListASObject(IEnumerable<BusinessCarVo> list)
        {
            return list.Select(AMFConvert.ToASObject).ToList();
        }
    }
}
