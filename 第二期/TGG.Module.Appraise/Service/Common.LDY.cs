using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Appraise;
using TGG.Core.Vo.Task;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Appraise.Service
{
    public partial class Common
    {
        #region  实体转换
        public List<ASObject> ConvertListAsObject(dynamic list)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model = EntityToVo.ToRoleVo(item);
                list_aso.Add(Core.AMF.AMFConvert.ToASObject(model));
            }
            return list_aso;
        }
        #endregion

        /// <summary> 家臣任务每日刷新 </summary>
        public void RoleTaskReflash()
        {
            tg_task.GetRoleTaskDel(); //删除除了还在做的所有的家臣任务
            var newtasks = new List<tg_task>();
            var users = tg_user.FindAll();
            foreach (var user in users)
            {
                newtasks.AddRange(new Share.TGTask().RandomTask(user.id));
            }
            tg_task.GetListInsert(newtasks);
            #region 在线则推送数据
            foreach (var item in users)
            {
                if (Variable.OnlinePlayer.ContainsKey(item.id))
                {
                    var item1 = item;
                    var dic = new Dictionary<string, object>
                    {
                        {"task", ConvertListAsObject(newtasks.Where(q=>q.user_id==item1.id&&q.task_state==(int)TaskStateType.TYPE_UNRECEIVED))},
                        {"count",2},
                    };
                    var aso = new ASObject(dic);
                    var session = Variable.OnlinePlayer[item.id] as TGGSession;
                    //推送数据
                    if (session == null) continue;
                    var pv = session.InitProtocol((int)ModuleNumber.APPRAISE,
                        (int)AppraiseCommand.PUSH_TASK_REFRESH, (int)ResponseType.TYPE_SUCCESS, aso);
                    session.SendData(pv);
                }
            }
            #endregion

        }



        /// <summary> 组装新的家臣任务 </summary>
        public tg_task BuildNewRoleTask(int baseid, Int64 userid)
        {
            return new tg_task()
            {
                task_id = baseid,
                task_state = (int)TaskStateType.TYPE_UNRECEIVED,
                task_type = (int)TaskType.ROLE_TASK,
                user_id = userid,
            };

        }

        /// <summary> 开启新线程 </summary>
        public void NewTaskStart(decimal time, tg_task newtask)
        {
            try
            {
                if (time < 0) return;
                if (Variable.RoleTask.ContainsKey(newtask.id)) return;
                Variable.RoleTask.TryAdd(newtask.id, true);
                var token = new CancellationTokenSource();
                var task = Task.Factory.StartNew(() => SpinWait.SpinUntil(() => false, (int)time), token.Token);
                task.ContinueWith(m =>
                {
                    if (Variable.RoleTask.ContainsKey(newtask.id))
                    {
                        var a = true;
                        Variable.RoleTask.TryRemove(newtask.id, out a);
                    }
                    var taskcheck = tg_task.FindByid(newtask.id); //验证该任务是否还存在，如果武将已经被放逐，任务则不存在
                    if (taskcheck != null) PUSH_TASK_END.GetInstance().CommandStart(newtask);
                    token.Cancel();
                }, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary>领取家臣任务奖励</summary>
        public void GetReward(string reward, tg_role role)
        {
            if (reward == "") return;

            var rewardlist = reward.Split('|');
            foreach (var item in rewardlist)
            {
                var typelist = item.Split('_');
                var type = Convert.ToInt32(typelist[0]);
                switch (type)
                {

                    #region 功勋
                    case (int)GoodsType.TYPE_HONOR:
                        {
                            var count = Convert.ToInt32(typelist[1]);
                            new Upgrade().UserIdentifyUpdate(role.user_id, count, role, (int)VocationType.Roles);  //用户身份是否提升
                        }
                        break;
                    #endregion
                }
            }
        }

        /// <summary> 获取家臣任务奖励字符串 </summary>
        public string GetRewardString(BaseAppraise baseinfo, RoleItem role)
        {
            if (CheckRewardCondition(baseinfo.rewardCCondition, role))
                return baseinfo.rewardC;
            return CheckRewardCondition(baseinfo.rewardBCondition, role)
                ? baseinfo.rewardB : baseinfo.rewardA;
        }

        /// <summary> 验证奖励标准 </summary>
        public bool CheckRewardCondition(string rewardcondition, RoleItem role)
        {
            var list = rewardcondition.Split('|').ToList();
            foreach (var item in list)
            {
                var type1 = Convert.ToInt32(item.Split('_')[0]);//0：技能 1：属性
                var type2 = Convert.ToInt32(item.Split('_')[1]);
                var value = Convert.ToInt32(item.Split('_')[2]);
                if (type1 == 0)
                {
                    #region 技能验证
                    switch (type2)
                    {
                        case (int)LifeSkillType.ARTILLERY: //铁炮
                            {
                                if (role.LifeSkill.sub_artillery_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.ARCHER: //弓术
                            {
                                if (role.LifeSkill.sub_archer_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.ASHIGARU: //足轻
                            {
                                if (role.LifeSkill.sub_ashigaru_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.BUILD:  //建筑
                            {
                                if (role.LifeSkill.sub_build_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.CALCULATE: //算术
                            {
                                if (role.LifeSkill.sub_calculate_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.CRAFT: //艺术
                            {
                                if (role.LifeSkill.sub_craft_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.ELOQUENCE: //辩才
                            {
                                if (role.LifeSkill.sub_eloquence_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.EQUESTRIAN: //马术
                            {
                                if (role.LifeSkill.sub_equestrian_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.ETIQUETTE: //礼法
                            {
                                if (role.LifeSkill.sub_etiquette_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.MARTIAL: //武艺
                            {
                                if (role.LifeSkill.sub_martial_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.MEDICAL: //医术
                            {
                                if (role.LifeSkill.sub_medical_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.MINE: //矿山
                            {
                                if (role.LifeSkill.sub_mine_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.NINJITSU: //忍术
                            {
                                if (role.LifeSkill.sub_ninjitsu_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.RECLAIMED: //开垦
                            {
                                if (role.LifeSkill.sub_reclaimed_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.TACTICAL: //军学
                            {
                                if (role.LifeSkill.sub_tactical_level < value)
                                    return false;
                            }
                            break;
                        case (int)LifeSkillType.TEA: //茶道
                            {
                                if (role.LifeSkill.sub_tea_level < value)
                                    return false;
                            }
                            break;
                    }
                    #endregion
                }
                if (type1 != 1) continue;
                #region 属性验证
                switch (type2)
                {
                    case (int)RoleAttributeType.ROLE_CAPTAIN: //统帅
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, role.Kind) < value)
                                return false;
                        }
                        break;
                    case (int)RoleAttributeType.ROLE_FORCE: //武力
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, role.Kind) < value)
                                return false;
                        }
                        break;
                    case (int)RoleAttributeType.ROLE_BRAINS: //智谋
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role.Kind) < value)
                                return false;
                        }
                        break;
                    case (int)RoleAttributeType.ROLE_CHARM: //魅力
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role.Kind) < value)
                                return false;
                        }
                        break;
                    case (int)RoleAttributeType.ROLE_GOVERN: //政务
                        {
                            if (tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role.Kind) < value)
                                return false;
                        }
                        break;
                }
                #endregion
            }
            return true;

        }

        /// <summary> 评定结束 </summary>
        public void AppraiseEnd(tg_task task)
        {
            var baseinfo = Variable.BASE_APPRAISE.FirstOrDefault(q => q.id == task.task_id);
            var roleitem = view_role.GetFindRoleById(task.rid);
            if (roleitem == null)
            {
                task.Delete(); return;
            }
            var reward = GetRewardString(baseinfo, roleitem);
#if DEBUG
            XTrace.WriteLine("{0}领取任务{1}奖励{2}", task.user_id, task.id, reward);
#endif
            GetReward(reward, roleitem.Kind);    //领取奖励
            task.Delete();
        }
    }
}
