using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 背包类
    /// </summary>
    [Serializable]
    public class BagItem
    {
        public BagItem()
        {
            BagIsFull = false;
            Surplus = 0;
            TempBag = new List<tg_bag>();
        }

        /// <summary>背包是否已满 (谁操作谁进行更新)</summary>
        public bool BagIsFull { get; set; }

        /// <summary>剩余格子数</summary>
        public int Surplus { get; set; }

        /// <summary>临时背包</summary>
        public List<tg_bag> TempBag { get; set; }
    }
}
