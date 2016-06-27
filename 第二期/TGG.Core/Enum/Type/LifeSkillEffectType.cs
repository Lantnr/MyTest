using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 生活技能效果类型
    /// </summary>
    public enum LifeSkillEffectType
    {
        /// <summary>增加步兵攻击力</summary>
        INCREASE_INFANTRY_ATTACK = 1,

        /// <summary>增加枪兵攻击力</summary>
        INCREASE_MARINES_ATTACK = 2,

        /// <summary>增加骑兵攻击力</summary>
        INCREASE_CAVALRY_ATTACK = 3,

        /// <summary>提升治安</summary>
        SECURITY_PROMOTE = 4,

        /// <summary>增加弓兵攻击力</summary>
        INCREASE_BOWMAN_ATTACK = 5,

        /// <summary>增加铁炮攻击力</summary>
        INCREASE_GUN_ATTACK = 6,

        /// <summary>增加生命值上限</summary>
        LIFE_INCREASE = 7,

        /// <summary>修复城池的开发度</summary>
        CITY_DEVELOPMENT = 8,

        /// <summary>增加城池的开发度</summary>
        INCREASE_CITY_DEVELOP_PROGRESS = 9,

        /// <summary>矿山产量增加</summary>
        MINE_INCREASE = 10,

        /// <summary>增加骑射攻击力</summary>
        HORSEMANSHIP_ARCHERY_INCREASE = 11,

        /// <summary>增加几率性任务几率</summary>
        INCREASE_TASK_PROBABILITY = 12,

        /// <summary>增加统帅</summary>
        INCREASE_CAPTAIN = 13,

        /// <summary>增加训练度</summary>
        INCREASE_TRAIN_PROGRESS = 14,

        /// <summary>增加士气</summary>
        INCREASE_MORALE = 15,

        /// <summary>征兵数量 </summary>
        INCREASE_CONSCRIPTION_NUMBER = 16,

        /// <summary>增加武力</summary>
        INCREASE_FORCE = 17,

        /// <summary>增加政务</summary>
        INCREASE_GOVERN = 18,

        /// <summary>增加智谋</summary>
        INCREASE_BRAINS = 19,

        /// <summary>讲价成功率</summary>
        BARGAIN_SUCCESS_RATE = 20,

        /// <summary>减少讲价后买入价格</summary>
        REDUCE_BARGAIN_BUY_PRICE = 21,

        /// <summary>增加讲价后卖出价格</summary>
        INCREASE_BARGAIN_SELL_PRICE = 22,

        /// <summary>增加魅力</summary>
        INCREASE_CHARM = 23,

        /// <summary>每次茶席获得魂 </summary>
        TEA_SPIRIT= 24,
    }
}
