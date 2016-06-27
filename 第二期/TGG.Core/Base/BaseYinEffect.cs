using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 印效果基表
    /// </summary>
    //[Serializable]
    public class BaseYinEffect : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseYinEffect CloneEntity()
        {
            return Clone() as BaseYinEffect;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>技能等级</summary>
        public int level { get; set; }

        /// <summary>印id</summary>
        public int yinId { get; set; }

        /// <summary>有效回合</summary>
        public int validRound { get; set; }

        /// <summary>升级魂数</summary>
        public int spiritFormula { get; set; }

        /// <summary>技能效果</summary>
        public String effects { get; set; }

        /// <summary>是否立即攻击 </summary>
        public int isQuickAttack { get; set; }

        /// <summary> 攻击范围 </summary>
        public int attackRange { get; set; }

    }
}
