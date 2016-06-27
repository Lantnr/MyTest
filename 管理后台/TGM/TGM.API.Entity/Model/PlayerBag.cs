using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 玩家背包
    /// </summary>
    public class PlayerBag : BaseEntity
    {
        /// <summary>编号</summary>
        public Int64 id { get; set; }

        /// <summary>名称</summary>
        public String name { get; set; }

        /// <summary>物品基表编号</summary>
        public Int32 baseid { get; set; }

        /// <summary>数量</summary>
        public Int32 count { get; set; }

        /// <summary>品质</summary>
        public String quality { get; set; }

        /// <summary>使用等级</summary>
        public Int32 level { get; set; }

        /// <summary>物品类型  7为装备</summary>
        public Int32 type { get; set; }

        /// <summary>装备类型</summary>
        public Int32 equip_type { get; set; }

        /// <summary>装备部位</summary>
        public String position { get; set; }

        /// <summary>属性1类型</summary>
        public Int32 attribute1_type { get; set; }

        /// <summary>属性1名称</summary>
        public String attribute1_name { get; set; }

        /// <summary>属性1值</summary>
        public Double attribute1_value { get; set; }

        /// <summary>属性1注魂值</summary>
        public Double attribute1_value_spirit { get; set; }

        /// <summary>属性2类型</summary>
        public Int32 attribute2_type { get; set; }

        /// <summary>属性2名称</summary>
        public String attribute2_name { get; set; }

        /// <summary>属性2值</summary>
        public Double attribute2_value { get; set; }

        /// <summary>属性2注魂值</summary>
        public Double attribute2_value_spirit { get; set; }

        /// <summary>属性3类型</summary>
        public Int32 attribute3_type { get; set; }

        /// <summary>属性3名称</summary>
        public String attribute3_name { get; set; }

        /// <summary>属性3值</summary>
        public Double attribute3_value { get; set; }

        /// <summary>属性3注魂值</summary>
        public Double attribute3_value_spirit { get; set; }

        /// <summary>备注</summary>   
        public String describe { get; set; }
    }
}
