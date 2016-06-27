using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.Core.Vo.Business;
using TGG.SocketServer;
using Task = System.Threading.Tasks.Task;
using NewLife.Log;

namespace TGG.Module.Business.Service
{
    public partial class Common
    {
        /// <summary> 转换前端需要的跑商货物Vo集合 </summary>
        /// <param name="list">List[tg_goods_business]</param>
        public List<BusinessGoodsVo> ConverBusinessGoodsVos(IEnumerable<tg_goods_business> list)
        {
            return list.Select(EntityToVo.ToBusinessGoodsVo).ToList();
        }

    }
}
