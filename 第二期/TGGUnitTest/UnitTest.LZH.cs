using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGGUnitTest
{
    /// <summary>
    /// UnitTest 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var session = new TGGSession { Player = new Player { User = new tg_user() } };
            var user = tg_user.FindByid(34);
            var ex = tg_user_extend.FindAllByName("user_id", 34, null, 0, 0);
            var daming = tg_daming_log.FindAllByName("user_id", 34, null, 0, 0);
            session.Player.User = user;
            session.Player.DamingLog = daming;
            session.Player.UserExtend = ex[0];
            session.Player.Game = new GameItem();
            Variable.OnlinePlayer.AddOrUpdate(session.Player.User.id, session, (k, v) => session);
            TGG.Core.XML.Util.ReadBaseEntity(); //加载基表数据

            #region 跑商测试
            //var session = new TGGSession();
            //session.Player = new Player();
            //session.Player.User = new tg_user();
            //session.Player.Order = new BusinessOrder();
            //session.Player.User.id = 100104;
            //var dic = new Dictionary<string, object>();

            //进入町测试
            //dic.Add("id", 300001);
            //TGG.Module.Business.Service.BUSINESS_TING_ENTER.GetInstance().CommandStart(session, new ASObject(dic));


            //session.Player.User.coin = 10000;
            //session.Player.User.gold = 10000;
            //session.Player.User.birthplace = 300196;
            //session.Player.User.player_level = 1;
            //session.Player.User.player_sex = 1;
            //session.Player.User.user_code = "HE";
            //session.Player.User.player_name = "HE";
            //session.Player.User.role_id = 7000000;

            //增加马车格子测试
            //dic.Add("carId", 140);
            //TGG.Module.Business.Service.BUSINESS_PACKET_BUY.GetInstance().CommandStart(session, new ASObject(dic));

            //市价情报
            //dic.Add("id", 1);
            //TGG.Module.Business.Service.BUSINESS_PRICE_QUERY.GetInstance().CommandStart(session, new ASObject(dic));

            //调查
            //var ids = new List<int>();
            //ids.Add(300001);
            //ids.Add(300002);
            //dic.Add("id", ids);
            //TGG.Module.Business.Service.BUSINESS_PRICE_INFO.GetInstance().CommandStart(session, new ASObject(dic));
            #endregion

            #region 武将测试

            //武将属性保存
            //var role = new tg_role
            //{
            //    user_id = 138,
            //    att_points = 100
            //};
            //var dic = new Dictionary<string, object>();
            //var list = new List<object> { -1, 2, 3, 4, 5 };
            //dic.Add("att", list);
            //TGG.Module.Role.Service.ROLE_ATTRIBUTE.GetInstance().CommandStart(session, new ASObject(dic));

            //武将放逐
            //dic.Add("roleId", 12048);
            //TGG.Module.Role.Service.ROLE_EXILE.GetInstance().CommandStart(session, new ASObject(dic));

            //dic.Add("roleId", 12081);
            //TGG.Module.Role.Service.ROLE_SKILL_ENTER.GetInstance().CommandStart(session, new ASObject(dic));

            //dic.Add("skillId", 14);
            //TGG.Module.Role.Service.ROLE_SKILL_SELECT.GetInstance().CommandStart(session, new ASObject(dic));

            #endregion

            #region 战斗技能学习测试

            //dic.Add("id", 16152001);
            //dic.Add("roleId", 12025);
            //TGG.Module.Skill.Service.SKILL_FIGHT_STUDY.GetInstance().CommandStart(session, new ASObject(dic));

            //dic.Add("id", 34);
            //TGG.Module.Skill.Service.SKILL_FIGHT_UPGRADE.GetInstance().CommandStart((int)GoodsType.TYPE_COIN, session, new ASObject(dic));

            #endregion

            #region 点将测试

            //武将难度选择
            //dic.Add("lv",3);
            //TGG.Module.RoleTrain.Service.TRAIN_HOME_LEVEL_SELECT.GetInstance().CommandStart(session, new ASObject(dic));

            //点将加载
            //TGG.Module.RoleTrain.Service.TRAIN_HOME_JOIN.GetInstance().CommandStart(session, new ASObject());

            //武将选择
            //dic.Add("id", 15);
            //TGG.Module.RoleTrain.Service.TRAIN_HOME_NPC_SELECT.GetInstance().CommandStart(session, new ASObject(dic));

            //茶道
            //dic.Add("id", 4);
            //TGG.Module.RoleTrain.Service.TRAIN_HOME_NPC_TEA.GetInstance().CommandStart(session, new ASObject(dic));

            //武将刷新
            //dic.Add("lv", 2);
            //TGG.Module.RoleTrain.Service.TRAIN_HOME_NPC_REFRESH.GetInstance().CommandStart(session, new ASObject(dic));

            //挑战
            //dic.Add("id", 80);
            //TGG.Module.RoleTrain.Service.TRAIN_HOME_NPC_FIGHT.GetInstance().CommandStart(session, new ASObject(dic));

            #endregion

            #region 一将讨测试
            //dic.Add("id", 15030001);
            //TGG.Module.SingleFight.Service.SINGLE_FIGHT_NPC.GetInstance().CommandStart(session, new ASObject(dic));

            #endregion

            #region 家臣称号测试

            //TGG.Module.Title.Service.ROLE_TITLE_JOIN.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.Title.Service.Common.GetInstance().RoleFightMethods(session.Player.User.id, session.Player.Role.Kind.equip_weapon);

            #endregion

            #region 忍术游戏测试验证

            //dic.Add("position", 4);
            //TGG.Module.Duplicate.Service.CHECKPOINT.TOWER_CHECKPOINT_NINJUTSU_GAME_RESULT.GetInstance()
            //    .CommandStart(session, new ASObject(dic));

            //var values = new[] { 2, 2, 2 };
            //dic.Add("values", values);
            //TGG.Module.Duplicate.Service.CHECKPOINT.TOWER_CHECKPOINT_CALCULATE_GAME.GetInstance().CommandStart(session, new ASObject(dic));

            //dic.Add("loc", 11);
            //TGG.Module.Duplicate.Service.CHECKPOINT.TOWER_CHECKPOINT_TEA_GAME_FLOP.GetInstance().CommandStart(session, new ASObject(dic));

            //TGG.Module.RoleTrain.Service.Common.GetInstance().UpdateNpcSpirit();

            //dic.Add("site", 4);
            //TGG.Module.Chat.Service.CHATS_TOWER_TEST.GetInstance().CommandStart(session, new ASObject(dic));

            #endregion

            #region 查看玩家信息

            //dic.Add("userId", 9);
            //TGG.Module.Role.Service.QUERY_PLAYER_ROLE.GetInstance().CommandStart(session, new ASObject(dic));

            #endregion

            #region 测试

            //var text = "测试数据";
            //Clipboard.SetText(text);

            #endregion

            #region 测试任务

            //Variable.TaskInfo.Add(CreatRumorTask());
            ////Variable.TaskInfo.Add(CreatArrestRumorTask());
            //session.Player.Scene.scene_id = 100012;
            //var dic = new Dictionary<string, object> { { "type", 1 }, { "npcId", 200055 }, { "taskId", 75 } };
            //TGG.Module.Task.Service.TASK_VOCATION_UPDATE.getInstance().CommandStart(session, new ASObject(dic));


            #endregion

            #region 测试加速指令
            //var dic = new Dictionary<string, object> { { "id", 25 } };
            //TGG.Module.Skill.Service.SKILL_FIGHT_ACCELERATE.GetInstance().CommandStart(session, new ASObject(dic));
            #endregion

            #region 大名指令  进入页面

            //var dic = new Dictionary<string, object> { { "count", 4 } };
            //TGG.Module.Guide.Service.ENTER.GetInstance().CommandStart(session, new ASObject(dic));        
            //session.Player.Bag.BagIsFull = false;
            //session.Player.Bag.Surplus = 10;
            //session.Player.Vip = new tg_user_vip { bargain = 4 };
            //TGG.Module.Guide.Service.REWARD.GetInstance().CommandStart(session, new ASObject(dic));

            #endregion

            #region 游艺园
            //TGG.Module.Business.Service.BUSINESS_BUY_BARGAIN.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.Games.Service.GAMES_RECEIVE.GetInstance().CommandStart(session, new ASObject(new Dictionary<string, object>()));
            #endregion
        }

        [TestMethod]
        public void TestMethod2()
        {
            var number = Rnd.Next(1, 5);
        }

        #region 练习
        private void NewTask(Int64 time)
        {
            try
            {
                var token = new CancellationTokenSource();
                var obj = new New() { time = time };

                Task.Factory.StartNew(m =>
                {


                    SpinWait.SpinUntil(() => false, Convert.ToInt32(time));
                }, obj, token.Token)
                    .ContinueWith((m, n) =>
                    {

                    }, obj, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }

        }

        public class New
        {
            public Int64 time { get; set; }
        }

        private static string Test()
        {
            const string word = "伟刚勇毅俊峰强军平保东文辉力明永健世广志义兴良海山仁波";
            var sb = new StringBuilder();
            var list = TGG.Core.Common.Randoms.RNG.Next(0, word.Length - 1, 5);
            foreach (var i in list)
            {
                sb.Append(word[i]);
            }
            return sb.ToString();
        }

        public static class Rnd
        {
            private static readonly RNGCryptoServiceProvider Rndom = new RNGCryptoServiceProvider();
            private static readonly byte[] Rb = new byte[8];

            public static int Next()
            {
                Rndom.GetBytes(Rb);
                var value = BitConverter.ToInt32(Rb, 0);
                return value < 0 ? -value : value;
            }

            public static int Next(int max)
            {
                var n = Next();
                var num = n % (max + 1);
                return num;
            }

            public static List<int> NextList(int number)
            {
                var list = new List<int>();
                for (var i = 0; i <= 5; i++)
                {
                    var num = Next() % (number + 1);
                    list.Add(num);
                }
                return list;
            }

            public static int Next(int min, int max)
            {
                var num = Next(max - min) + min;
                return num;
            }
        }

        #endregion 练习
    }
}
