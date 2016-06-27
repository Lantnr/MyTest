using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGG.Core.Global
{
    public partial class Variable
    {
        /// <summary>规则基表数据</summary>
        public static List<Base.BaseRule> BASE_RULE = new List<Base.BaseRule>();
        /// <summary>功能开放表数据</summary>
        public static List<Base.BaseModuleOpen> BASE_MODULEOPEN = new List<Base.BaseModuleOpen>();

        /// <summary>町基表数据</summary>
        public static List<Base.BaseTing> BASE_TING = new List<Base.BaseTing>();
        /// <summary>货物基表数据</summary>
        public static List<Base.BaseGoods> BASE_GOODS = new List<Base.BaseGoods>();
        /// <summary>部件基表数据</summary>
        public static List<Base.BasePart> BASE_PART = new List<Base.BasePart>();
        /// <summary>货物价格基表数据</summary>
        public static List<Base.BaseGoodsPrice> BASE_GOODSPRICE = new List<Base.BaseGoodsPrice>();


        /// <summary>道具基表数据</summary>
        public static List<Base.BaseProp> BASE_PROP = new List<Base.BaseProp>();
        /// <summary>印系统基表数据</summary>
        public static List<Base.BaseYin> BASE_YIN = new List<Base.BaseYin>();
        /// <summary>印效果基表数据</summary>
        public static List<Base.BaseYinEffect> BASE_YINEFFECT = new List<Base.BaseYinEffect>();


        /// <summary>装备基表数据</summary>
        public static List<Base.BaseEquip> BASE_EQUIP = new List<Base.BaseEquip>();
        /// <summary>装备品质几率表</summary>
        public static List<Base.BaseEquipRate> BASE_EQUIPRATE = new List<Base.BaseEquipRate>();
        /// <summary>装备属性几率据</summary>
        public static List<Base.BaseEquipAttRate> BASE_EQUIPATTRATE = new List<Base.BaseEquipAttRate>();

        /// <summary>购买体力基表</summary>
        public static List<Base.BaseBuyPower> BASE_BUYPOWER = new List<Base.BaseBuyPower>();


        /// <summary>角色出生点基表数据</summary>
        public static List<Base.BaseRoleBornPoint> BASE_ROLEBORNPOINT = new List<Base.BaseRoleBornPoint>();
        /// <summary>场景基表数据</summary>
        public static List<Base.BaseScene> BASE_SCENE = new List<Base.BaseScene>();
        /// <summary> NPC战斗部队基表数据</summary>
        public static List<Base.BaseNpcArmy> BASE_NPCARMY = new List<Base.BaseNpcArmy>();
        /// <summary>NPC武将基表数据</summary>
        public static List<Base.BaseNpcRole> BASE_NPCROLE = new List<Base.BaseNpcRole>();
        /// <summary>NPC怪物基表数据</summary>
        public static List<Base.BaseNpcMonster> BASE_NPCMONSTER = new List<Base.BaseNpcMonster>();
        /// <summary>一将讨怪物基表数据</summary>
        public static List<Base.BasePersonalNpc> BASE_NPCSINGLE = new List<Base.BasePersonalNpc>();
        /// <summary>任务戒严布谣npc基表数据</summary>
        public static List<Base.BaseTaskNpc> BASE_TASKNPC = new List<Base.BaseTaskNpc>();


        /// <summary>生活技能基表数据</summary>
        public static List<Base.BaseLifeSkill> BASE_LIFESKILL = new List<Base.BaseLifeSkill>();
        /// <summary>生活技能效果基表数据</summary>
        public static List<Base.BaseLifeSkillEffect> BASE_LIFESKILLEFFECT = new List<Base.BaseLifeSkillEffect>();
        /// <summary>战斗技能基表数据</summary>
        public static List<Base.BaseFightSkill> BASE_FIGHTSKILL = new List<Base.BaseFightSkill>();
        /// <summary>战斗技能效果基表数据</summary>
        public static List<Base.BaseFightSkillEffect> BASE_FIGHTSKILLEFFECT = new List<Base.BaseFightSkillEffect>();

        /// <summary>魂基表数据</summary>
        public static List<Base.BaseSpirit> BASE_SPIRIT = new List<Base.BaseSpirit>();

        /// <summary>主线任务基表数据</summary>
        public static List<Base.BaseTaskMain> BASE_TASKMAIN = new List<Base.BaseTaskMain>();
        /// <summary>职业任务基表数据</summary>
        public static List<Base.BaseTaskVocation> BASE_TASKVOCATION = new List<Base.BaseTaskVocation>();
        /// <summary>职业任务随机步骤基表数据</summary>
        public static List<Base.BaseTaskVocationRd> BASE_TASKVOCATIONRD = new List<Base.BaseTaskVocationRd>();
        /// <summary>家臣任务基表数据</summary>
        public static List<Base.BaseAppraise> BASE_APPRAISE = new List<Base.BaseAppraise>();
        /// <summary>职业任务概率表</summary>
        public static List<Base.BaseTaskVocationProb> BASE_TASK_PROB = new List<Base.BaseTaskVocationProb>();


        /// <summary>武将信息基表数据</summary>
        public static List<Base.BaseRoleInfo> BASE_ROLE = new List<Base.BaseRoleInfo>();
        /// <summary>等级升级基表数据</summary>
        public static List<Base.BaseRoleLvUpdate> BASE_ROLELVUPDATE = new List<Base.BaseRoleLvUpdate>();
        /// <summary>身份基表数据</summary>
        public static List<Base.BaseIdentity> BASE_IDENTITY = new List<Base.BaseIdentity>();
        /// <summary>武将修行基表数据</summary>
        public static List<Base.BaseTrain> BASE_TRAIN = new List<Base.BaseTrain>();
        /// <summary>武将宅偷窃加成概率表</summary>
        public static List<Base.BaseStealProb> BASE_STEAL_PROB = new List<Base.BaseStealProb>();


        /// <summary>武将点将表</summary>
        public static List<Base.BaseRoleHome> BASE_ROLE_HOME = new List<Base.BaseRoleHome>();
        /// <summary>战斗武将组合效果基表数据</summary>
        public static List<Base.BaseRoleGroup> BASE_ROLEGROUP = new List<Base.BaseRoleGroup>();
        /// <summary>家臣称号基表数据</summary>
        public static List<Base.BaseRoleTitle> BASE_ROLETITLE = new List<Base.BaseRoleTitle>();
        /// <summary>职业基表数据</summary>
        public static List<Base.BaseVocation> BASE_VOCATION = new List<Base.BaseVocation>();
        /// <summary>武将雇佣基表数据</summary>
        public static List<Base.BaseNpcHire> BASE_NPCHIRE = new List<Base.BaseNpcHire>();


        /// <summary>副本打气球基表数据</summary>
        public static List<Base.BaseCopyFamePass> BASE_COPYFAMEPASS = new List<Base.BaseCopyFamePass>();
        /// <summary>副本爬塔游戏信息表</summary>
        public static List<Base.BaseTowerGame> BASE_TOWERGAME = new List<Base.BaseTowerGame>();
        /// <summary>副本爬塔怪物信息表</summary>
        public static List<Base.BaseTowerEnemy> BASE_TOWERENEMY = new List<Base.BaseTowerEnemy>();
        /// <summary>副本爬塔关卡信息表</summary>
        public static List<Base.BaseTowerPass> BASE_TOWERPASS = new List<Base.BaseTowerPass>();
        /// <summary>副本爬塔茶道信息表</summary>
        public static List<Base.BaseTowerTea> BASE_TOWERTEA = new List<Base.BaseTowerTea>();
        /// <summary>副本爬塔辩才信息表</summary>
        public static List<Base.BaseTowerEloquence> BASE_TOWERELOQUENCE = new List<Base.BaseTowerEloquence>();

        /// <summary>竞技场奖励基表数据</summary>
        public static List<Base.BaseArenaRankReward> BASE_ARENARANKREWARD = new List<Base.BaseArenaRankReward>();

        /// <summary>家族等级表基表数据</summary>
        public static List<Base.BaseFamilyLevel> BASE_FAMILYLEVEL = new List<Base.BaseFamilyLevel>();
        /// <summary>家族职位表基表数据</summary>
        public static List<Base.BaseFamilyPost> BASE_FAMILYPOST = new List<Base.BaseFamilyPost>();
        /// <summary>家族日志表基表数据</summary>
        public static List<Base.BaseFamilyLog> BASE_FAMILYLOG = new List<Base.BaseFamilyLog>();


        /// <summary>美浓攻略属性技能相关影响基表数据</summary>
        public static List<Base.BaseSiege> BASE_SIEGE = new List<Base.BaseSiege>();
        /// <summary>美浓攻略BOSS基表数据</summary>
        public static List<Base.BaseNpcSiege> BASE_NPCSIEGE = new List<Base.BaseNpcSiege>();
        /// <summary>美浓攻略奖励基表数据</summary>
        public static List<Base.BaseSiegeReward> BASE_SIEGEREWARD = new List<Base.BaseSiegeReward>();
        /// <summary>美浓攻略Boss奖励基表数据</summary>
        public static List<Base.BaseSiegeBossReward> BASE_SIEGEBOSSREWARD = new List<Base.BaseSiegeBossReward>();
        /// <summary>美浓攻略云梯制作点基表数据</summary>
        public static List<Base.BaseCollectGoodsSiege> BASE_COLLECTGOODSSIEGE = new List<Base.BaseCollectGoodsSiege>();
        /// <summary>美浓攻略传送点基表数据</summary>
        public static List<Base.BaseEntryPointSiege> BASE_ENTRYPOINTSIEGE = new List<Base.BaseEntryPointSiege>();


        /// <summary>一夜墨俣收集物品概率基表数据</summary>
        public static List<Base.BaseBuildProbability> BASE_BUILD = new List<Base.BaseBuildProbability>();
        /// <summary>一夜墨俣大将城池基表数据</summary>
        public static List<Base.BaseNpcBuild> BASE_NPC_BUILD = new List<Base.BaseNpcBuild>();
        /// <summary>一夜墨俣活动奖励基表数据</summary>
        public static List<Base.BaseBuildingReward> BASE_BUILDING_REWARD = new List<Base.BaseBuildingReward>();
        /// <summary>一夜墨俣物品采集基表数据</summary>
        public static List<Base.BaseCollectGoodsBuild> BASE_BUILDING_COLLECT = new List<Base.BaseCollectGoodsBuild>();


        /// <summary>VIP基表数据</summary>
        public static List<Base.BaseVip> BASE_VIP = new List<Base.BaseVip>();

        /// <summary>酒馆招募基表数据</summary>
        public static List<Base.BaseRecruitRate> BASE_RECRUITRATE = new List<Base.BaseRecruitRate>();

        /// <summary>大名令基表数据</summary>
        public static List<Base.BaseDamingLog> BASE_DAMING = new List<Base.BaseDamingLog>();

        /// <summary>游艺园每周奖励基表数据</summary>
        public static List<Base.BaseYouYiYuanReward> BASE_YOUYIYUANREWARD = new List<Base.BaseYouYiYuanReward>();
        /// <summary>游艺园基表数据</summary>
        public static List<Base.BaseYouYiYuan> BASE_YOUYIYUAN = new List<Base.BaseYouYiYuan>();

    }
}
