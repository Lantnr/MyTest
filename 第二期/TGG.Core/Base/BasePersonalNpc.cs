using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 一将讨Npc基表类
    /// </summary>
    //[Serializable]
    public class BasePersonalNpc : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BasePersonalNpc CloneEntity()
        {
            return Clone() as BasePersonalNpc;
        }

        #endregion

        /// <summary>Id</summary>
        public int id { get; set; }

        /// <summary>npc战斗印效果</summary>
        public int yinEffectId { get; set; }

        /// <summary>npc战斗武将集合</summary>
        public string matrix { get; set; }

        /// <summary>掉落装备</summary>
        public string prop { get; set; }

        /// <summary>掉落装备数量</summary>
        public string count { get; set; }
    }
}
