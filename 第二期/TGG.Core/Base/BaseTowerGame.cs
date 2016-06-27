using System;

namespace TGG.Core.Base
{
    /// <summary>
    /// 爬塔游戏信息表
    /// </summary>
    //[Serializable]
    public class BaseTowerGame : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseTowerGame CloneEntity()
        {
            return Clone() as BaseTowerGame;
        }

        #endregion

        /// <summary>基础id</summary>
        public int id { get; set; }

        /// <summary>游戏类型</summary>
        public int type { get; set; }

        /// <summary>对应关卡</summary>
        public int pass { get; set; }

        /// <summary>电脑初始星数量</summary>
        public int enemyHp { get; set; }

        /// <summary>玩家初始星数量</summary>
        public int myHp { get; set; }

        /// <summary>过关星数量</summary>
        public int standardHp { get; set; }
    }
}
