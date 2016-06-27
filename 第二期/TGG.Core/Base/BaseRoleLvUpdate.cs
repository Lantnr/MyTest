using System;

namespace TGG.Core.Base
{
    //[Serializable]
    public class BaseRoleLvUpdate : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseRoleLvUpdate CloneEntity()
        {
            return Clone() as BaseRoleLvUpdate;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }

        /// <summary>升级经验</summary>
        public int exp { get; set; }

        /// <summary>下一等级id</summary>
        public int nextId { get; set; }

        /// <summary>升级增加血量</summary>
        public int AddBlood { get; set; }

          /// <summary>升到该等级消耗总经验</summary>
        public int SumExp { get; set; }
        

    }
}
