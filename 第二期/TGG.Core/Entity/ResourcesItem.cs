using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 资源类
    /// </summary>
    [Serializable]
    public class ResourcesItem
    {
        //类型_id_是否绑定_数量
        
        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>类型</summary>
        public int type { get; set; }

        /// <summary>是否绑定</summary>
        public int bind { get; set; }

        /// <summary>数量</summary>
        public int count { get; set; }

    }
}
