using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluorineFx;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.Module.Consume;
using TGG.Module.War.Service;
using TGG.Share.Event;
using TGG.SocketServer;

namespace TGGUnitTest
{
    [TestClass]
    public class UnitTest_LDY
    {
        [TestMethod]
        public void TestMethod1()
        {
            #region 初始session

            var session = new TGG.SocketServer.TGGSession
            {
                Player = new Player
                {
                    User = new tg_user(),
                },
                Fight = new FightItem(),
                TaskItems = new List<TaskItem>(),
                LastActiveTime = new DateTime(),
                SPM = new ConcurrentDictionary<string, ASObject>(),
            };
            var user = tg_user.FindByid(13);
            session.Player.User = user;
            session.Player.Bag.Surplus = 3;
            TGG.Core.XML.Util.Init(); //加载基表数据
            // Variable.OnlinePlayer.AddOrUpdate(user.id, session, (k, v) => session);
            //模拟登陆指令
            // TGG.Module.Scene.Service.LOGIN_ENTER_SCENE..CommandStart(session, null);

            #endregion

            //调用共享模块
            // new TGG.Share.Reward().GetReward("7_6010007_1", 1);
            #region 调用系统指令
            //GetFightSet(session);
            //GetPlanSave(session);
            ////拉取方案数据
            //GetWarPlan(session);
            ////添加防守武将
            //GetRoleAdd(session);
            //移除防守武将
            // GetRoleRemove(session);
            //解锁新的防守方案
            // GetNewPlanOpen(session);

            //战报拉取
            // GetWarReport(session);
            var dic = new Dictionary<string, object> { { "id", 10 }, { "location", 1 } };
            var aso = new ASObject(dic);
            var att = new TGG.Module.Building.Service.ADD_ATTACK();
            session.Player.UserExtend = new tg_user_extend();
            session.Player.UserExtend.building_add_pro = 0;
            session.Player.UserExtend.building_add_pro = 0;
            att.CommandStart(session, aso);

#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif
            //GetFightTest(session);
            GetLineAdd(session);

#if DEBUG
            sw.Stop();
            TimeSpan timespan = sw.Elapsed;
            DisplayGlobal.log.Write(timespan.ToString());
            XTrace.WriteLine("指令 {1} 运行总耗时：{0} 毫秒", timespan.ToString(), GetType().Namespace);
#endif
            #endregion



        }

        private void GetLineAdd(TGGSession session)
        {
            var line = "17_4|" + "16_4|" + "15_4|" + "14_4|" + "13_4|" + "12_4|" + "11_4|" + "10_4|" + "9_4|" + "8_4|" +
                       "7_4|" + "6_4|" + "5_4|" + "4_4|" + "3_4|" + "2_4|" + "1_4|"
                       + "0_4|";
            var dic = new Dictionary<string, object> { { "roles", 23 }, { "line", line }, { "type", 1 } };
            var aso = new ASObject(dic);
            // TGG.Module.War.Service.WAR_FIGHT_LINE_ADD.GetInstance().CommandStart(session, aso);
            var dic1 = new Dictionary<string, object> { { "frontId", 23 }, { "type", 1 } };
            aso = new ASObject(dic1);
            // TGG.Module.War.Service.WAR_FIGHT.GetInstance().CommandStart(session, aso);

        }

        /// <summary>
        /// 拉取城市防守计划
        /// </summary>
        /// <param name="session"></param>
        private void GetWarPlan(TGGSession session)
        {
            var dic = new Dictionary<string, object> { { "id", 1000053 } };
            var aso = new ASObject(dic);
            // TGG.Module.War.Service.Defence.WAR_DEFENCE_PLAN.GetInstance().CommandStart(session, aso);
        }
        /// <summary>
        /// 拉取城市防守计划
        /// </summary>
        /// <param name="session"></param>
        private void GetWarReport(TGGSession session)
        {
            var dic = new Dictionary<string, object> { };
            var aso = new ASObject(dic);
            // TGG.Module.War.Service.WAR_REPORT_JOIN.GetInstance().CommandStart(session, aso);
        }

        /// <summary>
        /// 添加防守武将
        /// </summary>
        /// <param name="session"></param>
        private void GetRoleAdd(TGGSession session)
        {
            session.Player.War.PlayerInCityId = 1000053;

            var dic = new Dictionary<string, object> { { "id", 10 }, { "location", 1 } };
            var aso = new ASObject(dic);
            //TGG.Module.War.Service.Defence.WAR_DEFENCE_ROLE_ADD.GetInstance().CommandStart(session, aso);
        }

        /// <summary>
        /// 移除防守武将
        /// </summary>
        /// <param name="session"></param>
        private void GetRoleRemove(TGGSession session)
        {
            session.Player.War.PlayerInCityId = 1000053;

            var dic = new Dictionary<string, object> { { "id", 10 } };
            var aso = new ASObject(dic);
            // TGG.Module.War.Service.Defence.WAR_DEFENCE_ROLE_REMOVE.GetInstance().CommandStart(session, aso);
        }

        /// <summary>
        /// 解锁新的防守方案
        /// </summary>
        /// <param name="session"></param>
        private void GetNewPlanOpen(TGGSession session)
        {
            session.Player.War.PlayerInCityId = 1000053;
            session.Player.User.coin = 1000000;
            var dic = new Dictionary<string, object> { };
            var aso = new ASObject(dic);
            // TGG.Module.War.Service.Defence.WAR_DEFENCE_PLAN_OPEN.GetInstance().CommandStart(session, aso);


        }

        /// <summary>
        /// 拉取防守数据
        /// </summary>
        /// <param name="session"></param>
        private void GetFightSet(TGGSession session)
        {
            var dic = new Dictionary<string, object> { { "cityId", 1000050 } };
            var aso = new ASObject(dic);
            //  TGG.Module.War.Service.Attack.WAR_FIGHT_SET.GetInstance().CommandStart(session, aso);


        }


        private void GetPlanSave(TGGSession session)
        {
            session.Player.War.PlayerInCityId = 1000053;
            var vo = new WarDefencePlanVo()
            {
                id = 1,
                roles = null,
                listarea = null,

            };
            session.Player.User.coin = 1000000;
            var dic = new Dictionary<string, object>
            {
                   { "warDefencePlanVo" ,vo}
            };
            var aso = new ASObject(dic);
            // TGG.Module.War.Service.Defence.WAR_DEFENCE_PLAN_UPDATE.GetInstance().CommandStart(session, aso);

        }

        /// <summary>
        /// 战斗过程测试
        /// </summary>
        /// <param name="session"></param>
        //private void GetFightTest(TGGSession session)
        //{

        //    var roles = new List<WarRolesLinesVo>       
        //    { 
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 17,},
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 16,},
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 15,},
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 14,},
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 13,},
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 12,},
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 11,},
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 10,},
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 9, },
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 8, },
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 7, },
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 6, },
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 5, },
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 4, },
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 3, },
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 2, },
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 1, },
        //          new WarRolesLinesVo() { rid = 28,y= 0, x = 0, },
        //          new WarRolesLinesVo() { rid = 28,y= 1, x = 0, },
        //          new WarRolesLinesVo() { rid = 28,y= 2, x = 0, },
        //          new WarRolesLinesVo() { rid = 28,y= 3, x = 0, },
        //          new WarRolesLinesVo() { rid = 28,y= 4, x = 0, },

        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 17,},
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 16,},
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 15,},
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 14,},
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 13,},
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 12,},
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 11,},
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 10,},
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 9, },
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 8, },
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 7, },
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 6, },
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 5, },
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 4, },
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 3, },
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 2, },
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 1, },
        //          new WarRolesLinesVo() { rid = 40,y= 8, x = 0, },
        //          new WarRolesLinesVo() { rid = 40,y= 7, x = 0, },
        //          new WarRolesLinesVo() { rid = 40,y= 6, x = 0, },
        //          new WarRolesLinesVo() { rid = 40,y= 5, x = 0, },
        //          new WarRolesLinesVo() { rid = 40,y= 4, x = 0, },

        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 17,},
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 16,},
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 15,},
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 14,},
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 13,},
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 12,},
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 11,},
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 10,},
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 9, },
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 8, },
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 7, },
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 6, },
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 5, },
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 4, },
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 3, },
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 2, },
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 1, },
        //          new WarRolesLinesVo() { rid = 41,y= 7, x = 0, },
        //          new WarRolesLinesVo() { rid = 41,y= 6, x = 0, },
        //          new WarRolesLinesVo() { rid = 41,y= 5, x = 0, },
        //          new WarRolesLinesVo() { rid = 41,y= 4, x = 0, },
        //          new WarRolesLinesVo() { rid = 41,y= 4, x = 0, },

        //    };
        //    var key = string.Format("{0}_{1}_{2}", 1000058, 7, 9);
        //    Int64 planid = 6;
        //    //Variable.FightPlan.TryAdd(key, planid);
        //    var dic = new Dictionary<string, object>
        //    {
        //           {"roles" ,roles.Select(set => AMFConvert.ToASObject(set)).ToList()},
        //           {"city",1000058} ,
        //           {"front",10001}
        //    };
        //    var aso = new ASObject(dic);

        //    new TGG.Module.War.Service.WAR_FIGHT().CommandStart(session, aso);
        //}

        /// <summary>
        /// 装备购买
        /// </summary>
        //public ASObject EquipBuy(TGGSession session)
        //{
        //    var dic = new Dictionary<string, object> { { "id", 6010001 }, { "count", 1 } };
        //    var aso = new ASObject(dic);

        //    TGG.Module.Task.Service.TASK_PUSH.getInstance().VocationTaskUpdate();
        //    return TGG.Module.Equip.Service.EQUIP_BUY.GetInstance().CommandStart(session, aso);
        //}

        public ASObject TASK_VOCATION_REFRESH(TGGSession session)
        {
            return null;
            //var task = new TASK_VOCATION_REFRESH();
            //var aso = task.CommandStart(session, new ASObject());
            //return aso;
        }
    }
}
