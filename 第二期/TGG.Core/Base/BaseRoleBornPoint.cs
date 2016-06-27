using System;

namespace TGG.Core.Base
{
    /// <summary>
    /// 角色出生点基表
    /// </summary>
    //[Serializable]
    public class BaseRoleBornPoint : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseRoleBornPoint CloneEntity()
        {
            return Clone() as BaseRoleBornPoint;
        }

        #endregion

        /// <summary>id</summary>
        public int id { get; set; }

        /// <summary>场景id</summary>
        public int sceneId { get; set; }

        /// <summary>坐标</summary>
        public string coorPoint { get; set; }

    }
}
