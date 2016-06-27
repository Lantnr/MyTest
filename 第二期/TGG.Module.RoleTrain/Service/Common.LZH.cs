using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 点将公共方法
    /// </summary>
    public partial class Common
    {
        #region 数据组装
        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BulidData(int result)
        {
            var dic = new Dictionary<string, object> { { "result", result }, };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> NpcBulidData(int result, List<tg_train_home> npcs)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "npc", ConvertToNpcAsobject(npcs) }, };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> TeaBulidData(int result, int spirit, int npcSpirit)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "spirit", spirit }, { "npcSpirit", npcSpirit } };
            return dic;
        }

        /// <summary>武将挑战组装数据</summary>
        public Dictionary<String, Object> NpcChallengeData(int result, int type)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "type", type }, };
            return dic;
        }

        /// <summary>武将挑战组装数据</summary>
        public Dictionary<String, Object> NpcStealData(int result, int type)
        {
            var dic = new Dictionary<string, object> { { "result", result }, { "type", type }, };
            return dic;
        }
        #endregion

        /// <summary>转为NpcMonsterASObject</summary>
        private List<ASObject> ConvertToNpcAsobject(IEnumerable<tg_train_home> lists)
        {
            var listnpcs = lists.Select(item => AMFConvert.ToASObject(EntityToVo.ToNpcMonsterVo(item))).ToList();
            return listnpcs;
        }

        #region 装备方法
        /// <summary>
        /// 挑战胜利根据概率判断是否掉落装备
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="equips">装备集合</param>
        /// /// <param name="probability">掉落概率</param>
        public tg_bag RandomEquip(Int64 userid, string equips, double probability)
        {
            if (string.IsNullOrEmpty(equips)) return null;
            var ids = new List<int>();
            if (equips.Contains("|"))
            {
                var n = equips.Split("|").ToList();
                ids.AddRange(n.Select(item => Convert.ToInt32(item)));
            }
            else
            {
                ids.Add(Convert.ToInt32(equips));
            }
            var equip = CommonHelper.RateRandomEquip(userid, probability, ids);
            return equip;
        }

        /// <summary>偷窃判断是否获得装备装备</summary>
        /// <param name="userid">用户id</param>
        /// <param name="baseinfo">npc携带装备信息</param>
        /// <param name="nprob">忍术加成的偷窃概率</param>
        /// /// <param name="bprob">智谋加成的偷窃概率</param>
        /// <returns></returns>
        public tg_bag RandomEquip(Int64 userid, BaseNpcMonster baseinfo, double nprob, double bprob)
        {
            if (string.IsNullOrEmpty(baseinfo.equip)) return null;
            var ids = new List<int>();
            if (baseinfo.equip.Contains("|"))
            {
                var n = baseinfo.equip.Split("|").ToList();
                ids.AddRange(n.Select(item => Convert.ToInt32(item)));
            }
            else
            {
                ids.Add(Convert.ToInt32(baseinfo.equip));
            }
            var change = baseinfo.rate + nprob + bprob;        //忍术技能加成后的偷窃概率
            if (change > 100) change = 100;
            var equip = CommonHelper.RateRandomEquip(userid, change, ids);
            return equip;
        }

        #endregion
    }
}
