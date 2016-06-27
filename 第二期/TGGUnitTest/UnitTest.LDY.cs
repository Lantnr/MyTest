using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FluorineFx;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGGUnitTest
{
    [TestClass]
    public class UnitTest_LDY
    {
        [TestMethod]
        public void TestMethod1()
        {

            var session = new TGG.SocketServer.TGGSession
            {
                Player = new Player
                {
                    User = new tg_user(),
                    
                },
                Fight = new FightItem(),
                TaskItems = new List<TaskItem>(),
                LastActiveTime = new DateTime(),
                SPM =  new ConcurrentDictionary<string, ASObject>(),
               

            };
            var user = tg_user.FindByid(1);
            session.Player.User = user;
            session.Player.Bag.Surplus = 3;
            TGG.Core.XML.Util.ReadBaseEntity(); //加载基表数据
            Variable.OnlinePlayer.AddOrUpdate(user.id, session, (k, v) => session);

            TGG.Module.Scene.Service.LOGIN_ENTER_SCENE.GetInstance().CommandStart(session, null);
            Variable.TaskInfo.Add(new Variable.UserTaskInfo() { userid = 27, GuardSceneId = 200055 });
            // TGG.Module.Building.Service.ENTER.GetInstance().CommandStart(session, null);
            //var roleinfo =  tg_role.FindByid(12321);
            //TGG.Module.User.Service.Common.GetInstance().UserLvUpdate(user.id, 500, roleinfo);
            //  TGG.Module.User.Service.Common.GetInstance().UserIdentifyUpdate(user.id, 500, roleinfo,(int)  VocationType.Roles);
                new TGG.Share.Reward().GetReward("7_6010007_1", 1);

            #region 职业任务更新
            //var dic = new Dictionary<string, object> { { "type", 1 }, { "npcId", 200055 }, { "taskId", 2130 } };
            //TGG.Module.Task.Service.TASK_VOCATION_UPDATE.getInstance().CommandStart(session, new ASObject(dic));
            #endregion

            #region 职业任务接受
            var dic1 = new Dictionary<string, object> { { "task", 291908 } };
            TGG.Module.Task.Service.TASK_VOCATION_ACCEPT.getInstance().CommandStart(session, new ASObject(dic1));
            #endregion

            //  TGG.Module.Task.Service.Common.getInstance().GetReward("6_4000013_3", session); //道具入包
            //  TGG.Module.Task.Service.Common.getInstance().GetReward("7_6080037_3", session); //装备入包

            #region 装备购买

            //var dic = new Dictionary<string, object> { { "id", 6010001 }, { "count", 1 } };
            //EquipBuy(session);
            #endregion

            // TGG.Module.Task.Service.TASK_PUSH.getInstance().VocationTaskUpdate();
            //TASK_VOCATION_REFRESH(session);
        }

        /// <summary>
        /// 装备购买
        /// </summary>
        public ASObject EquipBuy(TGGSession session)
        {
            var dic = new Dictionary<string, object> { { "id", 6010001 }, { "count", 1 } };
            var aso = new ASObject(dic);

            TGG.Module.Task.Service.TASK_PUSH.getInstance().VocationTaskUpdate();
            return TGG.Module.Equip.Service.EQUIP_BUY.GetInstance().CommandStart(session, aso);
        }

        public ASObject TASK_VOCATION_REFRESH(TGGSession session)
        {
            var aso = TGG.Module.Task.Service.TASK_VOCATION_REFRESH.getInstance().CommandStart(session, new ASObject());
            return aso;
        }
    }
}
