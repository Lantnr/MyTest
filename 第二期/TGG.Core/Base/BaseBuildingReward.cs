using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    //[Serializable]
    public class BaseBuildingReward : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseBuildingReward CloneEntity()
        {
            return Clone() as BaseBuildingReward;
        }

        #endregion

       /// <summary> id </summary>
       public int id { get; set; }

       /// <summary> 胜负类型：1,胜利 2.失败 3.平局 </summary>
       public int type { get; set; }

       /// <summary> 玩家声望区间（0:0-9） </summary>
       public int fame { get; set; }

       /// <summary> 获得金钱 </summary>
       public int money { get; set; }

       /// <summary> 获得声望 </summary>
       public int fameReward { get; set; }

    }
}
