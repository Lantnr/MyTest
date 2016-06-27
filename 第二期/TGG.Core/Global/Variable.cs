using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewLife;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Fight;
using TGG.Core.XML;

namespace TGG.Core.Global
{
    /// <summary>
    /// Class Variable.
    /// 全局变量
    /// </summary>
    public partial class Variable
    {
        public static ConcurrentDictionary<string, bool> CCI=new ConcurrentDictionary<string, bool>();

        /// <summary>加载模块集合</summary>
        public static List<XmlModule> LMM = new List<XmlModule>();
        /// <summary>加载队列模块集合</summary>
        public static List<XML.XmlModule> LMMQ = new List<XML.XmlModule>();

        /// <summary>在线玩家</summary>
        //public static Hashtable OnlinePlayer = new Hashtable();
        public static ConcurrentDictionary<Int64, dynamic> OnlinePlayer = new ConcurrentDictionary<Int64, dynamic>();

        /// <summary>货物刷新时间</summary>
        public static Int64 GRT = 0;
        /// <summary>货物预热时间</summary>
        public static Int64 GRWT = 0;
        /// <summary>货物预热时间 单位:秒</summary>
        public static Int32 GRWTS = 10;

        /// <summary>体力buff第一次更新时间</summary>
        public static DateTime PowerBuffTime1 = new DateTime();

        /// <summary>体力buff第二次更新时间</summary>
        public static DateTime PowerBuffTime2 = new DateTime();

        /// <summary>防沉迷检测时间单位:分钟</summary>
        public static Int32 FCM = 1;


        /// <summary>竞技场每日可挑战次数重置时间</summary>
        public static DateTime ArenaRefreshTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy MM dd") + " 00:00:00");

        /// <summary>每日公告记录时间</summary>
        public static DateTime NOTICETIME = DateTime.Parse(DateTime.Now.ToString("yyyy MM dd") + " 00:00:00");

        /// <summary>竞技场每日排名奖励时间</summary>
        public static DateTime ArenaRewardTime = new DateTime();// DateTime.Parse(DateTime.Now.ToString("yyyy MM dd") + " 21:00:00");

        /// <summary>武将招募刷新时间</summary>
        public static DateTime RoleRecruitRefreshTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy MM dd") + " 00:00:00");

        /// <summary>拉取公告时间</summary>
        public static Int64 GetNoticeTime = DateTime.Now.Ticks;

        /// <summary>全局货物</summary>
        public static List<Entity.GlobalGoods> GOODS = new List<Entity.GlobalGoods>();
        /// <summary>全局玩家场景</summary>
        public static ConcurrentDictionary<string, Entity.view_scene_user> SCENCE = new ConcurrentDictionary<string, Entity.view_scene_user>();

        /// <summary>全局公告</summary>
        public static List<tg_system_notice> NOTICE = new List<Entity.tg_system_notice>();

        /// <summary> 家臣任务线程 </summary>
        public static ConcurrentDictionary<Int64, bool> RoleTask = new ConcurrentDictionary<long, bool>();

        /// <summary> 玩家入狱线程 </summary>
        public static ConcurrentDictionary<Int64, bool> PrisonTask = new ConcurrentDictionary<long, bool>();

        /// <summary> 活动全局 </summary>
        public static ActivityItem Activity = new ActivityItem();

        public static ConcurrentDictionary<Int64, List<tg_bag>> TempProp = new ConcurrentDictionary<long, List<tg_bag>>();

        /// <summary>场景定时时间</summary>
        public static DateTime SceneTimer = DateTime.Now;
        public static double SceneTimerSpan = 1;
        /// <summary>按小时定时执行计划</summary>
        public static DateTime LoginTimeCheck = DateTime.Now;
        public static double TimerExecHourSpan = 1;
        /// <summary>任务定时时间</summary>
        public static DateTime TaskTimer = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy MM dd") + " 00:00:00");
        //public static DateTime TaskTimer = DateTime.Parse(DateTime.Now.ToString("yyyy MM dd") + " 00:00:00"); //测试数据
        public static DateTime UserExtendRefreshTimer = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy MM dd") + " 00:00:00");

        public static double TaskTimerSpan = 24;
        public static double UserExtendTimerSpan = 24;   //时间间隔

        /// <summary>固定刷新时间</summary>
        public static Int64 FT = 0;
        /// <summary>固定刷新时间间隔</summary>
        public const double FTS = 24;//小时

        /// <summary>定时刷新体力时间</summary>
        public static Int64 TPR = 0;

        /// <summary>定时刷新体力时间间隔</summary>
        public const double TPRS = 30;//分钟

        /// <summary>玩家战斗状态</summary>
        public static ConcurrentDictionary<Int64, int> PlayerFight = new ConcurrentDictionary<long, int>();

        /// <summary>多线程模块线程状态</summary>
        public static ConcurrentDictionary<XmlModule, bool> CDTSTM = new ConcurrentDictionary<XmlModule, bool>();

        public static List<UserTaskInfo> TaskInfo = new List<UserTaskInfo>();

        public static List<UserTaskInfo> WorkInfo = new List<UserTaskInfo>();

        /// <summary>玩家小游戏信息</summary>
        public static List<UserGameInfo> GameInfo = new List<UserGameInfo>();



        #region

        /// <summary>全局金钱上限</summary>
        public static Int64 MAX_COIN = 0;
        /// <summary>全局魂上限</summary>
        public static Int32 MAX_SPIRIT = 0;
        /// <summary>全局声望上限</summary>
        public static Int32 MAX_FAME = 0;
        /// <summary>全局功勋上限</summary>
        public static Int32 MAX_HONOR = 0;
        /// <summary>全局金钱上限</summary>
        public static Int32 MAX_GOLD = 100000000;
        #endregion


        /// <summary>多线程模块线程状态</summary>
        public static ConcurrentDictionary<string, bool> CD = new ConcurrentDictionary<string, bool>();

        public class UserTaskInfo
        {
            public UserTaskInfo(Int64 user_id)
            {
                userid = user_id;
            }

            public UserTaskInfo()
            {

            }
            /// <summary>用户id</summary>
            public Int64 userid { get; set; }

            /// <summary> 采集货物失败次数 </summary>
            public int SearchFailTimes { get; set; }

            /// <summary> 守护npcid </summary>
            public Int64 WatchNpcid { get; set; }

            /// <summary> 刺杀npcid </summary>
            public Int64 KillNpcid { get; set; }

            /// <summary> 守护到达时间 </summary>
            public Int64 time { get; set; }

            /// <summary> 与刺杀者战斗的状态 </summary>
            public Int64 WatchState { get; set; }

            /// <summary> 与守护者战斗的状态 </summary>
            public Int64 KillState { get; set; }

            /// <summary> 连续战斗剩余血量 </summary>
            public Int64 RoleHp { get; set; }

            /// <summary> 守护玩家查看的战斗vo </summary>
            public FightVo WatchFightVo { get; set; }

            /// <summary>布谣失败次数</summary>
            public int RumorFailCount { get; set; }

            /// <summary>纵火失败次数</summary>
            public int FireFailCount { get; set; }

            /// <summary>破坏失败次数</summary>
            public int BreakFailCount { get; set; }

            /// <summary>售卖失败次数</summary>
            public int SellFailCount { get; set; }

            /// <summary>戒严谣言场景id</summary>
            public Int64 ArrestRumorSceneId { get; set; }

            /// <summary>戒严纵火场景id</summary>
            public Int64 ArrestFireSceneId { get; set; }

            /// <summary>戒严破坏场景id</summary>
            public Int64 ArrestBreakSceneId { get; set; }

            /// <summary>戒严售卖场景id</summary>
            public Int64 ArrestSellSceneId { get; set; }

            /// <summary>站岗场景id</summary>
            public Int64 GuardSceneId { get; set; }

            /// <summary>站岗人的阵营</summary>
            public Int32 GuardCamp { get; set; }

            /// <summary>工作冷却时间</summary>
            public Int64 CoolingTime { get; set; }

            /// <summary>工作完成时间</summary>
            public Int64 LimitTime { get; set; }

            /// <summary>守护状态</summary>
            public int P_State { get; set; }


        }

        public class UserGameInfo
        {
            /// <summary>用户id</summary>
            public Int64 userid { get; set; }

            /// <summary>游戏类型</summary>
            public int type { get; set; }
        }

    }
}
