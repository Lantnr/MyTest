using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Share
{
    public class RoleTrain : IDisposable
    {
        /// <summary>资源回收</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>玩家喝茶获得的魂数</summary>
        public int GetSpirit(int teaid, int tealevel)
        {
            var spirit = 0;
            var teainfo = Variable.BASE_LIFESKILLEFFECT.FirstOrDefault(m => m.skillid == teaid && m.level == tealevel);
            if (teainfo == null) return spirit;
            if (string.IsNullOrEmpty(teainfo.effect)) return spirit;
            if (!teainfo.effect.Contains("_")) return spirit;
            var addtion = teainfo.effect.Split("_").ToList();
            var value = addtion[3];
            spirit = spirit + Convert.ToInt32(value);
            return spirit;
        }

        /// <summary>体力操作</summary>
        public bool PowerOperate(tg_role role)
        {
            var power = RuleConvert.GetCostPower();
            var totalpower = tg_role.GetTotalPower(role);
            if (totalpower < power) return false;
            new Role().PowerUpdateAndSend(role, power, role.user_id);
            return true;
        }

        /// <summary>判断用户等级信息</summary>
        public bool IsLevelOk(int lv, int level)
        {
            var home = Variable.BASE_ROLE_HOME.FirstOrDefault(m => m.id == lv);
            return home != null && level >= home.openLv;
        }

        /// <summary>随机出同一居城难度将册Npc</summary>
        public List<BaseNpcMonster> RandomNpc(int cityid, int level)
        {
            var basenpcs = Variable.BASE_NPCMONSTER.Where(m => m.type == level && m.cityId == cityid).ToList();
            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "17022");
            var npcs = new List<BaseNpcMonster>();
            if (rule == null) return npcs;
            if (!basenpcs.Any()) return npcs;
            if (basenpcs.Count <= Convert.ToInt32(rule.value)) { return basenpcs; }

            //随机指定个数的NPC
            var ids = basenpcs.Select(m => m.id).ToList();
            ids = RandomBase(ids, Convert.ToInt32(rule.value));
            npcs.AddRange(ids.Select(item => basenpcs.FirstOrDefault(m => m.id == item)));
            return npcs;
        }

        /// <summary>随机npcs</summary>
        private List<int> RandomBase(List<int> ids, int count)
        {
            var newids = RNG.Next(0, ids.Count - 1, count);
            return newids.Select(id => ids[id]).ToList();
        }

        /// <summary>插入Npc信息</summary>
        public bool InsertNpc(List<BaseNpcMonster> npcs, Int64 userid)
        {
            var list = npcs.Select(item => new tg_train_home
            {
                userid = userid,
                npc_id = item.id,
                npc_type = item.type,
                city_id = item.cityId,
                npc_spirit = item.spirit,
                total_spirit = item.spirit,
            }).ToList();
            return tg_train_home.GetInsertNpc(list);
        }

        /// <summary>向用户推送更新</summary>
        public void RewardsToUser(TGGSession session, tg_user user, int type)
        {
            session.Player.User = user;
            (new Share.User()).REWARDS_API(type, session.Player.User);
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, RoleInfoVo rolevo)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"role", rolevo}
            };
            return dic;
        }

        #region 武将修行

        /// <summary>获取可以锻炼的加成值</summary>
        public double GetCanAtt(tg_user user, tg_role role, int attribute, BaseTrain basetrain, int count)
        {
            count = basetrain.count * count;
            var base_rule = GetBaseRule(user, role.role_id);
            if (base_rule == null)
                return 0;
            var role_att = GetTrainAtt(role, attribute);
            var att = GetAtt(role_att, count, base_rule);
            return att;
        }

        /// <summary>获取上限值</summary>
        public BaseRule GetBaseRule(tg_user user, int roleid)
        {
            var baserule = new BaseRule();
            if (roleid == user.role_id)
            {
                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7013");
            }
            else
            {
                baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7019");
            }
            return baserule;
        }

        /// <summary>获取修行属性值</summary>
        public double GetTrainAtt(tg_role role, int attribute)
        {
            switch (attribute)
            {
                case (int)RoleAttributeType.ROLE_CAPTAIN:
                    {
                        return role.base_captain_train;
                    }
                case (int)RoleAttributeType.ROLE_CHARM:
                    {
                        return role.base_charm_train;
                    }
                case (int)RoleAttributeType.ROLE_FORCE:
                    {
                        return role.base_force_train;

                    }
                case (int)RoleAttributeType.ROLE_GOVERN:
                    {
                        return role.base_govern_train;

                    }
                case (int)RoleAttributeType.ROLE_BRAINS:
                    {
                        return role.base_brains_train;

                    }
            }
            return 0;
        }

        /// <summary>
        /// 获取加成值
        /// </summary>
        /// <param name="att">武将值</param>
        /// <param name="value">加成值</param>
        /// <param name="base_rule">上限值</param>
        private double GetAtt(double att, int value, BaseRule base_rule)
        {
            att = att + value;
            var _att = att - Convert.ToDouble(base_rule.value);
            if (!(_att > 0)) return value;

            var use = value - _att;
            return use < 0 ? 0 : use;
        }

        /// <summary>基础属性值转换对应战斗属性值</summary>
        public tg_role RoleAtt(tg_role role, tg_train_role train_role, double att)
        {
            var mn = (int)ModuleNumber.ROLETRAIN;
            var rc = (int)RoleTrainCommand.TRAIN_ROLE_PUSH;
            var type = train_role.attribute;
            switch (type)
            {
                #region
                case (int)RoleAttributeType.ROLE_CAPTAIN:
                    {
                        var captain = role.base_captain_train;
                        role.base_captain_train += att;
                        RoleAttrituteLog(role.user_id, type, att, captain, role.base_captain_train, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_CHARM:
                    {
                        var charm = role.base_charm_train;
                        role.base_charm_train += att;
                        RoleAttrituteLog(role.user_id, type, att, charm, role.base_charm_train, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_FORCE:
                    {
                        var force = role.base_force_train;
                        role.base_force_train += att;
                        RoleAttrituteLog(role.user_id, type, att, force, role.base_force_train, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_GOVERN:
                    {
                        var govern = role.base_govern_train;
                        role.base_govern_train += att;
                        RoleAttrituteLog(role.user_id, type, att, govern, role.base_govern_train, mn, rc);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_BRAINS:
                    {
                        var brains = role.base_brains_train;
                        role.base_brains_train += att;
                        RoleAttrituteLog(role.user_id, type, att, brains, role.base_brains_train, mn, rc);
                        break;
                    }
                #endregion
            }
            return role;
        }

        /// <summary>武将属性变动添加日志 </summary>
        /// <param name="userid">玩家id</param>
        /// <param name="type">属性类型</param>
        /// <param name="add">增加的值</param>
        /// <param name="ov">武将增加前的属性值</param>
        /// <param name="nv">武将增加后的属性值</param>
        /// <param name="ModuleNumber">模块号</param>
        /// <param name="command">指令号</param>
        private void RoleAttrituteLog(Int64 userid, int type, double add, double ov, double nv, int ModuleNumber, int command)
        {
            string logdata = "";
            switch (type)
            {
                #region
                case (int)RoleAttributeType.ROLE_BRAINS:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleBrains", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_CAPTAIN:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleCaptain", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_CHARM:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleCharm", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_FORCE:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleForce", ov, add, nv);
                        break;
                    }
                case (int)RoleAttributeType.ROLE_GOVERN:
                    {
                        logdata = string.Format("{0}_{1}_{2}_{3}", "RoleGovern", ov, add, nv);
                        break;
                    }
                #endregion
            }
            (new Share.Log()).WriteLog(userid, (int)LogType.Get, ModuleNumber, command, logdata);// (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_ROLE_LOCK, logdata);
        }



        /// <summary>创建武将修行线程</summary>
        public void RoleTrainThreading(Int64 user_id, Int64 rid, Int64 costtime)
        {
            #region
            //try
            //{
            //    var token = new CancellationTokenSource();
            //    var task = new Task(() => SpinWait.SpinUntil(() => false, (int)costtime), token.Token);
            //    task.Start();
            //    task.ContinueWith(m =>
            //    {
            //        XTrace.WriteLine("玩家名称{0}--玩家ID{1}", session.Player.User.player_name, session.Player.User.id);
            //        //if (Variable.OnlinePlayer.Contains(session.Player.User.id))
            //        if (Variable.OnlinePlayer.ContainsKey(session.Player.User.id))
            //        {
            //            //session = Variable.OnlinePlayer[session.Player.User.id] as TGGSession;
            //            session = Variable.OnlinePlayer[session.Player.User.id] as TGGSession;
            //        }
            //        TRAIN_ROLE_PUSH.GetInstance().CommandStart(session, roleitem);
            //        token.Cancel();
            //    }, token.Token);

            //}
            //catch (Exception ex)
            //{
            //    XTrace.WriteException(ex);
            //}
            #endregion
            try
            {
                var token = new CancellationTokenSource();
                Object obj = new RoleTrianObject { user_id = user_id, rid = rid };
                Task.Factory.StartNew(m =>
                {
                    var t = m as RoleTrianObject;
                    if (t == null) return;
                    var key = t.GetKey();
                    Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                    SpinWait.SpinUntil(() =>
                    {
                        var b = Convert.ToBoolean(Variable.CD[key]);
                        return b;
                    }, Convert.ToInt32(costtime));
                }, obj, token.Token)
                .ContinueWith((m, n) =>
                {
                    var lo = n as RoleTrianObject;
                    if (lo == null) { token.Cancel(); return; }
                    RoleTrainPush(lo.user_id, lo.rid);
                    //移除全局变量
                    var key = lo.GetKey();
                    bool r;
                    Variable.CD.TryRemove(key, out r);
                    token.Cancel();
                }, obj, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public class RoleTrianObject
        {
            public Int64 user_id { get; set; }
            public Int64 rid { get; set; }

            public String GetKey()
            {
                return string.Format("{0}_{1}_{2}", (int)CDType.RoleTrain, user_id, rid);
            }
        }

        /// <summary>获取单个能锻炼值</summary>
        /// <param name="type">属性类型</param>
        /// <param name="att">待锻炼点数</param>
        /// <param name="model">武将信息</param>
        public Double GetSingleCanTrain(RoleAttributeType type, Double att, tg_role model)
        {
            var c_t = tg_role.GetCanTrain(att, model);          //剩余可以锻炼总点数
            if (c_t < 0) return 0;

            Double current = 0;
            switch (type)
            {
                #region
                case RoleAttributeType.ROLE_CAPTAIN:
                    {
                        current = model.base_captain_train;
                        break;
                    }
                case RoleAttributeType.ROLE_FORCE:
                    {
                        current = model.base_force_train;
                        break;
                    }
                case RoleAttributeType.ROLE_BRAINS:
                    {
                        current = model.base_brains_train;
                        break;
                    }
                case RoleAttributeType.ROLE_CHARM:
                    {
                        current = model.base_charm_train;
                        break;
                    }
                case RoleAttributeType.ROLE_GOVERN:
                    {
                        current = model.base_govern_train;
                        break;
                    }
                #endregion
            }

            var c_s = tg_role.GetSingleTrainMax(type, model);   //单个属性锻炼最大值
            var s = c_s - current;                      //剩余单个锻炼值
            if (s <= 0) return 0;

            if (c_t >= s)
                return s;
            var c = s - c_t;
            //c大于等于0表示可以锻炼否则达到单个最大值
            return c >= 0 ? c_t : c;
        }

        public void RoleTrainPush(Int64 user_id, Int64 rid)
        {
            //var role = new RoleItem();
            var role = tg_role.GetEntityById(rid);
            if (role == null) return;
            var user = tg_user.GetUsersById(user_id);
            if (user == null) return;

            var train_role = tg_train_role.GetEntityByRid(role.id);
            if (train_role == null || train_role.state != (int)RoleTrainStatusType.TRAINING || train_role.time == 0) return;
            var basetrain = Variable.BASE_TRAIN.FirstOrDefault(m => m.id == train_role.type);
            train_role.state = (int)RoleTrainStatusType.FREE;
            train_role.time = 0;
            var att = GetCanAtt(user, role, train_role.attribute, basetrain, train_role.count);
            //var att2 = tg_role.GetSingleCanTrain((RoleAttributeType)train_role.attribute, att, role);
            var att2 = GetSingleCanTrain((RoleAttributeType)train_role.attribute, att, role);
            if (att2 > 0)
            {
                role = RoleAtt(role, train_role, att2);
                tg_role.GetRoleUpdate(role);
            }
            tg_train_role.GetUpdate(train_role);

            if (!Variable.OnlinePlayer.ContainsKey(user_id)) return;
            //向在线玩家推送数据
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return;
            if (role.id == session.Player.Role.Kind.id)
            {
                session.Player.Role.Kind = role;
            }
            var dic = new Dictionary<string, object> { { "role", (new Share.Role()).BuildRole(rid) } };
            TrainRolePush(session, new ASObject(dic));
        }

        /// <summary>发送修行结束协议</summary>
        private static void TrainRolePush(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "TRAIN_ROLE_PUSH", "武将修行结束协议发送");
#endif
            var pv = session.InitProtocol((int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_ROLE_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        #endregion
    }
}
