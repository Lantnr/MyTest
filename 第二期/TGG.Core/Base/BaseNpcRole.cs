using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// NPC武将基表
    /// </summary>
    //[Serializable]
    public class BaseNpcRole : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseNpcRole CloneEntity()
        {
            return Clone() as BaseNpcRole;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>等级</summary>
        public int lv { get; set; }

        /// <summary>攻击</summary>
        public int attack { get; set; }

        /// <summary>防御</summary>
        public int defense { get; set; }

        /// <summary>生命</summary>
        public int life { get; set; }

        /// <summary>增伤</summary>
        public double hurtIncrease { get; set; }

        /// <summary>减伤</summary>
        public double hurtReduce { get; set; }

        /// <summary>会心几率</summary>
        public double critProbability { get; set; }

        /// <summary>会心加成</summary>
        public double critAddition { get; set; }

        /// <summary>闪避几率</summary>
        public double dodgeProbability { get; set; }

        /// <summary>奥义触发几率</summary>
        public double mysteryProbability { get; set; }

        /// <summary>个人战奥义</summary>
        public int pmystery { get; set; }

        /// <summary>个人战秘籍</summary>
        public int pcheatCode { get; set; }

        /// <summary>合战奥义</summary>
        public int mmystery { get; set; }

        /// <summary>合战秘籍</summary>
        public int mcheatCode { get; set; }
    }
}
