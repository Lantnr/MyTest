using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    public class BaseFusion : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseFusion CloneEntity()
        {
            return Clone() as BaseFusion;
        }

        #endregion

        /// <summary>道具编号</summary>
        public int id { get; set; }

        /// <summary>品质</summary>
        public int grade { get; set; }

        /// <summary>名字</summary>
        public string name { get; set; }

        /// <summary>出售价</summary>
        public int price { get; set; }

        /// <summary>物品类型</summary>
        public int type { get; set; }

        /// <summary>物品子类型</summary>
        public int typeSub { get; set; }

        /// <summary>大小类型</summary>
        public int sizeType { get; set; }

        /// <summary>使用等级</summary>
        public int useLevel { get; set; }

        /// <summary>可否使用</summary>
        public int useMode { get; set; }

        /// <summary>可否出售</summary>
        public int sell { get; set; }

        /// <summary>可否销毁</summary>
        public int destroy { get; set; }

        /// <summary>可否叠加</summary>
        public int overlay { get; set; }

        /// <summary>效果值</summary>
        public int value { get; set; }

        /// <summary>可叠加数</summary>
        public int overlayNumber { get; set; }

        /// <summary>特性效果ID</summary>
        public int effectId { get; set; }

    }
}
