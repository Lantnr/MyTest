using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    public class BaseYouYiYuan : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseYouYiYuan CloneEntity()
        {
            return Clone() as BaseYouYiYuan;
        }

        #endregion

        /// <summary>编号</summary>
        public Int32 id { get; set; }

        /// <summary>游戏类型</summary>
        public Int32 type { get; set; }

        /// <summary>闯关总次数</summary>
        public Int32 num { get; set; }

    }
}
