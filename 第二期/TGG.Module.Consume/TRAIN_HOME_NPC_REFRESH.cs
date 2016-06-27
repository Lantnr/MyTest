using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Consume
{
    /// <summary>
    /// 武将刷新
    /// </summary>
    public class TRAIN_HOME_NPC_REFRESH : IConsume
    {
        public ASObject Execute(long userId, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userId)) return Result((int)ResultType.CHAT_NO_ONLINE);
            var session = Variable.OnlinePlayer[userId] as TGGSession;
            if (session == null) return Result((int)ResultType.CHAT_NO_ONLINE);
            return CommandStart(session, data);
        }

        /// <summary>武将刷新</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TRAIN_HOME_NPC_REFRESH", "武将刷新");
#endif
                var lv = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "lv").Value.ToString());
                var player = session.Player;
                if (!(new Share.RoleTrain()).IsLevelOk(lv, player.Role.Kind.role_level)) return Result((int)ResultType.BASE_PLAYER_LEVEL_ERROR);  //判断用户等级对应武将等级难度

                if (player.UserExtend.refresh_count >= TotalCount(player.Vip.vip_level))
                    return Result((int)ResultType.TRAIN_HOME_REFRESH_FULL);           //验证刷新次数

                var npc = tg_train_home.GetByUserIdCityIdLevel(player.User.id, player.Scene.scene_id, lv);
                if (npc.Any())    //获取已有居城NPC信息
                {
                    var ids = npc.Select(m => m.id).ToList();
                    if (!tg_train_home.GetDeleteByIds(ids)) { return Result((int)ResultType.DATABASE_ERROR); }
                }
                return RefreshResult(session, lv);     //刷新过程处理
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>处理刷新过程</summary>
        private ASObject RefreshResult(TGGSession session, int lv)
        {
            var player = session.Player.CloneEntity();
            var rcount = player.UserExtend.refresh_count;
            rcount += 1;
            if (!CheckGold(rcount, player.User.id)) return Result((int)ResultType.BASE_PLAYER_GOLD_ERROR);   //验证玩家元宝信息

            player.UserExtend.refresh_count = rcount;
            player.UserExtend.Update();                                     //更新user_extend刷新次数
            session.Player.UserExtend = player.UserExtend;

            var newnpc = (new Share.RoleTrain()).RandomNpc(Convert.ToInt32(player.Scene.scene_id), lv);         //刷新新的武将组
            if (!newnpc.Any()) return Result((int)ResultType.BASE_TABLE_ERROR);         //验证基表武将宅npc信息

            if (!(new Share.RoleTrain()).InsertNpc(newnpc, player.User.id)) return Result((int)ResultType.DATABASE_ERROR);   //验证是否添加新的npc信息
            var listnpc = tg_train_home.GetByUserIdCityIdLevel(player.User.id, Convert.ToInt32(player.Scene.scene_id), lv);
            return new ASObject(NpcBulidData((int)ResultType.SUCCESS, listnpc));
        }

        /// <summary>判断刷新武将消耗</summary>
        /// <param name="count">第几次刷新</param>
        /// <param name="userId">用户id</param>
        private bool CheckGold(int count, long userId)
        {
            var session = Variable.OnlinePlayer[userId] as TGGSession;
            if (session == null) return false;

            //第一次 免费刷新
            var one = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17018");
            if (one == null) return false;
            if (count == Convert.ToInt32(one.value)) return true;

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17019");
            if (rule == null) return false;
            var user = session.Player.User.CloneEntity();

            var cost = CostGold(rule.value, count);
            var gold = user.gold;
            user.gold = user.gold - cost;
            if (user.gold < 0) return false;
            user.Update();
            log.GoldInsertLog(cost, user.id, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_HOME_NPC_REFRESH); //金币消耗记录

            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_GOLD, user);
            var logdata = string.Format("{0}_{1}_{2}_{3}", "G", gold, cost, user.gold);   //记录元宝花费日志
            (new Share.Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_HOME_NPC_REFRESH, logdata);

            session.Player.User = user;
            return true;
        }

        /// <summary>计算刷新武将消耗的金币</summary>
        private int CostGold(string rulevalue, int count)
        {
            var temp = rulevalue;
            temp = temp.Replace("n", count.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(express);
            return cost;
        }

        /// <summary>最大刷新次数</summary>
        private int TotalCount(int viplv)
        {
            var l = 0;
            var count = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17017");  //基础刷新次数
            if (count == null) return l;
            l = l + Convert.ToInt32(count.value);

            var vip = Variable.BASE_VIP.FirstOrDefault(m => m.level == viplv);   //VIP 加成刷新次数
            if (vip == null) return l;
            l = l + vip.trainHome;
            return l;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> NpcBulidData(int result, List<tg_train_home> npcs)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "npc", ConvertToNpcAsobject(npcs) }, };
            return dic;
        }

        /// <summary>转为NpcMonsterASObject</summary>
        private List<ASObject> ConvertToNpcAsobject(IEnumerable<tg_train_home> lists)
        {
            var listnpcs = lists.Select(item => AMFConvert.ToASObject(EntityToVo.ToNpcMonsterVo(item))).ToList();
            return listnpcs;
        }

        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
