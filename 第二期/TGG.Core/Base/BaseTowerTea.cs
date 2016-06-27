using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 爬塔茶道游戏基表信息
    /// </summary>
    //[Serializable]
    public class BaseTowerTea : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTowerTea CloneEntity()
        {
            return Clone() as BaseTowerTea;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>玩家图案</summary>
        public int myIcon { get; set; }

        /// <summary>电脑图案</summary>
        public int enemyIcon { get; set; }

        /// <summary>玩家获得分值</summary>
        public int myScore { get; set; }

        /// <summary>电脑获得分值</summary>
        public int enemyScore { get; set; }
    }
}
