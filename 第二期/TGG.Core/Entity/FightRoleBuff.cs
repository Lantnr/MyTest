using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary> 战斗使用的武将身上Buff实体 </summary>
    public class FightRoleBuff
    {
        /// <summary> 武将主键  </summary>
        public Int64 id { get; set; }

        /// <summary> 到期回合数 </summary>
        public int round { get; set; }

        /// <summary> 技能效果类型 </summary>
        public int type { get; set; }

        /// <summary> 技能状态 0:新 1:旧 </summary>
        public int state { get; set; }

        /// <summary> 技能效果值 </summary>
        public double values { get; set; }
    }
}
