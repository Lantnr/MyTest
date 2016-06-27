//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Net.Mime;
//using FluorineFx;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TGG.Core.Common.Util;
//using TGG.Core.Entity;
//using TGG.Module.War.Service.Map;
//using TGG.SocketServer;
//using TGG.Core.Global;
//using NewLife.Log;
//using System.Windows.Forms;
//using System.Net;
//using System.Diagnostics;

//namespace TGGUnitTest
//{
//    /// <summary>
//    /// UnitTest_LGR 的摘要说明
//    /// </summary>
//    [TestClass]
//    public class UnitTest_LGR
//    {

//        [TestMethod]
//        public void TestMethod1()
//        {
//            //
//            // TODO:  在此处添加测试逻辑
//            //
//            TGGSession session = new TGGSession();
//            session.Player = new Player();
//            session.Player.User = new tg_user();
//            session.Fight = new FightItem();
//            session.Fight.Rival = 0;
//            session.Fight.Personal = new tg_fight_personal();
//            session.TaskItems = new List<TaskItem>();
//            session.Player.War = new WarItem();
//            //session.Player.War.Allys = new List<tg_war_partner>();
//            session.SPM = new System.Collections.Concurrent.ConcurrentDictionary<string, ASObject>();
//            string PATH = "C:\\Bin\\TGG\\Debug\\SERVER";

//            TGG.Core.XML.Util.Init();

//            var lists = new List<ASObject>();

//            lists.Add(new ASObject(Dic("roleId", "1", "arms", 1, "armsCount", 1)));
//            lists.Add(new ASObject(Dic("roleId", "2", "arms", 2, "armsCount", 2)));
//            lists.Add(new ASObject(Dic("roleId", "3", "arms", 3, "armsCount", 3)));
//            var aso1 = LOGIN(session, new ASObject(Dic("userName", lists, "server", 0)));
//            aso1 = WAR_MODEL_IN(session, new ASObject(Dic("id", "101")));
//            var list = new List<double>() { 1, 2, 3, 4, 5 };
//            aso1 = WAR_CITY_BUILD(session, new ASObject(Dic("rids", list, "funds", "0", "foods", "0")));

//            aso1 = LOGIN(session, new ASObject(Dic("userName", "lgr1", "server", 0)));
//            aso1 = WAR_MODEL_IN(session, new ASObject(Dic("id", "101")));
//            aso1 = WAR_CITY_BUILD(session, new ASObject(Dic("id", "1000002", "name", "sds")));
//            aso1 = WAR_DIPLOMACY(session, new ASObject(Dic("id", "1000001", "roleid", "141")));
//            //var aso12 = TASK_VOCATION_REFRESH(session);
//            //SYSTEM_NOTICE();
//            //aso1 = ARENA_JOIN(session, new ASObject());

//            //var aso = LOGIN(session, new ASObject(Dic("userName", "asd", "server", 0)));
//            //SYSTEM_NOTICE();
//            //aso = ARENA_DEKARON(session, new ASObject(Dic("id", 113751)));

//            //session = new TGGSession();
//            //session.Player = new Player();
//            //session.Player.User = new tg_user();
//            //session.Fight = new FightItem();
//            //session.Fight.Personal = new tg_fight_personal();
//            ////session.SecureProtocol="127.0.0.1";
//            //aso = LOGIN(session, new ASObject(Dic("userName", "asd", "server", 0)));
//            //aso = PICKUP_PROP(session, new ASObject());
//            //session.Fight.Rival = 113751;

//            //aso = YIN_JION(session, new ASObject());
//            //aso = YIN_UPGRADE(session, new ASObject(Dic("id", 1)));
//            //aso = FIGHT_PERSONAL_ROLE_SELECT(session, new ASObject(Dic("roleId", 1, "location", 1)));
//            //aso = FIGHT_PERSONAL_ROLE_DELETE(session, new ASObject(Dic("roleId", 1, "location", 1)));
//            //aso = FIGHT_PERSONAL_ROLE_CHANGE(session, new ASObject(Dic("roleId", 1, "location", 1, "roleIdNew", 1, "locationNew", 1)));
//            //aso = FIGHT_PERSONAL_YIN_SELECT(session, new ASObject(Dic("id", 1)));
//            // aso = FIGHT_PERSONAL_ENTER(session, new ASObject(Dic("type", 0)));
//        }

//        public ASObject TASK_VOCATION_REFRESH(TGGSession session)
//        {
//            string data = "202|1_200055_1|8_15020034_3_0";
//            var task = new tg_task();
//            task.task_step_data = "202|1_200055_0|8_15020034_0_0";
//            task.user_id = 1;
//            task.task_type = 0;
//            var aso = TGG.Module.Task.Service.Common.getInstance().ContinueFight(session, task, data, 200055);
//            return aso;
//        }

//        public ASObject LOGIN(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            var user =new TGG.Module.User.Service.USER_LOGIN();
//            return user.CommandStart(session, data);
//        }

//        private ASObject WAR_CITY_BUILD(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.War.Service.Map.WAR_GO.GetInstance().CommandStart(session, data);
//        }

//        private ASObject WAR_MODEL_IN(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.War.Service.Map.WAR_MODEL_IN.GetInstance().CommandStart(session, data);
//        }

//        private ASObject WAR_DIPLOMACY(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.War.Service.Map.WAR_DIPLOMACY.GetInstance().CommandStart(session, data);
//        }

//        public ASObject FIGHT_PERSONAL_ENTER(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Fight.Service.FIGHT_PERSONAL_ENTER.GetInstance().CommandStart(session, data);
//        }

//        public ASObject FIGHT_PERSONAL_JOIN(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Fight.Service.FIGHT_PERSONAL_JOIN.GetInstance().CommandStart(session, data);
//        }

//        public ASObject YIN_JION(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Fight.Service.YIN_JION.GetInstance().CommandStart(session, data);
//        }

//        public ASObject YIN_UPGRADE(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Fight.Service.YIN_UPGRADE.GetInstance().CommandStart(session, data);
//        }

//        public ASObject FIGHT_PERSONAL_ROLE_SELECT(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Fight.Service.FIGHT_PERSONAL_ROLE_SELECT.GetInstance().CommandStart(session, data);
//        }

//        public ASObject FIGHT_PERSONAL_ROLE_DELETE(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Fight.Service.FIGHT_PERSONAL_ROLE_DELETE.GetInstance().CommandStart(session, data);
//        }

//        public ASObject FIGHT_PERSONAL_ROLE_CHANGE(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Fight.Service.FIGHT_PERSONAL_ROLE_CHANGE.GetInstance().CommandStart(session, data);
//        }

//        public ASObject FIGHT_PERSONAL_YIN_SELECT(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Fight.Service.FIGHT_PERSONAL_YIN_SELECT.GetInstance().CommandStart(session, data);
//        }

//        public ASObject RECRUIT_JOIN(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Role.Service.RECRUIT_JOIN.GetInstance().CommandStart(session, data);
//        }

//        public ASObject RECRUIT_GET(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Role.Service.RECRUIT_GET.GetInstance().CommandStart(session, data);
//        }

//        public ASObject RECRUIT_REFRESH(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Role.Service.RECRUIT_REFRESH.GetInstance().CommandStart(session, data);
//        }

//        public ASObject PICKUP_PROP(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Props.Service.PROP_PICKUP.GetInstance().CommandStart(session, data);
//        }

//        public ASObject ARENA_JOIN(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Arena.Service.ARENA_JOIN.GetInstance().CommandStart(session, data);
//        }

//        public ASObject ARENA_DEKARON(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            return TGG.Module.Arena.Service.ARENA_DEKARON.GetInstance().CommandStart(session, data);
//        }

//        public ASObject ARENA_DEKARON1(TGG.SocketServer.TGGSession session, ASObject data)
//        {
//            var model = new tg_bag();
//            model.user_id = session.Player.User.id;
//            model.base_id = 4000013;
//            model.type = 6;
//            model.count = 20;
//            //TGG.Module.Props.Service.Common.GetInstance().InBagFinishing(session, model);
//            return new ASObject();
//        }

//        public void SYSTEM_NOTICE()
//        {
//            // TGG.Module.Notice.Service.SYSTEM_NOTICE.GetInstance().CommandStart();
//        }


//        public Dictionary<string, object> Dic(params object[] value)
//        {
//            var count = (int)(value.Length / 2);
//            var dic = new Dictionary<string, object>();
//            for (int i = 0; i < count; i++)
//            {
//                int a = i * 2;
//                dic.Add(value[a].ToString(), value[a + 1]);
//            }
//            return dic;
//        }
//    }
//}
