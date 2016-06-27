using System;
namespace TGG.Core.Entity
{
    /// <summary>
    /// 玩家模块
    /// </summary>
    [Serializable]
    public class ModuleCommand : ICloneable
    {
        /// <summary>模块号 </summary>
        public int moduleNumber { get; set; }

        /// <summary>命令号</summary>
        public int commandNumber { get; set; }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public ModuleCommand CloneEntity()
        {
            return Clone() as ModuleCommand;
        }

        #endregion
    }
}
