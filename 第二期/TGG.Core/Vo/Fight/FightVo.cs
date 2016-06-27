using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx;

namespace TGG.Core.Vo.Fight
{
    /// <summary> 战斗Vo</summary>
    [Serializable]
    public class FightVo : BaseVo, ICloneable
    {
        /// <summary>攻击方用户 id </summary>
        public double attackId { get; set; }

        ///// <summary> 怪物类型  0人物  1怪物 </summary>
        //public int monsterType { get; set; }

        /// <summary> 对手主角武将名 </summary>
        public string wuJiangName { get; set; }

        /// <summary> 攻击方印 </summary>
        public YinVo yinA { get; set; }

        /// <summary> 防守方印 </summary>
        public YinVo yinB { get; set; }

        /// <summary> 出招 vo </summary>
        public List<List<MovesVo>> moves { get; set; }

        /// <summary>战斗来源类型</summary>
        public int srcType { get; set; }

        /// <summary>我方胜负情况</summary>
        public bool isWin { get; set; }

        /// <summary>奖励数据[rewardVo]</summary>
        public List<RewardVo> rewards { get; set; }

        /// <summary>攻击方永久 Buff BuffVo</summary>
        public List<BuffVo> buffVoA { get; set; }

        /// <summary>防守方永久 Buff  BuffVo</summary>
        public List<BuffVo> buffVoB { get; set; }

        /// <summary>掉落物品是否含有道具 0:没有 1:有</summary>
        public int haveProp { get; set; }

        /// <summary>战斗胜利宝箱开启的道具</summary>
        public List<RewardVo> propReward { get; set; }

        public FightVo()
        {
            moves = new List<List<MovesVo>>();
            rewards = new List<RewardVo>();
            yinA = new YinVo();
            yinB = new YinVo();
            buffVoA = new List<BuffVo>();
            buffVoB = new List<BuffVo>();

        }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public FightVo CloneEntity()
        {
            return Clone() as FightVo;
        }

        #endregion

    }
}
