using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity
{
    public partial class tgm_notice
    {
        /// <summary>查询公告集合</summary>
        /// <param name="id">用户id</param>
        public static List<tgm_notice> GetEntityList(Int64 id)
        {
            var time = DateTime.Now.Ticks;
            return FindAll(string.Format("end_time>{0} and player_id={1}", time, id), null, null, 0, 0);
        }
    }
}
