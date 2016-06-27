using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGM.API.Entity
{
    public partial class tgm_record_day
    {
        /// <summary>查询服务器元宝信息记录</summary>
        /// <param name="sid">服务器sid</param>       
        /// <param name="index">分页索引值</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数量</param>
        /// <returns></returns>
        public static EntityList<tgm_record_day> GetPageEntity(Int32 sid, Int32 index, Int32 size, out Int32 count)
        {
            var where = string.Format("sid={0}", sid);
            count = FindCount(where, null, null, 0, 0);
            return FindAll(where, "createtime desc", "*", index * size, size);
        }
    }
}
