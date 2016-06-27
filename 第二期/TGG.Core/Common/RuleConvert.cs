using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Core.Common
{
    /// <summary>
    /// 规则公共转换方法
    /// </summary>
    public class RuleConvert
    {
        /// <summary> 修改该武将对应基础属性值 </summary>
        /// <param name="type">武将基础属性类型</param>
        /// <param name="values">要修改的该类型的值  增加为正数 减少为负数</param>
        public static double AttributeConvert(RoleAttributeType type, int values)
        {
            return GetConvertAttribute(type, values);
        }
        /// <summary> 根据公式转换对应战斗属性值 </summary>
        /// <param name="type">武将基础属性类型</param>
        /// <param name="values">要修改的该类型的值  增加为正数 减少为负数</param>
        public static double GetConvertAttribute(RoleAttributeType type, double values)
        {
            switch (type)
            {
                case RoleAttributeType.ROLE_FORCE: { return GetValues("7006", "force", values); }       //武力换算攻击力
                case RoleAttributeType.ROLE_GOVERN: { return GetValues("7008", "govern", values); }     //政务换算会心效果  (百分比)
                case RoleAttributeType.ROLE_CHARM: { return GetValues("7007", "charm", values); }       //魅力换算会心几率 (百分比)
                case RoleAttributeType.ROLE_BRAINS: { return GetValues("7009", "brains", values); }     //智谋换算闪避几率  (百分比)
                case RoleAttributeType.ROLE_CAPTAIN: { return GetValues("7010", "captain", values); }   //统帅换算奥义触发几率  (百分比)
                default: { return 0; }
            }
        }

        /// <summary> 得到固定规则表公式的结果 </summary>
        /// <param name="id">固定规则表Id</param>
        /// <param name="name">要替换的字符串</param>
        /// <param name="values">替换的值</param>
        private static double GetValues(string id, string name, double values)
        {
            var rule = GetRuleValues(id);
            if (rule == null) return 0;
            rule = rule.Replace(name, values.ToString("0.00"));
            return Math.Round(Convert.ToDouble(CommonHelper.EvalExpress(rule)), 2);
            /*
            if (rule.Contains("*"))
            {
                var s = rule.Split('*');
                if (s.Length != 2) return 0;
                var a = Convert.ToDouble(s[0]);
                var b = Convert.ToDouble(s[1]);
                return Math.Round(a * b, 2);
            }
            if (rule.Contains("/"))
            {
                var s = rule.Split('/');
                if (s.Length != 2) return 0;
                var a = Convert.ToDouble(s[0]);
                var b = Convert.ToDouble(s[1]);
                return Math.Round(a / b, 2);
            }
            return 0;
            */
        }

        private static string GetRuleValues(string id)
        {
            var baseRule = Variable.BASE_RULE.FirstOrDefault(m => m.id == id);
            return baseRule != null ? baseRule.value : null;
        }

        /// <summary>获取固定体力消耗</summary>
        /// <returns>体力消耗值</returns>
        public static int GetCostPower()
        {
            var power = Variable.BASE_RULE.FirstOrDefault(m => m.id == "1010");
            return power == null ? 0 : Convert.ToInt32(power.value);
        }

        /// <summary> 获取单个格子基数</summary>
        /// <returns></returns>
        public static int GetPacket()
        {
            var packet = Variable.BASE_RULE.FirstOrDefault(m => m.id == "3004");
            return packet == null ? 0 : Convert.ToInt32(packet.value);
        }

        #region VIP

        /// <summary>获取vip</summary>
        /// <param name="level">vip等级</param>
        public static BaseVip GetRuleVip(int level)
        {
            var vip = Variable.BASE_VIP.FirstOrDefault(m => m.level == level);
            return vip;
        }

        /// <summary>获取vip购买体力次数</summary>
        public static int GetBuyPower(int level)
        {
            var vip = GetRuleVip(level);
            return vip == null ? 0 : vip.power;
        }

        #endregion
    }
}
