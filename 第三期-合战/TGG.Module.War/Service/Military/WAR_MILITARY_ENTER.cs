using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Military
{
    /// <summary>
    /// 进入军需品功能
    /// arlen
    /// </summary>
    public class WAR_MILITARY_ENTER : IDisposable
    {
        //private static WAR_MILITARY_ENTER _objInstance;

        ///// <summary>WAR_MILITARY_ENTER单体模式</summary>
        //public static WAR_MILITARY_ENTER GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_MILITARY_ENTER());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 查看开始指令处理 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_MILITARY_ENTER", "进入军需品功能");
#endif
            if (!data.ContainsKey("id")) return null;
            var station = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value); //据点id
            if (station == 0) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);

            var list=Variable.WARGOODS.Values.Where(m => m.city_id == station).ToList();

            return list.Count <= 0 ? CommonHelper.ErrorResult((int)ResultType.DATA_NULL_ERROR) :
                new ASObject(BuildData((int)ResultType.SUCCESS, list));
        }

        /// <summary>仓库物品信息集合</summary>
        private List<ASObject> ToWarGoods(IEnumerable<GlobalWarGoods> list)
        {
            var war_goods = new List<ASObject>();
            war_goods.AddRange(list.Select(item => AMFConvert.ToASObject(EntityToVo.ToWarGoodsVo(item))));
            return war_goods;
        }

        private Dictionary<String, Object> BuildData(int result, List<GlobalWarGoods> list)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, 
                { "goods", list.Any() ? ToWarGoods(list) : null }, 
            };
            return dic;
        }
    }
}
