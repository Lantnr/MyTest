using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_messages
    {
        /// <summary>批量插入服务器福利卡激活码信息</summary>
        /// <summary>批量插入任务数据</summary>
        public static int GetListInsert(List<tg_messages> tasks)
        {
            var list = new EntityList<tg_messages>();
            list.AddRange(tasks);
            return list.Insert();
        }
    }
}
