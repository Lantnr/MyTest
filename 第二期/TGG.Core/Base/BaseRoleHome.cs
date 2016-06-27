using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
    /// <summary>
    /// 武将点将基表
    /// </summary>
    //[Serializable]
    public class BaseRoleHome : ICloneable
    {
        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public BaseRoleHome CloneEntity()
        {
            return Clone() as BaseRoleHome;
        }

        #endregion

        /// <summary>
        /// 将册难度id（固定不变）
        /// </summary>
        public int id { get; set; }

        /// <summary>开放等级</summary>
        public int openLv { get; set; }

        /// <summary>挑战体力</summary>
        public int fightPower { get; set; }

        /// <summary>茶道、偷窃体力</summary>
        public string teaPower { get; set; }

        /// <summary>怪物数量</summary>
        public int count { get; set; }
    }
}
