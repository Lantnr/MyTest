using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 玩家详细信息
    /// </summary>
    public class PlayerDetailed : BaseEntity
    {
        public PlayerDetailed()
        {
            Areas = new List<String>();
            Roles = new List<PlayerRoles>();
            Bags = new List<PlayerBag>();
            Citys = new List<PlayerCity>();
        }

        #region

        /// <summary>编号</summary>
        public Int64 id { get; set; }

        /// <summary>服务器主键编号</summary>
        public Int32 sid { get; set; }

        /// <summary>主角武将主键id</summary>
        public Int64 rid { get; set; }

        /// <summary>账号</summary>
        public String code { get; set; }

        /// <summary>名称</summary>
        public String name { get; set; }

        /// <summary>职业</summary>
        public String vocation { get; set; }

        /// <summary>是否在线</summary>
        public String login_state { get; set; }

        /// <summary>VIP等级</summary>
        public Int32 vip { get; set; }

        /// <summary>身份</summary>
        public String identity { get; set; }

        /// <summary>等级</summary>
        public Int32 level { get; set; }

        /// <summary>官职</summary>
        public String office { get; set; }

        /// <summary>元宝</summary>
        public Int32 gold { get; set; }

        /// <summary>金钱</summary>
        public Int64 coin { get; set; }

        /// <summary>魂</summary>
        public Int32 spirit { get; set; }

        /// <summary>声望</summary>
        public Int32 fame { get; set; }

        /// <summary>战功值</summary>
        public Int32 merit { get; set; }

        /// <summary>功勋值</summary>
        public Int32 honor { get; set; }

        /// <summary>马车数量</summary>
        public Int32 cars { get; set; }

        /// <summary>商圈数量</summary>
        public List<String> Areas { get; set; }

        /// <summary>武将集合信息</summary>
        public List<PlayerRoles> Roles { get; set; }

        /// <summary>背包集合信息</summary>
        public List<PlayerBag> Bags { get; set; }

        /// <summary>据点集合信息</summary>
        public List<PlayerCity> Citys { get; set; }

        #endregion
    }
}
