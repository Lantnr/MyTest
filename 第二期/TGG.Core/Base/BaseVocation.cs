using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 职业基表
    /// </summary>
    //[Serializable]
    public class BaseVocation : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseVocation CloneEntity()
        {
            return Clone() as BaseVocation;
        }

        #endregion

        /// <summary>id(类型)</summary>
        public int id { get; set; }

        /// <summary>职业</summary>
        public int vocation { get; set; }

        /// <summary>跑商手续费(%)</summary>
        public Double business { get; set; }

        /// <summary>战斗系数</summary>
        public Double fight { get; set; }

        /// <summary>合战系数</summary>
        public Double war { get; set; }

    }
}
