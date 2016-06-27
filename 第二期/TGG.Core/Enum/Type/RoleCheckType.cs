using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Enum.Type
{
    /// <summary>
    /// 武将属性上限枚举
    /// </summary>
    public enum RoleCheckType
    {
        /// <summary> 生活技能加点数</summary>
        LifeSkill = 1,

        /// <summary>职业初始点</summary>
        Vocation = 2,

        /// <summary>锻炼加点数</summary>
        Train = 3,

        /// <summary>升级加点数</summary>
        Level = 4,

        /// <summary>注魂附加点数</summary>
        Spirit = 5,

        /// <summary>装备附加点数</summary>
        Equip = 6,

        /// <summary>称号附加点数</summary>
        Title = 7,

    }
}
