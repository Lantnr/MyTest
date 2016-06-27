using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {

        public static Common ObjInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new Common());
        }
    }
}
