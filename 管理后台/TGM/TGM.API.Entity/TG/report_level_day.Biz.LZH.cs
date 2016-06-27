using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class report_level_day
    {
        /// <summary>根据时间信息查询等级信息记录</summary>
        /// <param name="index">分页索引值</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数量</param>
        public static List<report_level_day> GetPageEntity(Int32 index, Int32 size, out Int32 count)
        {
            count = FindCount();
            return FindAll(null, "createtime desc", "*", index * size, size);
        }

        /// <summary>根据时间查询最新记录</summary>
        public static report_level_day GetFindByTime()
        {
            var begin = Convert.ToDateTime(DateTime.Now.ToShortDateString()).Ticks;
            var end = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString()).Ticks;
            var where = String.Format(" createtime >{0} and createtime<{1}", begin, end);
            var entity = Find(where);
            return entity;
        }
    }
}
