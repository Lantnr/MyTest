using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 武将修行基表
    /// </summary>
    //[Serializable]
    public class BaseTrain : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTrain CloneEntity()
        {
            return Clone() as BaseTrain;
        }

        #endregion

        /// <summary>id(等级)</summary>
        public int id { get; set; }

        /// <summary>修行境界</summary>
        public String name { get; set; }

        /// <summary>体力</summary>
        public int power { get; set; }

        /// <summary>政务</summary>
        public int govern { get; set; }

        /// <summary>提升点数</summary>
        public int count { get; set; }

        /// <summary>修行时间</summary>
        public int time { get; set; }

        /// <summary>下一等级id</summary>
        public int nextId { get; set; }

    }
}
