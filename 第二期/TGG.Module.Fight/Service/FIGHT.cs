using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Module.Fight.Service
{
    /// <summary>
    /// 战斗类
    /// </summary>
    public partial class FIGHT
    {

        private FightObject FO = new FightObject();



        /// <summary> 组装双方数据 </summary>
        /// <param name="p">己方阵形</param>
        /// <param name="rivalid">对手Id</param>
        /// <param name="roleid">玩家主角武将id</param>
        private ResultType ReadyData(tg_fight_personal p, Int64 rivalid, Int64 roleid)
        {
            var pi = BuildPartyItem(p, roleid); //己方
            var pr = BuildRivalPartyItem(rivalid); //对方
            if (pr == null) return ResultType.FIGHT_RIVAL_PERSONAL_ERROR;    //获取对方数据错误
            SetMeLeadHp(pi.Roles, 0);
            SetRivalHp(pr.Roles, 0);
            SetSeme(pi, pr);
            InitRound();
            return ResultType.SUCCESS;
        }

        private void StartFight()
        {
            var flag = FO.shotIsSeme;
            if (!FindRoleIsExist(flag)) { FO.isSeme = false; return; }
            if (!FindRoleIsExist(!flag)) { FO.isSeme = true; return; }
            var role = GetShotRole(flag);
            if (!RoundCount(role.id, flag)) return; //武将所在回合验证
            if (!flag) FO.Uke.Yin.yinCount += 1; else FO.Seme.Yin.yinCount += 1;

            var move = new List<FightStep>();
            if (RoleBuffIsExist(role, (int)FightingSkillType.DIZZINESS)) //验证是否有被眩晕
            {
                move.Add(BuildFightStep(role.id, (int)FightShotType.DEFAULT, new List<Int64>()));
                goto Yin;
            }
            if (RoleBuffIsExist(role, (int)FightingSkillType.SEAL))      //验证封印
                goto Attack;
            var im = IsMystery(role);
            if (im.Any())
            {
                move.AddRange(im);
                goto Yin;
            }
            var isk = IsSkill(role);
            if (isk.Any())
            {
                move.AddRange(isk);
                goto Yin;
            }
        Attack:
            Attack(true, false);
        Yin:
            IsYin();
            FO.moves.Add(FO.moves.Count(), move);
            SetMatrix(FO.Seme.Matrix, role.id);
        }

        #region 攻击

        /// <summary> 攻击 </summary>
        /// <param name="flag">是否增加气力</param>
        /// <param name="sign">是否群体</param>
        private void Attack(bool flag, bool sign)
        {
            var list = new List<RoleObject>();
            var role = GetShotRole(FO.isSeme); //出手武将
            if (flag) role.angerCount += 1;
            if (role.angerCount > 8) role.angerCount = 8;
            if (!sign) list.Add(GetBlowRole(!FO.isSeme));
            else
                list = FO.shotIsSeme ? FO.Uke.Roles : FO.Seme.Roles;
            Attack(role, list, sign);
        }

        private void Attack(RoleObject attackrole, IEnumerable<RoleObject> roles, bool flag)
        {
            foreach (var item in roles)
            {
                var iscrit = false;
                if (!IsTrue(attackrole.IgnoreDuck)) { if (IsDuck(item))continue; }
                var number = DamageCount(attackrole, item, ref iscrit);
                if (flag) number = Convert.ToInt64(number * 0.3);                  //群攻增加0.3系数
                item.hp = item.hp - number;
                if (iscrit)
                {
                    item.Buff_A.Add(new BuffItem() { Type = (int)FightingSkillType.CRIT, Value = -number });
                }
                else
                {
                    item.damage = number;
                }
            }


        }

        /// <summary>是否闪避 </summary>
        private bool IsDuck(RoleObject role)
        {
            if (!IsTrue(role.dodgeProbability)) return false;
            role.Buff_A.Add(new BuffItem() { Holder = role.id, Type = (int)FightingSkillType.DODGE, Target = role.id });
            return true;
        }

        /// <summary>伤害计算</summary>
        /// <param name="attackrole">攻击武将</param>
        /// <param name="defenserole">防守武将</param>
        /// <param name="flag">是否暴击</param>
        private Int64 DamageCount(RoleObject attackrole, RoleObject defenserole, ref bool flag)
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

            var A0 = attackrole.attack;              //A总攻击
            var A1 = defenserole.defense;            //B总防御
            var A2 = attackrole.critProbability;     //A会心几率
            flag = IsTrue(A2);                       //是否暴击
            var A3 = flag ? 1.5 + (attackrole.critAddition / 100) : 1.0;  //A会心效果
            var A5 = A0 - A1;                        //A总攻击-B总防御
            var A6 = A5 > 0 ? A5 : 1;                //A总攻击-B总防御
            var A7 = 1 + (attackrole.hurtIncrease / 100);
            var A10 = (A6) * ((100 - defenserole.hurtReduce) / 100) * A3 * A7;

            double number = 1;
            var n = FO.round - 21;
            if (n >= 0)
            {
                var b = n / 10;
                if (b == 0)
                    number = number + 1;
                if (b > 0)
                    number = number + (1 + n * 0.5);
            }
            var count = A10 * number;
            //if (dic_vocation.ContainsKey(attackrole.user_id)) count = count * dic_vocation[attackrole.user_id];

            return count < 0 ? 1 : Convert.ToInt64(count);
        }

        #endregion

        #region 奥义

        /// <summary> 是否释放奥义</summary>
        /// <param name="attackrole">武将vo</param>
        private List<FightStep> IsMystery(RoleObject attackrole)
        {
            if (attackrole.mystery == null) return new List<FightStep>();
            var role = FO.shotIsSeme ?
                FO.Seme.InitRoles.FirstOrDefault(m => m.id == attackrole.id) :
                FO.Uke.InitRoles.FirstOrDefault(m => m.id == attackrole.id);//初始时武将状态  
            if (role == null) return new List<FightStep>();
            double hpcount = Convert.ToDouble(attackrole.hp) / Convert.ToDouble(role.hp);
            var probability = attackrole.mystery_probability;
            if (hpcount <= 0.5) probability = probability + (60 - (hpcount * 100));
            if (!IsTrue(probability)) return new List<FightStep>();
            return AnalySkill(attackrole, (int)FightShotType.MYSTERY);
        }

        /// <summary> 几率是否成功 </summary>
        /// <param name="number">几率</param>
        private bool IsTrue(double number)
        {
            return (number > 0) && new RandomSingle().IsTrue(number);
        }

        #endregion

        #region 秘技

        /// <summary>是否释放秘技</summary>
        /// <param name="attackrole">武将vo</param>
        private List<FightStep> IsSkill(RoleObject attackrole)
        {
            if (attackrole.cheatCode == null) return new List<FightStep>();
            return AnalySkill(attackrole, (int)FightShotType.SKILL);
        }

        #endregion

        #region 印

        /// <summary>是否触发印</summary>
        private void IsYin()
        {
            var yin = FO.shotIsSeme ? FO.Seme.Yin : FO.Uke.Yin;
            if (yin == null) return;
            var baseYin = Variable.BASE_YIN.FirstOrDefault(m => m.id == yin.baseid); //读取印基表
            if (baseYin == null) return;
            if (yin.yinCount < baseYin.yinCount) return;
            yin.yinCount -= baseYin.yinCount;
            var baseEffect = Variable.BASE_YINEFFECT.FirstOrDefault(m => m.yinId == baseYin.id && m.level == yin.level);//读取印效果表
            if (baseEffect == null) return;
            //var effects = baseEffect.effects; //技能效果
            var move = SkillEffect(baseEffect.effects, (int)FightShotType.YIN);
            if (baseEffect.isQuickAttack == 1)    //1为当前回合攻击  0为当前回合不攻击
                Attack(false, baseEffect.attackRange == (int)EffectRangeType.ALL);
        }

        #endregion

        #region 战斗技能公共解析方法

        private List<FightStep> AnalySkill(RoleObject role, int type)
        {
            SkillObject skill;
            switch (type)
            {
                case (int)FightShotType.MYSTERY: { skill = role.mystery; break; }
                case (int)FightShotType.SKILL: { skill = role.cheatCode; break; }
                //case (int)FightShotType.YIN: { skill = FO.isSeme ? FO.Seme.Yin : FO.Uke.Yin; break; }
                default: { return new List<FightStep>(); }
            }
            var bskill = Variable.BASE_FIGHTSKILL.FirstOrDefault(m => m.id == skill.baseId);
            if (bskill == null) return new List<FightStep>();
            if (role.angerCount < bskill.energy) return new List<FightStep>(); ;//气力验证
            role.angerCount = role.angerCount - bskill.energy;//扣除气力
            var baseEffect = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.skillid == skill.baseId && m.level == skill.level);
            if (baseEffect == null) return new List<FightStep>(); ;
            var move = SkillEffect(baseEffect.effects, type);
            if (baseEffect.isQuickAttack == 1)
                Attack(false, baseEffect.attackRange == (int)EffectRangeType.ALL);
            return move;

        }

        /// <summary> 技能效果 </summary>
        /// <param name="effects">技能效果</param>
        /// <param name="type">技能类型</param>
        private List<FightStep> SkillEffect(string effects, int type)
        {
#if DEBUG
            //effects = "10_1_1_2_50";
#endif
            var l = new List<FightStep>();
            var data = effects.Split('|');                 //拆分技能效果
            foreach (var item in data)
            {
                var skill = BuildEffectsObject(item);       //组装战斗技能效果实体
                if (skill == null) continue;
                var list = GetTargetRole(skill);
                if (!list.Any()) return new List<FightStep>();
                EffectType(list, skill);
                l.Add(new FightStep
                {
                    attackId = GetShotRole(FO.shotIsSeme).id,
                    showType = type,
                    //SemeRoles = GetRoles(true),
                    //UkeRoles = GetRoles(false),
                    SemeRoles = FO.Seme.Roles,
                    UkeRoles = FO.Uke.Roles,
                });
            }
            SetRoleBuff();
            return l;
        }

        private void SetRoleBuff()
        {

            foreach (var item in FO.Seme.Roles)
            {
                item.Buff_A.RemoveAll(m => m.Round <= FO.round);
                item.Buff_B.RemoveAll(m => m.Round <= FO.round);
                foreach (var model in item.Buff_A)
                {
                    if (model.Type == (int)FightingSkillType.DIZZINESS) continue;
                    if (model.Type == (int)FightingSkillType.SEAL) continue;
                    item.Buff_B.Add(model);
                }
            }
            foreach (var item in FO.Uke.Roles)
            {
                item.Buff_A.RemoveAll(m => m.Round <= FO.round);
                item.Buff_B.RemoveAll(m => m.Round <= FO.round);
                foreach (var model in item.Buff_A)
                {
                    if (model.Type == (int)FightingSkillType.DIZZINESS) continue;
                    if (model.Type == (int)FightingSkillType.SEAL) continue;
                    item.Buff_B.Add(model);
                }
            }
        }


        /// <summary> 获取把buff计算到属性的武将集合 </summary>
        /// <param name="falg"></param>
        /// <returns></returns>
        private List<RoleObject> GetRoles(bool falg)
        {
            return falg ? FO.Seme.Roles.Select(SetRoleAtt).ToList() : FO.Uke.Roles.Select(SetRoleAtt).ToList();
        }

        private RoleObject SetRoleAtt(RoleObject model)
        {
            var role = Clone(model);
            foreach (var item in role.Buff_A)
            {
                role = SetRoleAtt(role, item);
            }
            foreach (var item in role.Buff_B)
            {
                role = SetRoleAtt(role, item);
            }
            foreach (var item in role.Buff_C)
            {
                role = SetRoleAtt(role, item);
            }
            return role;
        }

        public T Clone<T>(T realObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, realObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }

        private RoleObject SetRoleAtt(RoleObject role, BuffItem item)
        {
            switch (item.Type) //效果类型
            {
                case (int)FightingSkillType.INCREASE_ATTACK: { role.attack += item.Value; break; } //增加攻击
                case (int)FightingSkillType.REDUCE_ATTACK:
                    {
                        role.attack += item.Value;
                        if (role.attack < 0) role.attack = 0; break;
                    }//减少攻击
                case (int)FightingSkillType.INCREASE_DEFENSE: { role.defense += item.Value; break; }//增加防御
                case (int)FightingSkillType.REDUCE_DEFENSE:
                    {
                        role.defense += item.Value;
                        if (role.defense < 0) role.defense = 0; break;
                    }//减少防御
                case (int)FightingSkillType.KMOWING_PROBABILITY: { role.critProbability += item.Value; break; }//会心几率
                case (int)FightingSkillType.INCREASE_KMOWING:
                    {
                        role.critAddition += item.Value; break;
                    }//增加会心效果
                case (int)FightingSkillType.DUCK_PROBABILITY:
                    {
                        role.dodgeProbability += item.Value; break;
                    }//闪避率
                case (int)FightingSkillType.IGNORE_DUCK_PROBABILITY: { role.IgnoreDuck += item.Value; break; }//无视闪避几率
                case (int)FightingSkillType.INCREASES_DAMAGE_PERCENTAGE: { role.hurtIncrease += item.Value; break; }//增加伤害百分比
                case (int)FightingSkillType.REDUCE_DAMAGE_PERCENTAGE: { role.hurtReduce += item.Value; break; }//降低伤害百分比
                case (int)FightingSkillType.INCREASES_STRENGTH: { role.angerCount += Convert.ToInt32(item.Value); break; }//增加气力
                case (int)FightingSkillType.REDUCE_STRENGTH:
                    {
                        role.angerCount -= Convert.ToInt32(item.Value);
                        if (role.angerCount < 0) role.angerCount = 0; break;
                    }//减少气力
                case (int)FightingSkillType.INCREASES_MYSTERY_PROBABILITY: { role.mystery_probability += item.Value; break; }//增加奥义触发几率
            }
            return role;
        }

        /// <summary>效果类型</summary>
        /// <param name="list">受影响的武将集合</param>
        /// <param name="skill"></param>
        private void EffectType(IEnumerable<RoleObject> list, EffectsObject skill)
        {
            foreach (var item in list)
            {
                if (skill.robabilityValues < 100) if (!IsTrue(skill.robabilityValues)) continue;
                switch (skill.type) //技能效果类型
                {
                    case (int)FightingSkillType.INCREASES_YINCOUNT:
                        {
                            if (FO.shotIsSeme) FO.Seme.Yin.yinCount += 1; else FO.Uke.Yin.yinCount += 1; break;
                        }//增加印计数
                    case (int)FightingSkillType.REDUCE__YINCOUNT:
                        {
                            if (FO.shotIsSeme)
                            {
                                var count = FO.Uke.Yin.yinCount - Convert.ToInt32(skill.values);
                                FO.Uke.Yin.yinCount = count < 0 ? 0 : count;
                            }
                            else
                            {
                                var count = FO.Seme.Yin.yinCount - Convert.ToInt32(skill.values);
                                FO.Seme.Yin.yinCount = count < 0 ? 0 : count;
                            }
                            break;
                        }//减少印计数
                    default: { break; }
                }
                var role = GetShotRole(FO.shotIsSeme);
                item.Buff_A.Add(new BuffItem() { Type = skill.type, InitValue = skill.values, Holder = role.id, Round = FO.round + skill.round, Target = item.id });
            }

        }

        /// <summary> 组装技能效果实体 </summary>
        /// <param name="item">要解析的字符串</param>
        private EffectsObject BuildEffectsObject(string item)
        {
            var array = item.Split('_');
            if (array.Length < 5 || array.Length > 6) return null;
            var skill = new EffectsObject();
            skill.type = Convert.ToInt32(array[0]);                  //技能效果类型
            skill.target = Convert.ToInt32(array[1]);                //技能效果目标 (1=本方 2=敌方)
            skill.range = Convert.ToInt32(array[2]);                 //技能效果范围 (1=单体 2=全体)
            skill.round = Convert.ToInt32(array[3]);                 //技能效果回合数
            skill.values = Convert.ToDouble(array[4]);               //技能效果值
            if (array.Length == 6)                                   //几率类型效果
                skill.robabilityValues = Convert.ToDouble(array[5]); //技能效果几率
            if (skill.robabilityValues <= 0) skill.robabilityValues = 100;
            return skill;
        }

        /// <summary> 获取技能目标武将 </summary>
        /// <param name="effect">技能</param>
        /// <returns></returns>
        private List<RoleObject> GetTargetRole(EffectsObject effect)
        {
            bool flag;
            switch (effect.target)                           //目标类型  1己方 2敌方
            {
                case (int)FightTargetType.OWN: { flag = true; break; }    //己方
                case (int)FightTargetType.ENEMY: { flag = false; break; } //敌方
                default: { return new List<RoleObject>(); }
            }

            switch (effect.range)                                                 //效果范围  1单人 2多人
            {
                case (int)EffectRangeType.SINGLE: { return new List<RoleObject> { flag ? GetBlowRole(FO.shotIsSeme) : GetBlowRole(!FO.shotIsSeme) }; } //单人
                case (int)EffectRangeType.ALL:
                    {
                        if (flag)
                            return FO.shotIsSeme ? FO.Seme.Roles : FO.Uke.Roles;
                        return FO.shotIsSeme ? FO.Uke.Roles : FO.Seme.Roles;
                    } //多人
                default: { return new List<RoleObject>(); }
            }
        }

        #endregion

        /// <summary> 回合计数 </summary>
        private bool RoundCount(Int64 roleid, bool flag)
        {
            if (!FO.roleRound.ContainsKey(roleid)) return false;

            var values = FO.roleRound[roleid] + 1;
            if (values <= FO.round) FO.roleRound[roleid] = values;
            else
            {
                if (!flag) return false;
                SetRoleRound();
                if (FO.roleRound.ContainsValue(FO.round - 1)) return false;
                FO.roleRound[roleid] = values;

            }
            return true;
        }

        /// <summary> 重新更新全局武将回合</summary>
        private void SetRoleRound()
        {
            var d = new Dictionary<Int64, int>();
            foreach (var item in FO.Seme.Roles.Where(m => m.hp > 0).Select(m => m.id))
            {
                if (FO.roleRound.ContainsKey(item))
                {
                    d.Add(item, FO.roleRound[item]);
                }
            }
            foreach (var item in FO.Uke.Roles.Where(m => m.hp > 0).Select(m => m.id))
            {
                if (FO.roleRound.ContainsKey(item))
                {
                    d.Add(item, FO.roleRound[item]);
                }
            }
            FO.roleRound.Clear();
            FO.roleRound = d;
        }

        /// <summary> 初始化回合 </summary>
        private void InitRound()
        {
            foreach (var item in FO.Seme.Roles.Select(m => m.id))
            {
                FO.roleRound.Add(item, 0);
            }
            foreach (var item in FO.Uke.Roles.Select(m => m.id))
            {
                FO.roleRound.Add(item, 0);
            }
        }

        /// <summary> 组装出招步骤 </summary>
        /// <param name="attackid"></param>
        /// <param name="showtype"></param>
        /// <param name="hitids"></param>
        /// <returns></returns>
        private FightStep BuildFightStep(Int64 attackid, int showtype, List<Int64> hitids)
        {
            var model = new FightStep();
            model.hitIds = hitids;
            model.attackId = attackid;
            model.showType = showtype;
            model.UkeRoles = FO.Uke.Roles;
            model.SemeRoles = FO.Seme.Roles;
            return model;
        }

        /// <summary> 武将Buff是否存在 </summary>
        /// <param name="role">当前武将</param>
        /// <param name="type">状态类型</param>
        /// <returns>true:有特殊状态 否则false</returns>
        private bool RoleBuffIsExist(RoleObject role, int type)
        {
            return role.Buff_A.Select(m => m.Type).Contains(type);
        }

        /// <summary> 查询某一方是否有存活武将 </summary>
        /// <param name="flag">true:主动方  false:被动方</param>
        private bool FindRoleIsExist(bool flag)
        {
            if (flag)
                return FO.Seme.Roles.Count(m => m.hp > 0) > 0;
            return FO.Uke.Roles.Count(m => m.hp > 0) > 0;
        }

        /// <summary> 获取出手武将 </summary>
        /// <param name="flag">true:主动方  false:被动方</param>
        private RoleObject GetShotRole(bool flag)
        {
            for (int i = 0; i < 5; i++)
            {
                RoleObject role;
                if (flag)
                {
                    role = GetRole(FO.Seme.Roles, FO.Seme.InitMatrix, FO.shotSeme);
                    if (role != null) return role;
                    FO.shotSeme = (FO.shotSeme >= 5) ? 1 : FO.shotSeme += 1;
                }
                else
                {
                    role = GetRole(FO.Uke.Roles, FO.Uke.InitMatrix, FO.shotUke);
                    if (role != null) return role;
                    FO.shotUke = (FO.shotUke >= 5) ? 1 : FO.shotUke += 1;
                }
            }
            return null;
        }

        /// <summary> 获取受击武将 </summary>
        /// <param name="flag">true:主动方  false:被动方</param>
        /// <returns></returns>
        private RoleObject GetBlowRole(bool flag)
        {
            for (int i = 1; i <= 5; i++)
            {
                var role = flag ? GetRole(FO.Seme.Roles, FO.Seme.Matrix, i) : GetRole(FO.Uke.Roles, FO.Uke.Matrix, i);
                if (role == null) continue;
                return role;
            }
            return null;
        }



        #region 获取武将

        private RoleObject GetRole(IEnumerable<RoleObject> roles, MatrixObject matrix, int number)
        {
            switch (number)
            {
                case 1: { return GetRole(roles, matrix.Matrix_1); }
                case 2: { return GetRole(roles, matrix.Matrix_2); }
                case 3: { return GetRole(roles, matrix.Matrix_3); }
                case 4: { return GetRole(roles, matrix.Matrix_4); }
                case 5: { return GetRole(roles, matrix.Matrix_5); }
                default: { return null; }
            }
        }

        private RoleObject GetRole(IEnumerable<RoleObject> list, Int64 roleid)
        {
            if (roleid == 0) return null;
            var role = list.FirstOrDefault(m => m.id == roleid);
            if (role != null && role.hp > 0) return role;
            return null;
        }

        #endregion

        #region 主动方位置更新

        /// <summary> 重新设置位置 </summary>
        /// <param name="matrix"></param>
        /// <param name="roleid"></param>
        private void SetMatrix(MatrixObject matrix, Int64 roleid)
        {
            if (matrix.Matrix_1 == roleid)
            {
                matrix.Matrix_1 = matrix.Matrix_2;
                matrix.Matrix_2 = matrix.Matrix_3;
                matrix.Matrix_3 = matrix.Matrix_4;
                matrix.Matrix_4 = matrix.Matrix_5;
                matrix.Matrix_5 = roleid;
                return;
            }
            if (matrix.Matrix_2 == roleid)
            {
                matrix.Matrix_2 = matrix.Matrix_3;
                matrix.Matrix_3 = matrix.Matrix_4;
                matrix.Matrix_4 = matrix.Matrix_5;
                matrix.Matrix_5 = matrix.Matrix_1;
                matrix.Matrix_1 = roleid;
                return;
            }
            if (matrix.Matrix_3 == roleid)
            {
                matrix.Matrix_3 = matrix.Matrix_4;
                matrix.Matrix_4 = matrix.Matrix_5;
                matrix.Matrix_5 = matrix.Matrix_1;
                matrix.Matrix_1 = matrix.Matrix_2;
                matrix.Matrix_2 = roleid;
                return;
            }
            if (matrix.Matrix_4 == roleid)
            {
                matrix.Matrix_4 = matrix.Matrix_5;
                matrix.Matrix_5 = matrix.Matrix_1;
                matrix.Matrix_1 = matrix.Matrix_2;
                matrix.Matrix_2 = matrix.Matrix_3;
                matrix.Matrix_3 = roleid;
                return;
            }
            if (matrix.Matrix_5 == roleid)
            {
                matrix.Matrix_5 = matrix.Matrix_1;
                matrix.Matrix_1 = matrix.Matrix_2;
                matrix.Matrix_2 = matrix.Matrix_3;
                matrix.Matrix_3 = matrix.Matrix_4;
                matrix.Matrix_4 = roleid;
                return;
            }
        }

        #endregion

        #region 设置数据

        /// <summary> 设置我方主角血量 (连续战斗任务，爬塔) </summary>
        private void SetMeLeadHp(List<RoleObject> MRs, Int64 hp)
        {
            switch (FO.srcType)
            {
                case (int)FightType.CONTINUOUS:
                case (int)FightType.DUPLICATE_SHARP:
                    {
                        var role = MRs.FirstOrDefault(m => m.isLead);
                        if (role == null) return;
                        role.hp = hp;
                        return;
                    }
                default: { return; }
            }
        }

        /// <summary> 设置对方血量 （美浓、一夜墨俣）</summary>
        private void SetRivalHp(List<RoleObject> RRs, int hp)
        {
            switch (FO.srcType)
            {
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING:
                    {
                        var role = RRs.FirstOrDefault();
                        if (role == null) return;
                        role.hp = hp;
                        return;
                    }
                default: { return; }
            }
        }

        /// <summary> 设置主动方 </summary>
        private void SetSeme(PartyItem pi, PartyItem pr)
        {
            var flag = false;
            switch (FO.srcType)
            {
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING: { flag = true; break; }
                default: { flag = pi.Roles.Sum(m => m.attack) >= pr.Roles.Sum(m => m.attack); break; }
            }
            if (flag)
            { FO.Seme = pi; FO.Uke = pr; }
            else
            { FO.Seme = pr; FO.Uke = pi; }
        }

        #endregion


        /// <summary> 组装对手战斗对象 </summary>
        /// <param name="id">对手id</param>
        private PartyItem BuildRivalPartyItem(Int64 id)
        {
            dynamic npc;
            switch (FO.srcType)
            {
                case (int)FightType.SIEGE:
                case (int)FightType.BUILDING:
                case (int)FightType.CONTINUOUS:
                case (int)FightType.NPC_FIGHT_ARMY: { npc = Variable.BASE_NPCARMY.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.SINGLE_FIGHT: { npc = Variable.BASE_NPCSINGLE.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.NPC_MONSTER: { npc = Variable.BASE_NPCMONSTER.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.DUPLICATE_SHARP: { npc = Variable.BASE_TOWERENEMY.FirstOrDefault(m => m.id == (int)id); break; }
                case (int)FightType.BOTH_SIDES:
                case (int)FightType.ONE_SIDE: { var p = tg_fight_personal.GetFindByUserId(id); return p == null ? null : BuildPartyItem(p); }
                default: { return null; }
            }
            if (npc == null) return null;
            return BuildNpcPartyItem(npc.matrix, npc.effectId);
        }

        #region 获取NPC 数据

        /// <summary>构建NPC个人战数据</summary>
        private List<Int64> GetNpcRoleIds(String matrix)
        {
            var list = new List<Int64>();
            var matrix_rid = matrix.Split(',');
            switch (FO.srcType)
            {
                case (int)FightType.NPC_MONSTER: { matrix_rid = GetMatrixRid(matrix_rid); break; }
            }

            for (var i = 0; i < matrix_rid.Length; i++)
            {
                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4: { list.Add(Convert.ToInt64(matrix_rid[i])); break; }
                }
            }
            return list;
        }

        /// <summary>构建NPC个人战数据</summary>
        private MatrixObject BuildFightPersonal(List<RoleObject> list)
        {
            var model = new MatrixObject();
            for (var i = 0; i < list.Count() - 1; i++)
            {
                switch (i)
                {
                    case 0:
                        model.Matrix_1 = Convert.ToInt64(list[0].id); break;
                    case 1:
                        model.Matrix_2 = Convert.ToInt64(list[1].id); break;
                    case 2:
                        model.Matrix_3 = Convert.ToInt64(list[2].id); break;
                    case 3:
                        model.Matrix_4 = Convert.ToInt64(list[3].id); break;
                    case 4:
                        model.Matrix_5 = Convert.ToInt64(list[4].id); break;
                }
            }
            return model;
        }

        /// <summary> 供点将用   多个Npc武将Id抽取5个 </summary>
        /// <param name="list">武将Id集合</param>
        /// <returns></returns>
        private string[] GetMatrixRid(string[] list)
        {
            int count = 5;
            int number = 4;
            var rolehome = Variable.BASE_ROLE_HOME.FirstOrDefault(m => m.id == FO.homeLv);
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

        /// <summary> 获取NPC武将 </summary>
        /// <param name="ids">要获取的id集合</param>
        /// <returns></returns>
        private List<RoleObject> GetNpcRoles(List<Int64> ids)
        {
            int number = 0;
            var list = new List<RoleObject>();
            var l = RNG.NextLess(-100, -1, ids.Count()).ToList();
            foreach (var item in ids)
            {
                var role = Variable.BASE_NPCROLE.FirstOrDefault(m => m.id == item);
                if (role == null) continue;
                var model = BuildRoleObject(role, l[number]);
                list.Add(model);
                number += 1;
            }
            return list;
        }

        #endregion

        #region 获取玩家战斗数据

        /// <summary> 组装玩家战斗参与对象 </summary>
        /// <param name="roleid">玩家主角武将id</param>
        private PartyItem BuildPartyItem(tg_fight_personal model, Int64 roleid = 0)
        {
            var matrix = BuildMatrixObject(model, roleid);
            return new PartyItem()
            {
                Matrix = matrix,
                InitMatrix = matrix,
                Yin = model.yid == 0 ? new YinObject() : BuildYinObject(tg_fight_yin.FindByid(model.yid)),
                Roles = GetMatrixRoles(matrix),
                userid = model.user_id,
            };
        }

        /// <summary> 获取武将信息 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<RoleObject> GetMatrixRoles(MatrixObject model)
        {
            var ids = GetMatrixRids(model);
            if (!ids.Any()) return new List<RoleObject>();
            var list = tg_role.GetFindAllByIds(ids);
            var rolelist = BuildRoleObject(list);
            var skilllist = GetRoleSkills(list);
            return SetRoleSkill(rolelist, skilllist);
        }

        /// <summary> 获取武将技能 </summary>
        /// <param name="rlist"></param>
        /// <param name="slist"></param>
        /// <returns></returns>
        private List<RoleObject> SetRoleSkill(List<RoleObject> rlist, List<SkillObject> slist)
        {
            foreach (var item in rlist)
            {
                var mystery = slist.FirstOrDefault(m => m.id == item.mystery.id);
                if (mystery != null) item.mystery = mystery;
                var cheatCode = slist.FirstOrDefault(m => m.id == item.cheatCode.id);
                if (cheatCode != null) item.cheatCode = cheatCode;
            }
            return rlist;
        }

        /// <summary> 获取集合武将中的技能和奥义 </summary>
        private List<SkillObject> GetRoleSkills(List<tg_role> list)
        {
            var ids = list.Select(m => Convert.ToInt64(m.art_cheat_code)).ToList();
            ids.AddRange(list.Select(m => Convert.ToInt64(m.art_mystery)));
            ids = ids.Where(m => m != 0).ToList();
            return ids.Any() ? BuildSkillObject(tg_role_fight_skill.GetFindAllByIds(ids)) : new List<SkillObject>();
        }

        /// <summary> 获取阵中武将Id </summary>
        /// <param name="model">要获取的阵</param>
        /// <returns>阵中武将Id集合</returns>
        private List<Int64> GetMatrixRids(MatrixObject model)
        {
            var ids = new List<Int64>();
            if (model.Matrix_1 != 0)
                ids.Add(model.Matrix_1);
            if (model.Matrix_2 != 0)
                ids.Add(model.Matrix_2);
            if (model.Matrix_3 != 0)
                ids.Add(model.Matrix_3);
            if (model.Matrix_4 != 0)
                ids.Add(model.Matrix_4);
            if (model.Matrix_5 != 0)
                ids.Add(model.Matrix_5);
            return ids;
        }

        #endregion

        #region 组装数据

        /// <summary> 组装阵数据 </summary>
        /// <param name="model"></param>
        /// <param name="roleid">玩家主角武将id</param>
        /// <returns></returns>
        private MatrixObject BuildMatrixObject(tg_fight_personal model, Int64 roleid = 0)
        {
            switch (FO.srcType)
            {
                case (int)FightType.CONTINUOUS:
                case (int)FightType.SINGLE_FIGHT:
                    {
                        return new MatrixObject()
                        {
                            Matrix_1 = roleid,
                            Matrix_2 = 0,
                            Matrix_3 = 0,
                            Matrix_4 = 0,
                            Matrix_5 = 0,
                        };
                    }
                default:
                    {
                        return new MatrixObject()
                        {
                            Matrix_1 = model.matrix1_rid,
                            Matrix_2 = model.matrix2_rid,
                            Matrix_3 = model.matrix3_rid,
                            Matrix_4 = model.matrix4_rid,
                            Matrix_5 = model.matrix5_rid,
                        };
                    }
            }
        }

        /// <summary> 组装印实体 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private YinObject BuildYinObject(tg_fight_yin model)
        {
            if (model == null) return null;
            return new YinObject()
            {
                id = model.id,
                baseid = model.yin_id,
                level = model.yin_level,
            };
        }

        /// <summary>根据印效果id组装YinObject </summary>
        /// <param name="effectId">效果表id</param>
        private YinObject BuildYinObject(int effectId)
        {
            var baseyinEffect = Variable.BASE_YINEFFECT.FirstOrDefault(m => m.id == effectId);
            return baseyinEffect == null ? null : new YinObject { baseid = baseyinEffect.yinId, level = baseyinEffect.level };
        }

        /// <summary> 组装技能实体集合 </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<SkillObject> BuildSkillObject(IEnumerable<tg_role_fight_skill> list)
        {
            return list.Select(BuildSkillObject).ToList();
        }

        /// <summary> 组装技能实体 </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private SkillObject BuildSkillObject(tg_role_fight_skill model)
        {
            return new SkillObject()
            {
                id = model.id,
                baseId = model.skill_id,
                level = model.skill_level,
            };
        }

        /// <summary> 组装武将实体 </summary>
        private List<RoleObject> BuildRoleObject(IEnumerable<tg_role> list)
        {
            return list.Select(BuildRoleObject).ToList();
        }

        /// <summary> 组装武将实体 </summary>
        private RoleObject BuildRoleObject(tg_role model)
        {
            var rolename = "";
            var baserole = TGG.Core.Global.Variable.BASE_ROLE.FirstOrDefault(m => m.id == model.role_id);
            if (baserole != null) rolename = baserole.name;
            return new RoleObject()
            {
                id = model.id,
                hp = model.att_life,
                roleName = rolename,
                lv = model.role_level,
                baseId = model.role_id,
                Buff_A = new List<BuffItem>(),
                Buff_B = new List<BuffItem>(),
                Buff_C = new List<BuffItem>(),
                hurtReduce = model.att_sub_hurtReduce,
                attack = tg_role.GetTotalAttack(model),
                monsterType = (int)FightRivalType.ROLE,
                hurtIncrease = model.att_sub_hurtIncrease,
                defense = Convert.ToInt32(model.att_defense),
                critAddition = tg_role.GetTotalCritAddition(model),
                mystery = new SkillObject() { id = model.art_mystery },
                critProbability = tg_role.GetTotalCritProbability(model),
                dodgeProbability = tg_role.GetTotalDodgeProbability(model),
                isLead = model.role_state == (int)RoleStateType.PROTAGONIST,
                cheatCode = new SkillObject() { id = model.art_cheat_code },
                mystery_probability = tg_role.GetTotalMysteryProbability(model),
            };
        }

        /// <summary> 组装武将实体 </summary>
        private RoleObject BuildRoleObject(BaseNpcRole model, int id)
        {
            return new RoleObject
            {
                id = id,
                lv = model.lv,
                hp = model.life,
                baseId = model.id,
                initHp = model.life,
                attack = model.attack,
                defense = model.defense,
                hurtReduce = model.hurtReduce,
                hurtIncrease = model.hurtIncrease,
                critAddition = model.critAddition,
                critProbability = model.critProbability,
                monsterType = (int)FightRivalType.MONSTER,
                dodgeProbability = model.dodgeProbability,
                mystery = BuildSkillObject(model.pmystery),
                cheatCode = BuildSkillObject(model.pcheatCode),
                mystery_probability = model.mysteryProbability,
            };
        }

        /// <summary> 根据技能效果id组装技能实体 </summary>
        /// <param name="id">技能效果id</param>
        private SkillObject BuildSkillObject(int id)
        {
            var skillEffect = Variable.BASE_FIGHTSKILLEFFECT.FirstOrDefault(m => m.id == id);
            return skillEffect == null ? null : new SkillObject { baseId = skillEffect.skillid, level = skillEffect.level };
        }

        /// <summary> 组装NPC PartyItem </summary>
        /// <param name="matrix">阵</param>
        /// <param name="effectId">印效果id</param>
        private PartyItem BuildNpcPartyItem(string matrix, int effectId)
        {
            var PI = new PartyItem();
            var ids = GetNpcRoleIds(matrix);
            PI.Roles = GetNpcRoles(ids);
            PI.Yin = BuildYinObject(effectId);
            PI.Matrix = BuildFightPersonal(PI.Roles);
            PI.InitMatrix = BuildFightPersonal(PI.Roles); ;
            return PI;
        }

        #endregion
    }

    /// <summary>
    /// 战斗对象
    /// </summary>
    public class FightObject
    {
        /// <summary>主动方</summary>
        public PartyItem Seme { get; set; }

        /// <summary>被动方</summary>
        public PartyItem Uke { get; set; }

        /// <summary>战斗来源类型</summary>
        public int srcType { get; set; }

        /// <summary>武将宅难度等级 武将宅时用</summary>
        public int homeLv { get; set; }

        /// <summary>是否主动方胜利</summary>
        public bool isSeme { get; set; }

        /// <summary>当前出手是否主动方</summary>
        public bool shotIsSeme { get; set; }

        /// <summary> 回合数 </summary>
        public int round { get; set; }

        /// <summary> 战斗出手步骤 </summary>
        //public List<List<FightStep>> moves { get; set; }
        public Dictionary<int, List<FightStep>> moves { get; set; }

        /// <summary> 当前主动方出手位置 </summary>
        public int shotSeme { get; set; }

        /// <summary> 当前被动方出手位置 </summary>
        public int shotUke { get; set; }

        /// <summary> 武将所在的回合  key:武将id  value:回合数</summary>
        public Dictionary<Int64, int> roleRound { get; set; }
    }

    /// <summary>
    /// 参与对象
    /// </summary>
    public class PartyItem
    {
        /// <summary>初始武将信息</summary>
        public List<RoleObject> InitRoles { get; set; }

        /// <summary>战斗武将对象集合</summary>
        public List<RoleObject> Roles { get; set; }

        /// <summary> 初始阵形 </summary>
        public MatrixObject InitMatrix { get; set; }

        /// <summary> 当前阵形 </summary>
        public MatrixObject Matrix { get; set; }

        public YinObject Yin { get; set; }

        public Int64 userid { get; set; }
    }

    /// <summary> 单次出手实体 </summary>
    public class FightStep
    {
        /// <summary> 出手武将 id</summary>
        public Int64 attackId { get; set; }

        /// <summary> 出手类型 </summary>
        public int showType { get; set; }

        /// <summary> 目标武将 id </summary>
        public List<Int64> hitIds { get; set; }

        /// <summary> 当次效果值 </summary>
        public List<BuffItem> buffs { get; set; }

        /// <summary>当前主动方武将</summary>
        public List<RoleObject> SemeRoles { get; set; }

        /// <summary>当前被动方武将</summary>
        public List<RoleObject> UkeRoles { get; set; }
    }

    /// <summary> 印实体 </summary>
    public class YinObject
    {
        /// <summary>主键id</summary>
        public double id { get; set; }

        /// <summary>印基表编号</summary>
        public int baseid { get; set; }

        /// <summary>印等级</summary>
        public int level { get; set; }

        /// <summary>状态 0:未使用 1:使用中</summary>
        public int state { get; set; }

        /// <summary>当前印计数</summary>
        public int yinCount { get; set; }
    }

    /// <summary> 阵实体 </summary>
    public class MatrixObject
    {
        public Int64 Matrix_1 { get; set; }
        public Int64 Matrix_2 { get; set; }
        public Int64 Matrix_3 { get; set; }
        public Int64 Matrix_4 { get; set; }
        public Int64 Matrix_5 { get; set; }
    }

    /// <summary> 武将实体 </summary>
    public class RoleObject
    {
        /// <summary>武将主键</summary>
        public Int64 id { get; set; }

        /// <summary>基础 id </summary>
        public int baseId { get; set; }

        /// <summary> 怪物类型  0人物  1怪物 </summary>
        public int monsterType { get; set; }

        /// <summary> 武将名称 </summary>
        public string roleName { get; set; }

        /// <summary> 是否主角武将 </summary>
        public bool isLead { get; set; }

        /// <summary> 奥义</summary>
        public SkillObject mystery { get; set; }

        /// <summary>秘技</summary>
        public SkillObject cheatCode { get; set; }

        /// <summary>伤害 </summary>
        public Int64 damage { get; set; }

        /// <summary> 生命 </summary>
        public Int64 hp { get; set; }

        /// <summary> 初始血量 </summary>
        public Int64 initHp { get; set; }

        /// <summary> 等级 </summary>
        public int lv { get; set; }

        /// <summary>攻击</summary>
        public Double attack { get; set; }

        /// <summary>防御 </summary>
        public Double defense { get; set; }

        /// <summary> 增伤 </summary>
        public Double hurtIncrease { get; set; }

        /// <summary>减伤</summary>
        public Double hurtReduce { get; set; }

        /// <summary> 会心几率 </summary>
        public Double critProbability { get; set; }

        /// <summary> 会心加成  </summary>
        public Double critAddition { get; set; }

        /// <summary> 闪避几率 </summary>
        public Double dodgeProbability { get; set; }

        /// <summary>奥义触发几率</summary>
        public Double mystery_probability { get; set; }

        /// <summary> 无视闪避几率 </summary>
        public Double IgnoreDuck { get; set; }

        /// <summary> 气力值 </summary>
        public int angerCount { get; set; }

        /// <summary>新buff集合</summary>
        public List<BuffItem> Buff_A { get; set; }

        /// <summary>持续buff集合</summary>
        public List<BuffItem> Buff_B { get; set; }

        /// <summary>永久buff集合</summary>
        public List<BuffItem> Buff_C { get; set; }
    }

    /// <summary> 技能实体 </summary>
    public class SkillObject
    {
        /// <summary>编号</summary>
        public Int64 id { get; set; }

        /// <summary>基础数据编号</summary>
        public Int64 baseId { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }
    }

    /// <summary>
    /// 战斗buff
    /// </summary>
    public class BuffItem
    {
        /// <summary>类型</summary>
        public int Type { get; set; }

        /// <summary> 初始数值</summary>
        public double InitValue { get; set; }

        /// <summary> 实际数值</summary>
        public double Value { get; set; }

        /// <summary>过期回合</summary>
        public int Round { get; set; }

        /// <summary>目标</summary>
        public Int64 Target { get; set; }

        /// <summary>持有者</summary>
        public Int64 Holder { get; set; }
    }

    /// <summary> 战斗使用的战斗技能效果实体 </summary>
    public class EffectsObject
    {
        /// <summary>技能效果类型</summary>
        public int type { get; set; }

        /// <summary>技能效果目标 (1=本方 2=敌方)</summary>
        public int target { get; set; }

        /// <summary>技能效果范围 (1=单体 2=全体)</summary>
        public int range { get; set; }

        /// <summary>技能效果回合数</summary>
        public int round { get; set; }

        /// <summary>技能效果值</summary>
        public double values { get; set; }

        ///<summary>技能效果几率</summary>
        public double robabilityValues { get; set; }
    }
}
