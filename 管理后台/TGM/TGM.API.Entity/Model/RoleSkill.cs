using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 武将技能
    /// </summary>
    public class RoleSkill : BaseEntity
    {
        /// <summary>生活技能</summary>
        public RoleLifeSkill LifeSkill { get; set; }

        /// <summary>战斗技能</summary>
        public List<RoleFightSkill> FightSkill { get; set; }
    }
}
