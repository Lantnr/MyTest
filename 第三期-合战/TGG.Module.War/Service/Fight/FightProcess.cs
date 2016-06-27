using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;
using TGG.Core.Vo.War;
using TGG.Module.War.Service.Fight;
using TGG.Share;
using TGG.Share.Event;

namespace TGG.Module.War.Service
{
    public partial class FightProcess
    {
        /// <summary>
        /// 初始玩家战斗过程
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="lines"></param>
        /// <param name="attackroles"></param>
        /// <param name="attackUserId"></param>
        /// <param name="cityid"></param>
        /// <param name="frontid"></param>
        /// <param name="morale"></param>
        /// <returns></returns> 
        public Tuple<int, WarFightVo> GetFightProcess(Int64 planid, List<WarRolesLinesVo> lines, List<tg_war_role> attackroles, Int64 attackUserId, Int32 cityid, Int32 frontid, int morale, string username)
        {
            WarFight fight = new WarFight(planid, lines, attackroles, attackUserId, cityid, frontid, morale);
            if (!fight.isInitSuccess) return null;
            return GetFightProcess(fight);
        }


        /// <summary>
        ///  通过战斗模型得到战斗过程
        /// </summary>
        /// <returns></returns>
        public Tuple<int, WarFightVo> GetFightProcess(WarFight fight)
        {
            WarFightVo vo = new WarFightVo();
            vo.moves = new List<WarMovesVo>();
            vo.result = new WarFightResultVo();
            //初始武将
            GetFightRoles(fight, vo);
            //回合开始
            for (int i = 1; i < 51; i++)
            {
#if DEBUG
                XTrace.WriteLine("--------- 回合数：{0}", i);
#endif
                var times = new WarMovesVo();
                times.roles = new List<WarFightRoleVo>();
                GetRoleBuff(fight, times);
                //天气和五常生成
                GetWeatherAndFive(fight, times, i);

                GetDarkRoleShow(fight.DefenseRoles);
                //循环武将
                for (var j = 0; j < fight.AttackSort.Count; j++)
                {
#if DEBUG
                    var t = vo.roles.Where(m => m.roleId == fight.AttackSort[j].RoleId).FirstOrDefault();
                    XTrace.WriteLine("武将id：{0} name:{4} 攻击值:{1} x:{2} y:{3}",
                        t.baseId, t.blood, t.point.x, t.point.y, t.roleName
                        );
#endif
                    if (fight.AttackSort[j].isDie) continue;
                    if (fight.AttackSort[j].type == 2) //防守武将
                    {
                        var tuple = CheckDefenseRole(fight, fight.AttackSort[j].RoleId, times);
                        if (tuple.Item1 < 0) return null;
                        if (tuple.Item2)
                        {
                            vo.moves.Add(times);
                            var defensedie = fight.DefenseSoldierCount - fight.DefenseRoles.Sum(q => q.SoldierCount);
                            var attackdie = fight.AttackSoldierCount - fight.AttackRoles.Sum(q => q.SoldierCount);
                            RolesCountUpdate(fight.DefenseRoles, fight.AttackRoles);
                            vo.result.myDieCount = attackdie;
                            vo.result.rivalDieCount = defensedie;
                            if (fight.City.SoldierCount <= 0) vo.result.isWin = 1;
                            return Tuple.Create((int)ResultType.SUCCESS, vo);
                        }
                    }
                    else
                    {
                        var tuple = CheckAttackRole(fight, fight.AttackSort[j].RoleId, times);
                        if (tuple.Item1 < 0) return null;
                        if (tuple.Item2)
                        {
                            vo.moves.Add(times);
                            var defensedie1 = fight.DefenseSoldierCount - fight.DefenseRoles.Sum(q => q.SoldierCount);
                            var attackdie1 = fight.AttackSoldierCount - fight.AttackRoles.Sum(q => q.SoldierCount);
                            RolesCountUpdate(fight.DefenseRoles, fight.AttackRoles);
                            if (fight.City.SoldierCount <= 0) vo.result.isWin = 1;
                            vo.result.myDieCount = attackdie1;
                            vo.result.rivalDieCount = defensedie1;
                            return Tuple.Create((int)ResultType.SUCCESS, vo);
                        }
                    }
                }
                vo.moves.Add(times);
            }
            vo.result.isWin = 0;
            var defensedie2 = fight.DefenseSoldierCount - fight.DefenseRoles.Sum(q => q.SoldierCount);
            var attackdie2 = fight.AttackSoldierCount - fight.AttackRoles.Sum(q => q.SoldierCount);
            RolesCountUpdate(fight.DefenseRoles, fight.AttackRoles);
            vo.result.myDieCount = defensedie2;
            vo.result.rivalDieCount = attackdie2;
            return Tuple.Create((int)ResultType.SUCCESS, vo);
        }
        #region

        /// <summary>
        /// 武将添加技能效果
        /// </summary>
        /// <param name="vo"></param>
        /// <param name="rid"></param>
        /// <param name="faceto"></param>
        /// <param name="type"></param>
        /// <param name="leftblood">我方剩余血量</param>
        /// <param name="rivalleft">敌方剩余血量</param>
        /// <param name="baseid">技能基表id</param>
        /// <param name="effecttype"></param>
        /// <param name="effectvalue"></param>
        private void WarFightRoleVoAddEffect(WarFightRoleVo vo, Int64 rid, int faceto, int type, int leftblood, int rivalleft, Int32 baseid, int effecttype, int effectvalue)
        {
            var effect = new WarEffectVo()
            {
                baseId = baseid,
                faceTo = faceto,
                rids = rid,
                myLeftBlood = leftblood,
                rivalLeftBlood = rivalleft,
                type = type,
                skillEffectType = effecttype,
                effectValue = effectvalue
            };

            vo.effects.Add(effect);
        }

        /// <summary>
        /// 武将兵数存入数据库
        /// </summary>
        private void RolesCountUpdate(List<DefenseRoles> roles, List<AttackRoles> attroles)
        {
            foreach (var role in roles)
            {
                tg_war_role.UpdateRoleSoldierCount(role.SoldierCount, role.RoleId);

            }
            //进攻武将资源更改
            foreach (var role in attroles)
            {
                var listroles = new List<tg_war_role>();
                var list = Common.GetInstance().GetResourseString(role.SoldierId, 0);
                foreach (var resourse in list)
                {
                    switch (resourse.type)
                    {
                        case (int)WarResourseType.苦无:
                            {
                                if (role.WarRole.army_kuwu >= resourse.value * role.SoldierCount)
                                    role.WarRole.army_kuwu = resourse.value * role.SoldierCount;
                            } break;
                        case (int)WarResourseType.薙刀:
                            {
                                if (role.WarRole.army_razor >= resourse.value * role.SoldierCount)
                                    role.WarRole.army_razor = resourse.value * role.SoldierCount;
                            } break;
                        case (int)WarResourseType.足轻:
                            {
                                if (role.WarRole.army_soldier >= resourse.value * role.SoldierCount)
                                    role.WarRole.army_soldier = resourse.value * role.SoldierCount;
                            } break;
                        case (int)WarResourseType.铁炮:
                            {
                                if (role.WarRole.army_gun >= resourse.value * role.SoldierCount)
                                    role.WarRole.army_gun = resourse.value * role.SoldierCount;
                            } break;
                        case (int)WarResourseType.马匹:
                            {
                                if (role.WarRole.army_horse >= resourse.value * role.SoldierCount)
                                    role.WarRole.army_horse -= resourse.value * role.SoldierCount;
                            } break;

                    }
                    listroles.Add(role.WarRole);
                }

                tg_war_role.GetListUpdate(listroles);
            }

        }

        /// <summary>
        /// 上回合出现的伏兵武将类型进行更改
        /// </summary>
        /// <param name="roles"></param>
        private void GetDarkRoleShow(List<DefenseRoles> roles)
        {
            var showdefenseroles = roles.Where(q => q.isShow).ToList();
            for (int m = 0; m < showdefenseroles.Count; m++)
            {
                showdefenseroles[m].type = (int)WarFightRoleType.武将;
            }
        }

        /// <summary>
        /// 武将添加新buff
        /// </summary>
        /// <param name="vo"></param>
        /// <param name="rid"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        private void WarFightRoleVoAddBuff(WarFightRoleVo vo, Int64 rid, int type, int value)
        {
            if (vo.newBuff == null) vo.newBuff = new List<WarBuffVo>();
            var buff = new WarBuffVo()
            {
                rid = rid,
                type = type,
                value = value,
            };
            vo.newBuff.Add(buff);
        }



        /// <summary>
        /// 验证进攻武将是否死亡
        /// </summary>
        /// <param name="role">进攻武将实体</param>
        /// <param name="fight">战斗实体</param>
        /// <returns></returns>
        private bool CheckAttackRoleBlood(AttackRoles role, WarFight fight)
        {
            if (role.SoldierCount <= 0)
            {
                role.SoldierCount = 0;
                //lock (this)
                //{
                //    fight.AttackRoles.Remove(role);
                //}
                fight.AttackSort.FirstOrDefault(q => q.RoleId == role.RoleId).isDie = true;
            }
            if (fight.AttackRoles.All(q => q.SoldierCount == 0))
                return true;
            return false;
        }

        /// <summary>
        /// 验证防守武将是否死亡
        /// </summary>
        /// <param name="role">防守武将实体</param>
        /// <param name="fight">战斗实体</param>
        /// <returns></returns>
        private bool CheckDefenseRoleBlood(DefenseRoles role, WarFight fight)
        {
            if (role.SoldierCount <= 0)
            {
                role.SoldierCount = 0;
                //lock (this)
                //{
                //    fight.DefenseRoles.Remove(role);
                //}
                fight.AttackSort.FirstOrDefault(q => q.RoleId == role.RoleId).isDie = true;
                //tg_war_role.UpdateRoleSoldierCount(0, role.RoleId);

            }
            return false;
        }


        /// <summary>
        /// 验证防守武将防守范围是否有进攻武将
        /// </summary>
        /// <param name="defenserange">武将防守范围</param>
        /// <param name="attroles">进攻武将集合</param>
        /// <returns></returns>
        private AttackRoles CheckIsAttackRole(List<DefenseRange> defenserange, List<AttackRoles> attroles)
        {
            //寻找进攻武将中血量最少的
            return attroles.OrderBy(q => q.SoldierCount).FirstOrDefault(attackRolese => defenserange.Any(m => m.X == attackRolese.X && m.Y == attackRolese.Y && attackRolese.SoldierCount > 0));
        }

        /// <summary>
        /// 验证进攻武将进攻范围是否有防守武将
        /// </summary>
        /// <param name="defenseroles">防守武将集合</param>
        /// <param name="attroles">进攻武将集合</param>
        /// <param name="door">城门</param>
        /// <param name="home">本丸</param>
        /// <returns></returns>
        private DefenseRoles CheckIsDefenseRole(List<DefenseRoles> defenseroles, AttackRoles attroles, DefenseRoles door, DefenseRoles home)
        {
            var getdefense = new List<DefenseRoles>();
#if DEBUG
            XTrace.WriteLine("进攻武将坐标({0},{1})", attroles.X, attroles.Y);
            for (int j = 0; j < attroles.AttackRange.Count; j++)
            {
                XTrace.WriteLine("进攻武将攻击范围({0},{1})", attroles.AttackRange[j].X, attroles.AttackRange[j].Y);
            }
#endif
            foreach (var defenserole in defenseroles)
            {
                if (defenserole.SoldierCount <= 0||defenserole.isShow==false) continue;
#if DEBUG

                XTrace.WriteLine("防守武将{0}的坐标({1},{2}),", defenserole.roleName, defenserole.X, defenserole.Y);
#endif
                if (attroles.AttackRange.Any(q => q.X == defenserole.X && q.Y == defenserole.Y))
                    getdefense.Add(defenserole);
            }

            if (!getdefense.Any())     //攻击范围内没有防守武将
            {
                //验证是否攻打城门
                if (door.SoldierCount > 0)
                {
                    if (attroles.AttackRange.Any(q => q.X == 2 && q.Y > 1 && q.Y < 7))
                    {
                        attroles.isFightDoor = true;
                        return door;
                    }
                }

                //验证是否攻打本丸
                if (attroles.AttackRange.Any(q => q.X == home.X && q.Y == home.Y))
                {
                    attroles.isFightCity = true;
                    return home;
                }

                return null;
            };
#if DEBUG
            XTrace.WriteLine("进攻武将{0}攻击范围内有防守武将{1}", attroles.roleName, getdefense.OrderBy(q => q.SoldierCount).FirstOrDefault().roleName);
#endif
            //寻找防守武将中血量最少的
            return getdefense.OrderBy(q => q.SoldierCount).FirstOrDefault();
        }

        /// <summary>
        /// 验证伏兵是否出现
        /// </summary>
        /// <param name="defenseroles">伏兵集合</param>
        /// <param name="attroles">进攻武将</param>
        /// <param name="defenserange">伏兵防守范围</param>
        /// <returns></returns>
        private List<DefenseRoles> CheckDarkRole(List<DefenseRoles> defenseroles, AttackRoles attroles, List<DefenseRange> defenserange)
        {
            //寻找能够攻击到该武将的进攻武将id集合
            var rids = defenserange.Where(q => q.X == attroles.X && q.Y == attroles.Y).Select(q => q.RoleId).Distinct();

            return defenseroles.Where(q => rids.Contains(q.RoleId)).ToList();
        }


        #endregion

        #region   天气和五常
        /// <summary>
        /// 生成天气和五常
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="vo"></param>
        /// <param name="times"></param>
        private void GetWeatherAndFive(WarFight fight, WarMovesVo vo, int times)
        {
            //回合开始  ,初始天气和五常
            fight.Times = times;
            fight.Weather = GetWeather(fight.Weather, times, fight.WeatherState);
            fight.FiveSharp = GetFive(fight.FiveSharp, times, fight.FiveState);
            vo.times = times;
            vo.weather = fight.Weather;
            vo.fiveSharp = fight.FiveSharp;
#if DEBUG
            XTrace.WriteLine("天气：{0}，五常{1}", vo.weather, vo.fiveSharp);
#endif
        }

        ///  <summary>
        /// 获取天气
        ///  </summary>
        ///  <param name="weather">天气</param>
        ///  <param name="times">回合数</param>
        /// <param name="weatherstate">持续回合数</param>
        /// <returns></returns>
        private int GetWeather(int weather, int times, int weatherstate)
        {
            if (times == 1) return Variable.WarWeather;
            if (times % weatherstate != 1) return weather;
            var random = RNG.Next(0, 1);
            return random;
        }

        /// <summary>
        /// 获取五常
        /// </summary>
        /// <param name="five">五常值</param>
        /// <param name="times">回合数</param>
        /// <param name="fivestate">持续回合数</param>
        /// <returns></returns>
        private int GetFive(int five, int times, int fivestate)
        {
            // if (times % fivestate != 1) return five;
            var random = RNG.Next(0, 4);
            return random;
        }

        #endregion

        /// <summary>
        /// 武将buff添加
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="times"></param>
        private void GetRoleBuff(WarFight fight, WarMovesVo times)
        {
            times.buffs = new List<WarBuffVo>();
            foreach (var defenserole in fight.DefenseRoles)
            {
                if (defenserole.buffs == null || !defenserole.buffs.Any()) continue;

                defenserole.buffs.RemoveAll(q => q.times <= 1);
                var rid = defenserole.RoleId;
                times.buffs.AddRange(defenserole.buffs.Where(q => q.usertype != (int)WarFightSkillType.Area).Select(q => new WarBuffVo()
                {
                    type = q.type,
                    value = q.value,
                    rid = rid
                }));
            }
            foreach (var attrole in fight.AttackRoles)
            {
                if (attrole.buffs == null || !attrole.buffs.Any()) continue;

                attrole.buffs.RemoveAll(q => q.times <= 1);
                var rid = attrole.RoleId;
                times.buffs.AddRange(attrole.buffs.Where(q => q.usertype != (int)WarFightSkillType.Area).Select(q => new WarBuffVo()
                {
                    type = q.type,
                    value = q.value,
                    rid = rid
                }));
            }
        }

        /// <summary>
        /// 初始合战武将值
        /// </summary>
        /// <param name="fight">战斗实体</param>
        /// <param name="vo">战斗vo</param>
        private void GetFightRoles(WarFight fight, WarFightVo vo)
        {
            vo.roles = new List<WarFightRoleVo>();
            vo.roles.AddRange(fight.AttackRoles.Select(q => new WarFightRoleVo()
            {
                roleId = q.RoleId,
                type = 0,
                campType = 0,
                lines = null,
                effects = null,
                baseId = q.Role.role_id,
                blood = q.SoldierCount,
                roleName = q.roleName,
                point = new PointVo() { x = q.X, y = q.Y },
                armyBaseId = q.SoldierId,


            }));
            vo.roles.AddRange(fight.DefenseRoles.Select(q => new WarFightRoleVo()
            {
                baseId = q.Role.role_id,
                roleId = q.RoleId,
                campType = 1,
                type = q.type,
                lines = null,
                effects = null,
                isNpc = q.isNpc,
                blood = q.SoldierCount,
                point = new PointVo() { x = q.X, y = q.Y },
                armyBaseId = q.SoldierId,
                roleName = q.roleName
            }));
            vo.roles.Add(new WarFightRoleVo()
            {
                roleId = fight.Door.RoleId,
                campType = 1,
                type = fight.Door.type,
                blood = fight.Door.SoldierCount,
                lines = null,
                effects = null,

                point = new PointVo() { x = 2, y = 2 }
            });
            vo.roles.Add(new WarFightRoleVo()
            {
                campType = 1,
                roleId = fight.City.RoleId,
                type = fight.City.type,
                blood = fight.City.SoldierCount,
                lines = null,
                effects = null,

                point = new PointVo() { x = 0, y = 4 }
            });

        }

        /// <summary>
        /// 进攻武将移动
        /// </summary>
        /// <param name="moves">回合</param>
        /// <param name="fight">合战实体</param>
        /// <param name="rid">武将id</param>
        private bool RolesMove(WarMovesVo moves, WarFight fight, Int64 rid)
        {
            var role = fight.AttackRoles.FirstOrDefault(q => q.RoleId == rid);
            if (role == null) return false;
            var attackrole = fight.AttackRoles;
            var defenseRoles = fight.DefenseRoles;
#if DEBUG
            XTrace.WriteLine("buff:{0}", role.buffs.Count);
#endif

            if (role.buffs.Any(q => q.type == (int)WarFightEffectType.StopAction)) return false;
            var rolevo = AddNewRoleVo(role, moves);
            if (rolevo.lines == null) rolevo.lines = new List<PointVo>();
            for (var i = 1; i <= role.speed; i++)
            {
                var p = role.Index + 1;
                if (p > role.Lines.Count - 1)
                {
                    return false;
                }
                var r_x = role.Lines[p].x;
                var r_y = role.Lines[p].y;
                //该位置是否有其他进攻武将
                var c_1 = attackrole.Any(q => q.SoldierCount > 0 && q.X == r_x && q.Y == r_y);
#if DEBUG
                XTrace.WriteLine("role:{4} p:{0} x:{1} y:{2} c:{3}", p, r_x, r_y, c_1, role.RoleId);
#endif
                if (c_1)
                {
                    RemoveNewRoleVo(rid, moves); return false;
                }
                //该位置是否有其他武将
                var c_2 = defenseRoles.Any(q => q.SoldierCount > 0 && q.X == r_x && q.Y == r_y);
                if (c_2) return false;
                //不能移动到本丸
                if (r_x == 0 && r_y == 4) { RemoveNewRoleVo(rid, moves); return false; }
                rolevo.lines.Add(new PointVo()
                {
                    x = r_x,
                    y = r_y,
                });
                role.Index++; role.X = r_x; role.Y = r_y;
                //进攻城门或者城门已破 进攻范围变为1格
                if (role.isFightDoor || fight.Door.SoldierCount == 0)
                {
                    role.AttackRange = Common.GetInstance().Sacked(role.X, role.Y);
                }
                else
                {
                    role.AttackRange = Common.GetInstance().GetAttackRangeInit(role.SoldierId, role.X, role.Y);
                }
                var isattack = CheckIsAttack(fight, rid, moves);
                if (isattack) return true;
            }
            return true;
        }



        /// <summary>
        /// 防守武将加入到回合中
        /// </summary>
        /// <param name="role">防守武将</param>
        /// <param name="moves">回合数据</param>
        /// <returns></returns>
        private WarFightRoleVo AddNewRoleVo(DefenseRoles role, WarMovesVo moves)
        {
            var vo = moves.roles.FirstOrDefault(q => q.roleId == role.RoleId);

            if (vo == null)
            {
                vo = EntityToRoleVo(role);
                moves.roles.Add(vo);
            }

            return vo;
        }

        /// <summary>
        /// 进攻武将加入到回合中
        /// </summary>
        /// <param name="role">进攻武将</param>
        /// <param name="moves">回合数据</param>
        /// <returns></returns>
        private WarFightRoleVo AddNewRoleVo(AttackRoles role, WarMovesVo moves)
        {
            var vo = moves.roles.FirstOrDefault(q => q.roleId == role.RoleId);

            if (vo == null)
            {
                vo = EntityToRoleVo(role);
                moves.roles.Add(vo);
            }

            return vo;
        }


        private void RemoveNewRoleVo(Int64 rid, WarMovesVo moves)
        {
            var vo = moves.roles.FirstOrDefault(q => q.roleId == rid);
            moves.roles.Remove(vo);
        }


        /// <summary>
        /// 防守武将实体转换成vo
        /// </summary>
        /// <param name="role">防守武将</param>
        /// <returns></returns>
        private WarFightRoleVo EntityToRoleVo(DefenseRoles role)
        {
            return new WarFightRoleVo()
            {
                blood = role.SoldierCount,
                roleId = role.RoleId,
                point = new PointVo() { x = role.X, y = role.Y },
                type = role.type,
                campType = 0,
                lines = null,
                effects = new List<WarEffectVo>(),
                energy = role.qili,
            };

        }

        /// <summary>
        /// 进攻武将实体转换成vo
        /// </summary>
        /// <param name="role">进攻武将</param>
        /// <returns></returns>
        private WarFightRoleVo EntityToRoleVo(AttackRoles role)
        {
            return new WarFightRoleVo()
            {
                blood = role.SoldierCount,
                roleId = role.RoleId,
                point = new PointVo() { x = role.X, y = role.Y },
                type = 0,
                campType = 0,
                lines = null,
                effects = new List<WarEffectVo>(),
                energy = role.qili

            };

        }
    }
}
