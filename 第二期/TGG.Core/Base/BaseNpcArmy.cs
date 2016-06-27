using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// NPC战斗部队基表
    /// </summary>
    //[Serializable]
    public class BaseNpcArmy : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseNpcArmy CloneEntity()
        {
            return Clone() as BaseNpcArmy;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary> 等级 </summary>
        public int level { get; set; }

        /// <summary>npc id</summary>
        public int npcId { get; set; }

        /// <summary>npc战斗印效果</summary>
        public int yinEffectId { get; set; }

        /// <summary>npc战斗武将集合</summary>
        public string matrix { get; set; }

        /// <summary> 类型 </summary>
        public int type { get; set; }
    }
}
