using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGM.API.Entity
{
    /// <summary>
    /// tgm_record_day逻辑类
    /// </summary>
    public partial class tgm_record_day
    {
        public static tgm_record_day GetFindBySidTime(Int32 sid)
        {
            var begin = Convert.ToDateTime(DateTime.Now.ToShortDateString()).Ticks;
            var end = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString()).Ticks;
            var where = String.Format("sid={0} and createtime>{1} and createtime<{2}", sid, begin, end);
            return Find(where);
        }

        /// <summary>服务器每天记录统计分页</summary>
        /// <param name="sid">服务器编号</param>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>

        public static EntityList<tgm_record_day> GetPageEntity(Int32 sid, DateTime begin, DateTime end, Int32 index, Int32 size, out Int32 count)
        {
            var _where = String.Format("[sid]={0} AND createtime>{1} AND createtime<{2}", sid, begin.Ticks, end.Ticks);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
        }
    }
}
