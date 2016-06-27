using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 生活技能效果基表
    /// </summary>
    //[Serializable]
    public class BaseLifeSkillEffect : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseLifeSkillEffect CloneEntity()
        {
            return Clone() as BaseLifeSkillEffect;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }

        /// <summary>技能id</summary>
        public int skillid { get; set; }

        /// <summary>熟练度</summary>
        public int progress { get; set; }

        /// <summary>消耗金钱</summary>
        public int costCoin { get; set; }

        /// <summary>消耗时间（分钟）</summary>
        public int costTimer { get; set; }

        /// <summary>消耗体力</summary>
        public int power { get; set; }

        /// <summary>技能效果</summary>
        public string effect { get; set; }

        /// <summary>下一等级id</summary>
        public int nextId { get; set; }

    }
}
