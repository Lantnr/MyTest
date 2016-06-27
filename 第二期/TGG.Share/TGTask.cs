using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Randoms;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Share
{
    /// <summary>
    /// 任务
    /// </summary>
    public class TGTask
    {
        /// <summary>组装特殊任务实体</summary>
        /// <param name="baseid">任务id</param>
        /// <param name="userid">用户主键</param>
        /// <param name="identify">玩家身份</param>
        /// <param name="step">任务步骤初始值</param>
        /// <param name="steptype">任务步骤类型</param>
        /// <param name="tasktype">任务类型</param>
        public tg_task BuildSpecialTask(string step, int steptype, int baseid, Int64 userid, int identify, int tasktype)
        {
            var newtask = new tg_task
            {
                task_type = tasktype,
                task_id = baseid,
                task_state = (int)TaskStateType.TYPE_UNRECEIVED,
                task_base_identify = identify,
                user_id = userid,
                task_step_type = steptype,
                task_step_data = step,
                is_lock = 0,
                task_starttime = 0,
                task_endtime = 0,
                is_special = 1 //是否是特殊任务  0:不是 1：是
            };
            switch (steptype)
            {
                case (int)TaskStepType.BUSINESS: //买卖类
                    {
                        newtask.task_step_data = GetBusinessStepTask(userid);
                    }
                    break;
                case (int)TaskStepType.RAISE_COIN: //挣钱
                    {
                        newtask.task_step_data = GetRaiseCoin(identify);
                    }
                    break;
                case (int)TaskStepType.SEARCH_GOODS: //搜寻宝物
                    {
                        newtask.task_step_data = SearchTaskStep();
                    }
                    break;

            }

            return newtask;
        }

        /// <summary> 将dynamic对象转换成ASObject对象 </summary>
        public List<ASObject> ConvertListASObject(dynamic list)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model;
                model = EntityToVo.ToVocationTaskVo(item);
                list_aso.Add(Core.AMF.AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        /// <summary> 根据用户身份返回用户的新的职业任务 </summary>
        public List<tg_task> GetNewVocationTasks(int identity, long userid, int vocation)
        {
            try
            {
                var baseidentify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.vocation == vocation);
                if (baseidentify == null) return null;
                var mytask = new List<tg_task>();
                var baseTaskVocation = Variable.BASE_TASKVOCATION.LastOrDefault(q => q.vocation == vocation);
                if (baseTaskVocation != null && identity == baseTaskVocation.id) return mytask;//大名没有职业任务
                if (identity == baseidentify.id) //本职业第一个身份,3个职业任务
                {
                    var list = Variable.BASE_TASKVOCATION.FindAll(q => q.identity.Split(',').ToList().Contains(identity.ToString()))
                        .Where(q => q.type == 1).ToList();
                    if (!list.Any()) return mytask;
                    var leftcount = list.Count >= 3 ? 3 : list.Count;
                    var indexlist = RNG.Next(0, list.Count - 1, leftcount);
                    mytask.AddRange(indexlist.Select(item => BuildNewVocationTask(list[item].stepInit, list[item].id, userid, identity, list[item].stepType)));
                }
                else
                {
                    var list = Variable.BASE_TASKVOCATION.FindAll(q => q.identity.Split(',').ToList().Contains((identity - 1).ToString()))
                       .Where(q => q.type == 1).ToList();
                    if (!list.Any()) return mytask;
                    var leftcount = list.Count >= 3 ? 3 : list.Count;
                    var indexlist = RNG.Next(0, list.Count - 1, leftcount);//随机三个任务

                    mytask.AddRange(indexlist.Select(item => BuildNewVocationTask(list[item].stepInit, list[item].id, userid, identity - 1, list[item].stepType)));
                    var taskids = mytask.Select(q => q.task_id).ToList();
                    var list1 = Variable.BASE_TASKVOCATION.FindAll(q => q.identity.Split(',').ToList().Contains(identity.ToString()))
                        .Where(q => q.type == 1 && !taskids.Contains(q.id)).ToList();
                    leftcount = list1.Count >= 3 ? 3 : list1.Count;
                    indexlist = RNG.Next(0, list1.Count - 1, leftcount);
                    mytask.AddRange(indexlist.Select(item => BuildNewVocationTask(list1[item].stepInit, list1[item].id, userid, identity, list1[item].stepType)));
                }
                return mytask;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region  商人任务初始

        /// <summary>货物跑商步骤数据(买卖类型任务--（商人）)</summary>
        /// <param name="areas">玩家当前商圈集合</param>
        /// <returns>任务类型_货物出售城市_货物编号_完成数量_当前数量</returns>
        private string GetBusinessStepTask(Int64 user_id)
        {
            List<tg_user_area> areas = tg_user_area.GetEntityByUserId(user_id).ToList();
            if (!areas.Any()) areas = InitArea(user_id);
            //根据商圈获取一个任务目的地町
            var ba = areas.Select(m => m.area_id);

            var tings = Variable.BASE_TING.Where(m => ba.Contains(m.areaId)).ToList();

            //随机一个自己商圈的町
            var index = RNG.Next(0, tings.Count() - 1);
            var ting = tings[index];

            var list_all = new List<int>();

            foreach (var item in tings)
            {
                var l = SplitGoods(item.goods).Where(m => !list_all.Contains(m)).ToList();
                list_all.AddRange(l);
            }

            var target = SplitGoods(ting.goods);

            var list_random = list_all.Except(target).ToList();

            //随机一个自己商圈的非目的地町的货物
            index = RNG.Next(0, list_random.Count() - 1);
            var goods = list_random[index];

            //获取货物所在町
            var inting =
                (from item in tings where item.goods.Contains(goods.ToString()) select item.id).FirstOrDefault();

            //随机货物数量
            var r_n = Variable.BASE_RULE.FirstOrDefault(m => m.id == "2001");
            if (r_n == null) return null;
            var t = r_n.value.Split('-').Select(m => Convert.ToInt32(m)).ToList();
            var min = t[0];
            var max = t[1];
            var num = RNG.Next(min, max);

            var step_data = new TaskStepItem
            {
                step_type = (int)TaskStepType.BUSINESS,
                inting = inting,
                ting = ting.id,
                goods = goods,
                total = num,
                count = 0,
            };

            return step_data.GetBusinessStepData();
        }

        /// <summary>获取货物</summary>
        private List<int> SplitGoods(string str)
        {
            return str.Split(',').Select(m => Convert.ToInt32(m)).ToList();
        }

        /// <summary>初始化默认跑商商圈</summary>
        private List<tg_user_area> InitArea(Int64 user_id)
        {
            var _r = Variable.BASE_RULE.FirstOrDefault(q => q.id == "1012");
            if (_r == null) return null;
            var temp = _r.value;
            var list_area = temp.Split(',');
            var list = new List<tg_user_area>();
            foreach (var item in list_area)
            {
                var entity = new tg_user_area() { user_id = user_id, area_id = Convert.ToInt32(item) };
                entity.Save();
                list.Add(entity);
            }
            return list;
        }

        /// <summary>筹措资金任务步骤数据</summary>
        /// <param name="voc">职业</param>
        /// <param name="ide">身份</param>
        public string GetRaiseCoin(int ide)
        {
            //类型_资金总数量_当前资金数量

            //获取当前身份筹措金钱区间
            var r = Variable.BASE_IDENTITY.FirstOrDefault(m => m.id == ide);
            if (r == null || string.IsNullOrEmpty(r.raiseCoin)) return string.Empty;

            var temp = r.raiseCoin;

            if (temp.IsNullOrEmpty() || !temp.Contains("-"))
                return string.Format("{0}_{1}_{2}", (int)TaskStepType.RAISE_COIN, 100000, 0);

            var s = temp.Split('-').Select(m => Convert.ToInt32(m)).ToList();
            var min = s[0];
            var max = s[1];
            //随机生成筹措资金
            var num = RNG.Next(min, max) * 1000; //金钱单位(文)

            var step_data = new TaskStepItem
            {
                step_type = (int)TaskStepType.RAISE_COIN,
                total = num,
                count = 0,
            };

            return step_data.GetRaiseStepData();
        }

        #endregion

        #region  高级职业任务更新

        /// <summary>高级任务推送</summary>
        /// <param name="user_id">玩家编号</param>
        /// <param name="task">推送任务实体</param>
        public void AdvancedTaskPush(Int64 user_id, tg_task task)
        {
            var token = new CancellationTokenSource();
            Object obj = new TaskObject { user_id = user_id, task = task };
            System.Threading.Tasks.Task.Factory.StartNew(m =>
            {
                var entity = m as TaskObject;
                if (entity == null) return;
                if (!Variable.OnlinePlayer.ContainsKey(user_id)) return;
                var session = Variable.OnlinePlayer[entity.user_id] as TGGSession;
                if (session == null) return;
                var data = new ASObject(BulidData((int)ResultType.SUCCESS, entity.task));
                Push(session, data, (int)TaskCommand.TASK_PUSH_UPDATE);
            }, obj, token.Token);
        }

        private class TaskObject
        {
            public Int64 user_id { get; set; }
            public tg_task task { get; set; }
        }

        public void Push(TGGSession session, ASObject data, int commandNumber)
        {
            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = (int)ModuleNumber.TASK,
                commandNumber = commandNumber,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }

        /// <summary> 组装数据 </summary>
        private Dictionary<String, Object> BulidData(int result, tg_task newtask)
        {
            return new Dictionary<string, object>
            {
                {"result", result},
                {"taskVo", newtask == null ? null : EntityToVo.ToVocationTaskVo(newtask)}
            };
        }

        #endregion

        /// <summary> 搜寻宝物任务步骤初始字符串 </summary>
        private string SearchTaskStep()
        {
            var basenpcs = Variable.BASE_RULE.FirstOrDefault(q => q.id == "2011");
            if (basenpcs == null) return null;
            var npcs = basenpcs.value.Split(',').ToList();
            var indexlist = RNG.Next(0, npcs.Count - 1);
            return string.Format("{0}_{1}_{2}", (int)TaskStepType.SEARCH_GOODS, npcs[indexlist], "0");

        }

        /// <summary>数据解析</summary>
        public TaskStepItem Ananalyze(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;
            var step_item = new TaskStepItem();
            var temp = data.Split('_');
            var t = temp.Length;
            switch (t)
            {
                #region 筹措资金

                case 3:
                    {
                        //类型_资金总数量_当前资金数量
                        step_item.step_type = Convert.ToInt32(temp[0]);
                        step_item.total = Convert.ToInt32(temp[1]);
                        step_item.count = Convert.ToInt32(temp[2]);
                        break;
                    }

                #endregion

                #region 买卖型任务

                case 6:
                    {
                        //任务类型_货物町_货物出售城市_货物编号_完成数量_当前数量
                        step_item.step_type = Convert.ToInt32(temp[0]);
                        step_item.inting = Convert.ToInt32(temp[1]);
                        step_item.ting = Convert.ToInt32(temp[2]);
                        step_item.goods = Convert.ToInt32(temp[3]);
                        step_item.total = Convert.ToInt32(temp[4]);
                        step_item.count = Convert.ToInt32(temp[5]);
                        break;
                    }

                #endregion
            }
            return step_item;
        }

        /// <summary>
        /// 任务步骤数据
        /// </summary>
        public class TaskStepItem
        {
            /// <summary>任务步骤类型</summary>
            public int step_type { get; set; }

            /// <summary>货物町</summary>
            public int inting { get; set; }

            /// <summary>跑商目标町</summary>
            public int ting { get; set; }

            /// <summary>跑商任务货物</summary>
            public int goods { get; set; }

            /// <summary>任务完成数量</summary>
            public int total { get; set; }

            /// <summary>当前完成数量</summary>
            public int count { get; set; }

            /// <summary>买卖任务步骤数据</summary>
            public string GetBusinessStepData()
            {
                return string.Format("{0}_{1}_{2}_{3}_{4}_{5}", step_type, inting, ting, goods, total, count);
            }

            /// <summary>筹措资金任务步骤数据</summary>
            public string GetRaiseStepData()
            {
                return string.Format("{0}_{1}_{2}", step_type, total, count);
            }
        }

        #region  家臣评定任务

        /// <summary>
        /// 删除任务并推送
        /// </summary>
        /// <param name="rid">武将主键id</param>
        /// <param name="userid">用户id</param>
        public void TaskDelete(Int64 rid, Int64 userid)
        {
            var task = tg_task.GetRoleTaskByRoleId(userid, rid);
            if (task == null) return;
            task.Delete();
            SendPv(userid, task.id);
        }

        /// <summary>
        /// 发送协议
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="taskmain">任务主键id</param>
        private void SendPv(Int64 userid, Int64 taskmain)
        {
            var dic = new Dictionary<string, object> { { "id", taskmain }, };
            var aso = new ASObject(dic);
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            //推送数据
            if (session == null) return;
            var pv = session.InitProtocol((int)ModuleNumber.APPRAISE,
                (int)AppraiseCommand.PUSH_DELETE, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        #endregion

        #region 谣言类任务公共方法

        /// <summary> 根据任务概率判断任务是否成功 </summary>
        public bool IsTaskSuccess(RoleItem role, int type)
        {
            var baselist = Variable.BASE_TASK_PROB.Where(q => q.type == type).ToList();
            if (!baselist.Any()) return false;
            var first = baselist.First();
            var myvalue = GetLifeSkillType(role, first.skillOrAtt);
            var basedata = baselist.FirstOrDefault(q => q.value >= myvalue);

            var myvalue2 = GetLifeSkillType(role, first.skillOrAtt2);
            var basedata2 = baselist.FirstOrDefault(q => q.value2 >= myvalue2);

            if (basedata == null || basedata2 == null) return false;
            var tprob = basedata.prob + basedata2.prob2;      //总的概率值

            return new RandomSingle().IsTrue(tprob);      //任务结果
        }

        /// <summary>获得等级或属性值</summary>
        public int GetLifeSkillType(RoleItem role, string basedata)
        {
            var type1 = Convert.ToInt32(basedata.Split('_')[0]);  //0：技能 1：属性
            var type2 = Convert.ToInt32(basedata.Split('_')[1]);
            if (type1 == 0)
            {
                #region 技能
                switch (type2)
                {
                    case (int)LifeSkillType.ARTILLERY: return role.LifeSkill.sub_artillery_level;
                    case (int)LifeSkillType.ARCHER: return role.LifeSkill.sub_archer_level;
                    case (int)LifeSkillType.ASHIGARU: return role.LifeSkill.sub_ashigaru_level;
                    case (int)LifeSkillType.BUILD: return role.LifeSkill.sub_build_level;
                    case (int)LifeSkillType.CALCULATE: return role.LifeSkill.sub_calculate_level;
                    case (int)LifeSkillType.CRAFT: return role.LifeSkill.sub_craft_level;
                    case (int)LifeSkillType.ELOQUENCE: return role.LifeSkill.sub_eloquence_level;
                    case (int)LifeSkillType.EQUESTRIAN: return role.LifeSkill.sub_equestrian_level;
                    case (int)LifeSkillType.ETIQUETTE: return role.LifeSkill.sub_etiquette_level;
                    case (int)LifeSkillType.MARTIAL: return role.LifeSkill.sub_martial_level;
                    case (int)LifeSkillType.MEDICAL: return role.LifeSkill.sub_medical_level;
                    case (int)LifeSkillType.MINE: return role.LifeSkill.sub_mine_level;
                    case (int)LifeSkillType.NINJITSU: return role.LifeSkill.sub_ninjitsu_level;
                    case (int)LifeSkillType.RECLAIMED: return role.LifeSkill.sub_reclaimed_level;
                    case (int)LifeSkillType.TACTICAL: return role.LifeSkill.sub_tactical_level;
                    case (int)LifeSkillType.TEA: return role.LifeSkill.sub_tea_level;
                }
                #endregion
            }
            if (type1 != 1) return 0;

            #region 属性
            switch (type2)
            {
                case (int)RoleAttributeType.ROLE_CAPTAIN: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, role.Kind);
                case (int)RoleAttributeType.ROLE_FORCE: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, role.Kind);
                case (int)RoleAttributeType.ROLE_BRAINS: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role.Kind);
                case (int)RoleAttributeType.ROLE_CHARM: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role.Kind);
                case (int)RoleAttributeType.ROLE_GOVERN: return (int)tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role.Kind);
            }
            #endregion

            return 0;
        }

        #endregion


        #region  家臣评定任务
        /// <summary>随机获取新的任务</summary>
        public List<tg_task> RandomTask(Int64 userid)
        {
            var mytask = new List<tg_task>();
            var list = Variable.BASE_APPRAISE.ToList();
            var basedata = Variable.BASE_RULE.FirstOrDefault(q => q.id == "21001");
            if (basedata == null) return mytask;
            var basecount = Convert.ToInt32(basedata.value);
            if (list.Count < basecount) return mytask;
            var indexlist = RNG.Next(0, list.Count - 1, basecount);
            mytask.AddRange(indexlist.Select(item => BuildNewRoleTask(list[item].id, userid)));
            return mytask;
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


        #region 组装数据
        public Dictionary<String, Object> BuildData(object result, List<tg_task> list_task, int count, int hasReset)
        {
            var dic = new Dictionary<string, object>();
            var maintask = list_task.FirstOrDefault(q => q.task_type == (int)TaskType.MAIN_TASK
                && q.task_state != (int)TaskStateType.TYPE_FINISHED);
            var dailytask = list_task.Where(q => q.task_type == (int)TaskType.VOCATION_TASK).ToList();
            dic.Add("result", result);
            dic.Add("mainTask", maintask != null ? EntityToVo.ToTaskVo(maintask) : null);
            dic.Add("vocationTask", ConvertListASObject(dailytask, "VocationTask"));
            dic.Add("count", count);
            dic.Add("hasReset", hasReset);
            return dic;
        }

        public ASObject BuildVoctionData(object result, List<tg_task> list_task, int count)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"vocationTask",list_task!=null? ConvertListASObject(list_task, "VocationTask"):null},
                {"count", count}
            };
            return new ASObject(dic);
        }

        /// <summary> 将dynamic对象转换成ASObject对象 </summary>
        public List<ASObject> ConvertListASObject(dynamic list, string classname)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model;
                switch (classname)
                {
                    case "VocationTask": model = EntityToVo.ToVocationTaskVo(item); break;
                    default: model = null; break;
                }
                list_aso.Add(Core.AMF.AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        /// <summary> 组装新的任务 </summary>
        public tg_task BuildNewVocationTask(string step, int taskid, long userid, int identify, int stepType)
        {
            var newtask = new tg_task
            {
                task_type = (int)TaskType.VOCATION_TASK,
                task_id = taskid,
                task_state = (int)TaskStateType.TYPE_UNRECEIVED,
                task_step_data = step,
                task_base_identify = identify,
                user_id = userid,
                task_step_type = stepType,
            };
            return newtask;
        }
        #endregion

        /// <summary>
        /// 测试数据
        /// </summary>
        /// <returns></returns>
        public List<view_scene_user> TestOhters(Int64 userid, Int64 sceneid, int mn)
        {
            var mykey = string.Format("{0}_{1}", mn, userid);
            var otherscontains = string.Format("{0}_", mn);
            var keys = Variable.SCENCE.Keys.Where(q => q.Contains(otherscontains) && q != mykey).ToList();
            var list = (from item in keys where Variable.SCENCE[item].model_number == (int)ModuleNumber.SCENE && Variable.SCENCE[item].scene_id == sceneid select Variable.SCENCE[item]).ToList();

            for (int i = 10000; i < 10500; i++)
            {
                //var session = new TGG.SocketServer.TGGSession
                //{
                //    Player = new Player
                //    {
                //        User = new tg_user(),

                //    },
                //    Fight = new FightItem(),
                //    TaskItems = new List<TaskItem>(),
                //    LastActiveTime = new DateTime(),
                //    SPM = new ConcurrentDictionary<string, ASObject>(),


                //};
                //Variable.OnlinePlayer.AddOrUpdate(i, session, (k, v) => session);
                //session.Player.Scene = new view_scene_user()
                //{
                //    user_id = i,
                //};
                list.Add(new view_scene_user()
                {
                    user_id = i,
                });
            }
            return list;
        }


        /// <summary>
        /// 任务步骤更新
        /// </summary>
        /// <param name="basestep">任务完成的条件，例如1_200001_1|1_200002_1</param>
        /// <param name="mystep">任务完成度,例如1_200001_0|1_200002_0，表示两步任务都未完成</param>
        /// <param name="npc">需要验证的npc的id,例如200001</param>
        /// <param name="userid">用户id</param>
        /// <returns>任务进度更新后的值，如例子中更新后为1_200001_1|1_200002_0</returns>
        public Tuple<int, string> CheckTaskStep(string basestep, string mystep, Int64 userid, int npc)
        {
            var newstep = "";
            var step = new TaskStep();
            var mysteplist = step.GetStepData(mystep);
            var basesteplist = step.GetStepData(basestep);
            if (mysteplist.Count != basesteplist.Count) return Tuple.Create((int)ResultType.TASK_STEP_WRONG, "");
            var onestep = mysteplist.Except(basesteplist, new TaskStepListEquality()).FirstOrDefault();  //找出需要更新的任务步骤
            var baseonestep = basesteplist.Except(mysteplist, new TaskStepListEquality()).FirstOrDefault();
            var i = mysteplist.IndexOf(onestep);
            if (onestep == null || baseonestep == null) return Tuple.Create((int)ResultType.TASK_STEP_WRONG, "");
            var tasktype = onestep.Type;
            switch (tasktype)
            {
                #region 任务类型
                case (int)TaskStepType.TYPE_DIALOG: //对话类型的任务
                    {
                        if (onestep.BaseId != npc) return Tuple.Create((int)ResultType.TASK_NPC_FALSE, "");
                        if (i + 1 > mysteplist.Count - 1 || mysteplist[i + 1].Type != (int)TaskStepType.NPC_FIGHT_TIMES)  //下一步任务步骤不是战斗
                        {
                            mysteplist[i].FinishValue = basesteplist[i].FinishValue;
                        }
                        else
                        {
                            var fightresult = FightStart(userid, mysteplist[i + 1].BaseId, FightType.NPC_FIGHT_ARMY);
                            //添加到session，并开始战斗
                            if (fightresult < 0) return Tuple.Create(fightresult, ""); //战斗错误

                            if (mysteplist[i + 1].FinishValue < basesteplist[i + 1].FinishValue) //战斗次数更新
                                mysteplist[i + 1].FinishValue++;
                            if (fightresult == (int)FightResultType.WIN && mysteplist[i + 1].FinishValue1 < basesteplist[i + 1].FinishValue1) //战胜胜利次数更新
                                mysteplist[i + 1].FinishValue1++;

                            if (mysteplist[i + 1].FinishValue >= basesteplist[i + 1].FinishValue
                                && mysteplist[i + 1].FinishValue1 >= basesteplist[i + 1].FinishValue1) //战斗任务完成，对话任务更新
                                mysteplist[i].FinishValue = basesteplist[i].FinishValue;

                        }
                    }
                    break;
                case (int)TaskStepType.BUILD://监督筑城
                case (int)TaskStepType.CLICK: //点击任意玩家
                    {
                        if (onestep.BaseId != npc) return Tuple.Create((int)ResultType.TASK_NPC_FALSE, "");
                        if (onestep.FinishValue < baseonestep.FinishValue) //战斗次数更新
                        {
                            mysteplist[i].FinishValue++;
                        }
                    } break;

                #endregion
            }

            newstep = step.GetStepValue(mysteplist);
            return Tuple.Create((int)ResultType.SUCCESS, newstep);


        }

        /// <summary>
        /// 调用战斗
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="baseid">战斗部队id</param>
        /// <param name="fighttype">战斗类型</param>
        public int FightStart(Int64 userid, int baseid, FightType fighttype)
        {
            var fight = new Share.Fight.Fight().GeFight(userid, baseid, fighttype, 0, false, true);
            new Share.Fight.Fight().Dispose();
            return fight.Result != ResultType.SUCCESS
                ? Convert.ToInt32(fight.Result)
                : (fight.Iswin ? (int)FightResultType.WIN : (int)FightResultType.LOSE);

        }

        /// <summary>
        /// 任务步骤类
        /// </summary>
        public class TaskStep
        {

            /// <summary> 任务步骤类型 </summary>
            public int Type { get; set; }

            /// <summary> 基表id </summary>
            public Int32 BaseId { get; set; }

            /// <summary> 玩家完成值1 </summary>
            public Int32 FinishValue { get; set; }

            /// <summary>玩家完成值2 </summary>
            public Int32 FinishValue1 { get; set; }

            /// <summary>
            /// 对任务步骤进行初始
            /// </summary>
            /// <param name="step">任务步骤字符串</param>
            public List<TaskStep> GetStepData(string step)
            {
                if (!step.Contains('_')) return null;
                var steplist = step.Split('|');
                var mylist = new List<TaskStep>();
                foreach (var s in steplist)
                {
                    //if (!s.Contains('_')) return null;
                    var newone = new TaskStep();
                    var stepvalue = s.Split('_');
                    var i = Convert.ToInt32(stepvalue[0]);
                    newone.Type = i;
                    switch (i)
                    {
                        #region 任务类型处理

                        case (int)TaskStepType.ESCORT:
                            {
                                if (stepvalue.Count() != 2) return null;
                                newone.BaseId = Convert.ToInt32(stepvalue[1]);
                            }
                            break;
                        case (int)TaskStepType.CLICK:
                            {
                                if (stepvalue.Count() != 2) return null;
                                newone.FinishValue = Convert.ToInt32(stepvalue[1]);
                            }
                            break;
                        case (int)TaskStepType.BUILD: //监督筑城
                        case (int)TaskStepType.TYPE_DIALOG:
                            {
                                if (stepvalue.Count() != 3) return null;
                                newone.BaseId = Convert.ToInt32(stepvalue[1]);
                                newone.FinishValue = Convert.ToInt32(stepvalue[2]);
                            }
                            break;
                        case (int)TaskStepType.NPC_FIGHT_TIMES:
                            {
                                if (stepvalue.Count() != 4) return null;
                                newone.BaseId = Convert.ToInt32(stepvalue[1]);
                                newone.FinishValue = Convert.ToInt32(stepvalue[2]);
                                newone.FinishValue1 = Convert.ToInt32(stepvalue[3]);
                            }
                            break;

                        #endregion
                    }
                    mylist.Add(newone);
                }
                return mylist;

            }

            /// <summary>
            /// 任务步骤转换成字符串
            /// </summary>
            /// <param name="list"></param>
            /// <returns></returns>
            public string GetStepValue(List<TaskStep> list)
            {
                var newstep = "";
                foreach (var step in list)
                {
                    var type = step.Type;
                    switch (type)
                    {
                        case (int)TaskStepType.ESCORT:
                            { newstep += string.Format("{0}_{1}", type, step.BaseId); } break;
                        case (int)TaskStepType.CLICK:
                            { newstep += string.Format("{0}_{1}", type, step.FinishValue); } break;
                        case (Int16)TaskStepType.BUILD:
                        case (Int16)TaskStepType.TYPE_DIALOG:
                            { newstep += string.Format("{0}_{1}_{2}", type, step.BaseId, step.FinishValue); } break;
                        case (Int16)TaskStepType.NPC_FIGHT_TIMES:
                            { newstep += string.Format("{0}_{1}_{2}_{3}", type, step.BaseId, step.FinishValue, step.FinishValue1); } break;
                    }
                    newstep += "|";
                }

                return newstep.Remove(newstep.Length - 1);

            }

            /// <summary>
            /// 任务步骤插入
            /// </summary>
            /// <param name="value">步骤字符串 例如1_200001_1|3_9010001_1</param>
            /// <returns>处理后的字符串1_200001_0|3_9010001_0</returns>
            public string GetInsertValue(List<TaskStep> list)
            {
                var newstep = "";
                foreach (var step in list)
                {

                    var type = step.Type;
                    switch (type)
                    {
                        case (Int16)TaskStepType.TYPE_DIALOG:
                            { newstep += string.Format("{0}_{1}_{2}", type, step.BaseId, 0); } break;
                        case (Int16)TaskStepType.NPC_FIGHT_TIMES:
                            { newstep += string.Format("{0}_{1}_{2}_{3}", type, step.BaseId, 0, 0); } break;
                    }
                    newstep += "|";
                }

                return newstep.Remove(newstep.Length - 1);
            }
        }

        /// <summary>
        /// 比较任务任务步骤
        /// </summary>
        private class TaskStepListEquality : IEqualityComparer<TaskStep>
        {
            public bool Equals(TaskStep x, TaskStep y)
            {
                return x.BaseId == y.BaseId && x.Type == y.Type &&
                    x.FinishValue == y.FinishValue && x.FinishValue1 == y.FinishValue1;
            }

            public int GetHashCode(TaskStep obj)
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return obj.ToString().GetHashCode();
                }
            }
        }


        /// <summary>
        /// 主线任务更新
        /// </summary>
        /// <param name="type">任务类型</param>
        /// <param name="userid">用户id</param>
        /// <param name="baseid">基表id</param>
        /// <param name="count">完成度数值</param>
        public void MainTaskUpdate(TaskStepType type, Int64 userid, Int32 baseid, Int32 count)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var taskinfo = session.MainTask.CloneEntity();
            if (taskinfo == null) return;
            if (!taskinfo.task_step_data.StartsWith(string.Format("{0}_", (int)type))) return;
            var splitlist = taskinfo.task_step_data.Split('_').ToList();

            if (splitlist.Count >= 2 && baseid != Convert.ToInt32(splitlist[1])) return; //验证基表id
            if (splitlist.Count >= 3 && count < Convert.ToInt32(splitlist[2])) return;//验证完成值

            taskinfo.task_state = (int)TaskStateType.TYPE_REWARD;
            taskinfo.Update();
            session.MainTask = taskinfo;
            var dic = new Dictionary<string, object>() { { "0", EntityToVo.ToTaskVo(taskinfo) } };
            var pv = session.InitProtocol((int)ModuleNumber.TASK,
                (int)TaskCommand.TASK_PUSH, (int)ResponseType.TYPE_SUCCESS, new ASObject(dic));
            session.SendData(pv);

        }

        public void MainTaskUpdate(TaskStepType type, Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var taskinfo = session.MainTask.CloneEntity();
            if (taskinfo == null) return;
            if (!taskinfo.task_step_data.StartsWith(string.Format("{0}_", (int)type))) return;
            taskinfo.task_state = (int)TaskStateType.TYPE_REWARD;
            taskinfo.Update();
            session.MainTask = taskinfo;
            var dic = new Dictionary<string, object>() { { "0", EntityToVo.ToTaskVo(taskinfo) } };
            var pv = session.InitProtocol((int)ModuleNumber.TASK,
                (int)TaskCommand.TASK_PUSH, (int)ResponseType.TYPE_SUCCESS, new ASObject(dic));
            session.SendData(pv);
        }

    }
}
