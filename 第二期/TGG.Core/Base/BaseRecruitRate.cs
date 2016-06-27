using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 酒馆几率基表
    /// </summary>
    public class BaseRecruitRate : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseRecruitRate CloneEntity()
        {
            return Clone() as BaseRecruitRate;
        }

        #endregion	

        /// <summary>id</summary>
        public int id { get; set; }
         /// <summary>身份ID</summary>
        public int identity { get; set; }
         /// <summary>家臣品质</summary>
        public int grade { get; set; }
         /// <summary>概率</summary>
        public int rate { get; set; }
        /// <summary>招募消耗(单位文，游戏中要转成贯为单位)</summary>
        //public int cost { get; set; }

    }
}
