using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 生活技能
    /// </summary>
    public class RoleLifeSkill : BaseEntity
    {
        /// <summary>编号</summary>
        public Int64 id { get; set; }

        /// <summary>茶道基表编号</summary>
        public Int32 sub_tea { get; set; }

        /// <summary>算数基表编号</summary>
        public Int32 sub_calculate { get; set; }

        /// <summary>建筑基表编号</summary>
        public Int32 sub_build { get; set; }

        /// <summary>辩才基表编号</summary>
        public Int32 sub_eloquence { get; set; }

        /// <summary>马术基表编号</summary>
        public Int32 sub_equestrian { get; set; }

        /// <summary>开垦基表编号</summary>
        public Int32 sub_reclaimed { get; set; }

        /// <summary>足轻基表编号</summary>
        public Int32 sub_ashigaru { get; set; }

        /// <summary>铁炮基表编号</summary>
        public Int32 sub_artillery { get; set; }

        /// <summary>矿山基表编号</summary>
        public Int32 sub_mine { get; set; }

        /// <summary>艺术基表编号</summary>
        public Int32 sub_craft { get; set; }

        /// <summary>弓术基表编号</summary>
        public Int32 sub_archer { get; set; }

        /// <summary>礼法基表编号</summary>
        public Int32 sub_etiquette { get; set; }

        /// <summary>武艺基表编号</summary>
        public Int32 sub_martial { get; set; }

        /// <summary>军学基表编号</summary>
        public Int32 sub_tactical { get; set; }

        /// <summary>医术基表编号</summary>
        public Int32 sub_medical { get; set; }

        /// <summary>忍术基表编号</summary>
        public Int32 sub_ninjitsu { get; set; }

        /// <summary>茶道等级</summary>
        public Int32 sub_tea_level { get; set; }

        /// <summary>算数等级</summary>
        public Int32 sub_calculate_level { get; set; }

        /// <summary>建筑等级</summary>
        public Int32 sub_build_level { get; set; }

        /// <summary>辩才等级</summary>
        public Int32 sub_eloquence_level { get; set; }

        /// <summary>马术等级</summary>
        public Int32 sub_equestrian_level { get; set; }

        /// <summary>开垦等级</summary>
        public Int32 sub_reclaimed_level { get; set; }

        /// <summary>足轻等级</summary>
        public Int32 sub_ashigaru_level { get; set; }

        /// <summary>铁炮等级</summary>
        public Int32 sub_artillery_level { get; set; }

        /// <summary>矿山等级</summary>
        public Int32 sub_mine_level { get; set; }

        /// <summary>艺术等级</summary>
        public Int32 sub_craft_level { get; set; }

        /// <summary>弓术等级</summary>
        public Int32 sub_archer_level { get; set; }

        /// <summary>礼法等级</summary>
        public Int32 sub_etiquette_level { get; set; }

        /// <summary>武艺等级</summary>
        public Int32 sub_martial_level { get; set; }

        /// <summary>军学等级</summary>
        public Int32 sub_tactical_level { get; set; }

        /// <summary>医术等级</summary>
        public Int32 sub_medical_level { get; set; }

        /// <summary>忍术等级</summary>
        public Int32 sub_ninjitsu_level { get; set; }
    }
}
