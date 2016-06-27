using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 服务器玩家信息
    /// </summary>
    public class ServerPlayer : BaseEntity
    {
        /// <summary>编号id</summary>
        public Int64 id { get; set; }

        /// <summary>账号</summary>
        public String code { get; set; }

        /// <summary>玩家昵称</summary>
        public String name { get; set; }
        
        /// <summary>等级</summary>
        public Int32 level { get; set; }

        /// <summary>VIP等级</summary>
        public Int32 vip { get; set; }

        /// <summary>身份</summary>
        public String identity { get; set; }

        /// <summary>金钱</summary>
        public Int64 coin { get; set; }

        /// <summary>元宝</summary>
        public Int64 gold { get; set; }

        /// <summary>累计充值金额</summary>
        public Int32 vip_gold { get; set; }

        /// <summary>最近一次登录时间</summary>
        public String login_time { get; set; }
    }
}
