using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Module.API.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {

        public static Common ObjInstance = null;

        /// <summary>
        /// Common 单体模式
        /// </summary>
        /// <returns></returns>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }
    }
}
