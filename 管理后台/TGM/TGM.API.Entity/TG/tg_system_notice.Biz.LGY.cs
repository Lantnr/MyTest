using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_system_notice
    {
        public static int GetDelete(Int64 start,Int64 end,string content)
        {
            return Delete(string.Format("start_time={0} and end_time={1} and content='{2}'", start, end, content));
        }
    }
}
