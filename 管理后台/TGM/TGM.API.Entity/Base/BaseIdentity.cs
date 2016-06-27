using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 身份基表
    /// </summary>
    //[Serializable]
    public class BaseIdentity : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseIdentity CloneEntity()
        {
            return Clone() as BaseIdentity;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>名称</summary>
        public string name { get; set; }

        /// <summary>职业</summary>
        public int vocation { get; set; }

        /// <summary>身份值</summary>
        public int value { get; set; }

        /// <summary>升级功勋值</summary>
        public int honor { get; set; }

        /// <summary>每日俸禄(金钱)</summary>
        public int salary { get; set; }

        /// <summary>筹措资金(贯)</summary>
        public string raiseCoin { get; set; }

        /// <summary>职业任务购买花费货币(文)</summary>
        public int coin { get; set; }

        /// <summary>职业任务购买花费元宝</summary>
        public int gold { get; set; }

        /// <summary>总功勋</summary>
        public int SumHonor { get; set; }

        /// <summary>带兵数量</summary>
        public int soldier { get; set; }

    }
}
