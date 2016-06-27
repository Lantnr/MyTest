using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.Chat.Service
{
    /// <summary>
    /// 爬塔测试指令
    /// </summary>
    public class CHATS_TOWER_TEST
    {
        public static CHATS_TOWER_TEST ObjInstance;

        /// <summary>CHATS_TOWER_TEST单体模式</summary>
        public static CHATS_TOWER_TEST GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new CHATS_TOWER_TEST());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "CHATS_TOWER_TEST", "爬塔测试指令");
#endif
                var site = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "site").Value.ToString());
                var count = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "count").Value.ToString());
                var player = session.Player.CloneEntity();

                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "9009");
                if (rule == null) return Result((int)ResultType.BASE_TABLE_ERROR);      //验证基表信息

                if (site < 1) site = 1;
                if (site > Convert.ToInt32(rule.value)) site = Convert.ToInt32(rule.value);    //塔层信息错误改为默认塔层

                var tower = tg_duplicate_checkpoint.GetEntityByUserId(player.User.id);
                if (tower != null)      //更新更改的爬塔信息
                {
                    if (!UpdateTower(tower, site)) return Result((int)ResultType.DATABASE_ERROR);
                }
                else
                {
                    if (!InsertTower(player.User.id, site)) Result((int)ResultType.DATABASE_ERROR);
                }
                player.UserExtend.challenge_count = count;
                if (!tg_user_extend.GetUpdate(player.UserExtend)) return Result((int)ResultType.DATABASE_ERROR);
                session.Player = player;
                return new ASObject(Result((int)ResultType.SUCCESS));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>更新关卡信息</summary>
        private bool UpdateTower(tg_duplicate_checkpoint tower, int site)
        {
            tower.site = site;
            tower.ninjutsu_star = null;
            tower.calculate_star = null;
            tower.state = (int)DuplicateClearanceType.CLEARANCE_UNBEGIN;
            tower.npcids = null;
            tower.npc_tea = 0;
            tower.user_tea = 0;
            tower.dekaron = 0;
            tower.blood = 1000;
            tower.user_blood = 0;
            tower.npc_blood = 0;
            tower.select_position = null;
            tower.all_cards = null;
            return tower.Update() > 0;
        }

        /// <summary>添加爬塔信息</summary>
        private bool InsertTower(Int64 userid, int site)
        {
            var tower = new tg_duplicate_checkpoint
            {
                user_id = userid,
                blood = 1000,
                state = (int)DuplicateClearanceType.CLEARANCE_UNBEGIN,
                site = site
            };
            return tower.Insert() > 0;
        }


        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
