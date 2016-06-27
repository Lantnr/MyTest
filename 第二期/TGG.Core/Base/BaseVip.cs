using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// VIP基表
    /// </summary>
    //[Serializable]
    public class BaseVip : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseVip CloneEntity()
        {
            return Clone() as BaseVip;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>VIP等级</summary>
        public int level { get; set; }

        /// <summary>充值元宝</summary>
        public int gold { get; set; }

        /// <summary>购买体力</summary>
        public int power { get; set; }

        /// <summary>可开启商圈</summary>
        public int area { get; set; }

        /// <summary>增加跑商队列</summary>
        public int car { get; set; }

        /// <summary>跳过战斗</summary>
        public int fight { get; set; }

        /// <summary>恢复讲价次数</summary>
        public int bargain { get; set; }

        /// <summary>恢复购买量</summary>
        public int buy { get; set; }

        /// <summary>竞技场挑战次数购买</summary>
        public int arenaBuy { get; set; }
        /// <summary>竞技场冷却CD免费消除次数</summary>
        public int arenaCD { get; set; }

        /// <summary>武将修行栏个数</summary>
        public int trainBar { get; set; }

        /// <summary>武将宅刷新次数</summary>
        public int trainHome { get; set; }

        /// <summary>免税</summary>
        public int freeTax { get; set; }

    }
}
