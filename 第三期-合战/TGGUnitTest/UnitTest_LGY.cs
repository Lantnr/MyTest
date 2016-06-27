using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.SocketServer;

namespace TGGUnitTest
{
    [TestClass]
    public class UnitTest_LGY
    {
        [TestMethod]
        public void TestMethod1()
        {
            TGG.Core.XML.Util.Init();
            TGGSession session = new TGGSession();
            session.Player = new Player();
            session.Player.User.id = 12;
            session.Player.User.gold = 10000;
            session.Player.User.coin = 1000000;
            session.Player.Role.Kind.role_level = 20;
            session.Player.War.PlayerInCityId = 1000058;
            //session.Player.User.player_level = 20;
            session.Player.User.player_name="昌紫翠";
            var dic = new Dictionary<string, object>();
            //dic.Add("id", 1);
            //dic.Add("type", 1603001);
            int goodsType = 0;
            dic.Add("count", 1);
            dic.Add("rid",19);
            dic.Add("id", 19);
            dic.Add("baseId", 1603001);
            dic.Add("attribute", 1);
            dic.Add("type", 1);
            dic.Add("npcId", 200055);
            dic.Add("taskId", 1924);
            dic.Add("notice", "aaa");
            dic.Add("characterId", 10003);
            dic.Add("userid", 113504);
            dic.Add("devote",20);
            
            tg_car car = new tg_car();
            car = this.car();
            List<tg_goods_business> goods = new List<tg_goods_business>();
            goods.Add(this.goods());
            goods.Add(this.goods());
            //TGG.Module.Equip.Service.EQUIP_JOIN.GetInstance().CommandStart(session,new ASObject(dic));
            //TGG.Module.Equip.Service.EQUIP_SPIRIT.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.Equip.Service.EQUIP_SPIRIT_LOCK.GetInstance().CommandStart(session, new ASObject(dic));

            //TGG.Module.RoleTrain.Service.TRAIN_ROLE_START.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.RoleTrain.Service.TRAIN_ROLE_LOCK.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.Family.Service.FAMILY_JOIN.GetInstance().CommandStart(session, new ASObject(dic));

            //TGG.Module.Skill.Service.SKILL_LIFE_STUDY.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.Duplicate.Service.CHECKPOINT.TOWER_CHECKPOINT_ENTER.GetInstance().CommandStart(session, new ASObject(dic));

            //TGG.Module.Rankings.Service.RANKING_HONOR_LIST.GetInstance().CommandStart(session, new ASObject(dic));

            //.Module.Rankings.Service.RANKING_COIN_LIST.GetInstance().CommandStart(session, new ASObject(dic));

            //TGG.Module.Duplicate.Service.CHECKPOINT.TOWER_CHECKPOINT_NPC_REFRESH.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.Duplicate.Service.CHECKPOINT.TOWER_CHECKPOINT_DARE.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.Duplicate.Service.CHECKPOINT.TOWER_CHECKPOINT_ELOQUENCE_GAME.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.Duplicate.Service.CHECKPOINT.TOWER_CHECKPOINT_ELOQUENCE_GAME_ENTER.GetInstance().CommandStart(session, new ASObject(dic));

            //TGG.Module.Task.Service.TASK_VOCATION_UPDATE.getInstance().CommandStart(session, new ASObject(dic));

            //TGG.Module.Skill.Service.SKILL_LIFE_ACCELERATE.GetInstance().CommandStart(session, new ASObject(dic));

            //TGG.Module.War.Service.SkyCity.WAR_SKYCITY_START.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.War.Service.SkyCity.WAR_SKYCITY_UNLOCK.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.War.Service.SkyCity.WAR_SKYCITY_ACCELERATE.GetInstance().CommandStart(session, new ASObject(dic));
            //TGG.Module.War.Service.SkyCity.WAR_SKYCITY_ENTER.GetInstance().CommandStart(session, new ASObject(dic)); 

            //TGG.Module.Props.Service.PROP_CHARACTER_USE.GetInstance().CommandStart(session, new ASObject(dic)); 
            //TGG.Module.Role.Service.ROLE_CHARACTER_DELETE.GetInstance().CommandStart(session, new ASObject(dic));
         //   TGG.Module.War.Service.Map.WAR_DEVOTE_CHANGE.GetInstance().CommandStart(session, new ASObject(dic)); 

        }

        public tg_car car()
        {
            return new tg_car()
            {
                id = 42,
                user_id = 100095,
                rid = 0,
                car_id = 331001,
                speed = 0,
                packet = 0,
                state = 0,
                start_ting_id = 300002,
                stop_ting_id = 300003,
                time = 10,
                distance = 10,
            };
        }

        public tg_goods_business goods()
        {
            return new tg_goods_business()
            {
                user_id = 100095,
                ting_id = 300003,
                cid = 42,
                goods_id = 310002,
                goods_type = 1,
                goods_number = 10,
                price = 10,
            };
        }
    }
}
