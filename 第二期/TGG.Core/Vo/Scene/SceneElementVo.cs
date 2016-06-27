using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Scene
{
    /// <summary>
    /// 场景元素数据
    /// </summary>
    public class SceneElementVo : BaseVo
    {
        /// <summary>
        /// x坐标
        /// </summary>
        /// <value>The x.</value>
        public int x { get; set; }
        /// <summary>
        /// y坐标
        /// </summary>
        /// <value>The y.</value>
        public int y { get; set; }
        /// <summary>
        /// 元素显示的名称(用户名)
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
    }
}
