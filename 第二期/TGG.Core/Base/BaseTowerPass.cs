using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 爬塔基表
    /// </summary>
    //[Serializable]
    public class BaseTowerPass : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTowerPass CloneEntity()
        {
            return Clone() as BaseTowerPass;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>关卡</summary>
        public int pass { get; set; }

        /// <summary>挑战对象</summary>
        public int enemyType { get; set; }

        /// <summary>对象id</summary>
        public int enemyId { get; set; }

        /// <summary>守护者</summary>
        public int watchmen { get; set; }

        /// <summary>守护者声望奖励</summary>
        public int watchmenAward { get; set; }

        /// <summary>过关声望奖励</summary>
        public string passAward { get; set; }

        /// <summary>下一关id</summary>
        public int nextId { get; set; }
    }
}
