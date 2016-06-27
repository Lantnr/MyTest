using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 魂基表
    /// </summary>
    //[Serializable]
    public class BaseSpirit : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseSpirit CloneEntity()
        {
            return Clone() as BaseSpirit;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>阶级</summary>
        public int lv { get; set; }

        /// <summary>装备使用等级区间</summary>
        public int userLv { get; set; }

        /// <summary>升阶魂数</summary>
        public int spirit { get; set; }

        /// <summary>解锁元宝</summary>
        public int gold { get; set; }

        /// <summary>统率</summary>
        public int captain { get; set; }

        /// <summary>武力</summary>
        public int force { get; set; }

        /// <summary>智谋</summary>
        public int brains { get; set; }

        /// <summary>政务</summary>
        public int govern { get; set; }

        /// <summary>魅力</summary>
        public int charm { get; set; }

        /// <summary>攻击力</summary>
        public int attack { get; set; }

        /// <summary>增伤</summary>
        public Double hurtIncrease { get; set; }

        /// <summary>防御力</summary>
        public int defense { get; set; }

        /// <summary>减伤</summary>
        public Double hurtReduce { get; set; }

        /// <summary>生命值</summary>
        public int life { get; set; }

        /// <summary>升阶id</summary>
        public int updateId { get; set; }

    }
}
