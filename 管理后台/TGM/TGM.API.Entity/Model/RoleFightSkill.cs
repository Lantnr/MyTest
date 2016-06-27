using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 战斗技能
    /// </summary>
    public class RoleFightSkill : BaseEntity
    {
        /// <summary>编号id</summary>
        public Int64 id { get; set; }

        /// <summary>基表id</summary>
        public Int32 baseid { get; set; }

        /// <summary>技能名称</summary>
        public String name { get; set; }

        /// <summary>技能等级</summary>
        public Int32 level { get; set; }

        /// <summary>技能流派</summary>
        public Int32 genre { get; set; }

        /// <summary>流派名称</summary>
        public String genreName { get; set; }
    }
}
