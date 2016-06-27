using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Vo.Prop
{
    /// <summary>
    /// PropVo 道具vo
    /// </summary>
    public class PropVo : BaseVo
    {

        /// <summary>
        /// 编号
        /// </summary>
        public double id { get; set; }

        /// <summary>
        /// 基础数据编号
        /// </summary>
        public double baseId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 是否绑定
        /// </summary>
        public int bind { get; set; }
    }
}
