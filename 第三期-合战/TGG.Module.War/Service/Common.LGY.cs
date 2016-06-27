using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Module.War.Service.Fight;
using TGG.Module.War.Service.SkyCity;
using TGG.Share;
using TGG.SocketServer;
using XCode;

namespace TGG.Module.War.Service
{
    public partial class Common
    {
        #region 数据组装
        public Dictionary<string, object> BuilData(int result)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result",result},
            };
            return dic;
        }


        public Dictionary<string, object> BuilDevoteData(int result, int devote)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result",result},
                {"surplus",devote},
            };
            return dic;
        }

        public Dictionary<string, object> BuildData(int result, int resource)
        {
            var dic = new Dictionary<string, object>()
            {
                {"result",result},
                 {"resource",resource},
            };
            return dic;
        }
        #endregion

        #region 线程

        /// <summary>开启线程</summary>
        public void TaskBegin(Int64 userid, Int64 id, Int64 costtime)
        {
            try
            {
                var token = new CancellationTokenSource();
                Object obj = new SkyCityObject { userid = userid, id = id, };
                Task.Factory.StartNew(m =>
                {
                    var t = m as SkyCityObject;
                    if (t == null) return;
                    var key = t.GetKey();
                    Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                    SpinWait.SpinUntil(() =>
                    {
                        if (!Variable.CD.ContainsKey(key))
                        {
                            token.Cancel();
                            return true;
                        }
                        return false;
                    }, Convert.ToInt32(costtime));
                }, obj, token.Token)
                .ContinueWith((m, n) =>
                {
                    var io = n as SkyCityObject;
                    if (io == null) { token.Cancel(); return; }

                    GetResource(userid, id);   //资源处理方法
                    //var key = io.GetKey();
                    //bool r;
                    //Variable.CD.TryRemove(key, out r);
                    token.Cancel();
                }, obj, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public class SkyCityObject
        {
            public Int64 userid { get; set; }

            public Int64 id { get; set; }

            public String GetKey()
            {
                return String.Format("{0}_{1}_{2}", (int)CDType.SkyCity, userid, id);
            }
        }

        private List<Int32> GetList()
        {
            var list = new List<Int32>()
            {
                 (int)WarRoleStateType.ASSART,
                 (int)WarRoleStateType.BUILDING,
                 (int)WarRoleStateType.BUILD_ADD,
                 (int)WarRoleStateType.MINING,
                 (int)WarRoleStateType.PEACE,
                 (int)WarRoleStateType.LEVY,
                 (int)WarRoleStateType.TRAIN
            };
            return list;
        }

        /// <summary>内政,军事开发重新起伏加载线程</summary>
        public void SkyCityRecovery()
        {
            var citys = Entity<tg_war_city>.FindAll().ToList();
            var s = GetList();
            var list = tg_war_role.GetEntityByState(s);
            var time = CurrentTime() + 5000;//+5000毫秒预热
            var a = list.Where(m => m.state_end_time <= time && m.count == 0).ToList(); //已完
            var b = list.Where(m => m.state_end_time <= time && m.count > 0).ToList(); //还有次数
            var c = list.Where(m => m.state_end_time > time).ToList();  //还有时间

            if (a.Any())
            {
                foreach (var wr in a)
                {
                    var city = citys.FirstOrDefault(m => m.user_id == wr.user_id && m.base_id == wr.station);
                    if (city == null || city.user_id != wr.user_id) { UpdateCity(wr); continue; }
                    city = (new Share.War()).GetUpdateCity(city, wr.state, wr.resource);
                    city.Save();
                    (new Share.War()).SaveWarCityAll(city);   //更新全局据点信息
                    wr.state_end_time = 0;
                    wr.state = (int)WarRoleStateType.IDLE;
                    wr.resource = 0;
                    wr.total_count = 0;
                }
                tg_war_role.GetUpdate(a);
            }
            if (b.Any())
            {
                foreach (var wr in b)
                {
                    var city = citys.FirstOrDefault(m => m.user_id == wr.user_id && m.base_id == wr.station);
                    if (city == null || city.user_id != wr.user_id) { UpdateCity(wr); continue; }
                    city = (new Share.War()).GetUpdateCity(city, wr.state, wr.resource);
                    city.Save();
                    (new Share.War()).SaveWarCityAll(city);   //更新全局据点信息
                    var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32029");
                    if (rule == null) continue;
# if DEBUG
                    rule.value = "1";
#endif
                    var t = Convert.ToInt32(rule.value) * 60 * 1000;
                    wr.count -= 1;
                    wr.state_end_time = t + CurrentTime();
                    wr.Save();
                    TaskBegin(wr.user_id, wr.id, t);

                }
            }
            if (!c.Any()) return;
            foreach (var wr in c)
            {
                var _time = wr.state_end_time - CurrentTime();
                if (_time < 0) continue;
                var city = citys.FirstOrDefault(m => m.user_id == wr.user_id && m.base_id == wr.station);
                if (city == null || city.user_id != wr.user_id) { UpdateCity(wr); continue; }
                TaskBegin(wr.user_id, wr.id, _time);
            }
        }
        #endregion

        #region 内政开始开发

        /// <summary>验证开发资源是否达到上限</summary>
        /// <param name="type">开发类型</param>
        /// <param name="city">据点实体</param>
        /// <param name="basecity">据点规模基表</param>
        /// <returns></returns>
        public int CheckCity(int type, tg_war_city city, BaseWarCitySize basecity)
        {
            var basesize = GetBase();
            if (basesize == null) return 0;
            switch (type)
            {
                #region
                case (int)WarRoleStateType.ASSART:
                    {
                        if (city.res_foods >= basecity.foods)
                            return (int)ResultType.WAR_SKYCITY_FOODS_ENOUGH;
                    }
                    break;
                case (int)WarRoleStateType.BUILD_ADD:
                    {
                        if (Convert.ToInt32(city.strong) >= basesize.strong)
                            return (int)ResultType.WAR_SKYCITY_STRONG_ENOUGH;
                    }
                    break;
                case (int)WarRoleStateType.PEACE:
                    {
                        if (Convert.ToInt32(city.peace) >= basesize.peace)
                            return (int)ResultType.WAR_SKYCITY_PEACE_ENOUGH;
                    }
                    break;
                case (int)WarRoleStateType.MINING:
                    {
                        if (Convert.ToInt32(city.res_funds) >= Convert.ToInt32(basecity.funds))
                            return (int)ResultType.WAR_SKYCITY_MINING_ENOUGH;
                    }
                    break;
                case (int)WarRoleStateType.BUILDING:
                    {
                        if (Convert.ToInt32(city.boom) >= basesize.boom)
                            return (int)ResultType.WAR_SKYCITY_BUILDING_ENOUGH;
                    }
                    break;
                #endregion
            }
            return 0;
        }

        public IEnumerable<int> GetTypes()
        {
            var list = new List<int>();
            list.Add((int)WarRoleStateType.ASSART);
            list.Add((int)WarRoleStateType.BUILDING);
            list.Add((int)WarRoleStateType.BUILD_ADD);
            list.Add((int)WarRoleStateType.MINING);
            list.Add((int)WarRoleStateType.PEACE);
            return list;
        }


        /// <summary>获取连续开发所消耗的元宝</summary>
        public int GetCost(int count)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32010");
            if (rule == null) return 0;
            var temp = rule.value;
            temp = temp.Replace("count", count.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(express);
            return cost;
        }

        /// <summary>体力操作</summary>
        public bool PowerOperate(tg_role role, int count, string name)
        {
            var power = RuleConvert.GetCostPower();
            power = power * count;
            var totalpower = tg_role.GetTotalPower(role);
            if (totalpower < power) return false;
            var r = role.CloneEntity();
            new Role().PowerUpdateAndSend(role, power, role.user_id);

            (new Role()).LogInsert(r, power, ModuleNumber.WAR, (int)WarCommand.WAR_SKYCITY_START, "合战", name);
            return true;
        }

        #endregion

        #region 内政结算资源

        /// <summary>更新据点实体</summary>
        /// <param name="cityid">据点主键id</param>
        /// <param name="type">内政开发类型</param>
        /// <param name="value">开发获得的值</param>
        /// <returns></returns>
        public tg_war_city UpdateCityAtt(Int64 cityid, int type, int value)
        {
            var city = tg_war_city.GetEntityById(cityid);
            if (city == null) return null;
            var citysize = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
            if (citysize == null) return null;
            switch (type)
            {
                #region
                case (int)WarRoleStateType.ASSART:
                    {
                        city.res_foods += value;
                        if (citysize.foods < city.res_foods)
                            city.res_foods = citysize.foods;
                    }
                    break;
                case (int)WarRoleStateType.BUILD_ADD:
                    {
                        city.strong += value;
                        if (citysize.strong < city.strong)
                            city.strong = citysize.strong;
                    }
                    break;
                case (int)WarRoleStateType.PEACE:
                    {
                        city.peace += value;
                        if (citysize.peace < city.peace)
                            city.peace = citysize.peace;
                    }
                    break;
                case (int)WarRoleStateType.MINING:
                    {
                        city.res_funds += value;
                        if (citysize.funds < city.res_funds)
                            city.res_funds = citysize.funds;
                    }
                    break;
                case (int)WarRoleStateType.BUILDING:
                    {
                        if (citysize.boom < city.boom)
                            city.boom += value;
                        city.boom = citysize.boom;
                    }
                    break;
                #endregion
            }
            return city;
        }

        /// <summary>获取内政开发后的值</summary>
        /// <param name="type">内政开发类型</param>
        /// <param name="govern">武将单个总属性值</param>
        /// <param name="effect">生活技能效果值</param>
        /// <returns></returns>
        public int GetInteriorValue(int type, double govern, int effect)
        {
            BaseRule rule;
            int value = 0;
            int v;
            switch (type)
            {
                #region
                case (int)WarRoleStateType.ASSART:
                    {
                        var rule1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32008");
                        rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32003");
                        v = GetRule(rule1, govern);
                        v += effect;
                        value = GetRule(rule, v);
                    }
                    break;
                case (int)WarRoleStateType.BUILD_ADD:
                    {
                        rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32005");
                        v = GetRule(rule, govern);
                        v += effect;
                        value = v;
                    }
                    break;
                case (int)WarRoleStateType.PEACE:
                    {
                        rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32006");
                        v = GetRule(rule, govern);
                        v += effect;
                        value = v;
                    }
                    break;
                case (int)WarRoleStateType.MINING:
                    {
                        var rule1 = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32009");
                        rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32004");
                        v = GetRule(rule1, govern);
                        v += effect;
                        value = GetRule(rule, v);
                    }
                    break;
                case (int)WarRoleStateType.BUILDING:
                    {
                        rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32007");
                        v = GetRule(rule, govern);
                        value = v;
                    }
                    break;
                case (int)WarRoleStateType.TRAIN:
                    {
                        rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32097");
                        v = GetRule(rule, govern);
                        v += effect;
                        value = v;
                    }
                    break;
                #endregion
            }
            return value;
        }

        /// <summary>获取生活技能等级</summary>
        /// <param name="rid">武将主键id</param>
        /// <param name="type">内政开发类型</param>
        /// <returns></returns>
        public int GetLifeLevel(Int64 rid, int type)
        {
            var life = tg_role_life_skill.GetEntityByRid(rid);
            if (life == null) return 0;
            switch (type)
            {
                #region
                case (int)WarRoleStateType.ASSART:
                    return life.sub_reclaimed_level;
                case (int)WarRoleStateType.BUILD_ADD:
                    return life.sub_build_level;
                case (int)WarRoleStateType.PEACE:
                    return life.sub_martial_level;
                case (int)WarRoleStateType.MINING:
                    return life.sub_mine_level;
                case (int)WarRoleStateType.BUILDING:
                    break;
                case (int)WarRoleStateType.LEVY:
                case (int)WarRoleStateType.TRAIN:
                    return life.sub_tactical_level;
                #endregion
            }
            return 0;
        }

        /// <summary>获取生活技能效果值</summary>      
        /// <param name="type">内政开发类型</param>
        /// <param name="level">生活技能等级</param>
        /// <returns></returns>
        public int GetLifeValue(int type, int level)
        {
            int value = 0;
            switch (type)
            {
                #region
                case (int)WarRoleStateType.ASSART:
                    value = GetEffectValue((int)LifeSkillType.RECLAIMED, level, (int)LifeSkillEffectType.INCREASE_CITY_DEVELOP_PROGRESS);
                    break;
                case (int)WarRoleStateType.BUILD_ADD:
                    value = GetEffectValue((int)LifeSkillType.BUILD, level, (int)LifeSkillEffectType.CITY_DEVELOPMENT);
                    break;
                case (int)WarRoleStateType.PEACE:
                    value = GetEffectValue((int)LifeSkillType.MARTIAL, level, (int)LifeSkillEffectType.SECURITY_PROMOTE);
                    break;
                case (int)WarRoleStateType.MINING:
                    value = GetEffectValue((int)LifeSkillType.MINE, level, (int)LifeSkillEffectType.MINE_INCREASE);
                    break;
                case (int)WarRoleStateType.BUILDING:
                    break;
                case (int)WarRoleStateType.LEVY:
                    value = GetEffectValue((int)LifeSkillType.TACTICAL, level, (int)LifeSkillEffectType.INCREASE_CONSCRIPTION_NUMBER);
                    break;
                case (int)WarRoleStateType.TRAIN:
                    value = GetEffectValue((int)LifeSkillType.TACTICAL, level, (int)LifeSkillEffectType.INCREASE_MORALE);
                    break;
                #endregion
            }
            return value;
        }

        /// <summary>获取生活技能效果值</summary>
        /// <param name="type">生活技能类型</param>
        /// <param name="level">生活技能等级</param>
        /// <param name="effecttype">生活技能效果类型</param>
        private int GetEffectValue(int type, int level, int effecttype)
        {
            var baselife = Variable.BASE_LIFESKILL.FirstOrDefault(m => m.type == type);
            if (baselife == null)
                return 0;
            var baseeffect = Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(m => m.skillid == baselife.id && m.level == level);
            var list = GetValues(baseeffect, effecttype);
            var sum = list.Sum();
            return sum;
        }

        private IEnumerable<int> GetValues(BaseLifeSkillEffect baseeffect, int type)
        {
            var eos = Sprit(baseeffect.effect);
            return eos.Select(m => GetValues(m, type)).ToList();
        }

        private int GetValues(EffectObject eo, int type)
        {
            if (eo.type == type)
                return eo.value;
            return 0;
        }

        /// <summary>切割字符串</summary>
        private IEnumerable<EffectObject> Sprit(string effect)
        {
            var eos = new List<EffectObject>();
            if (String.IsNullOrEmpty(effect)) return eos;
            if (effect.Contains('|'))
            {
                var es = effect.Split('|');
                foreach (var item in es)
                {
                    var eo = new EffectObject();
                    var e = item.Split('_');
                    eo.type = Convert.ToInt32(e[0]);
                    eo.value = Convert.ToInt32(e[3]);
                    eos.Add(eo);
                }
            }
            else
            {
                var eo = new EffectObject();
                var e = effect.Split('_');
                eo.type = Convert.ToInt32(e[0]);
                eo.value = Convert.ToInt32(e[3]);
                eos.Add(eo);
            }
            return eos;
        }

        public class EffectObject
        {
            public int type { get; set; }

            public int value { get; set; }
        }

        public int GetRule(BaseRule baserule, double value)
        {
            // var baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17015");
            if (baserule == null) return 0;
            var temp = baserule.value;
            temp = temp.Replace("value", value.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(express);
            return cost;
        }

        #endregion

        /// <summary> 当前时间毫秒</summary>
        public Int64 CurrentTime()
        {
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }

        private void UpdateCity(tg_war_role role)
        {
            role.state = (int)WarRoleStateType.IDLE;
            role.count = 0;
            role.resource = 0;
            role.state_end_time = 0;
            role.total_count = 0;
            role.Save();
        }

        /// <summary>获取武将单个总属性值</summary>
        public double GetRoleAtt(tg_role role, int type)
        {
            switch (type)
            {
                case (int)WarRoleStateType.ASSART:
                case (int)WarRoleStateType.BUILDING:
                case (int)WarRoleStateType.MINING: return tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role);
                case (int)WarRoleStateType.PEACE: return tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, role);
                case (int)WarRoleStateType.BUILD_ADD: return tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role);
                case (int)WarRoleStateType.TRAIN: return tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, role);
            }
            return 0;
        }

        /// <summary>获取备大将可锻炼资源</summary>
        public int GetResource(tg_war_role warrole)
        {
            var baserole = Variable.BASE_ROLE.FirstOrDefault(m => m.id == warrole.rid);
            if (baserole == null) return 0;
            switch (warrole.state)
            {
                case (int)WarRoleStateType.ASSART:
                case (int)WarRoleStateType.BUILDING:
                case (int)WarRoleStateType.MINING:
                    return GetResource(warrole, baserole.govern, 1);
                case (int)WarRoleStateType.PEACE:
                    return GetResource(warrole, baserole.force, 1);
                case (int)WarRoleStateType.BUILD_ADD:
                    return GetResource(warrole, baserole.brains, 1);
                case (int)WarRoleStateType.TRAIN:
                    return GetResource(warrole, baserole.captain, 1);
            }
            return 0;
        }

        /// <summary>获取内政开发一次的值</summary>
        private int GetResource(tg_war_role warrole, double govern, int level)
        {
            var effect = GetLifeValue(warrole.state, level);
            var value = GetInteriorValue(warrole.state, govern, effect);
            return value;
        }

    }
}
