using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 玩家武将
    /// </summary>
    public class PlayerRoles : BaseEntity
    {
        #region 属性

        /// <summary>编号</summary>
        public Int64 id { get; set; }

        /// <summary>基表id</summary>
        public Int32 roleid { get; set; }

        /// <summary>武将名称</summary>
        public String name { get; set; }

        /// <summary>武将状态  1：为主角</summary>
        public Int32 state { get; set; }

        /// <summary>等级</summary>
        public Int32 level { get; set; }

        /// <summary>身份</summary>
        public String identity { get; set; }

        /// <summary>品质</summary>
        public String quality { get; set; }

        /// <summary>合战状态</summary>
        public String war_status { get; set; }

        /// <summary>统帅（总值）</summary>
        public Double captain { get; set; }

        /// <summary>武力(总值)</summary>
        public Double force { get; set; }

        /// <summary>智谋(总值)</summary>
        public Double brains { get; set; }

        /// <summary>政务(总值)</summary>
        public Double govern { get; set; }

        /// <summary>魅力(总值)</summary>
        public Double charm { get; set; }

        /// <summary>武器(对应装备表主键编号)</summary>
        public Int64 equip_weapon { get; set; }

        /// <summary>南蛮物(对应装备表主键编号)</summary>
        public Int64 equip_barbarian { get; set; }

        /// <summary>坐骑(对应装备表主键编号)</summary>
        public Int64 equip_mounts { get; set; }

        /// <summary>铠甲(对应装备表主键编号)</summary>
        public Int64 equip_armor { get; set; }

        /// <summary>宝石(对应装备表主键编号)</summary>
        public Int64 equip_gem { get; set; }

        /// <summary>茶器(对应装备表主键编号)</summary>
        public Int64 equip_tea { get; set; }

        /// <summary>艺术品(对应装备表主键编号)</summary>
        public Int64 equip_craft { get; set; }

        /// <summary>书籍(对应装备表主键编号)</summary>
        public Int64 equip_book { get; set; }

        /// <summary>所在据点名称</summary>
        public String cityname { get; set; }

        #endregion
    }
}
