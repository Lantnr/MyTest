using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class report_day
    {
        /// <summary>根据时间查询最新记录</summary>
        /// <param name="time">当前时间</param>
        public static report_day GetFindByTime()
        {
            var begin = Convert.ToDateTime(DateTime.Now.ToShortDateString()).Ticks;
            var end = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString()).Ticks;
            var where = String.Format(" createtime >{0} and createtime<{1}", begin, end);
            var entity = Find(where);
            return entity;
        }

    }
}
