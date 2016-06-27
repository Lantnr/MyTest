using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewLife.Reflection;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Module.Notice.Service
{
    /// <summary>
    /// 部分公共方法
    /// </summary>
    public partial class Common
    {

        private static Common _objInstance;

        /// <summary>Common 单体模式</summary>
        public static Common GetInstance()
        {
            return _objInstance ?? (_objInstance = new Common());
        }

    }
}
