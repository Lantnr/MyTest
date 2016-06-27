using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Common.Randoms;
using TGG.Core.Global;

namespace TGG.Module.Duplicate.Service.CHECKPOINT
{
    /// <summary>
    /// 花月茶道翻牌
    /// </summary>
    public class TOWER_CHECKPOINT_TEA_GAME_FLOP
    {
        private static TOWER_CHECKPOINT_TEA_GAME_FLOP ObjInstance;

        /// <summary>TOWER_CHECKPOINT_TEA_GAME_FLOP单体模式</summary>
        public static TOWER_CHECKPOINT_TEA_GAME_FLOP GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TOWER_CHECKPOINT_TEA_GAME_FLOP());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TOWER_CHECKPOINT_TEA_GAME_FLOP", "花月茶道翻牌");
#endif
                var loc = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "loc").Value.ToString());         //获取玩家查看的牌的位置（1-30）

                var tower = tg_duplicate_checkpoint.GetEntityByUserId(session.Player.User.id);
                if (tower == null || tower.state != (int)DuplicateClearanceType.CLEARANCE_FIGHTING)  //验证关卡信息
                    return Result((int)ResultType.TOWER_SITE_ERROR);
                if (string.IsNullOrEmpty(tower.select_position) || string.IsNullOrEmpty(tower.all_cards))
                    return Result((int)ResultType.DATABASE_ERROR);     //验证茶道翻开卡牌及随机好的卡牌信息

                var _position = ConvertToList(tower.select_position);
                if (_position[loc - 1] != 0) return Result((int)ResultType.TOWER_CARD_FLOPED);        //验证卡牌是否翻过
                return CheckResult(tower, _position, loc);       //处理爬塔信息
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>处理爬塔信息</summary>
        private ASObject CheckResult(tg_duplicate_checkpoint tower, List<int> _position, int loc)
        {
            var _record = ConvertToList(tower.all_cards);
            var value = _record[loc - 1];                            //获取玩家卡牌图案
            _position[loc - 1] = value;                               //更新翻牌信息
            var nloc = RandomPosition(_position);
            var nvalue = _record[nloc];
            _position[nloc] = nvalue;
            tower.select_position = ConvertToString(_position);   //npc翻牌后更新牌位置

            var rule = Variable.BASE_TOWERTEA.FirstOrDefault(m => m.myIcon == value && m.enemyIcon == nvalue);
            if (rule == null) return Result((int)ResultType.BASE_TABLE_ERROR);     //验证数据

            tower.npc_tea += rule.enemyScore;
            if (tower.npc_tea > 10) tower.npc_tea = 10;
            tower.user_tea += rule.myScore;
            if (tower.user_tea > 10) tower.user_tea = 10;        //验证玩家星数是否超过上限
            return IsClearance(tower, _position, loc, nloc, value, nvalue);
        }

        /// <summary>判定是否通关</summary>
        private ASObject IsClearance(tg_duplicate_checkpoint tower, List<int> pInfo, int loc, int nloc, int photo, int nphoto)
        {
            var clearance = Variable.BASE_TOWERPASS.FirstOrDefault(m => m.pass == tower.site);
            if (clearance == null || string.IsNullOrEmpty(clearance.passAward))            //验证基表信息
                return Result((int)ResultType.BASE_TABLE_ERROR);
            var reward = clearance.passAward;

            if (tower.user_tea <= 0)         //通关失败
            {
                if (!AcquireReward(tower, reward, (int)DuplicateClearanceType.CLEARANCE_FAIL))
                    return Result((int)ResultType.DATABASE_ERROR);
            }
            else
            {
                if (tower.npc_tea <= 0)           //通关成功
                {
                    if (!AcquireReward(tower, reward, (int)DuplicateClearanceType.CLEARANCE_SUCCESS))
                        return Result((int)ResultType.DATABASE_ERROR);
                }
                else
                {
                    if (!CheckCards(pInfo).Any())       //通关成功
                    {
                        if (!AcquireReward(tower, reward, (int)DuplicateClearanceType.CLEARANCE_SUCCESS))
                            return Result((int)ResultType.DATABASE_ERROR);
                    }
                    if (!tg_duplicate_checkpoint.UpdateSite(tower)) return Result((int)ResultType.DATABASE_ERROR);
                }
            }
            return new ASObject(Common.GetInstance().TeaFlopData((int)ResultType.SUCCESS, tower.npc_tea, tower.user_tea, loc, photo, nloc + 1, nphoto));
        }

        /// <summary>根据结果处理相应信息</summary>
        private bool AcquireReward(tg_duplicate_checkpoint tower, string reward, int type)
        {
            tower.state = type;
            if (!tg_duplicate_checkpoint.UpdateSite(tower)) return false;
            TOWER_CHECKPOINT_DARE_PUSH.GetInstance().CommandStart(tower.user_id, tower.site, type);
            if (tower.state != (int)DuplicateClearanceType.CLEARANCE_SUCCESS) return true;

            if (!Variable.OnlinePlayer.ContainsKey(tower.user_id)) return false;         //玩家在线则推送奖励
            var session = Variable.OnlinePlayer[tower.user_id] as TGGSession;
            if (session == null) return false;

            var user = session.Player.User.CloneEntity();
            var fame = user.fame;
            user.fame = tg_user.IsFameMax(user.fame, Convert.ToInt32(reward));
            user.Update();
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_FAME, user);

            //记录获得魂日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Fame", fame, Convert.ToInt32(reward), user.fame);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.DUPLICATE, (int)DuplicateCommand.TOWER_CHECKPOINT_TEA_GAME_FLOP, logdata);

            return true;
        }

        /// <summary>转换卡牌信息</summary>
        private List<int> ConvertToList(string select)
        {
            var list = new List<int>();
            if (string.IsNullOrEmpty(select)) return list;
            if (!select.Contains("_")) return list;
            var n = select.Split("_").ToList();
            list.AddRange(n.Select(item => Convert.ToInt32(item)));
            return list;
        }

        /// <summary>npc随机翻牌位置</summary>
        private int RandomPosition(List<int> flopinfo)
        {
            var listIndex = new List<int>();
            for (var i = 0; i < flopinfo.Count; i++)
            {
                if (flopinfo[i] != 0) continue;
                listIndex.Add(i);
            }
            var n = RNG.Next(0, listIndex.Count - 1);
            return listIndex[n];
        }

        /// <summary>将位置list[int]转换为字符串</summary>
        private string ConvertToString(IEnumerable<int> position)
        {
            var record = "";
            foreach (var item in position)
            {
                record += Convert.ToString(item) + "_";
            }
            return record;
        }

        /// <summary>检查未翻的卡牌数量</summary>
        private IEnumerable<int> CheckCards(List<int> pInfo)
        {
            var list = new List<int>();
            if (!pInfo.Any()) return list;
            list.AddRange(pInfo.Where(item => item == 0));
            return list;
        }

        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}
