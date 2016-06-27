using System.Collections.Generic;
using TGG.Core.Vo.RoleTrain;
using TGG.Core.Vo.Skill;

namespace TGG.Core.Vo.Role
{
    /// <summary>
    /// 武将信息VO
    /// </summary>
    public class RoleInfoVo : BaseVo
    {
        public RoleInfoVo()
        {
            equipArray = new List<double>();
            fightSkillArrVo = new List<FightSkillVo>();
            lifeSkillArrVo = new List<LifeSkillVo>();
        }

        /// <summary>编号</summary>
        public double id { get; set; }

        /// <summary>基础数据编号</summary>
        public int baseId { get; set; }

        /// <summary>武将状态类型</summary>
        public int state { get; set; }

        /// <summary>等级</summary>
        public int level { get; set; }

        /// <summary>经验</summary>
        public int experience { get; set; }

        /// <summary>总体力</summary>
        public int power { get; set; }

         /// <summary>基础体力</summary>
        public int rolePower { get; set; }
        
        /// <summary>身份id</summary>
        public int identityId { get; set; }

        /// <summary>统帅</summary>
        public double captain { get; set; }

        /// <summary>武力</summary>
        public double force { get; set; }

        /// <summary>智谋</summary>
        public double brains { get; set; }

        /// <summary>魅力</summary>
        public double charm { get; set; }

        /// <summary>政务</summary>
        public double govern { get; set; }

        /// <summary>基础统率</summary>
        public double captainBase { get; set; }

        /// <summary>基础武力</summary>
        public double forceBase { get; set; }

        /// <summary>基础智谋</summary>
        public double brainsBase { get; set; }

        /// <summary>基础魅力</summary>
        public double charmBase { get; set; }

        /// <summary>基础政务</summary>
        public double governBase { get; set; }

        /// <summary>生命</summary>
        public int life { get; set; }

        /// <summary>攻击</summary>
        public double attack { get; set; }

        /// <summary>防御</summary>
        public double defense { get; set; }

        /// <summary>增伤</summary>
        public double hurtIncrease { get; set; }

        /// <summary>减伤</summary>
        public double hurtReduce { get; set; }

        /// <summary>会心几率</summary>
        public double critProbability { get; set; }

        /// <summary>会心效果</summary>
        public double critAddition { get; set; }

        /// <summary>闪避几率</summary>
        public double dodgeProbability { get; set; }

        /// <summary>奥义触发几率</summary>
        public double mysteryProbability { get; set; }

        /// <summary>奥义</summary>
        public int mysteryId { get; set; }

        /// <summary>秘技</summary>
        public int cheatCodeId { get; set; }

        /// <summary>战斗力</summary>
        public int fighting { get; set; }

        /// <summary>功勋</summary>
        public int honor { get; set; }

        /// <summary>装备集合</summary>
        public List<double> equipArray { get; set; }

        /// <summary>已学流派(忍者众)类型集</summary>
        public List<int> genreTypeArr { get; set; }

        /// <summary>已学战斗技能Vo集合</summary>
        public List<FightSkillVo> fightSkillArrVo { get; set; }

        /// <summary>已学生活技能Vo集合</summary>
        public List<LifeSkillVo> lifeSkillArrVo { get; set; }

        /// <summary>武将修行vo </summary>
        public TrainVo trainVo { get; set; }

        /// <summary>称号主键ids</summary>
        public List<double> roleTitleIdList { get; set; }

        /// <summary>所属流派</summary>
        public int genre { get; set; }

        /// <summary>所属忍者众</summary>
        public int ninja { get; set; }

    }
}
