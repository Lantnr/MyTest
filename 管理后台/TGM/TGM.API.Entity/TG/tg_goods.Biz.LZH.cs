using System.Collections.Generic;
using System.Linq;
using TGM.API.Entity;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_goods
    {
        /// <summary>批量插入服务器福利卡激活码信息</summary>
        public static bool InsertCodes(List<tgm_goods_code> codes)
        {
            var list = new EntityList<tg_goods>();
            list.AddRange(codes.Select(item => new tg_goods()
            {
                card_key = item.card_key,
                type = item.type,
            }));
            return list.Insert() > 0;
        }
    }
}
