using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Vo
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
