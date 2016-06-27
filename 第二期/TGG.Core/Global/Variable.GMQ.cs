using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Global
{
    public partial class Variable
    {
        #region 游戏模块队列

        #region 最新已启用模块
        /// <summary>游戏地图模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_SCENE = new ConcurrentQueue<dynamic>();

        /// <summary>游戏用户模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_USER = new ConcurrentQueue<dynamic>();

        /// <summary>游戏跑商模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_BUSINESS = new ConcurrentQueue<dynamic>();

        /// <summary>游戏道具模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_PROP = new ConcurrentQueue<dynamic>();

        /// <summary>聊天模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_CHAT = new ConcurrentQueue<dynamic>();

        /// <summary>邮箱模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_MESSAGES = new ConcurrentQueue<dynamic>();

        /// <summary>游戏武将模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_ROLE = new ConcurrentQueue<dynamic>();

        /// <summary>战斗模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_FIGHT = new ConcurrentQueue<dynamic>();

        /// <summary>游戏任务模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_TASK = new ConcurrentQueue<dynamic>();

        /// <summary>游戏装备模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_EQUIP = new ConcurrentQueue<dynamic>();

        /// <summary>游戏技能模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_SKILL = new ConcurrentQueue<dynamic>();

        /// <summary>武将修行模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_ROLETRAIN = new ConcurrentQueue<dynamic>();

        /// <summary>游戏副本模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_DUPLICATE = new ConcurrentQueue<dynamic>();

        /// <summary>游戏家族模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_FAMILY = new ConcurrentQueue<dynamic>();

        /// <summary>游戏公告模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_NOTICE = new ConcurrentQueue<dynamic>();

        /// <summary>游戏好友模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_FRIEND = new ConcurrentQueue<dynamic>();

        /// <summary>游戏家臣评定模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_APPRAISE = new ConcurrentQueue<dynamic>();

        /// <summary>游戏一将讨模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_SINGLEFIGHT = new ConcurrentQueue<dynamic>();

        /// <summary>游戏竞技场模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_ARENA = new ConcurrentQueue<dynamic>();

        /// <summary>游戏称号模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_TITLE= new ConcurrentQueue<dynamic>();

        /// <summary>游戏监狱模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_PRISON = new ConcurrentQueue<dynamic>();

        /// <summary>游戏美浓攻略系统(攻城)模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_SIEGE = new ConcurrentQueue<dynamic>();

        /// <summary>游戏一夜墨俣系统(筑城)模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_BUILDING = new ConcurrentQueue<dynamic>();

        /// <summary>工作系统模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_WORK = new ConcurrentQueue<dynamic>();

        /// <summary>大命令模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_GUIDE = new ConcurrentQueue<dynamic>();

        /// <summary>游戏集合模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_GAMES = new ConcurrentQueue<dynamic>();


        /// <summary>API模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_API = new ConcurrentQueue<dynamic>();

        

        #endregion



        /// <summary>游戏武将排名模块队列</summary>
        public static ConcurrentQueue<dynamic> GMQ_RANKINGS = new ConcurrentQueue<dynamic>();
       

        #endregion
    }
}
