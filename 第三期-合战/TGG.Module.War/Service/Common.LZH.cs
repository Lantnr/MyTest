using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluorineFx;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Module.War.Service.SkyCity;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.War.Service
{
    public partial class Common
    {
        /// <summary>组装数据</summary>
        public Dictionary<string, object> EnterData(int result, int interior, int levy, int train, List<ASObject> listdata)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, 
                { "interior", interior }, 
                { "levy", levy }, 
                { "train", train }, 
                { "skyCitys", listdata.Any() ? listdata : null }
            };
            return dic;

        }

        public Dictionary<string, object> StartData(int result, tg_war_role role)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"skyCity", role == null ? null : EntityToVo.ToSkyCityVo(role)}
            };
            return dic;
        }

        /// <summary>
        /// 更新玩家金币资源
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="cost">花费金币</param>
        /// <param name="cmd">指令号</param>
        public void UpdateGold(Int64 userid, int cost, int cmd)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            var user = session.Player.User.CloneEntity();
            var gold = user.gold;
            user.gold = user.gold - cost;

            var logdata = String.Format("{0}_{1}_{2}_{3}", "Gold", gold, cost, user.gold);  //元宝消费日志记录
            (new Log()).WriteLog(user.id, (int)LogType.Use, (int)ModuleNumber.WAR, cmd, "合战", "城市更名", "元宝", (int)GoodsType.TYPE_GOLD, cost, user.gold, logdata);

            user.Save();
            session.Player.User = user;
            (new User()).REWARDS_API((int)GoodsType.TYPE_GOLD, session.Player.User);
        }

        /// <summary>减少士气值</summary>
        /// <param name="count">徵兵数量</param>
        public int ReduceMorale(int count)
        {
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32032");
            if (rule == null) return 0;
            var temp = rule.value;
            temp = temp.Replace("n", count.ToString("0.00"));
            var value = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToInt32(value);
            return cost;
        }

        #region 线程完成处理资源方法

        /// <summary>线程结束资源处理</summary>
        public void GetResource(Int64 userId, Int64 id)
        {
            var role = tg_war_role.GetEntityById(id); //合战武将数据
            if (role == null)
            {
                RemoveTask(userId, id);
                return;
            }
            if (role.state == (int)WarRoleStateType.IDLE || role.count < 0)
            {
                RoleUpdate(role);
                RemoveTask(userId, id);
                return;
            }
            CheckCity(role);
        }

        /// <summary>验证据点信息并根据对应方案处理</summary>
        private void CheckCity(tg_war_role role)
        {
            //查询玩家据点信息
            var city = (new Share.War()).GetWarCity(role.station, role.user_id);
            //var city = tg_war_city.GetCityByBidUserId(role.station, role.user_id);
            if (city == null) //据点信息不存在或被占领，整理正在训练的武将信息
            {
                RoleUpdate(role);
                RemoveTask(role.user_id, role.id); //移除CD时间
            }
            else
            {
                CheckData(city, role);
            }
        }

        private BaseWarCitySize GetBase()
        {
            return Variable.BASE_WARCITYSIZE.LastOrDefault();
        }

        /// <summary>验证数据信息</summary>
        private void CheckData(tg_war_city city, tg_war_role role)
        {
            var basesize = GetBase();
            if (basesize == null) return;
            switch (role.state)
            {
                case (int)WarRoleStateType.ASSART: city.res_foods += role.resource; break;
                case (int)WarRoleStateType.BUILDING:
                    {
                        city.boom += role.resource;
                        if (city.boom > basesize.boom)
                            city.boom = basesize.boom;
                        break;
                    }
                case (int)WarRoleStateType.BUILD_ADD:
                    {
                        city.strong += role.resource;
                        if (city.strong > basesize.strong)
                            city.strong = basesize.strong;
                        break;
                    }
                case (int)WarRoleStateType.MINING: city.res_funds += role.resource; break;
                case (int)WarRoleStateType.PEACE:
                    {
                        city.peace += role.resource;
                        if (city.peace > basesize.peace)
                            city.peace = basesize.peace;
                        break;
                    }
                case (int)WarRoleStateType.LEVY:
                    {
                        city.res_soldier += role.resource;
                        var res = city.res_morale - ReduceMorale(role.resource);  //徵兵扣除士气
                        city.res_morale = res <= 0 ? 0 : res;
                    }
                    break;
                case (int)WarRoleStateType.TRAIN:
                    {
                        city.res_morale = city.res_morale + role.resource + GetCharacterMorale(city.user_id);
                        var basecity = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
                        if (basecity == null)            //基表判断
                            return;
                        if (city.res_morale > basecity.morale)
                            city.res_morale = basecity.morale;

                    }
                    break;
            }
            new Share.War().ReduceAdd(role, 0);
            city.Update();
            if (role.count == 0)
            {
                new Share.War().ReduceRes(role);
                role.state = (int)WarRoleStateType.IDLE;
                role.resource = 0;
                role.total_count = 0;
                role.state_end_time = 0;
                RemoveTask(role.user_id, role.id); //移除CD时间
                (new Share.War()).SendWarRole(role, "state");
            }
            else
            {
                //更新武将下一个半小时到达时间
                role.count -= 1;
                Int64 time = 30 * 60 * 1000;
# if DEBUG
                time = 1 * 60 * 1000;
#endif
                role.state_end_time = CurrentTime() + time;
                RemoveTask(role.user_id, role.id); //移除CD时间
                SpinWait.SpinUntil(() => false, 500);
                TaskBegin(role.user_id, role.id, time);   //重新开启线程
            }
            var _city = (new Share.War()).GetWarCity(role.station, role.user_id);
            if (_city == null)
            {
                RoleUpdate(role);
                RemoveTask(role.user_id, role.id); //移除CD时间
                return;
            }
            role.Update();
            (new Share.War()).SaveWarCityAll(city);   //更新全局据点信息
            new  WAR_SKYCITY_PUSH().ResourcePush(city.base_id, role);
        }

        /// <summary>关闭线程</summary>
        private void RemoveTask(Int64 userId, Int64 id)
        {
            var key = String.Format("{0}_{1}_{2}", (int)CDType.SkyCity, userId, id); //关闭线程，移除CD时间
            bool r;
            Variable.CD.TryRemove(key, out r);
        }

        /// <summary>更新并推送</summary>
        private void RoleUpdate(tg_war_role role)
        {
            role.state = (int)WarRoleStateType.IDLE;
            role.count = 0;
            role.total_count = 0;
            role.resource = 0;
            role.state_end_time = 0;

            role.Update();
            (new Share.War()).SendWarRole(role, "state");
            if (Variable.OnlinePlayer.ContainsKey(role.user_id))
            {
                var session = Variable.OnlinePlayer[role.user_id] as TGGSession;
                if (session != null)
                {
                    var skyVo = EntityToVo.ToSkyCityVo(role);
                    var data = new ASObject(new Dictionary<string, object> { { "skyCity", skyVo } });
                    var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.WAR_SKYCITY_PUSH,
                        (int)ResponseType.TYPE_SUCCESS, data);
                    session.SendData(pv);
                }
            }
        }

        #endregion

        /// <summary>根据玩家势力验证是否添加势力效果值</summary>
        public int GetCharacterMorale(Int64 userId)
        {
            int morale = 0;
            var user = tg_user.GetUsersById(userId);
            if (user == null) return morale;
            if (user.player_influence != (int)InfluenceType.ZhiTian) return morale;
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32065");
            if (rule == null) return morale;
            morale = Convert.ToInt32(rule.value);
            return morale;
        }

        /// <summary>根据玩家势力特性计算招募足轻是否降低消耗军资金</summary>
        public double GetFunds(double funds, Int64 userId)
        {
            var user = tg_user.GetUsersById(userId);
            if (user == null) return funds;

            switch (user.player_influence)
            {
                case (int)InfluenceType.ZhiTian:
                    {
                        var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32064");
                        if (rule == null) return funds;
                        funds = GetResult(rule.value, funds);
                    }
                    break;
                case (int)InfluenceType.YiDa:
                    {
                        var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32069");
                        if (rule == null) return funds;
                        funds = GetResult(rule.value, funds);
                    }
                    break;
            }
            return funds;
        }

        /// <summary>根据玩家势力特性计算购买军需品是否降价</summary>
        public double GetFunds(double funds, int influence, int type)
        {
            switch (influence)
            {
                case (int)InfluenceType.DeChuan:
                    {
                        if (type != (int)WarResourseType.苦无) return funds;
                        var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32066");
                        if (rule == null) return funds;
                        funds = GetResult(rule.value, funds);
                    } break;
                case (int)InfluenceType.WuTian:
                    {
                        if (type != (int)WarResourseType.马匹) return funds;
                        var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32067");
                        if (rule == null) return funds;
                        funds = GetResult(rule.value, funds);
                    } break;
                case (int)InfluenceType.Shangshan:
                    {
                        if (type != (int)WarResourseType.薙刀) return funds;
                        var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32068");
                        if (rule == null) return funds;
                        funds = GetResult(rule.value, funds);
                    } break;
                case (int)InfluenceType.YiDa:
                    {
                        if (type != (int)WarResourseType.铁炮) return funds;
                        var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32070");
                        if (rule == null) return funds;
                        funds = GetResult(rule.value, funds);
                    } break;
            }
            return funds;
        }

        /// <summary>计算结果</summary>
        public double GetResult(string rule, double funds)
        {
            if (String.IsNullOrEmpty(rule)) return funds;
            var temp = rule;
            temp = temp.Replace("cost", funds.ToString("0.00"));
            var value = CommonHelper.EvalExpress(temp);
            var cost = Convert.ToDouble(value);
            return cost;
        }
    }
}
