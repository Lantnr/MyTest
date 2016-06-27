using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;

namespace TGG.Module.War.Service.Fight
{
    public partial class WarFight
    {
        public WarFight(int planId, BaseWarCopy temp, IEnumerable<WarRolesLinesVo> attackrolRolesVos, List<tg_war_role> attackroles, Int32 frontid, int morale, string username)
        {
            #region 防守武将


            var zhenId = GetRandomNumber(temp.zhenId).First();
            var baseLand = Variable.BASE_LAND_POOL.FirstOrDefault(m => m.id == planId);
            if (baseLand == null) return;
            DefenseFrontId = zhenId;
            Area = GetMapArea(baseLand.landConfig);
            DefenseRoles = GetNpcDefenseRoles(baseLand.ambushConfig, temp.id);

            DefenseSoldierCount = DefenseRoles.Sum(m => m.SoldierCount);
            if (!isInitSuccess) { isInitSuccess = false; return; }
            //初始武将防守范围
            DefenseRange = Common.GetInstance().GetDefenseRangeInit(DefenseRoles);
            GetDoorAndCityInit(temp.gateDurable, temp.baseDurable); //城门本丸初始

            #endregion

            #region 进攻数据初始

            AttackRolesInit(attackrolRolesVos, attackroles, morale, username);
            AttackFrontId = frontid;

            #endregion

            WeatherState = GetWeatherStateInit();//初始天气持续回合

            FiveState = GetFiveStateInit(); //初始五常持续回合

            AttackSort = GetRolesSort();//武将出手顺序初始
        }

        /// <summary> 城门本丸初始</summary>
        private void GetDoorAndCityInit(int gateDurable, int baseDurable)
        {
            Door = new DefenseRoles();
            City = new DefenseRoles();
            Door.RoleId = RNG.Next();
            City.RoleId = RNG.Next();
            Door.SoldierCount = gateDurable;
            City.SoldierCount = baseDurable;
            Door.type = (int)WarFightRoleType.城门;
            City.type = (int)WarFightRoleType.本丸;
            City.X = 0;
            City.Y = 4;

        }

        /// <summary> 初始进攻武将 </summary>
        /// <param name="roles"> 武将vo集合 </param>
        /// <param name="username">玩家名字</param>
        /// <returns></returns>
        private void AttackRolesInit(IEnumerable<WarRolesLinesVo> roles, List<tg_war_role> warrolelist, int morale, string username)
        {
            var baseroles = tg_role.GetRoleByIds(warrolelist.Select(q => q.rid).Distinct().ToList());
            var selectroles = warrolelist.Select(q =>
            {
                var role = baseroles.FirstOrDefault(m => m.id == q.rid);
                var tgrolelines = roles.FirstOrDefault(m => m.rid == q.rid); //roles中的rid为个人战的主键Id或备大将的基表Id
                var basesoldier = Variable.BASE_WAR_ARMY_SOLDIER.FirstOrDefault(m => m.id == q.army_id);
                if (role == null || tgrolelines == null || basesoldier == null)
                {
                    isInitSuccess = false;
                    return null;
                }
                var baserole = Variable.BASE_ROLE.FirstOrDefault(n => n.id == role.role_id);
                if (baserole == null) return null;
                return new AttackRoles()
                {
                    morale = morale,
                    RoleId = q.rid,
                    Role = role,
                    WarRole = q,
                    Lines = tgrolelines.lines,
                    SoldierId = q.army_id,
                    SoldierCount = q.army_soldier,
                    bloodMax = q.army_soldier,
                    speed = basesoldier.speed,
                    X = tgrolelines.lines[0].x,
                    Y = tgrolelines.lines[0].y,
                    AttackRange = Common.GetInstance().GetAttackRangeInit(q.army_id, tgrolelines.lines[0].x, tgrolelines.lines[0].y),
                    attack = basesoldier.baseAttack,  //兵种基础进攻值
                    defense = basesoldier.baseDefence,//兵种基础防御值
                    crit = basesoldier.crit,          //基础暴击几率
                    hits = basesoldier.hits,            //初始命中率
                    hurtRange = basesoldier.hurtRange,
                    roleName = role.role_state == (int)RoleStateType.PROTAGONIST ? username : baserole.name

                };
            }).ToList();
            AttackRoles = selectroles;
            AttackSoldierCount = selectroles.Sum(q => q.SoldierCount);
            AttackInfluence();
            //技能
            AttackEffectInit();
            //装备
            AttackRolesEquipInit();
        }

        /// <summary>
        /// 已选武将集合
        /// </summary>
        public static List<int> selectedIds = new List<int>();

        /// <summary> 获取NPC防守武将集合 </summary>
        /// <param name="str">伏兵配置</param>
        /// <returns></returns>
        private static List<DefenseRoles> GetNpcDefenseRoles(string str, int copyId)
        {
            var list = new List<DefenseRoles>();
            selectedIds = new List<int>();
            foreach (var item in str.Split('|'))
            {
                var array = item.Split('_');
                if (array.Length != 3) continue;
                var type = array[0];//伏兵类型：1=远程，2=近战
                var x = Convert.ToInt32(array[1]);//坐标X
                var y = Convert.ToInt32(array[2]);//坐标Y

                var roleid = GetRoleId(type, copyId);
                if (roleid == 0) continue;
                selectedIds.Add(roleid);
                var baseWarMonster = Variable.BASE_WAR_MONSTER.FirstOrDefault(m => m.id == roleid);
                if (baseWarMonster == null) continue;
                var baseArmyType = GetBaseWarArmySoldier(type);
                if (baseArmyType == null) continue;
                var featuresId = GetRandomNumber(baseWarMonster.featuresId).First();
                var ninjaId = GetRandomNumber(baseWarMonster.ninjaId).First();
                var cheatcodeId = GetRandomNumber(baseWarMonster.cheatcodeId).First();
                var mysteryId = GetRandomNumber(baseWarMonster.mysteryId).First();
                var skills = new List<WarFightSkill>
                {
                    BuildSkill(featuresId),
                    BuildSkill(mysteryId, baseWarMonster.mysteryLevel),
                    BuildNinjaSkill(ninjaId, baseWarMonster.ninjaLevel),
                    BuildSkill(cheatcodeId, baseWarMonster.cheatcodeLevel),
                };
                var role = new DefenseRoles()
                {
                    skills = skills,
                    RoleId = RNG.Next(),
                    hits = baseArmyType.hits,
                    crit = baseArmyType.crit,
                    dodge = baseArmyType.dodge,
                    SoldierId = baseArmyType.id,
                    morale = baseWarMonster.morale,
                    attack = baseArmyType.baseAttack,
                    type = (int)WarFightRoleType.伏兵,
                    defense = baseArmyType.baseDefence,
                    bloodMax = baseWarMonster.forceValue,
                    SoldierCount = baseWarMonster.forceValue,
                    EquipAddAttack = baseWarMonster.equipAttack,
                    EquipAddDefense = baseWarMonster.equipDefense,
                    WarRole = new tg_war_role
                    {
                        army_morale = baseWarMonster.morale,
                        army_soldier = baseWarMonster.forceValue,
                        army_id = baseArmyType.id,
                    },
                    Role = new tg_role
                    {
                        role_id = baseWarMonster.roleId,
                        base_force = baseWarMonster.force,
                        base_brains = baseWarMonster.brains,
                        base_captain = baseWarMonster.captain,
                    },
                    isNpc = 1,
                    X = x,
                    Y = y,
                    hurtRange = baseArmyType.hurtRange,
                    roleName = baseWarMonster.name,
                };
                list.Add(role);
            }
           
            return list;
        }

        /// <summary> 获取NPC防守武将集合 </summary>
        /// <param name="str">伏兵配置</param>
        /// <param name="size">据点规模</param>
        /// <param name="influence">势力</param>
        /// <returns></returns>
        private static List<DefenseRoles> GetNpcDefenseRoles(string str, int size, int influence)
        {
            var list = new List<DefenseRoles>();
            selectedIds = new List<int>();
            foreach (var item in str.Split('|'))
            {
                var id = 0;
                var flag = true;
                int number = 0;
                var array = item.Split('_');
                if (array.Length != 3) continue;
                var type = array[0];//伏兵类型：1=远程，2=近战
                var x = Convert.ToInt32(array[1]);//坐标X
                var y = Convert.ToInt32(array[2]);//坐标Y

                var roleid = GetRoleId(type, size, influence);
                while (flag)
                {
                    if (selectedIds.Contains(roleid))
                    {
                        id = GetRoleId(type, size, influence); ;
                    }
                    else
                    {
                        selectedIds.Add(id);
                        flag = false;
                    }
                    if (number > 5)
                    {
                        flag = false;
                        id = -100;
                    }
                    number++;
                }
                if (roleid == -100) continue;

                var baseWarMonster = Variable.BASE_WAR_MONSTER.FirstOrDefault(m => m.id == roleid);
                if (baseWarMonster == null) continue;
                var baseArmyType = GetBaseWarArmySoldier(type);
                if (baseArmyType == null) continue;
                var featuresId = GetRandomNumber(baseWarMonster.featuresId).First();
                var ninjaId = GetRandomNumber(baseWarMonster.ninjaId).First();
                var cheatcodeId = GetRandomNumber(baseWarMonster.cheatcodeId).First();
                var mysteryId = GetRandomNumber(baseWarMonster.mysteryId).First();
                var skills = new List<WarFightSkill>
                {
                    BuildSkill(featuresId),
                    BuildSkill(mysteryId, baseWarMonster.mysteryLevel),
                    BuildNinjaSkill(ninjaId, baseWarMonster.ninjaLevel),
                    BuildSkill(cheatcodeId, baseWarMonster.cheatcodeLevel),
                };
                var role = new DefenseRoles()
                {
                    skills = skills,
                    RoleId = RNG.Next(),
                    hits = baseArmyType.hits,
                    crit = baseArmyType.crit,
                    dodge = baseArmyType.dodge,
                    SoldierId = baseArmyType.id,
                    morale = baseWarMonster.morale,
                    attack = baseArmyType.baseAttack,
                    type = (int)WarFightRoleType.伏兵,
                    defense = baseArmyType.baseDefence,
                    bloodMax = baseWarMonster.forceValue,
                    SoldierCount = baseWarMonster.forceValue,
                    EquipAddAttack = baseWarMonster.equipAttack,
                    EquipAddDefense = baseWarMonster.equipDefense,
                    WarRole = new tg_war_role
                    {
                        army_morale = baseWarMonster.morale,
                        army_soldier = baseWarMonster.forceValue,
                    },
                    Role = new tg_role
                    {
                        role_id = baseWarMonster.roleId,
                        base_force = baseWarMonster.force,
                        base_brains = baseWarMonster.brains,
                        base_captain = baseWarMonster.captain,
                    },
                    isNpc = 1,
                    X = x,
                    Y = y,
                    hurtRange = baseArmyType.hurtRange,
                    roleName = baseWarMonster.name,
                };
                list.Add(role);
            }
            return list;
        }

        private static BaseWarArmySoldier GetBaseWarArmySoldier(string type)
        {
            var list = Variable.BASE_WAR_ARMY_SOLDIER.Where(m => m.armyType.Contains(type)).ToList();
            if (!list.Any()) return null;
            if (list.Count() == 1) return list[0];
            var rd = new Random();
            var n = rd.Next(0, list.Count());
            return list[n];
        }

        private static int GetRoleId(string type, int copyId)
        {
            var list = Variable.BASE_WAR_MONSTER.Where(m => m.copyId == copyId && m.armyType.Contains(type) && !selectedIds.Contains(m.id)).ToList();
            var ids = list.Select(m => m.id).ToList();
            var ls = GetRandomNumber(ids);
            if (!ls.Any()) return 0;
            return ls.First();
        }

        /// <summary>
        /// 根据兵种类型、据点规模、所属势力随机获取怪物
        /// </summary>
        /// <param name="type"></param>
        /// <param name="size"></param>
        /// <param name="influence"></param>
        /// <returns></returns>
        private static int GetRoleId(string type, int size, int influence)
        {
            var list = Variable.BASE_WAR_MONSTER.Where(m => m.armyType.Contains(type)).ToList();
            var ids = list.Select(m => m.id).ToList();
            return GetRandomNumber(ids).First();

        }

        private static WarFightSkill BuildSkill(int skillid, int level = 1)
        {
            var skill = Variable.BASE_HEZHANSKILL.FirstOrDefault(m => m.id == skillid);
            var temp = Variable.BASE_HEZHANSKILLEFFECT.FirstOrDefault(m => m.skillid == skillid && m.level == level);
            if (skill == null || temp == null) return new WarFightSkill();
            return new WarFightSkill
                {
                    type = GetType(skill.type),
                    FightSkillEffects = WarSkillEffect.GetEffectStringInit(temp.effects, false),
                    effectBaseInfo = temp,
                    Condition = temp.condition.Split('|').ToList(),
                };
        }

        private static WarFightSkill BuildNinjaSkill(int skillid, int level = 1)
        {
            var skill = Variable.BASE_FIGHTSKILL.FirstOrDefault(m => m.id == skillid && m.typeSub == (int)FightSkillType.BATTLES);
            var temp = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.skillid == skillid && m.level == level);
            if (skill == null || temp == null) return new WarFightSkill();
            return new WarFightSkill
            {
                type = skill.type == (int)SkillType.ESOTERICA ? WarFightSkillType.NinjaSkill : WarFightSkillType.NinjaMystery,
                FightSkillEffects = WarSkillEffect.GetEffectStringInit(temp.effects, false),
                effectBaseInfo = BuildData(temp),
                Condition = temp.condition != null ? temp.condition.Split('|').ToList() : null,
            };
        }

        private static BaseHeZhanSkillEffect BuildData(BaseFightSkillEffect temp)
        {
            return new BaseHeZhanSkillEffect
            {
                skillid = temp.skillid,
                level = temp.level,
                effects = temp.effects,
                condition = temp.condition,
            };
        }

        /// <summary> 获取合战战斗类型 </summary>
        private static WarFightSkillType GetType(int type)
        {
            switch (type)
            {
                case (int)SkillType.COMMON:
                    {
                        return WarFightSkillType.Character;
                    }
                case (int)SkillType.HEZHAN_CHEATCODE:
                    {
                        return WarFightSkillType.Skill;
                    }
                case (int)SkillType.HEZHAN_MYSTERY:
                    {
                        return WarFightSkillType.Katha;
                    }
                default:
                    {
                        return WarFightSkillType.Area;
                    }
            }
        }

        /// <summary> 根据NPC地形方案池获取战争地图中的地形设定 </summary>
        /// <param name="str">地形配置</param>
        private static List<Common.MapArea> GetMapArea(string str)
        {
            var list = new List<Common.MapArea>();
            foreach (var item in str.Split('|'))
            {
                var array = item.Split('_');
                if (array.Length != 3) continue;

                var id = Convert.ToInt32(array[0]);
                var x = Convert.ToInt32(array[1]);
                var y = Convert.ToInt32(array[2]);
                if (x <= 0 || y <= 0) continue;
                var baseLand = Variable.BASE_WAR_AREA.FirstOrDefault(m => m.id == id);
                if (baseLand == null) continue;

                var sharplist = Common.GetInstance().GetSharp(baseLand.sharp, x, y);//将每种地形转换成战争地图信息
                list.AddRange(Common.GetInstance().GetOneSharpToMap(sharplist, id, baseLand.type, baseLand.sunEffect, baseLand.rainEffect));
            }
            return list;
        }

        /// <summary> 切割字符串获取随机数 </summary>
        /// <param name="str">字符串</param>
        /// <param name="count">要获取的个数</param>
        private static IEnumerable<int> GetRandomNumber(string str, int count = 1)
        {
            var array = str.Split('|').Select(m => Convert.ToInt32(m)).ToList();
            return GetRandomNumber(array, count);
        }

        /// <summary> 切割字符串获取随机数 </summary>
        /// <param name="array">字符串</param>
        /// <param name="count">要获取的个数</param>
        private static IEnumerable<int> GetRandomNumber(IReadOnlyList<int> array, int count = 1)
        {
            var length = array.Count;
            if (length < count) return new List<int>();
            if (length == 1) return new List<int> { Convert.ToInt32(array[0]) };
            var number = RNG.Next(0, length - 1, count).ToList();
            return number.Select(n => array[n]).ToList();
        }
    }
}
