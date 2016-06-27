using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Fight;

namespace TGG.Share.Fight
{
    public partial class Fight
    {
        /// <summary>是否触发印</summary>
        private void IsYin()
        {
            if (IsAttack)
                YinEffect(vo.yinA, move.yinA);
            else
                YinEffect(vo.yinB, move.yinB);
        }

        /// <summary> 印效果 </summary>
        private void YinEffect(YinVo yinvo, int yincount)
        {
            if (yinvo == null) return;
            //var movesvo = move.CloneDeepEntity();
            var baseYin = Variable.BASE_YIN.FirstOrDefault(m => m.id == yinvo.baseid); //读取印基表
            if (baseYin == null) return;
            if (!IsActivation(baseYin.yinCount, yincount))                                          //验证触发印方
                return;
            var baseEffect = Variable.BASE_YINEFFECT.FirstOrDefault(m => m.yinId == baseYin.id && m.level == yinvo.level);//读取印效果表
            if (baseEffect == null) return;

            var effects = baseEffect.effects; //技能效果
#if DEBUG
            XTrace.WriteLine(string.Format("{0} {1} - {2}", "出手武将", "触发印", "印基表效果Id " + baseEffect.id + " 效果值 " + baseEffect.effects));
#endif
            SkillEffect(effects, (int)SkillType.YIN);   //技能效果解析

            if (baseEffect.isQuickAttack == 1)    //1为当前回合攻击  0为当前回合不攻击
                Attack(GetShotRole(), baseEffect.attackRange == (int)EffectRangeType.ALL);
            RemoveBuff(false);
        }

        /// <summary> 验证印是否激活 </summary>
        /// <param name="baseyincount">基表需要印数</param>
        /// <param name="yincount">武将当前印数</param>
        private bool IsActivation(int baseyincount, int yincount)
        {
            if (yincount < baseyincount)
                return false;
            yincount = yincount - baseyincount;      //扣除消耗的印计数
            DeductionYinCount(IsAttack ? attack_matrix.user_id : defense_matrix.user_id, yincount);
            GetYinCount(move);                                                            //双方印数
            return true;
        }

        /// <summary> 扣除印计数 </summary>
        /// <param name="userid">用户id</param>
        /// <param name="number">扣除后的数量</param>
        private void DeductionYinCount(decimal userid, int number)
        {
            if (!dic_yincount.ContainsKey(userid))
                dic_yincount.Add(userid, 1);
            else
                dic_yincount[userid] = number;
        }

        /// <summary>获取双方当前印数</summary>
        private void GetYinCount(MovesVo movesvo)
        {
            if (!dic_yincount.ContainsKey(attack_matrix.user_id) || !dic_yincount.ContainsKey(defense_matrix.user_id)) AddYinCount(attack_matrix.user_id);
            movesvo.yinA = dic_yincount[attack_matrix.user_id];
            movesvo.yinB = dic_yincount[defense_matrix.user_id];
        }

    }
}
