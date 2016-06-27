using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 生活技能基表
    /// </summary>
    //[Serializable]
    public class BaseLifeSkill : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseLifeSkill CloneEntity()
        {
            return Clone() as BaseLifeSkill;
        }

        #endregion

        /// <summary>技能id</summary>
        public int id { get; set; }

        /// <summary>类型</summary>
        public int type { get; set; }

        /// <summary>等级上限</summary>
        public int levelLimit { get; set; }

        /// <summary>学习前置技能</summary>
        public string studyCondition { get; set; }

        /// <summary>前置技能等级</summary>
        public int conditionLevel { get; set; }

        /// <summary>学习后置技能</summary>
        public string studypostposition { get; set; }

        /// <summary>学习武将等级</summary>
        public int studyLevel { get; set; }

    }
}
