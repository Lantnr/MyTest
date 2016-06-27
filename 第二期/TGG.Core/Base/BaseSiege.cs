using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 美浓攻略活动基表
    /// </summary>
    //[Serializable]
    public class BaseSiege : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseSiege CloneEntity()
        {
            return Clone() as BaseSiege;
        }

        #endregion

        /// <summary> id </summary>
        public Int64 id { get; set; }

        /// <summary> 活动内容 </summary>
        public int contentType { get; set; }

        /// <summary> 属性类型 </summary>
        public int ributeType { get; set; }

        /// <summary> 技能类型 </summary>
        public int skillType { get; set; }

        /// <summary> 等级 </summary>
        public int level { get; set; }

        /// <summary> 属性值 </summary>
        public int ributeValues { get; set; }

        /// <summary> 概率 </summary>
        public int probability { get; set; }

        /// <summary> 加成点数 </summary>
        public int count { get; set; }
    }
}
