using FluorineFx;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;
using System.Linq;
using TGG.Core.Enum.Type;
using System.Collections.Generic;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Entity;
using System;
using TGG.Core.Global;
using TGG.Core.Base;
using NewLife.Log;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 进入战斗
    /// </summary>
    public partial class FIGHT_PERSONAL_ENTER
    {
        private static FIGHT_PERSONAL_ENTER _objInstance;

        /// <summary>FIGHT_PERSONAL_ENTER单体模式</summary>
        public static FIGHT_PERSONAL_ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new FIGHT_PERSONAL_ENTER());
        }

        /// <summary>进入战斗</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            XTrace.WriteLine("{0}:{1}", "FIGHT_PERSONAL_ENTER", "进入战斗");

            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
            var npcid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "npc").Value);
            var result = GetResult(session, npcid, type, 0);  //战斗处理结果
            return result != ResultType.SUCCESS ? new ASObject(BuildData((int)result, null)) :
                new ASObject(BuildData((int)ResultType.SUCCESS, vo));
        }

        /// <summary> 获取战斗实体 </summary>
        /// <param name="session">当前玩家session</param>
        /// <param name="rivalid">对手id</param>
        /// <param name="type">战斗类型</param>
        /// <param name="hp">要调控血量 (爬塔有效活动,连续战斗任务有效)</param>
        /// <param name="of">是否获取己方战斗Vo</param>
        /// <param name="po">是否推送己方战斗</param>
        /// <param name="or">是否获取对方战斗Vo</param>
        /// <param name="pr">是否推送对方战斗</param>
        /// <param name="rolehomeid">(武将宅类型时可用)要挑战武将宅id</param>
        /// <returns></returns>
        public Core.Entity.Fight GeFight(TGGSession session, Int64 rivalid, FightType type, Int64 hp = 0, bool of = false, bool po = false, bool or = false, bool pr = false, int rolehomeid = 0)
        {
            var roleid = session.Player.Role.Kind.id;
            var userid = session.Player.User.id;
            RoleHomeId = rolehomeid;
            var fight = new Core.Entity.Fight();
            fight.Result = GetResult(session, rivalid, Convert.ToInt32(type), hp);
            if (fight.Result != ResultType.SUCCESS) return fight;

            fight.Iswin = vo.isWin;
            fight.PlayHp = GetPlayHp(roleid);
            fight.Hurt = GetBossHurt(userid);
            fight.BoosHp = GetBossHp(userid);
            if (of) fight.Ofight = vo;
            if (po) SendProtocol(session, new ASObject(BuildData(Convert.ToInt32(fight.Result), vo)));
            if (or) fight.Rfight = GetRivalFightVo();
            if (!pr) return fight;
            if (type != FightType.BOTH_SIDES || type != FightType.ONE_SIDE) return fight;
            var s = Variable.OnlinePlayer[rivalid] as TGGSession;
            SendProtocol(s, new ASObject(BuildData(Convert.ToInt32(fight.Result), GetRivalFightVo())));
            return fight;
        }



        #region 获取战斗结果方法

        /// <summary> 获取战斗Vo </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        public FightVo GetFightVo(TGGSession session, Int64 rivalid, int type)
        {
            var result = GetResult(session, rivalid, type, 0);  //战斗处理结果
            return result != ResultType.SUCCESS ? null : vo;
        }

        /// <summary> 获取对方的战斗Vo </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        public FightVo GetRivalFightVo(TGGSession session, Int64 rivalid, int type)
        {
            var result = GetResult(session, rivalid, type, 0);  //战斗处理结果
            if (result == (int)ResultType.SUCCESS)
                SendProtocol(session, new ASObject(BuildData(Convert.ToInt32(result), vo)));
            else
                return null;
            return GetRivalFightVo();
        }

        /// <summary> 获取战斗Vo </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        /// <param name="rolehomeid">武将宅id</param>
        /// <param name="bosslife">boss血量</param>
        public FightVo GetFightVo(TGGSession session, Int64 rivalid, int type, int rolehomeid, ref Int64 bosslife)
        {
            RoleHomeId = 0;
            RoleHomeId = rolehomeid;
            return GetFightVo(session, rivalid, type, ref bosslife);
        }

        /// <summary> 获取战斗Vo </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        /// <param name="bosslife">boss血量</param>
        public FightVo GetFightVo(TGGSession session, Int64 rivalid, int type, ref Int64 bosslife)
        {
            var result = GetResult(session, rivalid, type, bosslife);
            if (result != ResultType.SUCCESS) return null;
            var fightrole = list_role.FirstOrDefault(m => m != null && m.user_id != session.Player.User.id);
            if (fightrole == null) return null;
            bosslife = fightrole.hp;
            return vo;
        }

        /// <summary> 获取战斗Vo并的到Boss剩余血量和对Boss的伤害值 </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        /// <param name="bosslife">boss血量</param>
        /// <param name="hurt">对boss的血量伤害值</param>
        public FightVo GetFightVo(TGGSession session, Int64 rivalid, int type, Int64 bosslife, ref Int64 hurt)
        {
            return GetFightVo(session, rivalid, Convert.ToInt32(type), ref bosslife, ref hurt);
        }

        /// <summary> 获取战斗Vo并的到Boss剩余血量和对Boss的伤害值 </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        /// <param name="bosslife">boss血量</param>
        /// <param name="hurt">对boss的血量伤害值</param>
        public FightVo GetFightVo(TGGSession session, Int64 rivalid, int type, ref Int64 bosslife, ref Int64 hurt)
        {
            var result = GetResult(session, rivalid, type, bosslife);
            if (result != ResultType.SUCCESS) return null;
            var fightrole = list_role.FirstOrDefault(m => m != null && m.user_id != session.Player.User.id);
            var _fightrole = list_role_hp.FirstOrDefault(m => m != null && m.user_id != session.Player.User.id);
            if (fightrole == null || _fightrole == null) return null;

            if (fightrole.hp <= 0)
                hurt = _fightrole.hp;
            else
                hurt = _fightrole.hp - bosslife;
            return vo;
        }

        /// <summary> 获取战斗结果，并得到boss血量 </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">rivalid  战斗部队Id</param>
        /// <param name="type">枚举FightType中的类型</param>
        /// <param name="bosslife">boss血量</param>
        public int GetBossLife(TGGSession session, Int64 rivalid, int type, ref Int64 bosslife)
        {
            var result = GetIsWin(session, rivalid, type, bosslife);
            if (result != (int)ResultType.SUCCESS) return Convert.ToInt32(result);

            var fightrole = list_role.FirstOrDefault(m => m != null && m.user_id != session.Player.User.id);
            if (fightrole == null) return Convert.ToInt32(ResultType.NO_DATA);
            bosslife = fightrole.hp;

            return result;
        }

        /// <summary> 得到战斗胜负 </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        /// <param name="playlife">玩家剩余血量</param>
        /// <returns>返回战斗结果</returns>
        public int GetIsWin(TGGSession session, Int64 rivalid, int type, ref Int64 playlife)
        {
            var result = GetIsWin(session, rivalid, type, playlife);
            if (result < 0) return result;

            var role = session.Player.Role.Kind;
            var fightrole = list_role.FirstOrDefault(m => m.id == role.id);
            if (fightrole == null) return Convert.ToInt32(ResultType.NO_DATA);
            playlife = fightrole.hp;
            return result;
        }

        /// <summary> 得到战斗胜负 </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        /// <param name="hp">血量</param>
        /// <returns>返回战斗结果</returns>
        public int GetIsWin(TGGSession session, Int64 rivalid, int type, Int64 hp)
        {
            var result = GetResult(session, rivalid, type, hp); //战斗处理结果
            if (result != ResultType.SUCCESS) return Convert.ToInt32(result);

            SendProtocol(session, new ASObject(BuildData(Convert.ToInt32(result), vo)));
            if (RivalFightType == Convert.ToInt32(FightType.BOTH_SIDES))
            {
                if (Variable.OnlinePlayer.ContainsKey(rivalid))
                {
                    var _session = Variable.OnlinePlayer[rivalid] as TGGSession;
                    SendProtocol(_session, new ASObject(BuildData(Convert.ToInt32(result), GetRivalFightVo())));
                }
            }
            var iswin = vo.isWin ? FightResultType.WIN : FightResultType.LOSE;
            return Convert.ToInt32(iswin);
        }

        #endregion

        /// <summary> 得到战斗执行结果 </summary>
        /// <param name="session">session</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="type"> 枚举FightType中的类型 </param>
        /// <param name="hp">血量</param>
        /// <returns>返回NPC执行战斗结果</returns>
        private ResultType GetResult(TGGSession session, Int64 rivalid, int type, Int64 hp)
        {
            if (rivalid == 0) return ResultType.FIGHT_RIVAL_ID_ERROR;
            var userid = session.Player.User.id;
            if (IsFight(userid)) return ResultType.FIGHT_FIGHT_IN; //检验玩家是否在战斗计算中
            RivalFightType = Convert.ToInt32(type);
            session.Fight.Rival = rivalid;

            var result = FightMethod(session, hp);  //战斗执行结果
            if (result != ResultType.SUCCESS)
            {
                XTrace.WriteLine("---- 战斗发生{0}错误:{1}  己方用户Id:{2}  对手Id:{3} ----", result, (int)result, session.Player.User.id, rivalid);
                RemoveFightState(userid);   //修改玩家战斗状态
                return result;
            }


            var iswin = vo.isWin ? FightResultType.WIN : FightResultType.LOSE;
            if (vo.isWin) WeaponCount(session.Player.User.id);  //调用武器使用统计
            TaskResult(session, iswin);          //检索玩家任务
            RemoveFightState(userid);            //玩家移除战斗队列
            return result;
        }

        /// <summary> 推送协议 </summary>
        private void SendProtocol(TGGSession session, ASObject aso)
        {
            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_ENTER, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>得到胜负结果</summary>
        private bool GetWinResult(decimal userid)
        {
            return (WIN == 0 && vo.attackId == Convert.ToDouble(userid)) || (WIN == 1 && vo.attackId != Convert.ToDouble(userid));
        }

        #region 套卡

        /// <summary>检索卡组  套卡</summary>
        private void SearchCardGroup()
        {
            var u1 = list_attack_role.FirstOrDefault(m => m != null);
            var u2 = list_defense_role.FirstOrDefault(m => m != null);
            if (u1 == null || u2 == null) return;
            if (u1.user_id != 0)
            {
                IsAttack = true;
                CardGroup(list_attack_role.Where(m => m != null).ToList());
            }
            if (u2.user_id == 0) return;
            IsAttack = false;
            CardGroup(list_defense_role.Where(m => m != null).ToList());
        }

        /// <summary> 套卡 </summary>
        private void CardGroup(List<FightRole> rolelist)
        {
            var list = new List<string>();

            foreach (var item in Variable.BASE_ROLEGROUP)
            {
                if (item.role1 != 0)
                {
                    if (rolelist.Count(m => m.baseId == item.role1) == 0) continue;
                }
                if (item.role2 != 0)
                {
                    if (rolelist.Count(m => m.baseId == item.role2) == 0) continue;
                }
                if (item.role3 != 0)
                {
                    if (rolelist.Count(m => m.baseId == item.role3) == 0) continue;
                }
                if (item.role4 != 0)
                {
                    if (rolelist.Count(m => m.baseId == item.role4) == 0) continue;
                }
                list.Add(item.effects);
            }

            if (!list.Any()) return;
            SkillEffect(list.Count() == 1 ? list.First() : GetNewString(list[0], list[1]), (int)SkillType.CARD);
        }

        /// <summary> 将2个技能效果组合成新的技能效果  同类型取最大值那条 </summary>
        private string GetNewString(string e1, string e2)
        {
            var effects1 = GetSkill(e1.Split('|'));
            var effects2 = GetSkill(e2.Split('|'));
            var types1 = effects1.Select(m => m.type).ToList();
            var types2 = effects2.Select(m => m.type).ToList();
            var types = types1.Where(types2.Contains).ToList(); //交集
            var effects3 = new List<FightSkillEffects>();
            foreach (var item in types)
            {
                var m1 = effects1.FirstOrDefault(m => m.type == item);
                var m2 = effects2.FirstOrDefault(m => m.type == item);
                if (m1 == null || m2 == null) continue;
                effects3.Add(m1.values < m2.values ? m2 : m1);
            }
            effects3.AddRange(types.Aggregate(effects1, (current, item) => current.Where(m => m.type != item).ToList()));
            effects3.AddRange(types.Aggregate(effects2, (current, item) => current.Where(m => m.type != item).ToList()));
            var strs = effects3.Select(item => item.type + "_" + item.target + "_" + item.range + "_" + item.round + "_" + item.values + "_" + item.robabilityValues).ToList();
            return string.Join("|", strs);
        }

        private List<FightSkillEffects> GetSkill(string[] list)
        {
            var ls = new List<FightSkillEffects>();
            foreach (var item in list[0].Split('|'))
            {
                var model = item.Split('_');
                if (model.Count() < 5) continue;
                ls.Add(new FightSkillEffects
                {
                    type = Convert.ToInt32(model[0]),
                    target = Convert.ToInt32(model[1]),
                    range = Convert.ToInt32(model[2]),
                    round = Convert.ToInt32(model[3]),
                    values = Convert.ToInt32(model[4]),
                    robabilityValues = model.Count() == 5 ? 100 : Convert.ToDouble(model[5])
                });
            }
            return ls;
        }

        #endregion

        #region 私有方法

        /// <summary> 获取对方战斗Vo </summary>
        private FightVo GetRivalFightVo()
        {
            var fightvo = vo.CloneEntity();
            fightvo.isWin = !fightvo.isWin;
            return fightvo;
        }

        /// <summary> 获取当前玩家剩余血量 </summary>
        private Int64 GetPlayHp(Int64 roleid)
        {
            var fightrole = list_role.FirstOrDefault(m => m.id == roleid);
            if (fightrole == null) return 0;
            return fightrole.hp;
        }

        /// <summary> 获取对Boss的伤害值 </summary>
        private Int64 GetBossHurt(Int64 userid)
        {
            var fightrole = list_role.FirstOrDefault(m => m != null && m.user_id != userid);
            var _fightrole = list_role_hp.FirstOrDefault(m => m != null && m.user_id != userid);
            if (fightrole == null || _fightrole == null) return 0;

            if (fightrole.hp <= 0) return _fightrole.hp;
            return _fightrole.hp - fightrole.hp;
        }

        /// <summary> 获取Boss剩余血量 </summary>
        private Int64 GetBossHp(Int64 userid)
        {
            var fightrole = list_role.FirstOrDefault(m => m != null && m.user_id != userid);
            return fightrole == null ? 0 : fightrole.hp;
        }

        #endregion
    }
}
