using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Fight;

namespace TGG.Share.Fight
{
    public partial class Fight
    {
        /// <summary>不加气力普通攻击操作</summary>
        /// <param name="attackrole">当前攻击武将</param>
        /// <param name="flag">范围类型:true 全体 false:单体</param>
        private void Attack(FightRole attackrole, bool flag)
        {
            var movesvo = move.CloneDeepEntity();
            if (attackrole == null) return;
            var list = GetRoleOrRoleAll(GetMatrix(false), flag);
            if (!list.Any()) return;

            foreach (var item in list)
            {
                var iscrit = false;

                if (!IsTrue(attackrole.IgnoreDuck)) { if (IsDuck(item))continue; }
                else
                {
#if DEBUG
                    XTrace.WriteLine(string.Format("{0} {1} {2}", "技能成功--> 被打方Id", item.id, "无视闪避触发成功 直接不计算闪避率进行攻击"));
#endif
                }
                var number = DamageCount(attackrole, item, ref iscrit);
                if (flag) number = Convert.ToInt64(number * 0.3);                  //群攻增加0.3系数
                item.hp = item.hp - number;
                movesvo.hitIds.Add(Convert.ToDouble(item.id));                     //普通受击武将
                if (iscrit)
                {
                    var roleAtt = new FightRoleBuff { id = item.id, round = 0, type = (int)FightingSkillType.CRIT, values = 0, state = 0 };
                    list_buff.Add(roleAtt);
                    item.buffVos2.Add(new BuffVo { type = (int)FightingSkillType.CRIT, buffValue = -number });
                }
                else
                {
                    item.damage = number;
                }
            }
            BuildMovesVo(movesvo, (int)SkillType.COMMON);
        }

        /// <summary> 单体普通攻击 增加气力值</summary>
        /// <param name="attackrole">攻击武将</param>
        private bool NormalAttack(FightRole attackrole)
        {
            var flag = false;

            if (attackrole.angerCount < 8) attackrole.angerCount = attackrole.angerCount + 1;//普通攻击气力加1
            attackrole.damage = 0;                                                           //修改攻击方属性
            var defenserole = GetFrontRole(GetMatrix(false));                                //最先受击战斗武将Vo
            if (defenserole == null) return false;

            if (IsDuck(defenserole)) return true;
            var number = DamageCount(attackrole, defenserole, ref flag);
            ByEffect(attackrole, defenserole, number, flag);
            return true;
        }

        /// <summary>是否闪避 </summary>
        private bool IsDuck(FightRole defenserole)
        {
            if (!IsTrue(defenserole.dodgeProbability)) return false;
            var movesvo = move.CloneDeepEntity();
            var roleAtt = new FightRoleBuff { id = defenserole.id, round = 0, type = (int)FightingSkillType.DODGE, values = 0, state = 0 };
            list_buff.Add(roleAtt);
            defenserole.buffVos2.Add(new BuffVo { type = (int)FightingSkillType.DODGE });
            movesvo.hitIds.Add(defenserole.id);
            BuildMovesVo(movesvo, (int)SkillType.COMMON);
#if DEBUG
            XTrace.WriteLine(string.Format("{0} {1}  {2}", "被打方Id", defenserole.id, "触发闪避 "));
#endif
            return true;
        }

        /// <summary> 几率是否成功 </summary>
        /// <param name="number">几率</param>
        private bool IsTrue(double number)
        {
            return (number > 0) && new RandomSingle().IsTrue(number);
        }

        /// <summary> 对相应的武将进行影响 </summary>
        /// <param name="attackrole">攻击武将</param>
        /// <param name="defenserole">防守武将</param>
        /// <param name="number">造成的伤害</param>
        /// <param name="flag">是否暴击</param>
        private void ByEffect(FightRole attackrole, FightRole defenserole, Int64 number, bool flag)
        {
            var movesvo = move.CloneDeepEntity();
            movesvo.hitIds.Add(Convert.ToDouble(defenserole.id));                         //防守武将id加入受击武将id
            defenserole.hp = defenserole.hp - number;                                     //修改防守方属性
            if (flag)
            {
                var roleAtt = new FightRoleBuff { id = defenserole.id, round = 0, type = (int)FightingSkillType.CRIT, values = 0, state = 0 };
                list_buff.Add(roleAtt);
                defenserole.buffVos2.Add(new BuffVo { type = (int)FightingSkillType.CRIT, buffValue = -number });
            }
            else
            {
                defenserole.damage = number;
            }
            BuildMovesVo(movesvo, (int)SkillType.COMMON);
        }

        /// <summary>伤害计算</summary>
        /// <param name="attackrole">攻击武将</param>
        /// <param name="defenserole">防守武将</param>
        /// <param name="flag">是否暴击</param>
        private Int64 DamageCount(FightRole attackrole, FightRole defenserole, ref bool flag)
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
            var n = Round - 21;
            if (n >= 0)
            {
                var b = n / 10;
                if (b == 0)
                    number = number + 1;
                if (b > 0)
                    number = number + (1 + n * 0.5);
            }
            var count = A10 * number;
            if (dic_vocation.ContainsKey(attackrole.user_id)) 
                count = count * dic_vocation[attackrole.user_id];

            return count < 0 ? 1 : Convert.ToInt64(count);
        }
    }
}
