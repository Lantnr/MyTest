using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 家族职位表
    /// </summary>
    //[Serializable]
    public class BaseFamilyPost : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseFamilyPost CloneEntity()
        {
            return Clone() as BaseFamilyPost;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>职位</summary>
        public int post { get; set; }

        /// <summary>解散家族</summary>
        public int dissolve { get; set; }

        /// <summary>更改职位</summary>
        public int setPost { get; set; }

        /// <summary>修改公告</summary>
        public int notice { get; set; }

        /// <summary>邀请玩家</summary>
        public int invite { get; set; }

        /// <summary>踢出人员</summary>
        public int expel { get; set; }

        /// <summary>升级建筑</summary>
        public int upgrade { get; set; }

        /// <summary>接受申请</summary>
        public int adopt { get; set; }

        /// <summary>修改家徽</summary>
        public int setBadge { get; set; }
    }
}
