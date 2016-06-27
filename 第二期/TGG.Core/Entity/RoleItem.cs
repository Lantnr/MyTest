using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 武将类
    /// </summary>
    [Serializable]
    public class RoleItem : ICloneable
    {
        public RoleItem()
        {
            Kind = new tg_role();
            LifeSkill = new tg_role_life_skill();
            FightSkill = new List<tg_role_fight_skill>();
        }

        /// <summary>武将基本属性</summary>
        public tg_role Kind { get; set; }

        /// <summary>武将生活技能</summary>
        public tg_role_life_skill LifeSkill { get; set; }

        /// <summary>武将个人技能</summary>
        public List<tg_role_fight_skill> FightSkill { get; set; }

        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public RoleItem CloneEntity()
        {
            return Clone() as RoleItem;
        }

        #endregion

    }
}
