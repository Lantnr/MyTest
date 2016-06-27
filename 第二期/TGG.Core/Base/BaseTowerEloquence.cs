using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 辩才小游戏基表
    /// </summary>
    //[Serializable]
    public class BaseTowerEloquence : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTowerEloquence CloneEntity()
        {
            return Clone() as BaseTowerEloquence;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>玩家卡牌</summary>
        public int myCard { get; set; }

        /// <summary>电脑卡牌</summary>
        public int enemyCard { get; set; }

        /// <summary>玩家扣除分值</summary>
        public int myScore { get; set; }

        /// <summary>电脑扣除分值</summary>
        public int enemyScore { get; set; }
    }
}
