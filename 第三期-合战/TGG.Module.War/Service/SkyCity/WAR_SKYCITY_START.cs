using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.War.Service.SkyCity
{
    /// <summary>
    /// 开始任务
    /// </summary>
    public class WAR_SKYCITY_START : IDisposable
    {
        //private static WAR_SKYCITY_START _objInstance;

        ///// <summary> WAR_SKYCITY_START单体模式 </summary>
        //public static WAR_SKYCITY_START GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_SKYCITY_START());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 开始任务</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_SKYCITY_START", "开始任务");
#endif
            if (!data.ContainsKey("id") || !data.ContainsKey("type") || !data.ContainsKey("count")) return null;
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value);
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
            var count = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "count").Value);

            if (id == 0 || type == 0 || count == 0) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);

            var maxcount = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32030");
            if (maxcount == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            if (count > Convert.ToInt32(maxcount.value))
                return CommonHelper.ErrorResult((int)ResultType.WAR_SKYCITY_TRAIN_ENOUGH);   //超过连续最大次数

            //var warrole = tg_war_role.GetEntityById(id);
            var warrole = tg_war_role.GetEntityByUserIdAndId(session.Player.User.id, id);

            if (warrole == null) return CommonHelper.ErrorResult((int)ResultType.WAR_ROLE_NOEXIST);
            if (warrole.state != (int)WarRoleStateType.IDLE) return CommonHelper.ErrorResult((int)ResultType.WAR_ROLE_ATSTATE);  //验证武将 状态
            if (warrole.type == (int)WarRoleType.NPC) return null;   //验证是否为备大将，备大将不可操作
            if (warrole.type == 1 && count > 1)
                return CommonHelper.ErrorResult((int)ResultType.WAR_SKYCITY_COUNT_ENOUGH); //备大将不能进行连续操作

            var user = session.Player.User;
            var cost = CostGold(count);
            if (user.gold < cost) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_GOLD_ERROR);  //验证元宝消费

            warrole.count = count;
            warrole.total_count = count;
            warrole.state = type;

            return WarRoleStart(warrole, cost);
        }

        /// <summary>进入方法分类</summary>
        private ASObject WarRoleStart(tg_war_role warrole, int cost)
        {
            switch (warrole.state)
            {
                case (int)WarRoleStateType.ASSART:
                case (int)WarRoleStateType.BUILDING:
                case (int)WarRoleStateType.BUILD_ADD:
                case (int)WarRoleStateType.MINING:
                case (int)WarRoleStateType.PEACE: return InteriorDevelop(warrole, cost);
                case (int)WarRoleStateType.LEVY: return LevyDevelop(warrole, cost);
                case (int)WarRoleStateType.TRAIN: return MoraleDevelop(warrole, cost);
            }
            return new ASObject();
        }

        #region 内政
        /// <summary>内政开发</summary>
        private ASObject InteriorDevelop(tg_war_role warrole, int cost)
        {
            var city = (new Share.War()).GetWarCity(warrole.station, warrole.user_id);
            if (city == null)
                return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

            var basecity = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
            if (basecity == null)            //基表判断
                return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            if (!CheckLockCount(warrole, city.interior_bar)) //验证开发栏数
                return CommonHelper.ErrorResult((int)ResultType.WAR_SKYCITY_LOCK_ENOUGH);
            var result = Common.GetInstance().CheckCity(warrole.state, city, basecity);//验证据点资源
            if (result != 0)
                return CommonHelper.ErrorResult(result);
            return Develop(warrole, cost);
        }

        /// <summary>武将或备大将开发处理</summary>
        private ASObject Develop(tg_war_role warrole, int cost)
        {
            int resource;
            if (warrole.type == (int)WarRoleType.PLAYER) //家臣
            {
                var role = tg_role.GetEntityById(warrole.rid);
                if (role == null) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

                if (!Common.GetInstance().PowerOperate(role, warrole.count, "内政开发")) //验证体力
                    return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_POWER_ERROR);
                (new Share.War()).UpdateGold(warrole.user_id, cost, (int)WarCommand.WAR_SKYCITY_START);

                var level = Common.GetInstance().GetLifeLevel(warrole.rid, warrole.state);
                var value = Common.GetInstance().GetRoleAtt(role, warrole.state);
                resource = GetResource(warrole, value, level);
                resource += new Share.War().GetCharacterEffect(warrole);
                resource += new Share.War().GetTacticsValue(warrole.user_id, warrole.state);
            }
            else //备大将
            {
                resource = Common.GetInstance().GetResource(warrole);
                resource += new Share.War().GetTacticsValue(warrole.user_id, warrole.state);
            }

            var basetime = GetTime();
            warrole = UpdateWarRole(warrole, resource, Convert.ToInt32(basetime));

            var time = Convert.ToInt32(basetime) * 60 * 1000;
# if DEBUG
            time = 1 * 60 * 1000; ;
#endif
            Common.GetInstance().TaskBegin(warrole.user_id, warrole.id, time);   //开始任务线程
            (new Share.War()).SendWarRole(warrole, "state");
            return new ASObject(Common.GetInstance().StartData((int)ResultType.SUCCESS, warrole));
        }


        /// <summary>验证内政正在开发的栏数</summary>
        private bool CheckLockCount(tg_war_role warrole, int count)
        {
            var ids = Common.GetInstance().GetTypes(); //验证最大栏数
            var _ids = string.Join(",", ids.ToList().Select(m => m).ToArray());
            var lockcount = tg_war_role.GetCount(warrole.station, warrole.user_id, _ids);
            if (lockcount == count)
                return false;
            return true;

        }

        /// <summary>更新合战武将实体</summary>
        private tg_war_role UpdateWarRole(tg_war_role role, int resource, int time)
        {
            role.resource = resource;
            var _time = Common.GetInstance().CurrentTime();
            role.state_end_time = time * 60 * 1000 + _time;//+ Common.GetInstance().CurrentTime();
# if DEBUG
            role.state_end_time = 1 * 60 * 1000 + Common.GetInstance().CurrentTime();
#endif
            role.count -= 1;
            role.Save();
            return role;
        }

        /// <summary>获取内政开发一次的值</summary>
        private int GetResource(tg_war_role warrole, double govern, int level)
        {
            var effect = Common.GetInstance().GetLifeValue(warrole.state, level);
            var value = Common.GetInstance().GetInteriorValue(warrole.state, govern, effect);
            return value;
        }

        /// <summary>获取开发时间</summary>
        private int GetTime()
        {
            var basetime = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32029");
            if (basetime == null) return 0;
            return Convert.ToInt32(basetime.value);
        }

        #endregion

        #region 徵兵

        /// <summary>徵兵</summary>
        private ASObject LevyDevelop(tg_war_role role, int cost)
        {
            //查询据点信息更验证
            var city = (new Share.War()).GetWarCity(role.station, role.user_id);
            if (city == null) return CommonHelper.ErrorResult((int)ResultType.WAR_CITY_NOEXIST);

            //查询判断徵兵任务的数据数量，验证徵兵栏是否满
            var taskCount = tg_war_role.GetCountByStateUserId(role.user_id, role.state, role.station);
            if (taskCount >= city.levy_bar) return CommonHelper.ErrorResult((int)ResultType.WAR_SKYCITY_LOCK_ENOUGH);

            var soldier = 0;  //单次徵兵数量
            var costpower = RuleConvert.GetCostPower() * role.count;  //普通武将消耗体力
            if (role.type == 0)  //普通玩家武将
            {
                var comrole = tg_role.GetEntityById(role.rid);
                if (comrole == null) return CommonHelper.ErrorResult((int)ResultType.WAR_ROLE__ERROR);
                //验证武将体力信息，判断体力是否足够
                var totalpower = tg_role.GetTotalPower(comrole);
                if (totalpower < costpower) return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_POWER_ERROR);

                var result = LevyArmy(comrole, role.rid, role.state, 0, role.user_id, ref soldier); //计算足轻并验证结果              
                if (result != (int)ResultType.SUCCESS) return CommonHelper.ErrorResult(result);
            }
            else
            {
                var result = LevyArmy(null, role.rid, role.state, 1, role.user_id, ref soldier); //计算足轻并验证结果
                if (result != (int)ResultType.SUCCESS) return CommonHelper.ErrorResult(result);
            }
            //验证徵兵后据点足轻是否超过上限值
            if (!CheckMaxSoidier(city.size, city.res_soldier, soldier, role.count))
                return CommonHelper.ErrorResult((int)ResultType.WAR_SKYCITY_LEVY_ENOUGH);   //足轻数量超过上限

            role.resource = soldier;   //半小时内产出的足轻数量
            return LevyArmyData(role, cost, costpower);
        }

        /// <summary>徵兵数据处理</summary>
        private ASObject LevyArmyData(tg_war_role role, int cost, int costPower)
        {
            var city = (new Share.War()).GetWarCity(role.station, role.user_id);
            if (city == null) return CommonHelper.ErrorResult((int)ResultType.WAR_CITY_NOEXIST);
            var consume = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32031");  //徵兵消耗规则
            if (consume == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);

            var totallevy = Convert.ToDouble(role.resource * role.count);    //总徵兵=半小时徵兵数量*半小时次数
            var costFunds = totallevy * Convert.ToInt32(consume.value);   // 需要消耗的军资金数量

            //根据玩家势力特性判断是否降低消耗军资金数量
            costFunds = Common.GetInstance().GetFunds(costFunds, city.user_id);

            if (costFunds > city.res_funds) return CommonHelper.ErrorResult((int)ResultType.WAR_RES_FUNDS_ERROR);

            if (role.type == 0)   //更新普通武将体力信息
            {
                var comrole = tg_role.GetEntityById(role.rid);
                if (comrole == null) return CommonHelper.ErrorResult((int)ResultType.WAR_ROLE__ERROR);
                var r = comrole.CloneEntity();   //更新武将体力信息并推送
                new Share.Role().PowerUpdateAndSend(comrole, costPower, role.user_id);
                (new Share.Role()).LogInsert(r, costPower, ModuleNumber.WAR, (int)WarCommand.WAR_SKYCITY_START, "合战", "徵兵  ");
            }
            if (cost > 0)  //连续徵兵消耗元宝，判断元宝消耗
            {
                (new Share.War()).UpdateGold(role.user_id, cost, (int)WarCommand.WAR_SKYCITY_START);
            }
            city.res_funds -= costFunds;

            city.Update();
            (new Share.War()).SendCity(city.base_id, role.user_id);  //推送据点信息更新

            //更新 合战武将状态, 开启新线程
            var half = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32029");
            if (half == null) return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            var time = Convert.ToInt32(half.value) * 60 * 1000;

#if DEBUG
            time = 60000;
#endif
            var rtime = Common.GetInstance().CurrentTime() + time;

            role.state_end_time = rtime;
            role.count -= 1;
            role.Update();
            Common.GetInstance().TaskBegin(role.user_id, role.id, time);   //开始徵兵任务线程
            (new Share.War()).SendWarRole(role, "state");
            return new ASObject(Common.GetInstance().StartData((int)ResultType.SUCCESS, role));
        }

        /// <summary>计算徵兵数量</summary>
        private int LevyArmy(tg_role role, Int64 rid, int state, int type, Int64 user_id, ref int soldier)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32044");
            if (rule == null) return 0;
            if (type == 0)  //玩家普通武将
            {
                if (role == null) { role = tg_role.GetEntityById(rid); }
                if (role == null) return (int)ResultType.DATABASE_ERROR;
                //计算武将魅力属性点总数
                var charm = tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role);
                //计算武将生活技能等级
                var level = Common.GetInstance().GetLifeLevel(rid, state);
                var effect = Common.GetInstance().GetLifeValue(state, level);  //获取生活技能加成效果值
                soldier = Convert.ToInt32(Common.GetInstance().GetRule(rule, charm) + effect);  //普通武将徵兵数量=  魅力值+军学技能效果值
            }
            else
            {
                var baseInfo = Variable.BASE_ROLE.FirstOrDefault(m => m.id == rid);
                if (baseInfo == null) return (int)ResultType.BASE_TABLE_ERROR;
                var pcharm = baseInfo.charm;  //被大将固定魅力属性点
                var peffect = Common.GetInstance().GetLifeValue(state, 1);  //军学生活技能为1级，计算生活技能效果值
                soldier = Convert.ToInt32(Common.GetInstance().GetRule(rule, pcharm) + peffect);   //备大将徵兵= 魅力值+军学技能效果值
            }

            soldier += new Share.War().GetTacticsValue(user_id, (int)WarRoleStateType.LEVY);   //内政策略加成的徵兵效果值
            return (int)ResultType.SUCCESS;
        }

        /// <summary>验证据点足轻上限</summary>
        private bool CheckMaxSoidier(int size, int cityarmy, int levy, int count)
        {
            var rule = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == size);
            if (rule == null) return false;
            var max = rule.soldier - cityarmy;
            var totallevy = levy * count;
            return totallevy <= max;
        }

        /// <summary>计算连续次数消费元宝</summary>
        private int CostGold(int count)
        {
            return count == 1 ? 0 : Common.GetInstance().GetCost(count);
        }

        #endregion

        #region 训练
        /// <summary>训练</summary>
        /// <param name="warrole"></param>
        /// <param name="cost">连续开发话费元宝</param>
        /// <returns></returns>
        public ASObject MoraleDevelop(tg_war_role warrole, int cost)
        {
            var city = (new Share.War()).GetWarCity(warrole.station, warrole.user_id);
            if (city == null)
                return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

            var basecity = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
            if (basecity == null)            //基表判断
                return CommonHelper.ErrorResult((int)ResultType.BASE_TABLE_ERROR);
            if (city.res_morale >= basecity.morale)
                return CommonHelper.ErrorResult((int)ResultType.WAR_SKYCITY_MORALE_ENOUGH);
            var count = tg_war_role.GetCountByStateUserId(warrole.user_id, warrole.state, warrole.station);
            if (count == city.train_bar) //验证开发栏数
                return CommonHelper.ErrorResult((int)ResultType.WAR_SKYCITY_LOCK_ENOUGH);
            return Train(warrole, cost);
        }

        /// <summary>武将或备大将开发处理</summary>
        private ASObject Train(tg_war_role warrole, int cost)
        {
            int resource;
            if (warrole.type == (int)WarRoleType.PLAYER) //家臣
            {
                var role = tg_role.GetEntityById(warrole.rid);
                if (role == null) return CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR);

                if (!Common.GetInstance().PowerOperate(role, warrole.count, "训练")) //验证体力
                    return CommonHelper.ErrorResult((int)ResultType.BASE_PLAYER_POWER_ERROR);
                (new Share.War()).UpdateGold(warrole.user_id, cost, (int)WarCommand.WAR_SKYCITY_START);

                var level = Common.GetInstance().GetLifeLevel(warrole.rid, warrole.state);
                var value = Common.GetInstance().GetRoleAtt(role, warrole.state);
                resource = GetResource(warrole, value, level);
                resource += new Share.War().GetCharacterEffect(warrole);
            }
            else //备大将
            {
                resource = Common.GetInstance().GetResource(warrole);
            }

            var basetime = GetTime();
#if DEBUG
            basetime = 1;
#endif
            warrole = UpdateWarRole(warrole, resource, Convert.ToInt32(basetime));

            var time = Convert.ToInt32(basetime) * 60 * 1000;
            Common.GetInstance().TaskBegin(warrole.user_id, warrole.id, time);   //开始任务线程
            (new Share.War()).SendWarRole(warrole, "state");
            return new ASObject(Common.GetInstance().StartData((int)ResultType.SUCCESS, warrole));
        }

        #endregion
    }
}
