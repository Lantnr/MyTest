using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_arena_reports
    {
        /// <summary>根据用户id获取所有战报信息</summary>
        public static EntityList<tg_arena_reports> FindByUserId(Int64 userid)
        {
            return FindAll(new String[] { _.user_id }, new Object[] { userid });
        }
    }
}
