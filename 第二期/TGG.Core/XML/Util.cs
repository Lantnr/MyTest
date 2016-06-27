using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using TGG.Core.Base;
using TGG.Core.Global;
using TGG.Resources;

namespace TGG.Core.XML
{
    /// <summary>
    /// 基础数据
    /// </summary>
    public class Util
    {
        /// <summary>读取XML 加载模块信息</summary>
        public static List<XmlModule> ReadModule()
        {
            var xdoc = XDocument.Parse(Resource.module);
            var list = from q in xdoc.Descendants("module")
                       select new XmlModule
                       {
                           Name = q.Attribute("name").Value,
                           Value = q.Attribute("value").Value,
                           Order = Convert.ToInt32(q.Attribute("order").Value),
                           type = Convert.ToInt32(q.Attribute("type").Value),
                       };
            return list.OrderBy(m => m.Order).ToList();
        }

        /// <summary>读取XML 模块队列信息</summary>
        public static List<XmlModule> ReadModuleQueue()
        {
            var xdoc = XDocument.Parse(Resource.modulequeue);
            var list = from q in xdoc.Descendants("module")
                       select new XmlModule
                       {
                           Name = q.Attribute("name").Value,
                           Value = q.Attribute("value").Value,
                           Order = Convert.ToInt32(q.Attribute("order").Value),
                           type = Convert.ToInt32(q.Attribute("type").Value),
                       };
            return list.OrderBy(m => m.Order).ToList();
        }

        /// <summary>读取基表数据</summary>
        public static void ReadBaseEntity()
        {
            GetYouYiYuan();
            GetRule();
            GetBusiness();
            GetProp();
            GetScene();
            GetFight();
            GetTask();
            GetRole();
            GetDuplicate();
            GetFamily();
            GetArena();
            GetActivity();
            InitRuleData();
        }

        /// <summary>初始化固定规则表</summary>
        private static void InitRuleData()
        {
            var coin = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1004");
            var fame = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1005");
            var spirit = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1006");
            var honor = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1007");
            if (coin == null || fame == null || honor == null || spirit == null) return;
            Variable.MAX_COIN = Convert.ToInt64(coin.value);
            Variable.MAX_FAME = Convert.ToInt32(fame.value);
            Variable.MAX_HONOR = Convert.ToInt32(honor.value);
            Variable.MAX_SPIRIT = Convert.ToInt32(spirit.value);
        }

        #region JSON
        /// <summary>游艺园基表数据</summary>
        private static void GetYouYiYuan()
        {
            Variable.BASE_YOUYIYUANREWARD = FromJson(Encoding.UTF8.GetString(Resource.YouYiYuanReward), typeof(List<BaseYouYiYuanReward>)) as List<BaseYouYiYuanReward>;
            Variable.BASE_YOUYIYUAN = FromJson(Encoding.UTF8.GetString(Resource.YouYiYuan), typeof(List<BaseYouYiYuan>)) as List<BaseYouYiYuan>;
        }

        /// <summary>家族基表数据</summary>
        private static void GetFamily()
        {
            Variable.BASE_FAMILYLEVEL = FromJson(Encoding.UTF8.GetString(Resource.FamilyLevel), typeof(List<BaseFamilyLevel>)) as List<BaseFamilyLevel>;
            Variable.BASE_FAMILYLOG = FromJson(Encoding.UTF8.GetString(Resource.FamilyLog), typeof(List<BaseFamilyLog>)) as List<BaseFamilyLog>;
            Variable.BASE_FAMILYPOST = FromJson(Encoding.UTF8.GetString(Resource.FamilyPost), typeof(List<BaseFamilyPost>)) as List<BaseFamilyPost>;
        }

        /// <summary>副本基表数据</summary>
        private static void GetDuplicate()
        {
            Variable.BASE_COPYFAMEPASS = FromJson(Encoding.UTF8.GetString(Resource.CopyFamePass), typeof(List<BaseCopyFamePass>)) as List<BaseCopyFamePass>;
            Variable.BASE_TOWERGAME = FromJson(Encoding.UTF8.GetString(Resource.TowerGame), typeof(List<BaseTowerGame>)) as List<BaseTowerGame>;
            Variable.BASE_TOWERENEMY = FromJson(Encoding.UTF8.GetString(Resource.TowerEnemy), typeof(List<BaseTowerEnemy>)) as List<BaseTowerEnemy>;
            Variable.BASE_TOWERPASS = FromJson(Encoding.UTF8.GetString(Resource.TowerPass), typeof(List<BaseTowerPass>)) as List<BaseTowerPass>;
            Variable.BASE_TOWERTEA = FromJson(Encoding.UTF8.GetString(Resource.TowerTea), typeof(List<BaseTowerTea>)) as List<BaseTowerTea>;
            Variable.BASE_TOWERELOQUENCE = FromJson(Encoding.UTF8.GetString(Resource.TowerEloquence), typeof(List<BaseTowerEloquence>)) as List<BaseTowerEloquence>;
        }

        /// <summary>竞技场基表数据</summary> 
        private static void GetArena()
        {
            Variable.BASE_ARENARANKREWARD = FromJson(Encoding.UTF8.GetString(Resource.ArenaRankingReward), typeof(List<BaseArenaRankReward>)) as List<BaseArenaRankReward>;
        }

        /// <summary>武将相关基表数据</summary>
        private static void GetRole()
        {
            Variable.BASE_ROLE = FromJson(Encoding.UTF8.GetString(Resource.RoleInfo), typeof(List<BaseRoleInfo>)) as List<BaseRoleInfo>;
            Variable.BASE_ROLELVUPDATE = FromJson(Encoding.UTF8.GetString(Resource.RoleLvUpdate), typeof(List<BaseRoleLvUpdate>)) as List<BaseRoleLvUpdate>;
            Variable.BASE_SPIRIT = FromJson(Encoding.UTF8.GetString(Resource.Spirit), typeof(List<BaseSpirit>)) as List<BaseSpirit>;
            Variable.BASE_EQUIP = FromJson(Encoding.UTF8.GetString(Resource.Equip), typeof(List<BaseEquip>)) as List<BaseEquip>;
            Variable.BASE_IDENTITY = FromJson(Encoding.UTF8.GetString(Resource.Identity), typeof(List<BaseIdentity>)) as List<BaseIdentity>;
            Variable.BASE_TRAIN = FromJson(Encoding.UTF8.GetString(Resource.Train), typeof(List<BaseTrain>)) as List<BaseTrain>;
            Variable.BASE_ROLE_HOME = FromJson(Encoding.UTF8.GetString(Resource.RoleHome), typeof(List<BaseRoleHome>)) as List<BaseRoleHome>;
            Variable.BASE_ROLEGROUP = FromJson(Encoding.UTF8.GetString(Resource.RoleGroup), typeof(List<BaseRoleGroup>)) as List<BaseRoleGroup>;
            Variable.BASE_ROLETITLE = FromJson(Encoding.UTF8.GetString(Resource.RoleTitle), typeof(List<BaseRoleTitle>)) as List<BaseRoleTitle>;

            Variable.BASE_EQUIPRATE = FromJson(Encoding.UTF8.GetString(Resource.EquipRate), typeof(List<BaseEquipRate>)) as List<BaseEquipRate>;
            Variable.BASE_EQUIPATTRATE = FromJson(Encoding.UTF8.GetString(Resource.EquipAttRate), typeof(List<BaseEquipAttRate>)) as List<BaseEquipAttRate>;
            Variable.BASE_BUYPOWER = FromJson(Encoding.UTF8.GetString(Resource.BuyPower), typeof(List<BaseBuyPower>)) as List<BaseBuyPower>;

            Variable.BASE_VOCATION = FromJson(Encoding.UTF8.GetString(Resource.Vocation), typeof(List<BaseVocation>)) as List<BaseVocation>;

            Variable.BASE_NPCHIRE = FromJson(Encoding.UTF8.GetString(Resource.NpcHire), typeof(List<BaseNpcHire>)) as List<BaseNpcHire>;

            Variable.BASE_RECRUITRATE = FromJson(Encoding.UTF8.GetString(Resource.RecruitRate), typeof(List<BaseRecruitRate>)) as List<BaseRecruitRate>;

            Variable.BASE_DAMING = FromJson(Encoding.UTF8.GetString(Resource.DaMingLog), typeof(List<BaseDamingLog>)) as List<BaseDamingLog>;
            Variable.BASE_STEAL_PROB = FromJson(Encoding.UTF8.GetString(Resource.StealAddProb), typeof(List<BaseStealProb>)) as List<BaseStealProb>;
        }

        /// <summary>任务相关基表数据</summary>
        private static void GetTask()
        {
            Variable.BASE_TASKMAIN = FromJson(Encoding.UTF8.GetString(Resource.TaskMain), typeof(List<BaseTaskMain>)) as List<BaseTaskMain>;
            Variable.BASE_TASKVOCATION = FromJson(Encoding.UTF8.GetString(Resource.TaskVocation), typeof(List<BaseTaskVocation>)) as List<BaseTaskVocation>;
            Variable.BASE_APPRAISE = FromJson(Encoding.UTF8.GetString(Resource.TaskRetainers), typeof(List<BaseAppraise>)) as List<BaseAppraise>;
            Variable.BASE_TASK_PROB = FromJson(Encoding.UTF8.GetString(Resource.TaskVocationProb), typeof(List<BaseTaskVocationProb>)) as List<BaseTaskVocationProb>;
            Variable.BASE_TASKNPC = FromJson(Encoding.UTF8.GetString(Resource.TaskNpc), typeof(List<BaseTaskNpc>)) as List<BaseTaskNpc>;
        }

        /// <summary>战斗相关基表数据</summary>
        private static void GetFight()
        {
            Variable.BASE_YIN = FromJson(Encoding.UTF8.GetString(Resource.Yin), typeof(List<BaseYin>)) as List<BaseYin>;
            Variable.BASE_YINEFFECT = FromJson(Encoding.UTF8.GetString(Resource.YinEffect), typeof(List<BaseYinEffect>)) as List<BaseYinEffect>;
            Variable.BASE_FIGHTSKILL = FromJson(Encoding.UTF8.GetString(Resource.FightSkill), typeof(List<BaseFightSkill>)) as List<BaseFightSkill>;
            Variable.BASE_FIGHTSKILLEFFECT = FromJson(Encoding.UTF8.GetString(Resource.FightSkillEffect), typeof(List<BaseFightSkillEffect>)) as List<BaseFightSkillEffect>;
            Variable.BASE_LIFESKILL = FromJson(Encoding.UTF8.GetString(Resource.LifeSkill), typeof(List<BaseLifeSkill>)) as List<BaseLifeSkill>;
            Variable.BASE_LIFESKILLEFFECT = FromJson(Encoding.UTF8.GetString(Resource.LifeSkillEffect), typeof(List<BaseLifeSkillEffect>)) as List<BaseLifeSkillEffect>;
            Variable.BASE_NPCARMY = FromJson(Encoding.UTF8.GetString(Resource.NpcArmy), typeof(List<BaseNpcArmy>)) as List<BaseNpcArmy>;
            Variable.BASE_NPCROLE = FromJson(Encoding.UTF8.GetString(Resource.NpcRole), typeof(List<BaseNpcRole>)) as List<BaseNpcRole>;
            Variable.BASE_NPCMONSTER = FromJson(Encoding.UTF8.GetString(Resource.NpcMonster), typeof(List<BaseNpcMonster>)) as List<BaseNpcMonster>;
            Variable.BASE_NPCSINGLE = FromJson(Encoding.UTF8.GetString(Resource.YiJiangTao), typeof(List<BasePersonalNpc>)) as List<BasePersonalNpc>;
        }

        /// <summary>场景相关基表数据</summary>
        private static void GetScene()
        {
            Variable.BASE_ROLEBORNPOINT = FromJson(Encoding.UTF8.GetString(Resource.RoleBornPoint), typeof(List<BaseRoleBornPoint>)) as List<BaseRoleBornPoint>;
            Variable.BASE_SCENE = FromJson(Encoding.UTF8.GetString(Resource.Scene), typeof(List<BaseScene>)) as List<BaseScene>;
        }

        /// <summary>活动相关基表数据</summary>
        private static void GetActivity()
        {
            Variable.BASE_SIEGE = FromJson(Encoding.UTF8.GetString(Resource.Siege), typeof(List<BaseSiege>)) as List<BaseSiege>;
            Variable.BASE_SIEGEREWARD = FromJson(Encoding.UTF8.GetString(Resource.SiegeReward), typeof(List<BaseSiegeReward>)) as List<BaseSiegeReward>;
            Variable.BASE_SIEGEBOSSREWARD = FromJson(Encoding.UTF8.GetString(Resource.SiegeBossReward), typeof(List<BaseSiegeBossReward>)) as List<BaseSiegeBossReward>;
            Variable.BASE_NPCSIEGE = FromJson(Encoding.UTF8.GetString(Resource.NpcSiege), typeof(List<BaseNpcSiege>)) as List<BaseNpcSiege>;
            Variable.BASE_COLLECTGOODSSIEGE = FromJson(Encoding.UTF8.GetString(Resource.CollectGoodsSiege), typeof(List<BaseCollectGoodsSiege>)) as List<BaseCollectGoodsSiege>;
            Variable.BASE_ENTRYPOINTSIEGE = FromJson(Encoding.UTF8.GetString(Resource.EntryPointSiege), typeof(List<BaseEntryPointSiege>)) as List<BaseEntryPointSiege>;
            Variable.BASE_BUILD = FromJson(Encoding.UTF8.GetString(Resource.BuildProbability), typeof(List<BaseBuildProbability>)) as List<BaseBuildProbability>;
            Variable.BASE_NPC_BUILD = FromJson(Encoding.UTF8.GetString(Resource.NpcBuild), typeof(List<BaseNpcBuild>)) as List<BaseNpcBuild>;
            Variable.BASE_BUILDING_REWARD = FromJson(Encoding.UTF8.GetString(Resource.BuildingReward), typeof(List<BaseBuildingReward>)) as List<BaseBuildingReward>;
            Variable.BASE_BUILDING_COLLECT = FromJson(Encoding.UTF8.GetString(Resource.CollectGoodsBuild), typeof(List<BaseCollectGoodsBuild>)) as List<BaseCollectGoodsBuild>;
        }

        /// <summary>道具相关基表数据</summary>
        private static void GetProp()
        {
            Variable.BASE_PROP = FromJson(Encoding.UTF8.GetString(Resource.Prop), typeof(List<BaseProp>)) as List<BaseProp>;
        }

        /// <summary>跑商相关基表数据</summary>
        private static void GetBusiness()
        {
            Variable.BASE_TING = FromJson(Encoding.UTF8.GetString(Resource.BusinessCity), typeof(List<BaseTing>)) as List<BaseTing>;
            Variable.BASE_PART = FromJson(Encoding.UTF8.GetString(Resource.BusinessCarPart), typeof(List<BasePart>)) as List<BasePart>;
            Variable.BASE_GOODS = FromJson(Encoding.UTF8.GetString(Resource.BusinessGoods), typeof(List<BaseGoods>)) as List<BaseGoods>;
            Variable.BASE_GOODSPRICE = FromJson(Encoding.UTF8.GetString(Resource.BusinessGoodsPrice), typeof(List<BaseGoodsPrice>)) as List<BaseGoodsPrice>;
        }

        /// <summary>固定规则基表数据</summary>
        private static void GetRule()
        {
            Variable.BASE_RULE = FromJson(Encoding.UTF8.GetString(Resource.Rule), typeof(List<BaseRule>)) as List<BaseRule>;
            Variable.BASE_MODULEOPEN = FromJson(Encoding.UTF8.GetString(Resource.ModuleOpen), typeof(List<BaseModuleOpen>)) as List<BaseModuleOpen>;
            Variable.BASE_VIP = FromJson(Encoding.UTF8.GetString(Resource.Vip), typeof(List<BaseVip>)) as List<BaseVip>;
        }

        /// <summary>格式化</summary>
        private static object FromJson(string strJson, Type type)
        {
            var ds = new DataContractJsonSerializer(type);
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson));
            return ds.ReadObject(ms);
        }

        #endregion
    }
}
