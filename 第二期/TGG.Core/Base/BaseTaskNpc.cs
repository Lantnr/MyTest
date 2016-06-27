using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 谣言戒严npc表
    /// </summary>
    //[Serializable]
    public class BaseTaskNpc : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTaskNpc CloneEntity()
        {
            return Clone() as BaseTaskNpc;
        }

        #endregion

        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 阵营
        /// </summary>
        public int camp { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public int taskType { get; set; }

        /// <summary>
        /// 任务子类型
        /// </summary>
        public int subType { get; set; }

        /// <summary>
        /// npc
        /// </summary>
        public string npc { get; set; }
    }
}
