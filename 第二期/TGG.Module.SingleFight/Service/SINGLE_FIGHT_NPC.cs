using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;
using TGG.Core.Global;

namespace TGG.Module.SingleFight.Service
{
    /// <summary>
    /// 一将讨挑战
    /// </summary>
    public class SINGLE_FIGHT_NPC
    {
        public static SINGLE_FIGHT_NPC ObjInstance;

        /// <summary>SINGLE_FIGHT_NPC单体模式</summary>
        public static SINGLE_FIGHT_NPC GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new SINGLE_FIGHT_NPC());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "SINGLE_FIGHT_NPC", "一将讨挑战");
#endif
                var npcid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value.ToString());
                var mainrole = session.Player.Role.Kind;

                var basenpc = Variable.BASE_NPCSINGLE.FirstOrDefault(m => m.id == npcid);
                if (basenpc == null) return Result((int)ResultType.BASE_TABLE_ERROR); //验证基表信息

                var cost = RuleConvert.GetCostPower();  //固定消耗体力

                if (!CheckPower(mainrole, cost)) return Result((int)ResultType.BASE_ROLE_POWER_ERROR);
                var fight = NpcChallenge(session.Player.User.id, npcid, FightType.SINGLE_FIGHT);      //获得战斗结果Vo

                if (fight == null) return Result((int)ResultType.FIGHT_ERROR);        //验证战斗是否出错
                //判断挑战结果信息
                return ChallengeResult(session, basenpc.prop, basenpc.count, fight, cost);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>挑战结果处理</summary>
        private ASObject ChallengeResult(TGGSession session, string bprop, string bcount, FightVo fight, int cost)
        {
            var reward = new List<RewardVo>();
            var isprop = false;
            var userid = session.Player.User.id;

            if (fight.isWin)        //挑战胜利
            {
                var mainrole = session.Player.Role.Kind.CloneEntity();
                new Share.Role().PowerUpdateAndSend(mainrole, cost, userid);  //推送体力消耗
                var prop = WinReward(userid, bprop, bcount);
                isprop = prop != null;
                if (prop != null)
                {
                    Variable.TempProp.AddOrUpdate(userid, new List<tg_bag> { prop }, (m, n) => n);
                    var listaso = new List<ASObject> { AMFConvert.ToASObject(EntityToVo.ToPropVo(prop)) };
                    reward.Add(new RewardVo
                    {
                        goodsType = (int)GoodsType.TYPE_PROP,
                        increases = listaso,
                    });
                }
            }
            FightSend(fight, isprop, reward, userid);    //发送战斗协议
            return Result((int)ResultType.SUCCESS);
        }

        /// <summary>战斗胜利获得物品</summary>
        private tg_bag WinReward(Int64 userid, string bprop, string bcount)
        {
            if (string.IsNullOrEmpty(bprop) || string.IsNullOrEmpty(bcount)) return null;
            var propId = Convert.ToInt32(bprop);
            var count = Common.GetInstance().AcquireCount(bcount);   //随机获得残卷数量
            return RewardProp(userid, propId, count);                   //获得残卷信息
        }

        /// <summary>验证武将主角体力信息</summary>
        private bool CheckPower(tg_role role, int cost)
        {
            var totalpower = tg_role.GetTotalPower(role);
            return totalpower >= cost;
        }

        /// <summary>进入战斗</summary>
        private FightVo NpcChallenge(Int64 userid, Int64 npcid, FightType type)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, npcid, type, 0, true);
            new Share.Fight.Fight().Dispose();
            return fight.Result != ResultType.SUCCESS ? null : fight.Ofight;
        }

        /// <summary>
        /// 战斗协议发送
        /// </summary>
        private void FightSend(FightVo fight, Boolean isprop, List<RewardVo> reward, Int64 userid)
        {
            fight.propReward = reward;
            fight.haveProp = isprop ? 1 : 0;
            (new Share.Fight.Fight()).SendProtocol(userid, fight);
        }

        /// <summary>获得的残卷</summary>
        private tg_bag RewardProp(Int64 userid, int propid, int count)
        {
            return new tg_bag
            {
                base_id = propid,
                user_id = userid,
                type = (int)GoodsType.TYPE_PROP,
                count = count,
            };
        }

        /// <summary>组装返回结果</summary>
        private ASObject Result(int result)
        {
            return new ASObject(new Dictionary<string, object> { { "result", result } });
        }
    }
}