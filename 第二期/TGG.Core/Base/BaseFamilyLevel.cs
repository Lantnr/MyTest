using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 家族等级表
    /// </summary>
    //[Serializable]
    public class BaseFamilyLevel : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseFamilyLevel CloneEntity()
        {
            return Clone() as BaseFamilyLevel;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }

        /// <summary>声望</summary>
        public int prestige { get; set; }

        /// <summary>每日俸禄</summary>
        public int daySalary { get; set; }

        /// <summary>副族长人数</summary>
        public int viceChairman { get; set; }

        /// <summary>长老人数</summary>
        public int elder { get; set; }

        /// <summary>家族总人数</summary>
        public int amount { get; set; }

        /// <summary>nextId</summary>
        public int nextId { get; set; }
    }
}
