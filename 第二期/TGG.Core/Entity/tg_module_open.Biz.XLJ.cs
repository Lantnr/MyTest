using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 用户开放模块部分类
    /// </summary>
    public partial class tg_module_open
    {
        /// <summary>根据用户id获取开发模块集合</summary>
        public static List<tg_module_open> GetListByUserId(Int64 userid)
        {
            return FindAll(new String[] { _.user_id }, new Object[] { userid });
        }
    }
}
