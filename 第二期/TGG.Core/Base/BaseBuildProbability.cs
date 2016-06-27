using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    //[Serializable]
    public class BaseBuildProbability : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseBuildProbability CloneEntity()
        {
            return Clone() as BaseBuildProbability;
        }

        #endregion

        /// <summary></summary>
        public int content { get; set; }

        /// <summary>技能或属性类型</summary>
        public int skill { get; set; }

        /// <summary>技能</summary>
        public int level { get; set; }

        /// <summary>成功概率</summary>
        public int probability { get; set; }

        /// <summary>筑城成功增加的耐久度</summary>
        public int count { get; set; }

        /// <summary>属性</summary>
        public int attr { get; set; }

        /// <summary>属性值</summary>
        public int value { get; set; }


    }
}
