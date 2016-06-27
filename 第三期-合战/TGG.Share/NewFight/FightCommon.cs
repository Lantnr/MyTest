using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.SocketServer;
using FluorineFx;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;

namespace TGG.Share.NewFight
{
    public class FightCommon : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~FightCommon()
        {
            Dispose();
        }

        /// <summary> 要挑战的武将宅Id </summary>
        public int RoleHomeId = 0;

        /// <summary> 获取战斗实体 </summary>
        /// <param name="userid">当前玩家id</param>
        /// <param name="rivalid">对手id</param>
        /// <param name="type">战斗类型</param>
        /// <param name="hp">要调控血量 (爬塔、活动、连续战斗调用)</param>
        /// <param name="of">是否获取己方战斗Vo</param>
        /// <param name="po">是否推送己方战斗</param>
        /// <param name="or">是否获取对方战斗Vo</param>
        /// <param name="pr">是否推送对方战斗</param>
        /// <param name="rolehomeid">(武将宅类型时可用)要挑战武将宅id</param>
        /// <returns></returns>
        public Core.Entity.Fight GeFight(Int64 userid, Int64 rivalid, FightType type, Int64 hp = 0, bool of = false, bool po = false, bool or = false, bool pr = false, int rolehomeid = 0)
        {
            var entity = new Core.Entity.Fight();

            if (IsFight(userid))
            {
                entity.Result = ResultType.FIGHT_FIGHT_IN;
                return entity; //检验玩家是否在战斗计算中
            }

            RoleHomeId = rolehomeid; //只有武将宅战斗类型有用

            #region 组装己方

            if (!Variable.OnlinePlayer.ContainsKey(userid)) return new Core.Entity.Fight();
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null)
            {
                entity.Result = ResultType.CHAT_NO_ONLINE;
                return entity;
            }

            var roleid = session.Player.Role.Kind.id;
            if (session.Fight.Personal.id == 0) session.Fight.Personal =
               tg_fight_personal.PersonalInsert(userid, roleid);  //验证己方阵形 没有就插入阵

            var user = session.Player.User.CloneEntity();
            var personal = session.Fight.Personal.CloneEntity();
            personal = IsUpdatePersonal(roleid, personal, type);  //验证战斗类型  是否需要更变己方阵形
            var add = CheckActivity((int)type, session.Player.UserExtend.building_add_force, session.Player.UserExtend.siege_add_force);

            var bv = Variable.BASE_VOCATION.FirstOrDefault(m => m.vocation == user.player_vocation);
            if (bv == null)
            {
                entity.Result = ResultType.BASE_TABLE_ERROR;
                return entity;
            }
            var userfight = new FightUserEntity
            {
                user = user,
                isRaval = false,
                personal = personal,
                xiShu = Convert.ToInt32(bv.fight),
                yin = BuildPlayerYinEntity(personal.yid),
                roleEntity = BuildListRoleEntity(personal, add)
            };
            userfight.roleEntity = UpdatePlayerHp(roleid, hp, type, userfight.roleEntity); //验证战斗类型  是否需要改变己方武将的血量
            userfight = GetHireRole(userfight, type); //根据战斗类型  获取雇佣武将
            #endregion

            #region 组装对方数据

            var rivalfight = GetRivalFightUserEntity(rivalid, type);
            if (rivalfight == null)
            {
                entity.Result = ResultType.FIGHT_RIVAL_PERSONAL_ERROR;
                return new Core.Entity.Fight();
            }
            rivalfight.isRaval = true;
            rivalfight.roleEntity = UpdateRivalHp(hp, type, rivalfight.roleEntity); //验证战斗类型  是否需要更变对方武将血量

            #endregion


            var fight = new FightGlobalEntity();
            fight.buff = new List<BuffEntity>();
            fight.isAttack = true;

            #region 判定先后手

            switch (type)
            {
                case FightType.SIEGE:
                case FightType.BUILDING:
                    {
                        fight.attack = userfight;
                        fight.defense = rivalfight;
                        break;
                    }
                default:
                    {
                        var o = userfight.roleEntity.Sum(m => m.attack); //己方总攻击
                        var r = rivalfight.roleEntity.Sum(m => m.attack);//对方总攻击
                        if (o > r)
                        {
                            fight.attack = userfight;
                            fight.defense = rivalfight;
                        }
                        else
                        {
                            fight.attack = rivalfight;
                            fight.defense = userfight;
                        }
                        break;
                    }
            }

            #endregion

            fight.attack.isAttack = true;
            fight.defense.isAttack = false;

            #region 组装战斗Vo

            var vo = new FightVo
            {
                attackerVo = fight.attack.user == null
                    ? ToFightPlayerVo()
                    : ToFightPlayerVo(fight.attack.user),
                hitterVo = fight.defense.user == null
                    ? ToFightPlayerVo()
                    : ToFightPlayerVo(fight.defense.user),
                srcType = Convert.ToInt32(type),
                yinA = fight.attack.yin == null ? null : EntityToVo.ToFightYinVo(fight.attack.yin.yin),
                yinB = fight.defense.yin == null ? null : EntityToVo.ToFightYinVo(fight.defense.yin.yin),
            };

            #region 卡组加成

            if (fight.attack.user != null)
            {

                vo.buffVoA = ToBuffVos(CardGroupEffect(fight));
            }
            if (fight.defense.user != null)
            {
                fight = UpdateGlobalEntity(fight);
                vo.buffVoB = ToBuffVos(CardGroupEffect(fight));
                fight = UpdateGlobalEntity(fight);
            }
            #endregion

            var flag = fight.attack.isRaval;
            var onsllist = flag ? fight.defense.roleEntity : fight.attack.roleEntity;
            var rivallist = flag ? fight.attack.roleEntity : fight.defense.roleEntity;

            try
            {
                vo.moves = (BuildProcess(fight));
            }
            catch (Exception ex)
            {
                RemoveFightState(userid);                   //玩家移除战斗队列
                XTrace.WriteException(ex);
                entity.Result = ResultType.FIGHT_ERROR;
                return entity;
            }
            //vo.moves = (BuildProcess(fight));
            vo.isWin = fight.isAttackWin;
            entity.Iswin = vo.isWin;
            entity.Result = ResultType.SUCCESS;
            entity.BoosHp = GetBossHp(rivallist);
            entity.Hurt = GetBossHurt(rivallist);
            entity.PlayHp = GetPlayHp(roleid, onsllist);
            entity.ShotCount = vo.moves.Sum(m => m.Count());
            UpdateWin(vo, entity.PlayHp, roleid, type);
            #endregion

            if (vo.isWin) WeaponCount(userid);          //调用武器使用统计
            RemoveFightState(userid);                   //玩家移除战斗队列

            #region MyRegion

            if (of) entity.Ofight = vo;    //获取己方战斗Vo
            if (po) SendProtocol(session, new ASObject(BuildData(Convert.ToInt32(entity.Result), vo))); //send己方战斗Vo给自己
            if (or || pr)   //获取敌方战斗Vo
            {
                var rivalVo = vo.CloneEntity();
                rivalVo.isWin = !rivalVo.isWin;
                entity.Rfight = rivalVo;
            }
            if (!pr) return entity;
            if (type != FightType.BOTH_SIDES || type != FightType.ONE_SIDE) return entity;
            var s = Variable.OnlinePlayer[rivalid] as TGGSession;
            SendProtocol(s, new ASObject(BuildData(Convert.ToInt32(entity.Result), entity.Rfight)));

            #endregion

            return entity;
        }

        /// <summary>
        /// 验证活动战斗是否增加攻击力
        /// </summary>
        private int CheckActivity(int type, int buildingAdd, int siegeAdd)
        {
            if (type == (int)FightType.BUILDING)
            {
                if (buildingAdd == 0) return 0;
                var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == "26014");
                if (baseinfo == null) return 0;
                return Convert.ToInt32(baseinfo.value);
            }
            if (type != (int)FightType.SIEGE) return 0;
            if (siegeAdd == 0) return 0;
            var baseinfo1 = Variable.BASE_RULE.FirstOrDefault(q => q.id == "26014");
            if (baseinfo1 == null) return 0;
            return Convert.ToInt32(baseinfo1.value);
        }



        #region 获取雇佣武将

        private FightUserEntity GetHireRole(FightUserEntity userEntity, FightType type)
        {
            #region 执行方法的条件

            switch (type)
            {
                case FightType.NPC_MONSTER:
                case FightType.NPC_FIGHT_ARMY: { break; }
                default: { return userEntity; }
            }

            #endregion

            var p = userEntity.personal;
            if (p.matrix1_rid == 0)
            {
                var role = GetHireRole(p.user_id, 1);
                if (role == null) return userEntity;
                p.matrix1_rid = role.id;
                userEntity.roleEntity.Add(role);
                return userEntity;
            }
            if (p.matrix2_rid == 0)
            {
                var role = GetHireRole(p.user_id, 2);
                if (role == null) return userEntity;
                p.matrix2_rid = role.id;
                userEntity.roleEntity.Add(role);
                return userEntity;
            }
            if (p.matrix3_rid == 0)
            {
                var role = GetHireRole(p.user_id, 3);
                if (role == null) return userEntity;
                p.matrix3_rid = role.id;
                userEntity.roleEntity.Add(role);
                return userEntity;
            }
            if (p.matrix4_rid == 0)
            {
                var role = GetHireRole(p.user_id, 4);
                if (role == null) return userEntity;
                p.matrix4_rid = role.id;
                userEntity.roleEntity.Add(role);
                return userEntity;
            }
            if (p.matrix5_rid == 0)
            {
                var role = GetHireRole(p.user_id, 5);
                if (role == null) return userEntity;
                p.matrix5_rid = role.id;
                userEntity.roleEntity.Add(role);
                return userEntity;
            }
            return userEntity;
        }

        /// <summary> 获取雇佣武将 </summary>
        private RoleEntity GetHireRole(Int64 userid, int weizhi)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return null;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return null;
            var hireid = session.Player.UserExtend.hire_id;
            if (hireid == 0) return null;
            var hire = Variable.BASE_NPCHIRE.FirstOrDefault(m => m.id == hireid);
            if (hire == null) return null;
            var npc = Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == hire.npcRoleId);
            if (npc == null) return null;
            var role = ConvertNpcRoleFight(npc, weizhi);
            if (role == null) return null;
            role.user_id = userid;
            role.monsterType = (int)FightRivalType.MONSTER;
            role.id = RNG.Next();
            return role;
        }

        #endregion

        #region 卡组

        /// <summary> 套卡 </summary>
        private List<BuffEntity> CardGroupEffect(FightGlobalEntity fight)
        {
            var effectList = new List<SkillEffects>();
            var list = GetBaseEffects(fight.attack.roleEntity);
            if (!list.Any()) return new List<BuffEntity>();
            if (list.Count() == 1)
            {
                var str = list.FirstOrDefault();
                effectList = BuildSkillEffects(str);
            }
            else
                effectList = GetMaxSkillEffects(list[0], list[1]);
            return AddOverBuff(fight, effectList);
        }

        /// <summary> 获取符合条件的套卡效果 </summary>
        /// <param name="rolelist">战斗武将集合</param>
        private List<string> GetBaseEffects(List<RoleEntity> rolelist)
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
            return list;
        }

        /// <summary> 将2个技能效果组合成新的技能效果  同类型取最大值那条 </summary>
        private List<SkillEffects> GetMaxSkillEffects(string e1, string e2)
        {
            var effects1 = BuildSkillEffects(e1);
            var effects2 = BuildSkillEffects(e2);
            var types1 = effects1.Select(m => m.type).ToList();
            var types2 = effects2.Select(m => m.type).ToList();
            var types = types1.Where(types2.Contains).ToList(); //交集
            var effects3 = new List<SkillEffects>();
            foreach (var item in types)
            {
                var m1 = effects1.FirstOrDefault(m => m.type == item);
                var m2 = effects2.FirstOrDefault(m => m.type == item);
                if (m1 == null || m2 == null) continue;
                effects3.Add(m1.values < m2.values ? m2 : m1);
            }
            effects3.AddRange(types.Aggregate(effects1, (current, item) => current.Where(m => m.type != item).ToList()));
            effects3.AddRange(types.Aggregate(effects2, (current, item) => current.Where(m => m.type != item).ToList()));
            return effects3;
        }

        /// <summary> 增加受技能影响的Buff </summary>
        /// <param name="fight"></param>
        /// <param name="list"></param>
        /// <returns>受影响的目标集合</returns>
        private List<BuffEntity> AddOverBuff(FightGlobalEntity fight, IEnumerable<SkillEffects> list)
        {
            var ls = new List<BuffEntity>();
            foreach (var item in list)
            {
                if (!IsTrue(item.robabilityValues)) continue;
                ls.Add(BuildBuffEntity(item, 0, 2));
                if (item.target == 1)
                {
                    foreach (var role in fight.attack.roleEntity)
                    {
                        var buff = BuildBuffEntity(item, role.id, 2);
                        buff.rid = role.id;
                        fight.buff.Add(buff);
                        AddStrength(role, item.type, Convert.ToInt32(item.values));
                    }
                    AddYinCount(fight.attack.yin, item.type, Convert.ToInt32(item.values));
                }
                else
                {
                    foreach (var role in fight.defense.roleEntity)
                    {
                        var buff = BuildBuffEntity(item, role.id, 2);
                        fight.buff.Add(buff);
                        AddStrength(role, item.type, Convert.ToInt32(item.values));
                    }
                    AddYinCount(fight.defense.yin, item.type, Convert.ToInt32(item.values));
                }
            }
            return ls;
        }

        #endregion

        #region 战斗过程

        /// <summary> 武将当前出手次数  </summary>
        private Dictionary<long, int> dic_round = new Dictionary<long, int>();

        /// <summary> 当前回合数 </summary>
        private int Round = 0;

        private List<List<MovesVo>> BuildProcess(FightGlobalEntity fight)
        {
            var sign = false;
            dic_round = new Dictionary<long, int>();
            var list = new List<List<MovesVo>>();
            dic_round = InitRoleShot(fight);

            for (; ; )
            {
                var moves = new List<MovesVo>();

                #region 初始武将

                if (!sign)
                {
                    moves.Add(BuildMovesVo(fight, new List<double>()));
                    fight.attack.attackRole = fight.attack.roleEntity.OrderBy(m => m.initweizhi).FirstOrDefault();
                    fight.defense.attackRole = fight.defense.roleEntity.OrderBy(m => m.initweizhi).FirstOrDefault();
                    fight.attack.defenseRole = fight.attack.roleEntity.OrderBy(m => m.weizhi).FirstOrDefault();
                    fight.defense.defenseRole = fight.defense.roleEntity.OrderBy(m => m.weizhi).FirstOrDefault();
                    sign = true;
                }

                #endregion

                #region 验证初始数据

                var role = fight.attack.attackRole;

                if (role == null) return new List<List<MovesVo>>();
                if (role.hp <= 0)
                {
                    fight = GetNextRole(fight);
                    role = fight.attack.attackRole;
                    if (role == null) return new List<List<MovesVo>>();
                }

                var flag = RoundCount(role.id, fight);
                if (!flag)
                {
                    fight = UpdateGlobalEntity(fight);
                    continue;
                }

                fight.attack.yin.count++;

                #endregion

                #region 检索眩晕跟封印

                var roleBuff = fight.buff.Where(m => m.rid == role.id).ToList();
                var isExist = roleBuff.Count(m => m.type == FightingSkillType.DIZZINESS) > 0;
                if (isExist)
                {
                    var move = BuildMovesVo(fight, new List<double>());
                    moves.Add(move);
                    list.Add(moves);
                    fight = UpdateWeiZhi(fight);
                    fight = UpdateGlobalEntity(fight);
                    continue;
                }
                isExist = roleBuff.Count(m => m.type == FightingSkillType.SEAL) > 0;
                if (isExist) goto attack;

                #endregion

                #region 是否触发奥义

                var ms = IsMystery(fight);
                if (ms.Any())
                {
                    moves.AddRange(ms);
                    goto Yin;
                }

                #endregion

                #region 是否出发秘技

                ms = IsSkill(fight);                                         //是否释放秘技
                if (ms.Any())
                {
                    moves.AddRange(ms);
                    goto Yin;
                }

                #endregion

            attack:
                ms = IsRenZheZong(fight);                                    //是否释放忍者众
                if (ms.Any())
                {
                    moves.AddRange(ms);
                    goto Yin;
                }

                moves.Add(NormalAttack(fight));                             //普通攻击
            Yin:
                ms = IsYin(fight);                                          //是否释放印技能
                if (ms.Any()) moves.AddRange(ms);
                list.Add(moves);

                if (fight.isResult) return list;

                fight = UpdateWeiZhi(fight);

                fight = UpdateGlobalEntity(fight);
            }

            return list;
        }

        #region 触发印

        /// <summary> 是否触发印</summary>
        /// <param name="fight">战斗实体</param>
        /// <param name="move"></param>
        private List<MovesVo> IsYin(FightGlobalEntity fight)
        {
            var yin = fight.attack.yin;
            if (yin == null) return new List<MovesVo>();
            if (yin.yin.yin_id == 0) return new List<MovesVo>();
            if (yin.count < yin.yinCount) return new List<MovesVo>();
            yin.count -= yin.yinCount;
            return SkillRelease(fight, SkillType.YIN);
        }

        #endregion

        #region 触发奥义

        /// <summary> 是否释放奥义</summary>
        /// <param name="fight"></param>
        private List<MovesVo> IsMystery(FightGlobalEntity fight)
        {
            var attackrole = fight.attack.attackRole;
            if (attackrole.mystery == null) return new List<MovesVo>();
            double hpcount = Convert.ToDouble(attackrole.initHp) / Convert.ToDouble(attackrole.hp);
            var probability = attackrole.mystery_probability;
            if (hpcount <= 0.5) probability = probability + (60 - (hpcount * 100));

            if (attackrole.angerCount < attackrole.mystery.energy) return new List<MovesVo>();//气力验证

            if (IsTrue(probability)) return SkillRelease(fight, SkillType.MEANING);
            return new List<MovesVo>();
        }

        #region 是否出发秘技

        private List<MovesVo> IsSkill(FightGlobalEntity fight)
        {
            var attackrole = fight.attack.attackRole;
            if (attackrole.cheatCode == null) return new List<MovesVo>();

            if (attackrole.angerCount < attackrole.cheatCode.energy) return new List<MovesVo>();//气力验证
            attackrole.angerCount -= attackrole.cheatCode.energy;
            return SkillRelease(fight, SkillType.ESOTERICA);
        }

        #endregion

        #region 技能共享方法

        /// <summary> 释放技能 </summary>
        /// <param name="fight"></param>
        /// <param name="type"> 要释放的技能类型 </param>
        private List<MovesVo> SkillRelease(FightGlobalEntity fight, SkillType type)
        {
            var skill = new SkillEntity();
            var list = new List<MovesVo>();
            var isRz = false;
            #region MyRegion

            switch (type)
            {
                case SkillType.MEANING:
                    {
                        skill = fight.attack.attackRole.mystery;
                        break;
                    }
                case SkillType.ESOTERICA:
                    {
                        skill = fight.attack.attackRole.cheatCode;
                        break;
                    }
                case SkillType.RZMJ1:
                    {
                        skill = fight.attack.attackRole.art_ninja_cheat_code1; isRz = true;
                        break;
                    }
                case SkillType.RZMJ2:
                    {
                        skill = fight.attack.attackRole.art_ninja_cheat_code2; isRz = true;
                        break;
                    }
                case SkillType.RZMJ3:
                    {
                        skill = fight.attack.attackRole.art_ninja_cheat_code3; isRz = true;
                        break;
                    }
                case SkillType.RZAY:
                    {
                        skill = fight.attack.attackRole.art_ninja_mystery; isRz = true;
                        break;
                    }
                case SkillType.YIN:
                    {
                        skill = new SkillEntity
                        {
                            skillEffect = fight.attack.yin.yinEffect,
                            isQuickAttack = 0,
                        };
                        break;
                    }
                default: { return new List<MovesVo>(); }
            }

            #endregion

            var hitIds = AddBuff(fight, skill.skillEffect, type);
            list.Add(BuildMovesVo(fight, hitIds, type));
            if (skill.isQuickAttack == 0) return list; //1为当前回合攻击  0为当前回合不攻击
            var isAll = skill.attackRange == (int)EffectRangeType.ALL;
            list.AddRange(SkillAttack(fight, isAll, isRz));
            return list;
        }

        /// <summary> 增加受技能影响的Buff </summary>
        /// <param name="fight"></param>
        /// <param name="list"></param>
        /// <returns>受影响的目标集合</returns>
        private List<double> AddBuff(FightGlobalEntity fight, IEnumerable<SkillEffects> list, SkillType type)
        {
            var ids = new List<double>();
            foreach (var item in list)
            {
                switch (type)
                {
                    case SkillType.RZMJ1:
                    case SkillType.RZMJ2:
                    case SkillType.RZMJ3:
                    case SkillType.RZAY: { break; }
                    default:
                        {
                            if (!IsTrue(item.robabilityValues)) continue;
                            break;
                        }
                }
                var ad = item.target == 1 ? fight.attack : fight.defense;
                if (item.range == 1)
                {
                    var role = item.target == 1 ? fight.attack.attackRole : fight.defense.defenseRole;
                    CheckBuff(fight, item, role.id);
                    ids.Add(role.id);
                    var value = Convert.ToInt32(item.values);
                    AddStrength(role, item.type, value);
                    AddYinCount(ad.yin, item.type, value);
                }
                else
                {
                    if (item.target == 1)
                    {
                        foreach (var role in fight.attack.roleEntity)
                        {
                            CheckBuff(fight, item, role.id);
                            ids.Add(role.id);
                            AddStrength(role, item.type, Convert.ToInt32(item.values));
                        }
                        AddYinCount(ad.yin, item.type, Convert.ToInt32(item.values));
                    }
                    else
                    {
                        foreach (var role in fight.defense.roleEntity)
                        {
                            CheckBuff(fight, item, role.id);
                            ids.Add(role.id);
                            AddStrength(role, item.type, Convert.ToInt32(item.values));
                        }
                        AddYinCount(ad.yin, item.type, Convert.ToInt32(item.values));
                    }
                }
            }
            return ids;
        }

        /// <summary> 检查武将buff是否重复 重复覆盖 否则增加 </summary>
        /// <param name="fight">战斗全局实体</param>
        /// <param name="item">技能效果实体</param>
        /// <param name="rid">武将主键Id</param>
        private void CheckBuff(FightGlobalEntity fight, SkillEffects item, Int64 rid)
        {
            var bf = fight.buff.FirstOrDefault(m => m.rid == rid && m.bufftype == 1 && m.type == (FightingSkillType)item.type && m.count != 0);
            if (bf == null)
            {
                var buff = BuildBuffEntity(item, rid, 0);
                if (buff.count != 0) buff.count += Round;
                fight.buff.Add(buff);
            }
            else
            {
                bf.count = Round + item.round;
                bf.buffValue = item.values;
                bf.bufftype = 0;
            }
        }

        /// <summary> 组装Buff实体 </summary>
        /// <param name="item">SkillEffects 技能效果实体</param>
        /// <param name="rid"> 武将主键Id </param>
        /// <param name="buffType">Buff类型</param>
        private BuffEntity BuildBuffEntity(SkillEffects item, Int64 rid, int buffType)
        {
            return new BuffEntity
            {
                rid = rid,
                count = item.round,
                bufftype = buffType,
                buffValue = item.values,
                type = (FightingSkillType)item.type,
                IsOk = true,
            };
        }

        /// <summary> 增加武将气力 </summary>
        /// <param name="yin">RoleEntity 要增加的武将实体</param>
        /// <param name="type">战斗技能效果</param>
        /// <param name="values">要增加的值</param>
        private void AddStrength(RoleEntity role, int type, int values)
        {
            switch (type) //技能效果类型
            {
                case (int)FightingSkillType.INCREASES_STRENGTH:
                    {
                        role.angerCount += values;
                        if (role.angerCount > 8) role.angerCount = 8;
                        break;
                    }//增加气力
                case (int)FightingSkillType.REDUCE_STRENGTH:
                    {
                        role.angerCount -= values;
                        if (role.angerCount < 0) role.angerCount = 0;
                        break;
                    }//减少气力
                default: { break; }
            }
        }

        /// <summary> 增加印数 </summary>
        /// <param name="yin">YinEntity 印实体</param>
        /// <param name="type">战斗技能效果</param>
        /// <param name="values">要增加的值</param>
        private void AddYinCount(YinEntity yin, int type, int values)
        {
            switch (type)
            {
                case (int)FightingSkillType.INCREASES_YINCOUNT:
                    {
                        yin.count += values;
                        break;
                    }//增加印计数
                case (int)FightingSkillType.REDUCE__YINCOUNT:
                    {
                        yin.count -= values;
                        break;
                    }//减少印计数
                default: { break; }
            }
        }

        #endregion

        #endregion

        #region 位置变动

        #region 当前更换攻击方
        /// <summary> 攻击方 防守方 调换 </summary>
        /// <param name="fight"></param>
        /// <returns></returns>
        private FightGlobalEntity UpdateGlobalEntity(FightGlobalEntity fight)
        {
            fight.isAttack = !fight.isAttack;
            var attack = fight.attack;
            fight.attack = new FightUserEntity();
            fight.attack = fight.defense;
            fight.defense = new FightUserEntity();
            fight.defense = attack;
            return fight;
        }

        #endregion

        /// <summary> 修改武将的位置 </summary>
        /// <param name="fight"></param>
        /// <returns></returns>
        private FightGlobalEntity UpdateWeiZhi(FightGlobalEntity fight)
        {
            var role = fight.attack.attackRole;
            if (role == null) return fight;
            var weizhi = role.initweizhi;
            for (var i = 1; i <= 5; i++)
            {
                if (weizhi == 5) weizhi = 1;
                else weizhi += 1;
                fight.attack.attackRole = fight.attack.roleEntity.FirstOrDefault(m => m.initweizhi == weizhi && m.hp > 0);
                if (fight.attack.attackRole != null)
                {
                    foreach (var item in fight.attack.roleEntity)
                    {
                        if (item == null) continue;
                        item.weizhi = GetWeiZhi(item.weizhi);
                    }
                    fight.attack.defenseRole = fight.attack.roleEntity.Where(n => n.hp > 0)
                        .OrderBy(m => m.weizhi).FirstOrDefault();
                    //fight.defense.defenseRole = fight.defense.roleEntity.Where(n => n.hp > 0)
                    //                     .OrderBy(m => m.weizhi).FirstOrDefault();
                    return fight;
                }
            }
            return fight;
        }

        /// <summary> 获取位置规则 </summary>
        /// <param name="weizhi">当前位置</param>
        private int GetWeiZhi(int weizhi)
        {
            switch (weizhi)
            {
                case 1: { return 5; }
                case 2: { return 1; }
                case 3: { return 2; }
                case 4: { return 3; }
                case 5: { return 4; }
                default: { return 0; }
            }
        }

        /// <summary> 获取下一次操作的武将 </summary>
        /// <param name="fight"></param>
        private FightGlobalEntity GetNextRole(FightGlobalEntity fight)
        {
            var role = fight.attack.attackRole;
            if (role == null) return fight;
            var weizhi = role.initweizhi;
            for (var i = 1; i <= 5; i++)
            {
                if (weizhi == 5) weizhi = 1;
                else weizhi += 1;
                fight.attack.attackRole = fight.attack.roleEntity.FirstOrDefault(m => m.initweizhi == weizhi && m.hp > 0);
                if (fight.attack.attackRole == null) continue;

                fight.attack.defenseRole = fight.attack.roleEntity.Where(n => n.hp > 0)
                  .OrderBy(m => m.weizhi).FirstOrDefault();
                //fight.defense.defenseRole = fight.defense.roleEntity.Where(n => n.hp > 0)
                //                     .OrderBy(m => m.weizhi).FirstOrDefault();
                return fight;
            }
            return fight;
        }

        #endregion

        #region 是否触发忍者众

        private List<MovesVo> IsRenZheZong(FightGlobalEntity fight)
        {
            var attackrole = fight.attack.attackRole;
            if (attackrole.art_ninja_mystery != null)
            {
                double hpcount = Convert.ToDouble(attackrole.initHp) / Convert.ToDouble(attackrole.hp);
                var probability = attackrole.mystery_probability;
                if (hpcount <= 0.5) probability = probability + (60 - (hpcount * 100));
                if (IsTrue(probability)) return SkillRelease(fight, SkillType.RZAY);
            }
            if (attackrole.art_ninja_cheat_code1 == null) return new List<MovesVo>();
            {
                var skill = attackrole.art_ninja_cheat_code1.skillEffect.FirstOrDefault();
                if (IsTrue(skill.robabilityValues))
                    return SkillRelease(fight, SkillType.RZMJ1);
            }
            if (attackrole.art_ninja_cheat_code2 == null) return new List<MovesVo>();
            {
                var skill = attackrole.art_ninja_cheat_code2.skillEffect.FirstOrDefault();
                if (IsTrue(skill.robabilityValues))
                    return SkillRelease(fight, SkillType.RZMJ2);
            }
            if (attackrole.art_ninja_cheat_code3 == null) return new List<MovesVo>();
            {
                var skill = attackrole.art_ninja_cheat_code3.skillEffect.FirstOrDefault();
                if (IsTrue(skill.robabilityValues))
                    return SkillRelease(fight, SkillType.RZMJ3);
            }
            return new List<MovesVo>();
        }

        #endregion

        #region 普通攻击

        /// <summary> 技能后的攻击 </summary>
        /// <param name="fight"></param>
        /// <param name="move"></param>
        /// <param name="flag">是否全体攻击</param>
        /// <returns></returns>
        private IEnumerable<MovesVo> SkillAttack(FightGlobalEntity fight, bool flag, bool isRz = false)
        {
            var sgin = false;
            var list = new List<MovesVo>();
            if (isRz)
                if (fight.attack.attackRole.angerCount < 8) fight.attack.attackRole.angerCount = fight.attack.attackRole.angerCount + 1;//普通攻击气
            if (!flag)
            {
                var ids = NormalAttack(fight, fight.attack.attackRole, fight.defense.defenseRole, ref sgin);
                foreach (var item in fight.buff.Where(m => m.count == 0 && m.IsOk))
                {
                    if (item.type == FightingSkillType.CRIT || item.type == FightingSkillType.DODGE) continue;
                    item.IsOk = false;
                }
                var temp = BuildMovesVo(fight, ids);
                list.Add(temp);
                fight.defense.defenseRole.damage = 0;
                if (!sgin) return list;
                IsResultWin(fight);
            }
            else
            {
                var ids = new List<double>();
                foreach (var role in fight.defense.roleEntity)
                {
                    var _sgin = false;
                    var id = NormalAttack(fight, fight.attack.attackRole, role, ref _sgin, true);
                    ids.AddRange(id);
                    if (_sgin) sgin = true;
                }
                foreach (var item in fight.buff.Where(m => m.count == 0 && m.IsOk))
                {
                    if (item.type == FightingSkillType.CRIT || item.type == FightingSkillType.DODGE) continue;
                    item.IsOk = false;
                }
                var temp = BuildMovesVo(fight, ids);
                list.Add(temp);
                foreach (var role in fight.defense.roleEntity) //清除上一次的伤害
                {
                    role.damage = 0;
                }
                if (sgin) IsResultWin(fight);
            }
            return list;
        }

        /// <summary> 单体普通攻击 增加气力值</summary>
        private MovesVo NormalAttack(FightGlobalEntity fight)
        {
            var sgin = false;
            var attack = fight.attack;
            var defense = fight.defense;

            if (attack.attackRole.angerCount < 8) attack.attackRole.angerCount = attack.attackRole.angerCount + 1;//普通攻击气力加1
            var ids = NormalAttack(fight, attack.attackRole, defense.defenseRole, ref sgin);
            var temp = BuildMovesVo(fight, ids);
            defense.defenseRole.damage = 0;
            if (!sgin) return temp;
            IsResultWin(fight);
            return temp;
        }

        /// <summary> 是否结算胜负 </summary>
        /// <returns></returns>
        private static void IsResultWin(FightGlobalEntity fight)
        {
            var b = fight.defense.roleEntity.Count(m => m.hp > 0) > 0;
            if (b) return;
            fight.isResult = true;
            fight.isAttackWin = fight.defense.isRaval;
        }

        /// <summary> </summary>
        /// <param name="fight"></param>
        /// <param name="attack"></param>
        /// <param name="defense"></param>
        /// <param name="move"></param>
        /// <param name="flag">当前防守方是否有死亡</param>
        /// <param name="isAll">是否群攻</param>
        /// <returns></returns>
        private List<double> NormalAttack(FightGlobalEntity fight, RoleEntity attack, RoleEntity defense, ref bool flag, bool isAll = false)
        {
            var ids = new List<double>();
            if (!IsTrue(attack.IgnoreDuck + GetValue(fight.buff, attack.id, FightingSkillType.IGNORE_DUCK_PROBABILITY)))
                if (IsTrue(defense.dodgeProbability + GetValue(fight.buff, defense.id, FightingSkillType.DODGE)))
                {
#if DEBUG
                    XTrace.WriteLine(string.Format("{0} {1}  {2}", "被打方Id", defense.id, "触发闪避 "));
#endif
                    fight.buff.Add(new BuffEntity
                    {
                        count = 0,
                        IsOk = true,
                        bufftype = 0,
                        buffValue = 0,
                        rid = defense.id,
                        type = FightingSkillType.DODGE,
                    });
                    ids.Add(defense.id);
                    return ids;
                }

            var isBaoJi = false;
            var count = DamageCount(attack, defense, fight.buff, fight.attack.xiShu, ref isBaoJi);
            if (isAll) count = Convert.ToInt32(Convert.ToDouble(count) * 0.3); //群攻系数
            defense.hp = defense.hp - count;                                   //修改防守方属性
            if (defense.hp <= 0)
            {
                flag = true;
                dic_round.Remove(defense.id);
                var weizhi = defense.weizhi;
                var isNext = false;
                for (var i = 1; i <= 5; i++)
                {
                    if (isNext) continue;
                    if (weizhi == 5) weizhi = 1;
                    else weizhi += 1;
                    fight.defense.defenseRole = fight.defense.roleEntity.Where(q => q != null).FirstOrDefault(m => m.weizhi == weizhi && m.hp > 0);
                    if (fight.defense.defenseRole == null)
                    {
                        if (i == 5) fight.defense.defenseRole = defense;
                        continue;
                    }
                    isNext = true;
                }
            }

            ids.Add(defense.id);                                       //防守武将id加入受击武将id

            if (isBaoJi)
            {
                fight.buff.Add(new BuffEntity
                {
                    count = 0,
                    IsOk = true,
                    bufftype = 0,
                    rid = defense.id,
                    buffValue = -count,
                    type = FightingSkillType.CRIT,
                });
            }
            else
                defense.damage = count;
            return ids;
        }

        /// <summary> 获取指定武将身上指定buff类型的值 </summary>
        /// <param name="list">buff集合</param>
        /// <param name="roleId">武将主键Id</param>
        /// <param name="type">战斗技能效果类型</param>
        /// <returns></returns>
        private double GetValue(List<BuffEntity> list, Int64 roleId, FightingSkillType type)
        {
            var number = 0.0;
            var ls = list.Where(m => m.type == type && m.rid == roleId);
            foreach (var item in ls)
            {
                number += item.buffValue;
            }
            return number;
        }

        #region 伤害公式计算

        /// <summary>伤害计算</summary>
        /// <param name="attack">攻击武将</param>
        /// <param name="defense">防守武将</param>
        /// <param name="buff"></param>
        /// <param name="xishu">战斗系数</param>
        /// <param name="flag">是否暴击</param>
        private Int64 DamageCount(RoleEntity attack, RoleEntity defense, List<BuffEntity> buff, double xishu, ref bool flag)
        {

            //A属性 = A基础属性+A装备属性+A生活技能属性+A称号属性
            //A总攻击 = A武力*1.5+A装备攻击力+A效果之增加攻击力-A效果之减少攻击力
            //B总防御 = B装备防御力+B效果之增加防御力-B效果之减少防御力
            //A会心几率 = A魅力/25 *0.01 +A效果之会心几率
            //A会心效果 = 150% + （A政务/15）*0.01 +A效果之会心效果
            //B闪避几率 = （B智谋/28）*0.01 +B效果之会心效果
            //A伤害加成 = A效果之增加伤害+A装备增加伤害
            //B减伤 = B装备减伤 + B效果之减伤
            //B生命 = B基础生命+B装备生命+B技能生命

            //攻击伤害 = （A总攻击-B总防御）*（1-B减伤）*（100% + A会心成功值*(A会心效果-100%）)*（100% + A伤害加成）

            var attackrole = AddBuffValueByRole(attack, buff);
            var defenserole = AddBuffValueByRole(defense, buff);

            var A0 = attackrole.attack;                  //A总攻击
            var A1 = defenserole.defense;                //B总防御
            var A2 = attackrole.critProbability;         //A会心几率
            flag = IsTrue(A2);                           //是否暴击
            var A3 = flag ? 1.5 + (attackrole.critAddition / 100) : 1.0;  //A会心效果
            var A5 = A0 - A1;                            //A总攻击-B总防御
            var A6 = A5 > 0 ? A5 : 1;                    //A总攻击-B总防御
            var A7 = 1 + (attackrole.hurtIncrease / 100);//增伤
            var A10 = (A6) * ((100 - defenserole.hurtReduce) / 100) * A3 * A7; //计算出来的伤害

            double number = 1;
            var n = Round - 21;
            if (n >= 0)
            {
                var b = n / 10;
                if (b == 0)
                    number = number + 1;
                if (b > 0)
                    number = number + (1 + n * 0.5);
            }
            if (xishu == 0) xishu = 1;
            var count = A10 * number * xishu;

            return count < 0 ? 1 : Convert.ToInt64(count);
        }

        /// <summary> 给武将增加当前回合自身buff效果 </summary>
        /// <param name="role">要操作的武将</param>
        /// <param name="buff">buff集合</param>
        private static RoleEntity AddBuffValueByRole(RoleEntity role, IEnumerable<BuffEntity> buff)
        {
            var m = role.CloneEntity();
            var list = buff.Where(q => q.rid == m.id).ToList();
            foreach (var item in list)
            {
                if (!item.IsOk) continue;
                switch (item.type)
                {
                    #region 增加Buff属性

                    case FightingSkillType.INCREASE_ATTACK:
                        {
                            var count = m.attack + item.buffValue;
                            m.attack = count < 0 ? 0 : count;
                            break;
                        } //增加攻击
                    case FightingSkillType.REDUCE_ATTACK:
                        {
                            var count = m.attack - item.buffValue;
                            m.attack = count < 0 ? 0 : count;
                            break;
                        }//减少攻击
                    case FightingSkillType.INCREASE_DEFENSE:
                        {
                            var count = m.defense + item.buffValue;
                            m.defense = count < 0 ? 0 : count;
                            break;
                        }//增加防御
                    case FightingSkillType.REDUCE_DEFENSE:
                        {
                            var count = m.defense - item.buffValue;
                            m.defense = count;
                            //m.defense = count < 0 ? 0 : count;
                            break;
                        }//减少防御
                    case FightingSkillType.KMOWING_PROBABILITY:
                        {
                            var count = m.critProbability + item.buffValue;
                            m.critProbability = count < 0 ? 0 : count;
                            break;
                        }//会心几率
                    case FightingSkillType.INCREASE_KMOWING:
                        {
                            var count = m.critAddition + item.buffValue;
                            m.critAddition = count < 0 ? 0 : count;
                            break;
                        }//增加会心效果
                    case FightingSkillType.DUCK_PROBABILITY:
                        {
                            var count = m.dodgeProbability + item.buffValue;
                            m.dodgeProbability = count < 0 ? 0 : count;
                            break;
                        }//闪避率
                    case FightingSkillType.IGNORE_DUCK_PROBABILITY:
                        {
                            var count = m.IgnoreDuck + item.buffValue;
                            m.IgnoreDuck = count < 0 ? 0 : count;
                            break;
                        }//无视闪避几率
                    case FightingSkillType.INCREASES_DAMAGE_PERCENTAGE:
                        {
                            var count = m.hurtIncrease + item.buffValue;
                            m.hurtIncrease = count < 0 ? 0 : count;
                            break;
                        }//增加伤害百分比
                    case FightingSkillType.REDUCE_DAMAGE_PERCENTAGE:
                        {
                            var count = m.hurtReduce + item.buffValue;
                            m.hurtReduce = count < 0 ? 0 : count;
                            break;
                        }//降低伤害百分比
                    case FightingSkillType.INCREASES_MYSTERY_PROBABILITY:
                        {
                            var count = m.mystery_probability + item.buffValue;
                            m.mystery_probability = count < 0 ? 0 : count;
                            break;
                        }//增加奥义触发几率

                    #endregion
                }
            }
            return m;
        }

        #endregion

        /// <summary> 几率是否成功 </summary>
        /// <param name="number">几率</param>
        private static bool IsTrue(double number)
        {
            return (number > 0) && new RandomSingle().IsTrue(number);
        }

        #endregion

        /// <summary> 将新Buff改为旧Buff </summary>
        private void UpdateBuff(FightGlobalEntity fight)
        {
            var xin = fight.buff.Where(m => m.bufftype == 0).ToList();
            var jiu = fight.buff.Where(m => m.bufftype != 0).ToList();
            foreach (var buff in xin)
            {
                if (buff.type == FightingSkillType.CRIT || buff.type == FightingSkillType.DODGE ||
                    buff.type == FightingSkillType.INCREASES_YINCOUNT || buff.type == FightingSkillType.REDUCE__YINCOUNT ||
                    buff.type == FightingSkillType.INCREASES_STRENGTH || buff.type == FightingSkillType.REDUCE_STRENGTH)
                    continue;
                if (buff.count != 0) buff.bufftype = 1;
                jiu.Add(buff);
            }
            fight.buff = jiu;
        }

        /// <summary> 组装触发的技能类型 如：奥义、秘技、印 </summary>
        /// <param name="movesvo">出招Vo</param>
        /// <param name="type">类型</param>
        private static void BuildSkillType(MovesVo movesvo, SkillType type)
        {
            switch (type)
            {
                case SkillType.RZAY:
                case SkillType.MEANING: { movesvo.isMystery = true; break; }
                case SkillType.RZMJ1:
                case SkillType.RZMJ2:
                case SkillType.RZMJ3:
                case SkillType.ESOTERICA: { movesvo.isSkill = true; break; }
                case SkillType.YIN: { movesvo.isYin = true; break; }
                default: { break; }
            }
        }

        /// <summary> 初始化武将出手回合 </summary>
        /// <param name="fight"></param>
        /// <returns></returns>
        private Dictionary<long, int> InitRoleShot(FightGlobalEntity fight)
        {
            var dic = fight.attack.roleEntity.ToDictionary(item => item.id, item => 0);
            foreach (var item in fight.defense.roleEntity)
            {
                dic.Add(item.id, 0);
            }
            return dic;
        }

        #endregion

        #region 添加移除玩家战斗状态

        /// <summary> 验证玩家是否还在战斗计算中 </summary>
        /// <param name="userid">要验证的玩家</param>
        private bool IsFight(Int64 userid)
        {
            if (Variable.PlayerFight.ContainsKey(userid)) return true;
            Variable.PlayerFight.AddOrUpdate(userid, 0, (m, n) => 0);
            return false;
        }

        /// <summary> 将玩家移除战斗 </summary>
        /// <param name="userid">要修改的玩家Id</param>
        private void RemoveFightState(Int64 userid, int count = 0)
        {
            int a;
            if (!Variable.PlayerFight.ContainsKey(userid)) return;
            if (count > 5) return;
            count++;

            var flag = Variable.PlayerFight.TryRemove(userid, out a);
            if (!flag) RemoveFightState(userid, count);
        }

        #endregion

        #region 组装数据

        /// <summary> 验证是否修改为只有主角武将阵形 </summary>
        /// <param name="roleid">当前玩家主角武将Id</param>
        /// <param name="oneself">当前玩家 FightPersonal</param>
        private tg_fight_personal IsUpdatePersonal(Int64 roleid, tg_fight_personal oneself, FightType type)
        {
            switch (type)
            {
                case FightType.CONTINUOUS:
                case FightType.SINGLE_FIGHT:
                    {
                        oneself.matrix1_rid = roleid;
                        oneself.matrix2_rid = 0;
                        oneself.matrix3_rid = 0;
                        oneself.matrix4_rid = 0;
                        oneself.matrix5_rid = 0;
                        return oneself;
                    }
            }
            return oneself;
        }

        /// <summary> 组装印实体 </summary>
        /// <param name="yin"></param>
        /// <returns></returns>
        private YinEntity BuildYinEntity(tg_fight_yin yin)
        {
            var baseYin = Variable.BASE_YIN.FirstOrDefault(m => m.id == yin.yin_id);
            var baseEffect = Variable.BASE_YINEFFECT.FirstOrDefault(m => m.yinId == yin.yin_id && m.level == yin.yin_level);
            if (baseYin == null || baseEffect == null) return new YinEntity();
            var temp = new YinEntity
            {
                yin = yin,
                yinCount = baseYin.yinCount,
                yinEffect = BuildSkillEffects(baseEffect.effects)
            };
            return temp;
        }

        /// <summary> 组装NPC印实体 </summary>
        /// <param name="yinId"></param>
        /// <returns></returns>
        private YinEntity BuildNpcYinEntity(Int64 yinId)
        {
            var baseEffect = Variable.BASE_YINEFFECT.FirstOrDefault(m => m.id == yinId);
            if (baseEffect == null) return new YinEntity();
            var temp = new tg_fight_yin()
            {
                yin_id = baseEffect.yinId,
                yin_level = baseEffect.level,
            };
            return BuildYinEntity(temp);
        }

        /// <summary> 组装玩家印实体 </summary>
        /// <param name="yinId"></param>
        /// <returns></returns>
        private YinEntity BuildPlayerYinEntity(Int64 yinId)
        {
            var yin = tg_fight_yin.FindByid(yinId);
            if (yin == null) return new YinEntity();
            var temp = BuildYinEntity(yin);
            return temp;
        }

        /// <summary> 组装技能实体集合 </summary>
        /// <param name="list">个人战技能实体 tg_role_fight_skill集合</param>
        private List<SkillEntity> BuildListSkillEntity(IEnumerable<tg_role_fight_skill> list)
        {
            return list.Select(BuildSkillEntity).Where(temp => temp != null).ToList();
        }

        /// <summary> 组装技能实体 </summary>
        /// <param name="model">个人战技能实体 tg_role_fight_skill</param>
        private SkillEntity BuildSkillEntity(tg_role_fight_skill model)
        {
            if (model == null) return null;
            var baseSkill = Variable.BASE_FIGHTSKILL.FirstOrDefault(m => m.id == model.skill_id);
            var baseEffect = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.skillid == model.skill_id && m.level == model.skill_level);
            if (baseSkill == null || baseEffect == null) return null;
            if (baseSkill.typeSub != Convert.ToInt32(FightSkillType.PERSONAL_WAR)) return null;
            var temp = new SkillEntity
            {
                skill = model,
                energy = baseSkill.energy,
                attackRange = baseEffect.attackRange,
                isQuickAttack = baseEffect.isQuickAttack,
                skillEffect = BuildSkillEffects(baseEffect.effects),
            };
            return temp;
        }

        /// <summary> 组装技能效果集合 </summary>
        /// <param name="effects">技能效果字符串</param>
        private List<SkillEffects> BuildSkillEffects(string effects)
        {
            var list = new List<SkillEffects>();
            var data = effects.Split('|');                 //拆分技能效果
            foreach (var item in data)
            {
                var array = item.Split('_');
                if (array.Length < 5 || array.Length > 6) return null;
                var skill = new SkillEffects();
                skill.type = Convert.ToInt32(array[0]);                  //技能效果类型
                skill.target = Convert.ToInt32(array[1]);                //技能效果目标 (1=本方 2=敌方)
                skill.range = Convert.ToInt32(array[2]);                 //技能效果范围 (1=单体 2=全体)
                skill.round = Convert.ToInt32(array[3]);                 //技能效果回合数
                skill.values = Convert.ToDouble(array[4]);               //技能效果值
                if (array.Length == 6)                                   //几率类型效果
                    skill.robabilityValues = Convert.ToDouble(array[5]); //技能效果几率
                if (skill.robabilityValues <= 0) skill.robabilityValues = 100;
                list.Add(skill);
            }
            return list;
        }

        /// <summary> 获取个人战阵形的武将 </summary>
        /// <param name="personal">个人战阵形</param>
        private List<RoleEntity> BuildListRoleEntity(tg_fight_personal personal)
        {
            var roles = new List<RoleEntity>();
            var ids = GetMatrixRids(personal);
            var list = tg_role.GetFindAllByIds(ids);
            var skillList = GetPlayerRoleSkill(list);
            foreach (var item in list)
            {
                int number = 0;
                if (personal.matrix1_rid == item.id) number = 1;
                if (personal.matrix2_rid == item.id) number = 2;
                if (personal.matrix3_rid == item.id) number = 3;
                if (personal.matrix4_rid == item.id) number = 4;
                if (personal.matrix5_rid == item.id) number = 5;
                if (number == 0) continue;
                roles.Add(BuildRoleEntity(item, skillList, number));
            }
            return roles;
        }

        private List<RoleEntity> BuildListRoleEntity(tg_fight_personal personal, int add)
        {
            var roles = new List<RoleEntity>();
            var ids = GetMatrixRids(personal);
            var list = tg_role.GetFindAllByIds(ids);
            var skillList = GetPlayerRoleSkill(list);
            foreach (var item in list)
            {
                int number = 0;
                if (personal.matrix1_rid == item.id) number = 1;
                if (personal.matrix2_rid == item.id) number = 2;
                if (personal.matrix3_rid == item.id) number = 3;
                if (personal.matrix4_rid == item.id) number = 4;
                if (personal.matrix5_rid == item.id) number = 5;
                if (number == 0) continue;
                var entity = BuildRoleEntity(item, skillList, number);
                if (add != 0)
                {
                    entity.attack = entity.attack + entity.attack * add / 100;
                }
                roles.Add(entity);

            }
            return roles;
        }

        /// <summary> BaseNpcRole 转换 RoleFight </summary>
        private RoleEntity ConvertNpcRoleFight(BaseNpcRole model, int weizhi)
        {
            return new RoleEntity
            {
                id = RNG.Next(),
                damage = 0,
                lv = model.lv,
                angerCount = 0,
                hp = model.life,
                HP = model.life,
                weizhi = weizhi,
                baseId = model.id,
                initweizhi = weizhi,
                initHp = model.life,
                attack = model.attack,
                defense = model.defense,
                art_ninja_mystery = null,
                art_ninja_cheat_code1 = null,
                art_ninja_cheat_code2 = null,
                art_ninja_cheat_code3 = null,
                hurtReduce = model.hurtReduce,
                hurtIncrease = model.hurtIncrease,
                critAddition = model.critAddition,
                critProbability = model.critProbability,
                dodgeProbability = model.dodgeProbability,
                mystery = BuildNpcSkillEntity(model.pmystery),
                mystery_probability = model.mysteryProbability,
                cheatCode = BuildNpcSkillEntity(model.pcheatCode),
            };
        }

        private RoleEntity BuildRoleEntity(tg_role model, List<tg_role_fight_skill> list, int number)
        {
            var temp = new RoleEntity();
            temp.damage = 0;
            temp.id = model.id;
            temp.angerCount = 0;
            temp.weizhi = number;
            temp.initweizhi = number;
            temp.hp = model.att_life;
            temp.HP = model.att_life;
            temp.initHp = model.att_life;
            temp.lv = model.role_level;
            temp.baseId = model.role_id;
            temp.user_id = model.user_id;
            temp.hurtReduce = model.att_sub_hurtReduce;
            temp.monsterType = (int)FightRivalType.ROLE;
            temp.critAddition = tg_role.GetTotalCritAddition(model);
            temp.hurtIncrease = tg_role.GetTotalHurtIncrease(model);
            temp.attack = tg_role.GetTotalAttack(model);
            temp.defense = Convert.ToInt32(model.att_defense);
            temp.critProbability = tg_role.GetTotalCritProbability(model);
            temp.dodgeProbability = tg_role.GetTotalDodgeProbability(model);
            temp.mystery_probability = tg_role.GetTotalMysteryProbability(model);
            //temp.force = tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, model);
            temp.mystery = BuildSkillEntity(list.FirstOrDefault(m => m.id == model.art_mystery));
            temp.cheatCode = BuildSkillEntity(list.FirstOrDefault(m => m.id == model.art_cheat_code));
            temp.art_ninja_mystery = BuildSkillEntity(list.FirstOrDefault(m => m.id == model.art_ninja_mystery));
            temp.art_ninja_cheat_code1 = BuildSkillEntity(list.FirstOrDefault(m => m.id == model.art_ninja_cheat_code1));
            temp.art_ninja_cheat_code2 = BuildSkillEntity(list.FirstOrDefault(m => m.id == model.art_ninja_cheat_code2));
            temp.art_ninja_cheat_code3 = BuildSkillEntity(list.FirstOrDefault(m => m.id == model.art_ninja_cheat_code3));
            return temp;
        }




        private SkillEntity BuildNpcSkillEntity(int skillId)
        {
            var baseEffect = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.id == skillId);
            if (baseEffect == null) return null;
            var temp = new tg_role_fight_skill()
            {
                skill_id = baseEffect.skillid,
                skill_level = baseEffect.level,
            };
            return BuildSkillEntity(temp);
        }

        /// <summary>构建NPC个人战数据</summary>
        private tg_fight_personal BuildFightPersonal(String matrix, FightType type)
        {
            var model = new tg_fight_personal();
            var matrix_rid = matrix.Split(',');
            switch (type)
            {
                case FightType.NPC_MONSTER: { matrix_rid = GetMatrixRid(matrix_rid); break; }
            }

            for (var i = 0; i < matrix_rid.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        model.matrix1_rid = Convert.ToInt64(matrix_rid[0]); break;
                    case 1:
                        model.matrix2_rid = Convert.ToInt64(matrix_rid[1]); break;
                    case 2:
                        model.matrix3_rid = Convert.ToInt64(matrix_rid[2]); break;
                    case 3:
                        model.matrix4_rid = Convert.ToInt64(matrix_rid[3]); break;
                    case 4:
                        model.matrix5_rid = Convert.ToInt64(matrix_rid[4]); break;
                }
            }
            return model;
        }

        #endregion

        #region 获取数据

        /// <summary>获取双方当前印数</summary>
        private MovesVo AddYinCount(FightGlobalEntity fight, MovesVo movesvo)
        {
            if (fight.attack.isAttack)
            {
                movesvo.yinA = fight.attack.yin.yin.yin_id == 0 ? 0 : fight.attack.yin.count;
                movesvo.yinB = fight.defense.yin.yin.yin_id == 0 ? 0 : fight.defense.yin.count;
            }
            else
            {
                movesvo.yinA = fight.defense.yin.yin.yin_id == 0 ? 0 : fight.defense.yin.count;
                movesvo.yinB = fight.attack.yin.yin.yin_id == 0 ? 0 : fight.attack.yin.count;
            }
            return movesvo;
        }

        /// <summary> 获取集合武将中的技能和奥义 </summary>
        private List<tg_role_fight_skill> GetPlayerRoleSkill(List<tg_role> list)
        {
            var ids = list.Select(m => m.art_cheat_code).ToList();
            ids.AddRange(list.Select(m => m.art_ninja_cheat_code1));
            ids.AddRange(list.Select(m => m.art_ninja_cheat_code2));
            ids.AddRange(list.Select(m => m.art_ninja_cheat_code3));
            ids.AddRange(list.Select(m => m.art_ninja_mystery));
            ids.AddRange(list.Select(m => m.art_mystery));
            ids = ids.Where(m => m != 0).ToList();
            return ids.Any() ? tg_role_fight_skill.GetFindAllByIds(ids) : new List<tg_role_fight_skill>();
        }

        /// <summary> 获取阵中武将Id </summary>
        /// <param name="personal">要获取的阵</param>
        /// <returns>阵中武将Id集合</returns>
        private List<Int64> GetMatrixRids(tg_fight_personal personal)
        {
            var ids = new List<Int64>();
            if (personal.matrix1_rid != 0)                             //阵中的所有武将
                ids.Add(personal.matrix1_rid);
            if (personal.matrix2_rid != 0)
                ids.Add(personal.matrix2_rid);
            if (personal.matrix3_rid != 0)
                ids.Add(personal.matrix3_rid);
            if (personal.matrix4_rid != 0)
                ids.Add(personal.matrix4_rid);
            if (personal.matrix5_rid != 0)
                ids.Add(personal.matrix5_rid);
            return ids;
        }

        /// <summary> 获取对方战斗用户实体 </summary>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private FightUserEntity GetRivalFightUserEntity(Int64 userid, FightType type)
        {
            var rivalfight = new FightUserEntity();
            switch (type)
            {
                case FightType.BOTH_SIDES:
                case FightType.ONE_SIDE:
                    {
                        #region 对手玩家数据

                        if (!Variable.OnlinePlayer.ContainsKey(userid)) return GetDbPlayFightUserEntity(userid);
                        var session = Variable.OnlinePlayer[userid] as TGGSession;
                        if (session == null) return GetDbPlayFightUserEntity(userid);

                        var roleid = session.Player.Role.Kind.id;
                        var personal = session.Fight.Personal;

                        if (session.Fight.Personal.id == 0) session.Fight.Personal =
                            tg_fight_personal.PersonalInsert(userid, roleid); //验证己方阵形 没有就插入阵
                        var user = session.Player.User.CloneEntity();
                        var bv = Variable.BASE_VOCATION.FirstOrDefault(m => m.vocation == user.player_vocation);

                        rivalfight.user = user;
                        rivalfight.personal = personal;
                        rivalfight.yin = BuildPlayerYinEntity(personal.yid);
                        rivalfight.roleEntity = BuildListRoleEntity(personal);
                        rivalfight.xiShu = bv == null ? 0 : Convert.ToDouble(bv.fight);
                        return rivalfight;

                        #endregion
                    }
                default: { return GetNpcRivalFightUserEntity(userid, type); }
            }
        }

        /// <summary> 获取战斗用户实体 </summary>
        /// <param name="userid">用户主键Id</param>
        private FightUserEntity GetDbPlayFightUserEntity(Int64 userid)
        {
            var rivalfight = new FightUserEntity();
            var p = tg_fight_personal.GetFindByUserId(userid);
            var user = tg_user.FindByid(userid);
            var bv = Variable.BASE_VOCATION.FirstOrDefault(m => m.vocation == user.player_vocation);
            if (p == null || user == null || bv == null) return null;
            rivalfight.personal = p;
            rivalfight.user = user;
            rivalfight.yin = BuildPlayerYinEntity(p.yid);
            rivalfight.roleEntity = BuildListRoleEntity(p);
            rivalfight.xiShu = Convert.ToDouble(bv.fight);
            return rivalfight;
        }

        /// <summary>获取对手战斗阵信息</summary>
        private FightUserEntity GetNpcRivalFightUserEntity(Int64 id, FightType type)
        {
            dynamic npc;
            var rivalfight = new FightUserEntity();
            switch (type)
            {
                case FightType.SIEGE:
                case FightType.BUILDING:
                case FightType.CONTINUOUS:
                case FightType.NPC_FIGHT_ARMY: { npc = Variable.BASE_NPCARMY.FirstOrDefault(m => m.id == (int)id); break; }
                case FightType.SINGLE_FIGHT: { npc = Variable.BASE_NPCSINGLE.FirstOrDefault(m => m.id == (int)id); break; }
                case FightType.NPC_MONSTER: { npc = Variable.BASE_NPCMONSTER.FirstOrDefault(m => m.id == (int)id); break; }
                case FightType.DUPLICATE_SHARP: { npc = Variable.BASE_TOWERENEMY.FirstOrDefault(m => m.id == (int)id); break; }
                default: { return null; }
            }
            if (npc == null) return null;
            var p = BuildFightPersonal(npc.matrix, type);

            rivalfight.personal = p;
            rivalfight.yin = BuildNpcYinEntity(npc.yinEffectId);
            rivalfight.roleEntity = GetNpcFightRole(p);
            rivalfight.user = null;
            return rivalfight;
        }

        /// <summary>设置NPC战斗全局数据</summary>
        private List<RoleEntity> GetNpcFightRole(tg_fight_personal rival)
        {
            var roles = new List<RoleEntity>();
            var base_1 = rival.matrix1_rid == 0 ? null : GetNpcData(rival.matrix1_rid, 1);
            if (base_1 != null) { roles.Add(base_1); }
            var base_2 = rival.matrix2_rid == 0 ? null : GetNpcData(rival.matrix2_rid, 2);
            if (base_2 != null) { roles.Add(base_2); }
            var base_3 = rival.matrix3_rid == 0 ? null : GetNpcData(rival.matrix3_rid, 3);
            if (base_3 != null) { roles.Add(base_3); }
            var base_4 = rival.matrix4_rid == 0 ? null : GetNpcData(rival.matrix4_rid, 4);
            if (base_4 != null) { roles.Add(base_4); }
            var base_5 = rival.matrix5_rid == 0 ? null : GetNpcData(rival.matrix5_rid, 5);
            if (base_5 != null) { roles.Add(base_5); }
            return roles;
        }

        /// <summary> 获取NPC FightRole实体 </summary>
        /// <param name="rid">要获取的NPC 实体</param>
        /// <param name="weizhi">武将所在位置</param>
        private RoleEntity GetNpcData(Int64 rid, int weizhi)
        {
            var model = Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == Convert.ToInt32(rid));
            if (model == null) return null;
            var role = ConvertNpcRoleFight(model, weizhi);
            role.monsterType = (int)FightRivalType.MONSTER;
            return role;
        }

        /// <summary> 供点将用   多个Npc武将Id抽取5个 </summary>
        /// <param name="list">武将Id集合</param>
        private string[] GetMatrixRid(string[] list)
        {
            int count = 5;
            int number = 4;
            var rolehome = Variable.BASE_ROLE_HOME.FirstOrDefault(m => m.id == RoleHomeId);
            if (rolehome != null)
            {
                count = rolehome.count;
                number = count - 1;
            }
            if (list.Count() <= count) return list;
            var numbers = RNG.Next(1, list.Count() - 1, number).ToList();

            ICollection<string> l = new Collection<string>();
            l.Add(list[0]);
            for (int i = 0; i < numbers.Count(); i++)
            {
                l.Add(list[numbers[i]]);
            }
            return l.ToArray();
        }
        #endregion

        #region 组装VO数据

        /// <summary> 组装MovesVo </summary>
        /// <param name="fight"></param>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        private MovesVo BuildMovesVo(FightGlobalEntity fight, List<double> ids, SkillType type = SkillType.OTHER)
        {
            var move = new MovesVo();
            move.hitIds = ids;
            move.times = Round;
            BuildSkillType(move, type);
            move = AddYinCount(fight, move);
            move.attackId = fight.attack.attackRole.id;
            move.rolesA = ToRoleFightVos(fight, fight.buff, true, type);
            move.rolesB = ToRoleFightVos(fight, fight.buff, false, type);
            UpdateBuff(fight);
            return move;
        }

        /// <summary> 转换战斗武将vo实体 </summary>
        /// <param name="entity"></param>
        /// <param name="buffs"></param>
        /// <param name="flag"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<RoleFightVo> ToRoleFightVos(FightGlobalEntity entity, List<BuffEntity> buffs, bool flag, SkillType type = SkillType.OTHER)
        {
            var ls = new List<RoleFightVo>();
            var list = flag ? entity.attack.isAttack ? entity.attack.roleEntity : entity.defense.roleEntity
            : entity.attack.isAttack ? entity.defense.roleEntity : entity.attack.roleEntity;

            for (int i = 1; i < 6; i++)
            {
                var role = list.FirstOrDefault(m => m.initweizhi == i);
                ls.Add(role == null ? null : ToRoleFightVo(role, buffs, type));
            }
            return ls;
        }

        /// <summary> 转换战斗武将vo实体 </summary>
        /// <param name="model"></param>
        /// <param name="buffs"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private RoleFightVo ToRoleFightVo(RoleEntity model, List<BuffEntity> buffs, SkillType sign)
        {
            var temp = new RoleFightVo
            {
                id = model.id,
                hp = model.hp,
                damage = model.damage,
                initHp = model.initHp,
                baseId = model.baseId,
                hurtReduce = model.hurtReduce,
                angerCount = model.angerCount,
                monsterType = model.monsterType,
                critAddition = model.critAddition,
                hurtIncrease = model.hurtIncrease,
                attack = Convert.ToInt32(model.attack),
                critProbability = model.critProbability,
                defense = Convert.ToInt32(model.defense),
                dodgeProbability = model.dodgeProbability,
                buffVos = ToBuffVos(buffs.Where(m => m.rid == model.id && m.bufftype != 2 && m.IsOk)),
            };

            #region 忍者众奥义秘技

            switch (sign)
            {
                case SkillType.RZMJ1: { temp.cheatCode = model.art_ninja_cheat_code1 == null ? 0 : model.art_ninja_cheat_code1.skill.skill_id; break; }
                case SkillType.RZMJ2: { temp.cheatCode = model.art_ninja_cheat_code2 == null ? 0 : model.art_ninja_cheat_code2.skill.skill_id; break; }
                case SkillType.RZMJ3: { temp.cheatCode = model.art_ninja_cheat_code3 == null ? 0 : model.art_ninja_cheat_code3.skill.skill_id; break; }
                case SkillType.RZAY: { temp.mystery = model.art_ninja_mystery == null ? 0 : model.art_ninja_mystery.skill.skill_id; break; }
                default:
                    {
                        temp.mystery = model.mystery == null ? 0 : model.mystery.skill.skill_id;
                        temp.cheatCode = model.cheatCode == null ? 0 : model.cheatCode.skill.skill_id;
                        break;
                    }
            }

            #endregion

            return temp;
        }

        private static List<BuffVo> ToBuffVos(IEnumerable<BuffEntity> buffs)
        {
            return buffs.Select(ToBuffVos).ToList();
        }

        private static BuffVo ToBuffVos(BuffEntity buff)
        {
            return new BuffVo()
            {
                buffValue = buff.buffValue,
                type = Convert.ToInt32(buff.type),
                moveTimes = buff.count,
                buffState = buff.bufftype,
            };
        }

        private FightPlayerVo ToFightPlayerVo(tg_user user)
        {
            return new FightPlayerVo()
            {
                name = user.player_name,
                playId = user.id,
                sex = user.player_sex,
                type = 0,
                vocation = user.player_vocation,
            };
        }

        private FightPlayerVo ToFightPlayerVo()
        {
            return new FightPlayerVo()
            {
                type = 1,
            };
        }
        #endregion

        #region 回合计算

        /// <summary> 回合计数 </summary>
        private bool RoundCount(Int64 roleid, FightGlobalEntity fight)
        {
            if (!dic_round.ContainsKey(roleid)) return false;

            var values = dic_round[roleid] + 1;
            if (values <= Round) dic_round[roleid] = values;
            else
            {
                if (!fight.isAttack) return false;
                if (dic_round.ContainsValue(Round - 1)) return false;
#if DEBUG
                XTrace.WriteLine("{0}", "回合结束");
                XTrace.WriteLine(string.Format("{0}  {1}", "开始回合数", values));
#endif
                dic_round[roleid] = values;
                Round = values;
                RemoveBuff(fight);
            }
            return true;
        }


        /// <summary> 移除回合过期的Buff </summary>
        private void RemoveBuff(FightGlobalEntity fight)
        {

#if DEBUG
            var list = fight.buff.Where(m => m.count < Round).ToList();
            foreach (var item in list)
            {
                XTrace.WriteLine(string.Format("{0} {1} {2}", "移除 " + item.rid + " 身上BUFF ", "类型 " + (int)item.type, "值 " + item.buffValue));
            }

#endif
            fight.buff = fight.buff.Where(m => m.count > Round).ToList();
        }

        #endregion

        #region 获取伤害跟血量

        /// <summary> 获取当前玩家剩余血量 </summary>
        private Int64 GetPlayHp(Int64 roleid, IEnumerable<RoleEntity> list)
        {
            var fightrole = list.FirstOrDefault(m => m.id == roleid);
            return fightrole == null ? 0 : fightrole.hp;
        }

        /// <summary> 获取对Boss的伤害值 </summary>
        private Int64 GetBossHurt(IEnumerable<RoleEntity> list)
        {
            var role = list.FirstOrDefault(m => m != null);
            if (role == null) return 0;
            var count = role.HP - role.hp;
            if (count < 0) count = 0;
            return count;
        }

        /// <summary> 获取对Boss的伤害值 </summary>
        private Int64 GetBossHp(IEnumerable<RoleEntity> list)
        {
            var role = list.FirstOrDefault(m => m != null);
            if (role == null) return 0;
            var count = role.hp;
            if (count < 0) count = 0;
            return count;
        }

        /// <summary> 验证是否修改需要改变血量的模块 </summary>
        /// <param name="hp"> 血量 </param>
        private List<RoleEntity> UpdateRivalHp(Int64 hp, FightType type, List<RoleEntity> list)
        {
            if (hp <= 0) return list;
            switch (type)
            {
                case FightType.SIEGE:
                case FightType.BUILDING:
                    {
                        var role = list.FirstOrDefault(m => m != null);
                        if (role == null) return list;
                        role.hp = hp;
                        role.HP = hp;
                        return list;
                    }
                default: { return list; }
            }
        }

        /// <summary> 验证是否修改需要改变血量的模块 </summary>
        /// <param name="hp"> 血量 </param>
        private List<RoleEntity> UpdatePlayerHp(Int64 roleid, Int64 hp, FightType type, List<RoleEntity> list)
        {
            if (hp <= 0) return list;
            switch (type)
            {
                case FightType.CONTINUOUS:
                case FightType.DUPLICATE_SHARP:
                    {
                        var role = list.FirstOrDefault(m => m.id == roleid);
                        if (role == null) return list;
                        role.hp = hp;
                        role.HP = hp;
                        return list;
                    }
                default: { return list; }
            }
        }

        #endregion

        #region 爬塔

        /// <summary>
        /// 爬塔用 人跟怪物打 主角死了必须输
        /// </summary>
        /// <param name="playhp">主角血量</param>
        private void UpdateWin(FightVo vo, Int64 playhp, Int64 roleid, FightType type)
        {
            switch (type)
            {
                case FightType.DUPLICATE_SHARP:
                    {
                        if (playhp <= 0)
                        {
                            bool flag = false;
                            vo.isWin = false;
                            var list = new List<List<MovesVo>>();
                            foreach (var l in vo.moves)
                            {
                                if (flag) continue;
                                var model = new List<MovesVo>();
                                foreach (var item in l)
                                {
                                    var role = item.rolesA.FirstOrDefault(m => m != null && m.id == roleid);
                                    if (role == null) { role = item.rolesB.FirstOrDefault(m => m != null && m.id == roleid); if (role == null)return; }
                                    if (flag) continue;
                                    if (role.hp <= 0) flag = true;
                                    model.Add(item);
                                }
                                list.Add(model);
                            }
                            vo.moves = list;
                        }
                        break;
                    }
            }
        }

        #endregion

        /// <summary> 调用武器使用统计 </summary>
        private void WeaponCount(Int64 userid)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "WeaponCount", "调用武器使用统计");
            var sw = Stopwatch.StartNew();
#endif
            (new Title()).RoleFightMethods(userid);

#if DEBUG
            sw.Stop();
            XTrace.WriteLine("总运行时间：{0} 毫秒", sw.ElapsedMilliseconds.ToString());
#endif
        }

        /// <summary> 推送协议 </summary>
        private void SendProtocol(TGGSession session, ASObject aso)
        {
            var pv = session.InitProtocol((int)ModuleNumber.FIGHT, (int)FightCommand.FIGHT_PERSONAL_ENTER, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(int result, FightVo model)
        {
            var dic = new Dictionary<string, object> 
            { 
            { "result", result },
            { "fight", model },
            };
            return dic;
        }
    }
}
