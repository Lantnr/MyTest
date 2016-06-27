using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 顿悟
    /// </summary>
    public class TRAIN_TEA_INSIGHT : IConsume
    {
        public ASObject Execute(long user_id, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(user_id)) return Error((int)ResultType.CHAT_NO_ONLINE);
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return Error((int)ResultType.CHAT_NO_ONLINE);
            return CommandStart(session, data);
        }

        /// <summary>茶道顿悟</summary>
        private ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TRAIN_TEA_INSIGHT", "茶道顿悟");
#endif
                var npcid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value.ToString());
                var player = session.Player.CloneEntity();
                if (!(new Share.RoleTrain()).PowerOperate(player.Role.Kind)) return Error((int)ResultType.BASE_ROLE_POWER_ERROR);  //验证武将体力信息

                var npc = tg_train_home.GetNpcById(npcid);
                if (npc == null) return Error((int)ResultType.TRAIN_HOME_GET_ERROR);
                if (npc.is_steal == (int)TrainHomeStealType.STEAL_YES) return Error((int)ResultType.TRAIN_HOME_STEAL_YES);
                if (npc.npc_spirit == 0) return Error((int)ResultType.TRAIN_HOME_SPIRIT_LACK);  //验证NPC 剩余魂数

                var basenpc = Variable.BASE_NPCMONSTER.FirstOrDefault(m => m.id == npc.npc_id);
                if (basenpc == null) return Error((int)ResultType.BASE_TABLE_ERROR);    //验证基表信息
                if (basenpc.limit == (int)TrainHomeLimitType.JUST_CHALLENGE) return Error((int)ResultType.TRAIN_HOME_NO_TEA);  //验证武将是否受限

                var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17013");
                var ulevel = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17014");
                if (rule == null || ulevel == null) return Error((int)ResultType.BASE_TABLE_ERROR);       //验证基表数据 顿悟花费 暂时提高茶道等级
                if (Convert.ToInt32(ulevel.value) < basenpc.level) return Error((int)ResultType.TRAIN_HOME_TEA_LEVEL_LACK);   //验证武将茶道等级

                var gold = player.User.gold;
                if (player.User.gold < Convert.ToInt32(rule.value)) return Error((int)ResultType.BASE_PLAYER_GOLD_ERROR);    //验证玩家元宝信息
                player.User.gold -= Convert.ToInt32(rule.value);

                //奖励更新推送  
                (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, player.User);  //推元宝更新
                log.GoldInsertLog(Convert.ToInt32(rule.value), player.User.id, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_TEA_INSIGHT);  //玩家消费金币记录
                RecordGold(player.User, gold, Convert.ToInt32(rule.value));   //记录消耗金币日志

                return AcquireSpirit(player.User, npc, player.Role.LifeSkill.sub_tea, Convert.ToInt32(ulevel.value));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>处理喝茶获得魂信息</summary>
        private ASObject AcquireSpirit(tg_user user, tg_train_home npc, int teaid, int level)
        {
            var spirit = (new Share.RoleTrain()).GetSpirit(teaid, level);   //顿悟茶道获得的魂数
            var sp = npc.npc_spirit;
            sp = sp - spirit;
            if (sp < 0)
            {
                spirit = npc.npc_spirit;
                npc.npc_spirit = 0;
            }
            else { npc.npc_spirit = sp; }

            return !tg_train_home.UpdateNpc(npc) ? Error((int)ResultType.DATABASE_ERROR) : TeaInfo(npc.npc_spirit, user, spirit);
        }

        /// <summary>处理喝茶信息</summary>
        private ASObject TeaInfo(int nspirit, tg_user user, int spirit)
        {
            var session = Variable.OnlinePlayer[user.id] as TGGSession;
            if (session == null) return Error((int)ResultType.BASE_PLAYER_OFFLINE_ERROR);

            var uspirit = user.spirit;
            user.spirit = tg_user.IsSpiritMax(user.spirit, spirit);     //玩家喝茶获得的魂
            user.Update();
            session.Player.User = user;

            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_SPIRIT, user);          //推送魂更新
            (new Share.Title()).IsTitleAcquire(user.id, (int)TitleGetType.USE_TEA_TABLE);  //判断称号信息

            //记录获得魂日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "S", uspirit, spirit, user.spirit);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_TEA_INSIGHT, logdata);

            return new ASObject(TeaBulidData((int)ResultType.SUCCESS, spirit, nspirit));
        }

        /// <summary>记录金币</summary>
        private void RecordGold(tg_user user, int gold, int value)
        {
            //记录元宝花费日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Gold", gold, value, user.gold);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_TEA_INSIGHT, logdata);
        }

        /// <summary>组装数据</summary>
        private Dictionary<String, Object> TeaBulidData(int result, int spirit, int npcSpirit)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "spirit", spirit }, { "npcSpirit", npcSpirit } };
            return dic;
        }

        /// <summary>错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }
    }
}
