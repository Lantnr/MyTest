using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 窗口
    /// </summary>
    public class Window : ICloneable
    {

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public Window CloneEntity()
        {
            return Clone() as Window;
        }

        #endregion

        public Window()
        {
            X = 1024;
            Y = 768;
        }

        /// <summary>
        /// X
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public Int32 Y { get; set; }
    }
}
