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
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using XCode;

namespace TGG.Share
{
    /// <summary>
    /// 升级方法
    /// </summary>
    public class Upgrade : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        #region  公共方法
        /// <summary> 主角武将升级 </summary> 
        public bool UserLvUpdate(Int64 userid, int count, tg_role roleinfo, int modulenumber, int command)
        {
            try
            {
                if (roleinfo == null || count <= 0) return false;

                var oldlevel = roleinfo.role_level; var oldlife = roleinfo.att_life;
                var oldexp = roleinfo.role_exp; var oldpoint = roleinfo.att_points;
                roleinfo.total_exp = tg_user.IsExpMax(roleinfo.total_exp, count);  //总经验增加
                //XTrace.WriteLine("{0}:{1}", "武将当前等级:", roleinfo.role_level);
                if (!GetNewExpAndLevel(roleinfo)) return false;
                AddBlood(roleinfo, oldlevel);
                IsGetYin(roleinfo, oldlevel); //是否获取Yin
                VocationTaskPush(userid, roleinfo, oldlevel);
                CheckDaMing(userid, roleinfo, oldlevel);    //检测是否开启大名令
                AddPoint(roleinfo, roleinfo.role_level - oldlevel, oldlevel);
                MainTaskUpdate(roleinfo); //检测有没有开启主线任务
                WriteLog(count, oldlevel, oldexp, oldlife, oldpoint, roleinfo, modulenumber, command);

                tg_role.GetLevelExpUpdate(roleinfo.role_level, roleinfo.role_exp, roleinfo.id, roleinfo.att_life, roleinfo.att_points, roleinfo.total_exp);
                var strings = new List<String>() { "experience" };
                int type = 0;
                if (roleinfo.role_level > oldlevel)
                {
                    SceneInfoUpdate(userid, "level", roleinfo.role_level);//场景数据更新
                    strings.Add("level");
                    strings.Add("life");
                    type = 1;
                    (new ActivityOpenService()).LevelAdd(userid, roleinfo.role_level);
                    (new ActivityOpenService()).LevelPacketAdd(userid, roleinfo.role_level);
                    (new ActivityOpenService()).ActivityLevelAdd(userid, roleinfo.role_level);
                }
                UserUpdatePush(roleinfo, userid, strings, type, roleinfo.att_points); //向在线玩家发送协议
                return true;
            }
            catch (Exception)
            {
                XTrace.WriteLine("主角升级出错");
                return false;

            }

        }


        /// <summary> 其他武将升级 </summary> 
        public bool RoleLvUpdate(Int64 userid, int count, tg_role roleinfo, int modulenumber, int command)
        {
            try
            {
                if (roleinfo == null || count <= 0) return false;
                var oldlevel = roleinfo.role_level; var oldlife = roleinfo.att_life; var oldexp = roleinfo.role_exp;
                roleinfo.total_exp = tg_user.IsExpMax(roleinfo.total_exp, count);  //总经验增加

                var mainrole = GetMainRole(userid);
                if (mainrole == null) return false;
                if (!GetNewExpAndLevel(roleinfo, mainrole)) return false;
                AddBlood(roleinfo, oldlevel);
                tg_role.GetLevelExpUpdate(roleinfo.role_level, roleinfo.role_exp, roleinfo.id, roleinfo.att_life, roleinfo.total_exp);
                //写入日志
                WriteLog(count, oldlevel, oldexp, oldlife, roleinfo, modulenumber, command);

                var strings = new List<String>() { "experience" };
                if (roleinfo.role_level > oldlevel) { strings.Add("level"); strings.Add("life"); }
                SendRoinLevelUp(userid, roleinfo, strings); //向在线玩家发送协议
                return true;
            }
            catch (Exception)
            {
                XTrace.WriteLine("武将升级出错");
                return false;

            }


        }

        /// <summary>
        /// 验证其他武将增加经验在否超过主角
        /// </summary>
        /// <param name="role">武将实体</param>
        /// <param name="count">获得经验数</param>
        /// <returns></returns>
        public bool CheckIsFull(tg_role role, int count)
        {
            var newexp = role.total_exp + count;
            var mainrole = GetMainRole(role.user_id);
            if (mainrole == null) return false;

            return newexp <= mainrole.total_exp;
        }

        /// <summary>
        /// 验证其他武将增加功勋是否达到上限
        /// </summary>
        /// <param name="role">武将实体</param>
        /// <param name="count">获取功勋数</param>
        /// <param name="vocation">玩家职业</param>
        /// <returns></returns>
        public bool CheckHonorIsFull(tg_role role, int count, int vocation)
        {
            var newhonor = role.total_honor + count;
            var mainrole = GetMainRole(role.user_id);
            if (mainrole == null) return false;
            var mainroleindex = Variable.BASE_IDENTITY.FindIndex(q => q.id == mainrole.role_identity);
            var firstindex = Variable.BASE_IDENTITY.FindIndex(q => q.vocation == vocation);
            var lastindex = Variable.BASE_IDENTITY.FindLastIndex(q => q.vocation == vocation);
            if (mainroleindex == firstindex) return false;
            //return mainroleindex == lastindex || newhonor < Variable.BASE_IDENTITY[mainroleindex - 1].SumHonor;
            return mainroleindex == lastindex || newhonor < Variable.BASE_IDENTITY[mainroleindex].SumHonor;

        }

        /// <summary>武将身份更新 </summary>
        public void UserIdentifyUpdate(Int64 userid, int count, tg_role roleinfo, int vocation, int modulenumber, int command)
        {
            if (roleinfo == null || count <= 0) { return; }
            var oldidentify = roleinfo.role_identity; var oldhonor = roleinfo.role_honor;
            var oldsum = roleinfo.total_honor;
            roleinfo.total_honor += count;
            roleinfo.total_honor = tg_user.IsHonorMax(oldsum, count, roleinfo.role_state); //功勋上限限定

            if (!GetNewHonorAndIdentify(roleinfo, vocation, oldidentify)) return;

            if (oldidentify < roleinfo.role_identity && roleinfo.role_state == (int)RoleStateType.PROTAGONIST)
            {
                SetTaskInit(userid);
                SetWorkTaskSend(userid, roleinfo.role_identity);
                (new Share.ActivityOpenService()).IdentityAdd(userid, roleinfo.role_identity);
                (new Share.ActivityOpenService()).ActivityIdentityAdd(userid, roleinfo.role_identity);
            }
            //格式：武将主键id_基表id_获得功勋数|升级前身份_升级后|原始总功勋_升级后
            var logdata = string.Format("RI_{0}_{1}_{2}|{3}_{4}|{5}_{6}|{7}_{8}|",
                roleinfo.id, roleinfo.role_id, count, oldidentify, roleinfo.role_identity, oldhonor, roleinfo.role_honor, oldsum, roleinfo.total_honor);

            var tuple = GetName(modulenumber, command);
            new Share.Log().WriteLog(userid, (int)LogType.Update, (int)ModuleNumber.ROLE, 0, tuple.Item1,tuple.Item2, "功勋", (int)GoodsType.TYPE_HONOR, count, roleinfo.total_honor, logdata);

            SendRoleAddHonor(userid, roleinfo, roleinfo.role_identity > oldidentify);


        }
        #endregion

        #region 私有方法

        /// <summary> 向在线玩家发送武将经验增加或者升级协议</summary>
        private void SendRoinLevelUp(Int64 userid, tg_role roleinfo, IEnumerable<string> strings)
        {
            new RoleAttUpdate().RoleUpdatePush(roleinfo, userid, strings);
        }

        #region 主角升级任务处理
        /// <summary> 职业任务推送 </summary>
        private void VocationTaskPush(Int64 userid, tg_role role, int oldlevel)
        {
            if (role.role_state != (int)RoleStateType.PROTAGONIST) return;//主角验证
            var baseinfo = Variable.BASE_MODULEOPEN.FirstOrDefault(q => q.id == 19);//职业任务面板开启等级
            if (baseinfo == null && role.role_level != baseinfo.level) return;

            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            if (oldlevel >= baseinfo.level || role.role_level < baseinfo.level) return;
            var newtasks = tg_task.GetTaskQueryByType(userid, (int)TaskType.VOCATION_TASK);
            var aso = BuildVocationData(newtasks);
            var pv = session.InitProtocol((int)ModuleNumber.TASK,
                (int)TaskCommand.TASK_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>主线任务检测</summary>
        private void MainTaskUpdate(tg_role role)
        {
            try
            {
                if (role == null) return;
                if (!Variable.OnlinePlayer.ContainsKey(role.user_id)) return;
                var session = Variable.OnlinePlayer[role.user_id] as TGGSession;
                if (session == null || session.MainTask == null) return;
                var task = session.MainTask.CloneEntity(); //查询有没有未开放的任务
                if (task == null || task.task_state != (int)TaskStateType.TYPE_UNOPEN) return;
                var baseinfo = Variable.BASE_TASKMAIN.FirstOrDefault(q => q.id == task.task_id);
                if (baseinfo == null) return;
                if (baseinfo.openLevel > role.role_level) return; //任务开放条件验证
                task.task_state = (int)TaskStateType.TYPE_UNRECEIVED;
                task.Update();
                session.MainTask = task;
                var dic = new Dictionary<string, object>() { { "0", EntityToVo.ToTaskVo(task) } };
                var pv = session.InitProtocol((int)ModuleNumber.TASK,
                    (int)TaskCommand.TASK_PUSH, (int)ResponseType.TYPE_SUCCESS, new ASObject(dic));
                session.SendData(pv);
            }
            catch (Exception)
            {
                XTrace.WriteLine("任务出错");
                return;
            }

        }
        #endregion

        /// <summary>组装玩家场景信息改变的数据</summary>
        /// <param name="userid">用户id</param>
        /// <param name="change">改变信息的字符串，如"level"</param>
        /// <param name="value">改变后的值</param>
        private ASObject BuildData(Int64 userid, string change, int value)
        {
            var dic1 = new Dictionary<string, object>()
            {
                {"userId", userid},
                {"data", new ASObject(new Dictionary<string, object>()
                {
                    { change, value }
                })},
            };
            return new ASObject(dic1);
        }

        /// <summary>主角升级组装更新数据 </summary>
        public ASObject BuildUpdateRoleData(tg_role role, IEnumerable<string> name, int type, int addcount)
        {
            var dic = new Dictionary<string, object> { { "id", role.id } };
            var dic1 = new Dictionary<string, object>() { { "type", type }, { "growAddCount", addcount } };
            foreach (var item in name)
            {
                var _item = item.ToLower();

                switch (_item)
                {
                    #region
                    //体力
                    case "power": { dic.Add("power", tg_role.GetTotalPower(role)); break; }
                    //基础体力
                    case "rolepower": { dic.Add("rolePower", role.power); break; }
                    //经验
                    //case "experience": { dic.Add("experience", role.role_exp = role.role_level == 60 ? 10000 : role.role_exp); break; }
                    case "experience": { dic.Add("experience", role.role_exp =  role.role_exp); break; }
                    //身份
                    case "identityid": { dic.Add("identityId", role.role_identity); break; }
                    //功勋
                    case "honor": { dic.Add("honor", role.role_honor); break; }
                    //等级
                    case "level": { dic.Add("level", role.role_level); break; }
                    //流派
                    case "genre": { dic.Add("genre", role.role_genre); break; }
                    //忍者众
                    case "ninja": { dic.Add("ninja", role.role_ninja); break; }
                    //统率
                    case "captain": { dic.Add("captain", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CAPTAIN, role), 2)); break; }
                    //基础统率
                    case "captainbase": { dic.Add("captainBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CAPTAIN, role), 2)); break; }
                    //武力
                    case "force": { dic.Add("force", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_FORCE, role), 2)); break; }
                    //基础武力
                    case "forcebase": { dic.Add("forceBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_FORCE, role), 2)); break; }
                    //智谋
                    case "brains": { dic.Add("brains", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_BRAINS, role), 2)); break; }
                    //基础智谋
                    case "brainbase": { dic.Add("brainsBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_BRAINS, role), 2)); break; }
                    //魅力
                    case "charm": { dic.Add("charm", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, role), 2)); break; }
                    //基础魅力
                    case "charmbase": { dic.Add("charmBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_CHARM, role), 2)); break; }
                    //政务
                    case "govern": { dic.Add("govern", Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_GOVERN, role), 2)); break; }
                    //基础政务
                    case "governbase": { dic.Add("governBase", Math.Round(tg_role.GetSingleFixed(RoleAttributeType.ROLE_GOVERN, role), 2)); break; }
                    //攻击
                    case "attack": { dic.Add("attack", role.att_attack); break; }
                    //会心几率
                    case "critprobability": { dic.Add("critProbability", Math.Round(role.att_crit_probability, 2)); break; }
                    //会心效果
                    case "critaddition": { dic.Add("critAddition", Math.Round(role.att_crit_addition, 2)); break; }
                    //闪避几率
                    case "dodgeprobability": { dic.Add("dodgeProbability", Math.Round(role.att_dodge_probability, 2)); break; }
                    //奥义触发秘技
                    case "mysteryprobability": { dic.Add("mysteryProbability", Math.Round(role.att_mystery_probability, 2)); break; }
                    //血量
                    case "life": { dic.Add("life", role.att_life); break; }
                    default: { return new ASObject(); }
                    #endregion
                }
            }
            dic1.Add("data", new ASObject(dic));
            return new ASObject(dic1);
        }

        /// <summary> 推送协议 </summary>
        private void SendPv(TGGSession session, ASObject aso, int commandNumber, int mn, Int64 otheruserid)
        {
            var key = string.Format("{0}_{1}_{2}", mn, commandNumber, otheruserid);
            session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        }

        /// <summary>主角升级推送协议 </summary>
        private void UserUpdatePush(tg_role role, Int64 userid, IEnumerable<string> name, int type, int addcount)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            if (role.id == session.Player.Role.Kind.id)
                session.Player.Role.Kind = role;
            var aso = BuildUpdateRoleData(role, name, type, addcount);
            var pv = session.InitProtocol((int)ModuleNumber.ROLE, (int)RoleCommand.ROLE_UPDATE, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>
        /// 向在线玩家发送武将功勋增加或身份提升协议
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="roleinfo">武将信息</param>
        /// <param name="isup">身份是否提升</param>
        private void SendRoleAddHonor(Int64 userid, tg_role roleinfo, bool isup)
        {
            if (isup) SceneInfoUpdate(userid, "identityId", roleinfo.role_identity);

            if (!Variable.OnlinePlayer.ContainsKey(userid)) { return; }
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) { return; }
            if (roleinfo.role_state == (int)RoleStateType.PROTAGONIST) //主角
            {
                session.Player.Role.Kind = roleinfo;
            }
            if (isup)
            {
                new RoleAttUpdate().RoleUpdatePush(roleinfo, userid, new List<String>() { "identityId", "honor" });
            }
            else
            {
                new RoleAttUpdate().RoleUpdatePush(roleinfo, userid, new List<String>() { "honor" });
            }
        }

        /// <summary> 主角增加的可分配属性点 </summary>
        private void AddPoint(tg_role role, int level, int oldlevle)
        {
            if (level == 0) return;
            var baseRule1 = Variable.BASE_RULE.FirstOrDefault(q => q.id == "7014"); //加点上限
            if (baseRule1 == null) return;
            var top = Convert.ToInt32(baseRule1.value);

            if (oldlevle > top) return;
            //例如60上限 58-61
            if (role.role_level > top) level = top - oldlevle + 1;
            //例如60上限 60-61
            if (oldlevle == top) level = 1;
            var baseRule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "7004");
            if (baseRule == null) return;
            var basepoint = Convert.ToInt32(baseRule.value);
            role.att_points += (basepoint * level);//升级增加可分配属性点
        }

        /// <summary>
        /// 玩家场景信息改变推送
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="change">改变的场景vo字段如"level"</param>
        /// <param name="value">改变后的值</param>
        private void SceneInfoUpdate(Int64 userid, string change, int value)
        {
            var mykey = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, userid);
            if (!Variable.SCENCE.ContainsKey(mykey)) return;
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;

            if (change == "level") Variable.SCENCE[mykey].role_level = value;
            if (change == "identityId") Variable.SCENCE[mykey].role_identity = value;

            var aso = BuildData(userid, change, value);
            var others = Core.Common.Scene.GetOtherPlayers(userid, Variable.SCENCE[mykey].scene_id, (int)ModuleNumber.SCENE);
            foreach (var other in others)
            {
                var otherid = other.user_id;
                var tokenTest = new CancellationTokenSource();
                var temp = other.CloneEntity();
                Task.Factory.StartNew(m =>
                {
                    SendPv(session, aso, (int)SceneCommand.PUSH_PLAYER_VO, (int)ModuleNumber.SCENE, otherid);
                    tokenTest.Cancel();
                }, temp, tokenTest.Token);
            }
        }

        #region Yin操作

        /// <summary> 验证玩家等级是否达到 是否获取印 </summary>
        /// <param name="role">武将信息</param>
        private void IsGetYin(tg_role role, int oldlevel)
        {

            if (role.role_state != (int)RoleStateType.PROTAGONIST) return;//主角
            if (role.role_level == oldlevel) return;
            var temp = Variable.BASE_YIN.FirstOrDefault(m => m.level == role.role_level);
            if (temp == null) return;
            InsertYin(temp, role.user_id);
            //XTrace.WriteLine("{0}:{1}", "武将等级:", role.role_level);
            //var list = Variable.BASE_YIN.Where(m => m.level > oldlevel && m.level <= role.role_level).ToList();
            //if (!list.Any()) return;
            //InsertYin(list, role.user_id);
        }

        /// <summary> 插入印数据 </summary>
        /// <param name="listbase">>yin基表集合</param>
        /// <param name="userid">用户Id</param>
        private void InsertYin(List<BaseYin> listbase, Int64 userid)
        {
            var entitylist = new EntityList<tg_fight_yin>();
            var list = listbase.Select(model => new tg_fight_yin()
            {
                state = (int)YinStateType.UNUSED,
                user_id = userid,
                yin_id = model.id,
                yin_level = 0
            }).ToList();
            entitylist.AddRange(list);
            entitylist.Insert();
        }

        /// <summary> 插入印数据 </summary>
        /// <param name="listbase">>yin基表</param>
        /// <param name="userid">用户Id</param>
        private void InsertYin(BaseYin byin, Int64 userid)
        {
            var entitylist = new EntityList<tg_fight_yin>();
            var entity = new tg_fight_yin()
            {
                state = (int)YinStateType.UNUSED,
                user_id = userid,
                yin_id = byin.id,
                yin_level = 0
            };
            //var count = 
            entity.Insert();
            //XTrace.WriteLine("{0}:{1}", "插入yin数量:", count);
        }
        #endregion

        /// <summary>组装职业任务数据</summary>
        private ASObject BuildVocationData(List<tg_task> newtasks)
        {
            dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Task", "Common"); //反射得到vo
            var taskvo = obje.ConvertListASObject(newtasks, "VocationTask");
            var dic = new Dictionary<string, object> { { "1", taskvo } };
            var aso = new ASObject(dic);
            return aso;
        }

        /// <summary> 职业任务设置可重置 </summary>
        private void SetTaskInit(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var userextend = session.Player.UserExtend.CloneEntity();
            userextend.task_vocation_isgo = 1;
            userextend.Save();
            session.Player.UserExtend = userextend;
        }

        /// <summary>
        /// 身份提升，工作任务更新推送
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="identify"></param>
        private void SetWorkTaskSend(Int64 userid, int identify)
        {
            var newtasks = new List<tg_task>();
            var s_tasks = SpecialTasksInit(userid, identify);
            if (s_tasks != null)
                newtasks.AddRange(s_tasks);
            tg_task.GetListInsert(newtasks);
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var dic = new Dictionary<string, object>
                {
                    {"work",new TGTask().ConvertListASObject(newtasks)}
                };
            var aso = new ASObject(dic);
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            //推送数据
            if (session == null) return;
            var pv = session.InitProtocol((int)ModuleNumber.WORK,
                (int)WorkCommand.WORK_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>
        /// 初始新身份得到的工作任务
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="identify">身份值</param>
        /// <returns></returns>
        private IEnumerable<tg_task> SpecialTasksInit(Int64 userid, int identify)
        {
            var entitytasks = new List<tg_task>();

            var baseidentify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.id == identify);
            if (baseidentify == null) return null;
            var mytasks = Variable.BASE_TASKVOCATION.Where(q => q.type == 2 && q.identifyValue == baseidentify.value).ToList();
            if (!mytasks.Any()) return null;

            var clonelist = new List<BaseTaskVocation>();
            mytasks.ForEach((item) => clonelist.Add(item.CloneEntity()));
            lock (clonelist)
            {
                foreach (var item in clonelist)
                {
                    var _model = item.CloneEntity();
                    var step = _model.stepInit;
                    var steptype = _model.stepType;
                    var taskid = _model.id;
                    if (_model.stepType == (int)TaskStepType.FIGHTING_CONTINUOUS || _model.stepType == (int)TaskStepType.SEARCH_GOODS
                        || _model.stepType == (int)TaskStepType.ESCORT || _model.stepType == (int)TaskStepType.RUMOR
                        || _model.stepType == (int)TaskStepType.FIRE || _model.stepType == (int)TaskStepType.BREAK
                         || _model.stepType == (int)TaskStepType.SEll_WINE || _model.stepType == (int)TaskStepType.ASSASSINATION
                         || _model.stepType == (int)TaskStepType.GUARD || _model.stepType == (int)TaskStepType.ARREST_RUMOR
                         || _model.stepType == (int)TaskStepType.ARREST_FIRE || _model.stepType == (int)TaskStepType.ARREST_BREAK
                         || _model.stepType == (int)TaskStepType.ARREST_SEll_WINE || _model.stepType == (int)TaskStepType.STAND_GUARD)
                    {
                        var steptypelist = entitytasks.Select(q => q.task_step_type).ToList();
                        var basetask = RandomTask(steptypelist, mytasks, steptype);
                        if (basetask == null) continue;
                        step = basetask.stepInit;
                        steptype = basetask.stepType;
                        taskid = basetask.id;
                        var newone = new TGTask().BuildSpecialTask(step, steptype, taskid, userid, identify,
                            (int)TaskType.WORK_TASK);
                        entitytasks.Add(newone);
#if DEBUG
                        XTrace.WriteLine("id {0},step:{1}", newone.id, newone.task_step_data);
#endif
                        continue;
                    }
                    var newone1 = new TGTask().BuildSpecialTask(step, steptype, taskid, userid, identify,
                        (int)TaskType.WORK_TASK);
#if DEBUG
                    XTrace.WriteLine("id {0},step:{1}", newone1.id, newone1.task_step_data);
#endif
                    entitytasks.Add(newone1);
                }
                clonelist.Clear();


            }


            return entitytasks;
        }

        /// <summary>
        ///基表 随机一个生成同类型任务（玩家已有类型则跳过）
        /// </summary>
        /// <param name="entitytasks">已经拥有的任务</param>
        /// <param name="basetasks">基表筛选出来的任务数据</param>
        /// <param name="steptype">任务步骤类型</param>
        public BaseTaskVocation RandomTask(IEnumerable<int> entitytasks, IEnumerable<BaseTaskVocation> basetasks, int steptype)
        {
            if (entitytasks.Any(q => q == steptype)) return null;//已经有该类型任务
            var s_task = basetasks.Where(q => q.stepType == steptype).ToList();
            if (!s_task.Any()) return null;
            var indextask = RNG.Next(0, s_task.Count - 1);
            return s_task[indextask];

        }
        #endregion

        /// <summary>检验大名令是否开启</summary>
        private void CheckDaMing(Int64 userid, tg_role role, int oldlevel)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            if (role.role_state != (int)RoleStateType.PROTAGONIST) return;
            if (oldlevel >= 30 || role.role_level < 30) return;
            var player = session.Player.CloneEntity();
            player.UserExtend.dml = 1;
            player.UserExtend.Update();
            session.Player = player;
        }

        /// <summary>
        /// 获取其他武将新的等级和经验
        /// </summary>
        /// <param name="roleinfo"></param>
        /// <param name="mainrole"></param>
        /// <returns></returns>
        private bool GetNewExpAndLevel(tg_role roleinfo, tg_role mainrole)
        {
            var baselv = Variable.BASE_ROLELVUPDATE.LastOrDefault(q => q.SumExp <= roleinfo.total_exp);
            var lastelv = Variable.BASE_ROLELVUPDATE.LastOrDefault();
            if (baselv == null || lastelv == null) return false;

            if (roleinfo.total_exp > mainrole.total_exp) roleinfo.total_exp = mainrole.total_exp;
            var newlevel = baselv.level;

            var baseexp = roleinfo.total_exp - baselv.SumExp;
            if (baselv == lastelv && baselv.exp < baseexp) baseexp = baselv.exp;
            roleinfo.role_level = newlevel;
            roleinfo.role_exp = baseexp;

            return true;

        }

        /// <summary>
        /// 获取主角新的等级和经验
        /// </summary>
        private bool GetNewExpAndLevel(tg_role roleinfo)
        {
            var baselv = Variable.BASE_ROLELVUPDATE.LastOrDefault(q => q.SumExp <= roleinfo.total_exp);
            var lastelv = Variable.BASE_ROLELVUPDATE.LastOrDefault();
            if (baselv == null || lastelv == null) return false;
            var newlevel = baselv.level;

            var newexp = roleinfo.total_exp - baselv.SumExp;
            //最后一个等级经验处理
            if (baselv == lastelv && baselv.exp < newexp)
                newexp = baselv.exp;
            roleinfo.role_level = newlevel;
            roleinfo.role_exp = newexp;
            return true;

        }


        /// <summary>
        /// 获取玩家主将
        /// </summary>
        /// <param name="userid">用户id</param>
        private tg_role GetMainRole(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return null;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            return session == null ? null : session.Player.Role.Kind;
        }

        /// <summary>
        /// 升级以后增加的经验
        /// </summary>
        /// <param name="role">武将实体</param>
        /// <param name="oldlevel">升级前等级</param>
        private void AddBlood(tg_role role, int oldlevel)
        {
            if (oldlevel == role.role_level) return;
            var add = Variable.BASE_ROLELVUPDATE.Where(q => q.level >= oldlevel && q.level < role.role_level)
                    .Sum(q => q.AddBlood);
            role.att_life += add;

        }

        #region 写入日志
        /// <summary>
        /// 武将升级写入日志
        /// </summary>
        /// <param name="rid">武将主键ID</param>
        /// <param name="baseid">武将基表id</param>
        /// <param name="count">获得经验数</param>
        /// <param name="oldlevel">原来的等级</param>
        /// <param name="oldexp">原来的经验</param>
        /// <param name="oldlife">原来的血量</param>
        /// <param name="role">武将实体</param>
        private void WriteLog(int count, int oldlevel, int oldexp, int oldlife, tg_role role, int modulenumber, int command)
        {
            try
            {
                //格式：武将主键id_基表id_获得经验数|升级前等级_升级后|原始经验_升级后经验|血量_升级后血量
                var logdata = string.Format("RL_{0}_{1}_{2}|{3}_{4}|{5}_{6}|{7}_{8}",
                   role.id, role.role_id, count, oldlevel, role.role_level, oldexp, role.role_exp, oldlife, role.att_life);

                var tuple = GetName(modulenumber, command);
                new Share.Log().WriteLog(role.user_id, (int)LogType.Update, modulenumber, command, tuple.Item1, tuple.Item2, "武将经验", (int)GoodsType.TYPE_EXP, count, role.total_exp, logdata);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary>
        /// 主角升级写入日志
        /// </summary>
        private void WriteLog(int count, int oldlevel, int oldexp, int oldlife, int oldpoint, tg_role roleinfo, int modulenumber, int command)
        {
            try
            {
                //格式：武将主键id_基表id_获得经验数|升级前等级_升级后|原始经验_升级后经验|血量_升级后血量|附加属性点_升级后
                var logdata = string.Format("L_{0}_{1}_{2}|{3}_{4}|{5}_{6}|{7}_{8}|{9}_{10}",
                    roleinfo.id, roleinfo.role_id, count, oldlevel, roleinfo.role_level, oldexp, roleinfo.role_exp, oldlife,
                    roleinfo.att_life, oldpoint, roleinfo.att_points);

                var tuple = GetName(modulenumber, command);
                new Share.Log().WriteLog(roleinfo.user_id, (int)LogType.Update, modulenumber, command, tuple.Item1, tuple.Item2, "主角经验", (int)GoodsType.TYPE_EXP, count, roleinfo.total_exp, logdata);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public Tuple<string, string> GetName(int mn, int cn)
        {
            string m = "";
            string n = "";
            switch (mn)
            {
                #region 任务
                case (int)ModuleNumber.TASK:
                    {
                        m = "任务";
                        switch (cn)
                        {
                            case (int)TaskCommand.TASK_VOCATION_UPDATE: n = "评定任务更新"; break;
                            case (int)TaskCommand.TASK_PUSH:
                            case (int)TaskCommand.TASK_FINISH:
                                n = "完成任务"; break;
                        }
                    }
                    break;
                #endregion
                #region 背包
                case (int)ModuleNumber.BAG:
                    {
                        m = "背包";
                        switch (cn)
                        {
                            case (int)PropCommand.PROP_USE: n = "背包使用"; break;
                            case (int)PropCommand.PROP_RONGLIAN_USE: n = "经验或功勋道具使用"; break;
                        }
                    }
                    break;
                #endregion
                #region 邮箱
                case (int)ModuleNumber.MESSAGES:
                    {
                        m = "邮件";
                        switch (cn)
                        {
                            case (int)MessageCommand.MESSAGE_ATTACHMENT: n = "提取邮件"; break;
                        }
                    }
                    break;
                #endregion
                #region 武将宅
                case (int)ModuleNumber.ROLETRAIN:
                    {
                        m = "武将宅";
                        switch (cn)
                        {
                            case (int)RoleTrainCommand.TRAIN_HOME_NPC_FIGHT: n = "挑战"; break;
                        }
                    }
                    break;
                #endregion
                #region 工作
                case (int)ModuleNumber.WORK:
                    {
                        m = "工作";
                        switch (cn)
                        {
                            case (int)WorkCommand.WORK_UPDATE: n = "工作更新"; break;
                        }
                    }
                    break;
                #endregion
                #region 大名令
                case (int)ModuleNumber.GUIDE:
                    {
                        m = "大名令";
                        switch (cn)
                        {
                            case (int)GuideCommand.REWARD: n = "领取奖励"; break;
                        }
                    }
                    break;
                #endregion
                #region 游艺园
                case (int)ModuleNumber.GAMES:
                    {
                        m = "游艺园";
                        switch (cn)
                        {
                            case (int)GameCommand.GAMES_RECEIVE: n = "每日领取奖励"; break;
                        }
                    }
                    break;
                #endregion
                #region 家臣评定 
                case (int)ModuleNumber.APPRAISE:
                    {
                        m = "家臣评定";
                        switch (cn)
                        {
                            case (int)AppraiseCommand.PUSH_TASK_END: n = "评定结束领取奖励"; break;
                        }
                    }
                    break;
                #endregion
            }

            return Tuple.Create(m, n);

        }

        #endregion

        /// <summary>
        /// 获取新的身份和功勋值
        /// </summary>
        /// <param name="roleinfo">武将实体</param>
        /// <param name="vocation">职业</param>
        /// <param name="oldidentify">之前的身份值</param>
        private bool GetNewHonorAndIdentify(tg_role roleinfo, int vocation, int oldidentify)
        {
            var baseidentify = Variable.BASE_IDENTITY.LastOrDefault(q => q.SumHonor <= roleinfo.total_honor && q.vocation == vocation);
            var last = Variable.BASE_IDENTITY.LastOrDefault(q => q.vocation == vocation);
            if (baseidentify == null || last == null) return false;

            var newhonor = roleinfo.total_honor - baseidentify.SumHonor;
            //最后一个身份功勋处理
            if (baseidentify == last && baseidentify.honor < newhonor)
                newhonor = baseidentify.honor;

            roleinfo.role_identity = baseidentify.id;
            roleinfo.role_honor = newhonor;
            roleinfo.Update();
            //主角刚达到大名身份
            if (roleinfo.role_identity > oldidentify && roleinfo.role_identity == last.id && vocation != (int)VocationType.Roles)
                WarDataInit(roleinfo.user_id, roleinfo.id, roleinfo.role_id);
            return true;
        }

        /// <summary>
        /// 初始达到大名的数据
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rid">武将主键id</param>
        /// <param name="baseid">武将基表id</param>
        private void WarDataInit(Int64 userid, Int64 rid, Int32 baseid)
        {
            var baselist = Variable.BASE_WAR_AREA.Where(q => q.free == 1).ToList();
            var userlist = baselist.Select(q => new tg_war_user_area() { user_id = userid, base_id = q.id }).ToList();
            //插入用户地形表
            tg_war_user_area.GetListInsert(userlist);

            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == "32002");
            if (baseinfo == null) return;
            var freecount = Convert.ToInt32(baseinfo.value);
            var listdata = new List<tg_war_area>();
            for (int i = 0; i < freecount; i++)
                listdata.Add(new tg_war_area() { user_id = userid, location = i + 1 }.CloneEntity());

            //插入用户地形设定关联表
            tg_war_area.GetListInsert(listdata);
            //插入地形设定表
            var listset = listdata.Select(q => new tg_war_area_set() { area_id = q.id }).ToList();
            tg_war_area_set.GetListInsert(listset);
        }
    }
}
