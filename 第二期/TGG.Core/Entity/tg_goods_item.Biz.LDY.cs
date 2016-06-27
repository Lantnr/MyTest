using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_goods_item
    {
        public static void GetGoodsNumber()
        {
            Update("number=number_max", null);
        }
    }
}
