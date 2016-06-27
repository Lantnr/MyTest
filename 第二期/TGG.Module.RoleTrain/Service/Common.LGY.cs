using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Role;
using TGG.SocketServer;
using Task = System.Threading.Tasks.Task;

namespace TGG.Module.RoleTrain.Service
{
    public partial class Common
    {
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

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
            };
            return dic;
        }

        /// <summary>组装数据</summary>
        public Dictionary<String, Object> BuilData(int result, int bar)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"bar", bar},
            };
            return dic;
        }

        #region 注释
        ///// <summary> 当前时间毫秒</summary>
        //public Int64 CurrentTime()
        //{
        //    // ReSharper disable once PossibleLossOfFraction
        //    return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        //}

        ///// <summary>创建武将修行线程</summary>
        //public void RoleTrainThreading(Int64 user_id, Int64 rid, Int64 costtime)
        //{
        //    #region
        //    //try
        //    //{
        //    //    var token = new CancellationTokenSource();
        //    //    var task = new Task(() => SpinWait.SpinUntil(() => false, (int)costtime), token.Token);
        //    //    task.Start();
        //    //    task.ContinueWith(m =>
        //    //    {
        //    //        XTrace.WriteLine("玩家名称{0}--玩家ID{1}", session.Player.User.player_name, session.Player.User.id);
        //    //        //if (Variable.OnlinePlayer.Contains(session.Player.User.id))
        //    //        if (Variable.OnlinePlayer.ContainsKey(session.Player.User.id))
        //    //        {
        //    //            //session = Variable.OnlinePlayer[session.Player.User.id] as TGGSession;
        //    //            session = Variable.OnlinePlayer[session.Player.User.id] as TGGSession;
        //    //        }
        //    //        TRAIN_ROLE_PUSH.GetInstance().CommandStart(session, roleitem);
        //    //        token.Cancel();
        //    //    }, token.Token);

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    XTrace.WriteException(ex);
        //    //}
        //    #endregion
        //    try
        //    {
        //        var token = new CancellationTokenSource();
        //        Object obj = new RoleTrianObject { user_id = user_id, rid = rid };
        //        Task.Factory.StartNew(m =>
        //        {
        //            var t = m as RoleTrianObject;
        //            if (t == null) return;
        //            var key = t.GetKey();
        //            Variable.CD.AddOrUpdate(key, false, (k, v) => false);
        //            SpinWait.SpinUntil(() =>
        //                {
        //                    var b = Convert.ToBoolean(Variable.CD[key]);
        //                    return b;
        //                }, Convert.ToInt32(costtime));
        //        }, obj, token.Token)
        //        .ContinueWith((m, n) =>
        //        {
        //            var lo = n as RoleTrianObject;
        //            if (lo == null) { token.Cancel(); return; }
        //            TRAIN_ROLE_PUSH.GetInstance().RoleTrainPush(lo.user_id, lo.rid);
        //            //移除全局变量
        //            var key = lo.GetKey();
        //            bool r;
        //            Variable.CD.TryRemove(key, out r);
        //            token.Cancel();
        //        }, obj, token.Token);
        //    }
        //    catch (Exception ex)
        //    {
        //        XTrace.WriteException(ex);
        //    }
        //}

        //public class RoleTrianObject
        //{
        //    public Int64 user_id { get; set; }
        //    public Int64 rid { get; set; }

        //    public String GetKey()
        //    {
        //        return string.Format("{0}_{1}_{2}", (int)CDType.RoleTrain, user_id, rid);
        //    }
        //}

        ///// <summary>反射武将公共方法</summary>
        //public RoleInfoVo RoleInfo(Int64 rid)
        //{
        //    return (new Share.Role()).BuildRole(rid);
        //}

        ///// <summary>向用户推送更新</summary>
        //public void RewardsToUser(TGGSession session, tg_user user, int type)
        //{
        //    session.Player.User = user;
        //    (new Share.User()).REWARDS_API(type, session.Player.User);
        //}

        ///// <summary>基础属性值转换对应战斗属性值</summary>
        //public tg_role RoleAtt(tg_role role, tg_train_role train_role, double att)
        //{
        //    var mn = (int)ModuleNumber.ROLETRAIN;
        //    var rc = (int)RoleTrainCommand.TRAIN_ROLE_PUSH;
        //    var type = train_role.attribute;
        //    switch (type)
        //    {
        //        #region
        //        case (int)RoleAttributeType.ROLE_CAPTAIN:
        //            {
        //                var captain = role.base_captain_train;
        //                role.base_captain_train += att;
        //                RoleAttrituteLog(role.user_id, type, att, captain, role.base_captain_train, mn, rc);
        //                break;
        //            }
        //        case (int)RoleAttributeType.ROLE_CHARM:
        //            {
        //                var charm = role.base_charm_train;
        //                role.base_charm_train += att;
        //                RoleAttrituteLog(role.user_id, type, att, charm, role.base_charm_train, mn, rc);
        //                break;
        //            }
        //        case (int)RoleAttributeType.ROLE_FORCE:
        //            {
        //                var force = role.base_force_train;
        //                role.base_force_train += att;
        //                RoleAttrituteLog(role.user_id, type, att, force, role.base_force_train, mn, rc);
        //                break;
        //            }
        //        case (int)RoleAttributeType.ROLE_GOVERN:
        //            {
        //                var govern = role.base_govern_train;
        //                role.base_govern_train += att;
        //                RoleAttrituteLog(role.user_id, type, att, govern, role.base_govern_train, mn, rc);
        //                break;
        //            }
        //        case (int)RoleAttributeType.ROLE_BRAINS:
        //            {
        //                var brains = role.base_brains_train;
        //                role.base_brains_train += att;
        //                RoleAttrituteLog(role.user_id, type, att, brains, role.base_brains_train, mn, rc);
        //                break;
        //            }
        //        #endregion
        //    }
        //    return role;
        //}

        ///// <summary>武将属性变动添加日志 </summary>
        ///// <param name="userid">玩家id</param>
        ///// <param name="type">属性类型</param>
        ///// <param name="add">增加的值</param>
        ///// <param name="ov">武将增加前的属性值</param>
        ///// <param name="nv">武将增加后的属性值</param>
        ///// <param name="ModuleNumber">模块号</param>
        ///// <param name="command">指令号</param>
        //private void RoleAttrituteLog(Int64 userid, int type, double add, double ov, double nv, int ModuleNumber, int command)
        //{
        //    string logdata = "";
        //    switch (type)
        //    {
        //        #region
        //        case (int)RoleAttributeType.ROLE_BRAINS:
        //            {
        //                logdata = string.Format("{0}_{1}_{2}_{3}", "RoleBrains", ov, add, nv);
        //                break;
        //            }
        //        case (int)RoleAttributeType.ROLE_CAPTAIN:
        //            {
        //                logdata = string.Format("{0}_{1}_{2}_{3}", "RoleCaptain", ov, add, nv);
        //                break;
        //            }
        //        case (int)RoleAttributeType.ROLE_CHARM:
        //            {
        //                logdata = string.Format("{0}_{1}_{2}_{3}", "RoleCharm", ov, add, nv);
        //                break;
        //            }
        //        case (int)RoleAttributeType.ROLE_FORCE:
        //            {
        //                logdata = string.Format("{0}_{1}_{2}_{3}", "RoleForce", ov, add, nv);
        //                break;
        //            }
        //        case (int)RoleAttributeType.ROLE_GOVERN:
        //            {
        //                logdata = string.Format("{0}_{1}_{2}_{3}", "RoleGovern", ov, add, nv);
        //                break;
        //            }
        //        #endregion
        //    }
        //    (new Share.Log()).WriteLog(userid, (int)LogType.Get, ModuleNumber, command, logdata);// (int)ModuleNumber.ROLETRAIN, (int)RoleTrainCommand.TRAIN_ROLE_LOCK, logdata);
        //}

        ///// <summary>获取可以锻炼的加成值</summary>
        //public double GetCanAtt(tg_user user, tg_role role, int attribute, BaseTrain basetrain)
        //{
        //    var base_rule = GetBaseRule(user, role.role_id);
        //    var role_att = GetTrainAtt(role, attribute);
        //    var att = GetAtt(role_att, basetrain.count, base_rule);
        //    return att;
        //}

        ///// <summary>获取上限值</summary>
        //public BaseRule GetBaseRule(tg_user user, int roleid)
        //{
        //    var baserule = new BaseRule();
        //    if (roleid == user.role_id)
        //    {
        //        baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7013");
        //    }
        //    else
        //    {
        //        baserule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "7019");
        //    }
        //    return baserule;
        //}

        ///// <summary>获取修行属性值</summary>
        //public double GetTrainAtt(tg_role role, int attribute)
        //{
        //    switch (attribute)
        //    {
        //        case (int)RoleAttributeType.ROLE_CAPTAIN:
        //            {
        //                return role.base_captain_train;
        //            }
        //        case (int)RoleAttributeType.ROLE_CHARM:
        //            {
        //                return role.base_charm_train;
        //            }
        //        case (int)RoleAttributeType.ROLE_FORCE:
        //            {
        //                return role.base_force_train;

        //            }
        //        case (int)RoleAttributeType.ROLE_GOVERN:
        //            {
        //                return role.base_govern_train;

        //            }
        //        case (int)RoleAttributeType.ROLE_BRAINS:
        //            {
        //                return role.base_brains_train;

        //            }
        //    }
        //    return 0;
        //}

        ///// <summary>
        ///// 获取加成值
        ///// </summary>
        ///// <param name="att">武将值</param>
        ///// <param name="value">加成值</param>
        ///// <param name="base_rule">上限值</param>
        //private double GetAtt(double att, int value, BaseRule base_rule)
        //{
        //    att = att + value;
        //    var _att = att - Convert.ToDouble(base_rule.value);
        //    if (!(_att > 0)) return value;

        //    var use = value - _att;
        //    return use < 0 ? 0 : use;
        //}
        #endregion

        /// <summary>武将修行重新加入线程</summary>
        //public void RoleTrainRecovery()
        //{

        //    var list_user = tg_user.FindAll().ToList();
        //    foreach (var user in list_user)
        //    {
        //        var time = CurrentMs() + 5000;//+5000毫秒预热
        //        var list = tg_role.GetFindAllByUserId(user.id);
        //        var ids = list.Select(m => m.id).ToList();
        //        //var roles = view_role.GetRoleById(ids);
        //        //var rids = string.Join(",", roles.ToList().Select(m => m.Kind.id).ToArray());

        //        var rids = string.Join(",", ids.ToList().Select(m => m).ToArray());
        //        var list_train = tg_train_role.GetEntityListByTime(time, rids);//对该武将修行完成未修改的修行信息进行修改
        //        if (list_train.Any())
        //        {
        //            foreach (var tr in list_train)
        //            {
        //                var role = list.FirstOrDefault(m => m.id == tr.rid);
        //                var basetrain = Variable.BASE_TRAIN.FirstOrDefault(m => m.id == tr.type);
        //                var att = GetCanAtt(user, role, tr.attribute, basetrain);
        //                var att2 = tg_role.GetSingleCanTrain((RoleAttributeType)tr.attribute, att, role);
        //                role = RoleAtt(role, tr, att2);
        //                tg_role.GetRoleUpdate(role);
        //            }
        //        }

        //        var train_role_list = tg_train_role.GetEntityListByState((int)RoleTrainStatusType.TRAINING, rids);
        //        if (!train_role_list.Any()) continue;
        //        foreach (var item in train_role_list)
        //        {
        //            var _time = item.time - CurrentMs();
        //            if (_time < 0) continue;
        //            var role = list.FirstOrDefault(m => m.id == item.rid);
        //            if (role == null) return;
        //            RoleTrainThreading(role.user_id, role.id, _time);
        //        }
        //    }
        //}

        #region
        /// <summary>武将修行重新加入线程</summary>
        public void RoleTrainRecovery()
        {
            var list_user = tg_user.FindAll().ToList();
            var list_role = tg_role.FindAll().ToList();
            if (!list_user.Any() || !list_role.Any()) return;
            var ids = list_role.Select(m => m.id).ToList();
            if (!ids.Any()) return;
            var rids = string.Join(",", ids.ToList().Select(m => m).ToArray());
            var time = CurrentMs() + 5000;//+5000毫秒预热
            var list_trains = tg_train_role.GetEntityListByTime(time, rids);//对该武将修行完成未修改的修行信息进行修改
            var trainings = tg_train_role.GetEntityListByState((int)RoleTrainStatusType.TRAINING, rids);
            foreach (var user in list_user)
            {
                var roles = list_role.Where(m => m.user_id == user.id).ToList();
                var _rids = roles.Select(m => m.id).ToList();
                foreach (var item in _rids)
                {
                    var tr = list_trains.FirstOrDefault(m => m.rid == item);
                    if (tr != null)
                    {
                        var role = roles.FirstOrDefault(m => m.id == tr.rid);
                        var basetrain = Variable.BASE_TRAIN.FirstOrDefault(m => m.id == tr.type);
                        var att = (new Share.RoleTrain()).GetCanAtt(user, role, tr.attribute, basetrain,tr.count);
                        var att2 = (new Share.RoleTrain()).GetSingleCanTrain((RoleAttributeType)tr.attribute, att, role);
                        if (att2 > 0)
                        {
                            role = (new Share.RoleTrain()).RoleAtt(role, tr, att2);
                            tg_role.GetRoleUpdate(role);
                        }
                    }
                }
                if (!trainings.Any()) continue;
                foreach (var item in _rids)
                {
                    var tr = trainings.FirstOrDefault(m => m.rid == item);
                    if (tr != null)
                    {
                        var _time = tr.time - CurrentMs();
                        if (_time < 0) continue;
                        var role = roles.FirstOrDefault(m => m.id == tr.rid);
                        if (role == null) continue;
                        (new Share.RoleTrain()).RoleTrainThreading(role.user_id, role.id, _time);
                    }
                }
            }
        }
        #endregion

        /// <summary>当前毫秒</summary>
        private Int64 CurrentMs()
        {
            // ReSharper disable once PossibleLossOfFraction
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }



    }
}
