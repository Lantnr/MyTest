using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 爬塔怪物表
    /// </summary>
   //[Serializable]
    public class BaseTowerEnemy : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTowerEnemy CloneEntity()
        {
            return Clone() as BaseTowerEnemy;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>关卡</summary>
        public int pass { get; set; }

        /// <summary>难度</summary>
        public int difficulty { get; set; }

        /// <summary>npc战斗印效果</summary>
        public int yinEffectId { get; set; }

        /// <summary>npc战斗武将集合</summary>
        public string matrix { get; set; }

        /// <summary>机率</summary>
        public double odds { get; set; }

        /// <summary>声望奖励</summary>
        public int award { get; set; }
    }
}
