using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 战斗技能效果基表
    /// </summary>
    //[Serializable]
    public class BaseFightSkillEffect : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseFightSkillEffect CloneEntity()
        {
            return Clone() as BaseFightSkillEffect;
        }

        #endregion

        /// <summary> id</summary>
        public int id { get; set; }

        /// <summary>技能等级 </summary>
        public int level { get; set; }

        /// <summary> 技能id</summary>
        public int skillid { get; set; }

        /// <summary>升级消耗金钱 </summary>
        public int costCoin { get; set; }

        /// <summary>消耗时间（分钟） </summary>
        public int costTimer { get; set; }

        /// <summary>技能效果 </summary>
        public string effects { get; set; }

        /// <summary>是否立即攻击 </summary>
        public int isQuickAttack { get; set; }

        /// <summary> 攻击范围 </summary>
        public int attackRange { get; set; }

    }
}
