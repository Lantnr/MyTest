using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 玩家信息
    /// </summary>
    [Serializable]
    public class Player : ICloneable
    {

        public Player()
        {
            User = new tg_user();
            //Role = new view_role();
            Role = new RoleItem();
            Scene = new view_scene_user();
            Order = new BusinessOrder();
            Bag = new BagItem();
            Family = new tg_family_member();
            // PrisonPoint = new Prison_point();
            BlackList = new List<long>();
            Position = new Ninjutsu_position();
            moduleIds = new List<int>();
            BusinessArea = new List<tg_user_area>();
            DamingLog = new List<tg_daming_log>();
            Game = new GameItem();
        }

        /// <summary>玩家信息</summary>
        public tg_user User { get; set; }

        /// <summary>玩家自身武将</summary>
        //public view_role Role { get; set; }
        public RoleItem Role { get; set; }

        /// <summary>玩家场景信息</summary>
        public view_scene_user Scene { get; set; }

        /// <summary>跑商货物锁定订单</summary>
        public BusinessOrder Order { get; set; }

        /// <summary>玩家扩展信息</summary>
        public tg_user_extend UserExtend { get; set; }

        /// <summary>背包(谁操作谁进行更新)</summary>
        public BagItem Bag { get; set; }

        /// <summary>家族</summary>
        public tg_family_member Family { get; set; }

        /// <summary>非玩家自身武将集合</summary>
        public List<RoleItem> OtherRole { get; set; }

        //  public Prison_point PrisonPoint { get; set; }

        /// <summary> 黑名单集合 </summary>
        public List<Int64> BlackList { get; set; }

        /// <summary>忍术游戏色子位置</summary>
        public Ninjutsu_position Position { get; set; }

        /// <summary>开放模块</summary>
        public List<int> moduleIds { get; set; }

        /// <summary>玩家所属商圈</summary>
        public List<tg_user_area> BusinessArea { get; set; }

        /// <summary>VIP</summary>
        public tg_user_vip Vip { get; set; }

        /// <summary>大名令</summary>
        public List<tg_daming_log> DamingLog { get; set; }

        /// <summary>在线时间</summary>
        public Int64 onlinetime { get; set; }

        /// <summary>游艺园</summary>
        public GameItem Game { get; set; }

        #region 公共方法

        /// <summary>模块是否开启</summary>
        /// <param name="id">模块开启基表id</param>
        public bool IsOpenModule(int id)
        {
            return moduleIds.Contains(id);
        }

        #endregion


        #region 拷贝

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>浅拷贝</summary>
        public Player CloneEntity()
        {
            return Clone() as Player;
        }

        #endregion
    }
}
