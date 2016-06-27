using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 战斗技能基础基表
    /// </summary>
    //[Serializable]
    public class BaseFightSkill : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseFightSkill CloneEntity()
        {
            return Clone() as BaseFightSkill;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>秘技类型</summary>
        public int type { get; set; }

        /// <summary>战斗类型</summary>
        public int typeSub { get; set; }

        /// <summary>配置限制</summary>
        public int configuration { get; set; }

        /// <summary>所属流派</summary>
        public int genre { get; set; }

        /// <summary>等级上限</summary>
        public int levelLimit { get; set; }

        /// <summary>学习武将等级</summary>
        public int studyLevel { get; set; }

        /// <summary>学习前置技能</summary>
        public String studyCondition { get; set; }

        /// <summary>前置技能等级</summary>
        public int conditionLevel { get; set; }

        /// <summary>战场限制</summary>
        public int site { get; set; }

        /// <summary>天气限制</summary>
        public String weather { get; set; }

        /// <summary>使用兵种</summary>
        public int militaryType { get; set; }

        /// <summary>气力</summary>
        public int energy { get; set; }
    }
}
