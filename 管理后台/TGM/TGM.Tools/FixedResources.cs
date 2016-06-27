using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using TGG.Core.Base;
using TGM.API.Entity;
using TGM.API.Entity.Base;

namespace TGM.Tools
{
    /// <summary>
    /// 固定资源类
    /// </summary>
    public class FixedResources
    {
        /// <summary>武将信息基表数据</summary>
        public static List<BaseRoleInfo> BASE_ROLE = new List<BaseRoleInfo>();
        /// <summary>身份基表信息</summary>
        public static List<BaseIdentity> BASE_IDENTITY = new List<BaseIdentity>();
        /// <summary>官职基表信息</summary>
        public static List<BaseOffice> BASE_OFFICE = new List<BaseOffice>();
        /// <summary>跑商商圈基表信息</summary>
        public static List<BaseBusinessArea> BASE_BUSSINESS_AREA = new List<BaseBusinessArea>();
        /// <summary>战斗技能基表信息</summary>
        public static List<BaseFightSkill> BASE_FIGHT_SKILL = new List<BaseFightSkill>();
        /// <summary>炼化基表信息</summary>
        public static List<BaseFusion> BASE_FUSION = new List<BaseFusion>();
        /// <summary>装备基表信息</summary>
        public static List<BaseEquip> BASE_EQUIP = new List<BaseEquip>();
        /// <summary>道具基表信息</summary>
        public static List<BaseProp> BASE_PROP = new List<BaseProp>();
        /// <summary>固定规则表信息</summary>
        public static List<BaseRule> BASE_RULE = new List<BaseRule>();
    }
    /// <summary>
    /// 公共帮助类
    /// </summary>
    public class Util
    {
        /// <summary>Json数据转换</summary>

        private static object FromJson(string strJson, Type type)
        {
            var ds = new DataContractJsonSerializer(type);
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson));
            return ds.ReadObject(ms);
        }
        /// <summary>读取基表</summary>
        public static void ReadBaseEntity()
        {
            GetRole();
            GetBusinessArea();
            GetSkill();
            GetProp();
            GetRule();
        }
        /// <summary>获取武将信息</summary>
        private static void GetRole()
        {
            FixedResources.BASE_ROLE = FromJson(Encoding.UTF8.GetString(SQLResource.RoleInfo), typeof(List<BaseRoleInfo>)) as List<BaseRoleInfo>;
            FixedResources.BASE_IDENTITY = FromJson(Encoding.UTF8.GetString(SQLResource.Identity), typeof(List<BaseIdentity>)) as List<BaseIdentity>;
            FixedResources.BASE_OFFICE = FromJson(Encoding.UTF8.GetString(SQLResource.Office), typeof(List<BaseOffice>)) as List<BaseOffice>;
        }

        /// <summary>获取跑商商圈信息</summary>
        private static void GetBusinessArea()
        {
            FixedResources.BASE_BUSSINESS_AREA = FromJson(Encoding.UTF8.GetString(SQLResource.BusinessArea), typeof(List<BaseBusinessArea>)) as List<BaseBusinessArea>;
        }

        /// <summary>获取技能信息</summary>
        private static void GetSkill()
        {
            FixedResources.BASE_FIGHT_SKILL = FromJson(Encoding.UTF8.GetString(SQLResource.FightSkill), typeof(List<BaseFightSkill>)) as List<BaseFightSkill>;
        }

        /// <summary>获取道具信息</summary>
        private static void GetProp()
        {
            FixedResources.BASE_FUSION = FromJson(Encoding.UTF8.GetString(SQLResource.Fusion), typeof(List<BaseFusion>)) as List<BaseFusion>;
            FixedResources.BASE_EQUIP = FromJson(Encoding.UTF8.GetString(SQLResource.Equip), typeof(List<BaseEquip>)) as List<BaseEquip>;
            FixedResources.BASE_PROP = FromJson(Encoding.UTF8.GetString(SQLResource.Prop), typeof(List<BaseProp>)) as List<BaseProp>;
        }

        /// <summary>固定规则表信息</summary>
        private static void GetRule()
        {
            FixedResources.BASE_RULE = FromJson(Encoding.UTF8.GetString(SQLResource.Rule), typeof(List<BaseRule>)) as List<BaseRule>;
        }
    }
}