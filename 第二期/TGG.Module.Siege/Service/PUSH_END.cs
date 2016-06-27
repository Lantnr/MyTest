using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 推送活动结束
    /// </summary>
    public class PUSH_END
    {
        private static PUSH_END _objInstance;

        /// <summary>PUSH_END单体模式</summary>
        public static PUSH_END GetInstance()
        {
            return _objInstance ?? (_objInstance = new PUSH_END());
        }

        /// <summary> 推送活动结束</summary>
        public void CommandStart(SiegeResultType type)
        {
            Variable.Activity.Siege.IsOpen = false;   //关闭活动入口
            new Share.Activity().PushActivity(0, 14);  //推送关闭图标
            SendAllEndAndExit(type);
            SendEndNotice(type);
        }

        /// <summary> Send 所有活动玩家 活动结束和活动退出 </summary>
        /// <param name="type">活动结果枚举类型</param>
        private void SendAllEndAndExit(SiegeResultType type)
        {
            foreach (var item in Variable.Activity.Siege.PlayerData)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var temp = m as SiegePlayer;
                    if (temp == null) return;

                    var reward = ConvertRewardVos(temp, type);          //计算玩家活动结束奖励所得

                    (new Share.Message()).BuildMessagesSend(temp.user_id, "美浓活动奖励", "美浓活动奖励", reward);

                    (new Share.DaMing()).CheckDaMing(temp.user_id, (int)DaMingType.美浓攻略);
                    if (!Variable.OnlinePlayer.ContainsKey(temp.user_id)) return;
                    var session = Variable.OnlinePlayer[temp.user_id] as TGGSession;

                    if (Common.GetInstance().IsActivities(temp.user_id)) //是否在活动中
                    {
                        var aso = EXIT.GetInstance().CommandStart(session);      //退出活动（会推送其他玩家 该玩家离开美浓活动场景）
                        Common.GetInstance().TrainingSiegeEndSend(session, aso, (int)SiegeCommand.EXIT); //(SendData)退出活动
                    }
                    var asobj = new ASObject(BuildData((int)type, reward));
                    Common.GetInstance().TrainingSiegeEndSend(session, asobj, (int)SiegeCommand.PUSH_END);//Send活动结束
                    token.Cancel();
                }, item, token.Token);
            }

            Variable.Activity.Siege.PlayerData = new List<SiegePlayer>();
            Variable.Activity.Siege.BossCondition = new List<CampCondition>();
        }

        /// <summary> 活动公告结束跑马灯推送 </summary>
        private void SendEndNotice(SiegeResultType type)
        {
            switch (type)
            {
                case SiegeResultType.DOGFALL: { SendPlayersNoyice(100008, ""); break; }
                case SiegeResultType.EAST_WIN: { SendPlayersNoyice(100006, "东军"); break; }
                case SiegeResultType.EAST_LOSE: { SendPlayersNoyice(100006, "西军"); break; }
            }
        }

        /// <summary> 公告推送 </summary>
        /// <param name="baseid"></param>
        /// <param name="content"></param>
        private void SendPlayersNoyice(int baseid, string content)
        {
            (new Share.Notice()).TrainingPlayer(baseid, content);
        }

        #region 获取数据

        /// <summary> 区分玩家应获得奖励 </summary>
        /// <param name="model">玩家活动数据</param>
        /// <param name="type">活动胜利类型</param>
        private List<RewardVo> ConvertRewardVos(SiegePlayer model, SiegeResultType type)
        {
            var list = new List<RewardVo>();
            switch (type)
            {
                case SiegeResultType.DOGFALL: { list = GetRewardVos(model.fame, Convert.ToInt32(ActivityRewardType.DRAW)); break; }
                case SiegeResultType.EAST_LOSE: { list = GetRewardVos(model.fame, model.player_camp == (int)CampType.East ? Convert.ToInt32(ActivityRewardType.LOSE) : Convert.ToInt32(ActivityRewardType.WIN)); break; }
                case SiegeResultType.EAST_WIN: { list = GetRewardVos(model.fame, model.player_camp == (int)CampType.East ? Convert.ToInt32(ActivityRewardType.WIN) : Convert.ToInt32(ActivityRewardType.LOSE)); break; }
                default: { break; }
            }
            return list;
        }

        /// <summary> 获取奖励 </summary>
        /// <param name="fame">玩家当前声望</param>
        /// <param name="type">奖励类型:胜利或失败 或平局</param>
        private List<RewardVo> GetRewardVos(int fame, int type)
        {
            var list = new List<RewardVo>();
            var reward = GetBaseSiegeReward(type, fame);
            if (reward == null) return list;
            list.Add(ConvertRewardVo((int)GoodsType.TYPE_COIN, reward.money));
            list.Add(ConvertRewardVo((int)GoodsType.TYPE_FAME, reward.fameReward));
            return list;
        }

        /// <summary> 获取活动团队奖励 </summary>
        /// <param name="type">类型：胜利失败平局</param>
        /// <param name="fame">玩家当前声望</param>
        private BaseSiegeReward GetBaseSiegeReward(int type, int fame)
        {
            return Variable.BASE_SIEGEREWARD.Where(
                                   m => m.type == type && m.fame <= fame)
                                   .OrderByDescending(m => m.fame)
                                   .FirstOrDefault();
        }

        #endregion

        #region 组装数据

        /// <summary> 组装奖励Vo </summary>
        /// <param name="type">数值货品类型</param>
        /// <param name="values">数值</param>
        private RewardVo ConvertRewardVo(int type, int values)
        {
            return new RewardVo
            {
                goodsType = type,
                value = values,
            };
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(int type, List<RewardVo> list)
        {
            var dic = new Dictionary<string, object>
            {
                {"type", type},
                {"rewards", list},
            };
            return dic;
        }

        #endregion
    }
}
