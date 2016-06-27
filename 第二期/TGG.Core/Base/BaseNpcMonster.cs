using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// NPC怪物基表
    /// </summary>
    //[Serializable]
    public class BaseNpcMonster : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseNpcMonster CloneEntity()
        {
            return Clone() as BaseNpcMonster;
        }

        #endregion

        /// <summary>Id</summary>
        public int id { get; set; }

        /// <summary>类型</summary>
        public int type { get; set; }

        /// <summary>获得魂数</summary>
        public int spirit { get; set; }

        /// <summary>npc战斗印效果</summary>
        public int yinEffectId { get; set; }

        /// <summary>npc战斗武将集合</summary>
        public String matrix { get; set; }

        /// <summary>所带装备</summary>
        public String equip { get; set; }

        /// <summary>偷窃成功率</summary>
        public Double rate { get; set; }

        /// <summary>装备掉落概率</summary>
        public Double probability { get; set; }

        /// <summary>所属居城</summary>
        public int cityId { get; set; }

        /// <summary>武将限制</summary>
        public int limit { get; set; }

        /// <summary>玩家喝茶所需等级</summary>
        public int level { get; set; }

        /// <summary>挑战npc胜利获得经验奖励</summary>
        public int experience { get; set; }

        /// <summary>等级限制</summary>
        public string levelLimit { get; set; }

        /// <summary>阵营</summary>
        public int camp { get; set; }

    }
}
