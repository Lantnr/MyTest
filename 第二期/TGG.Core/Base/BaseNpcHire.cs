using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 忍者雇佣基表
    /// </summary>
    //[Serializable]
    public class BaseNpcHire : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseNpcHire CloneEntity()
        {
            return Clone() as BaseNpcHire;
        }

        #endregion

        /// <summary> id </summary>
        public int id { get; set; }

        ///// <summary> 雇佣对应的身份限制 </summary>
        //public int identity { get; set; }

        /// <summary> 雇佣消耗金钱 单位:文</summary>
        public int money { get; set; }

        /// <summary> 雇佣时间 单位:分钟</summary>
        public int time { get; set; }

        /// <summary> 怪物id </summary>
        public int npcRoleId { get; set; }
    }
}
