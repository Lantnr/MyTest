using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo
{
    [Serializable]
    public class BaseVo
    {
        /// <summary> 获取类名</summary>
        public string className
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }
}
