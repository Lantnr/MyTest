using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 战斗类
    /// </summary>
    [Serializable]
    public class FightItem : ICloneable
    {
        public FightItem()
        {
            Personal = new tg_fight_personal();
            Type = FightType.ONE_SIDE;
            Rival = 0;
        }
        /// <summary>个人战信息</summary>
        public tg_fight_personal Personal { get; set; }

        /// <summary>战斗类型</summary>
        public FightType Type { get; set; }

        /// <summary>对手user_id </summary>
        public Int64 Rival { get; set; }

        /// <summary>回合数</summary>
        public int BoutCount { get; set; }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public FightItem CloneEntity()
        {
            return Clone() as FightItem;
        }

        #endregion
    }
}
