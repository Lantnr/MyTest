using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_activity_building
    {
        /// <summary>批量插入任务数据</summary>
        public static int GetListInsert(IEnumerable<tg_activity_building> tasks)
        {
            var list = new EntityList<tg_activity_building>();
            list.AddRange(tasks);
            return list.Insert();
        }

    }
}
