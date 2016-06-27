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
using System.Transactions;

namespace TGG.Share
{
    /// <summary>
    /// 升级方法
    /// </summary>
    public class Upgrade
    {

        #region  公共方法
        /// <summary> 主角武将升级 </summary> 
        public bool UserLvUpdate(Int64 userid, int count, tg_role roleinfo)
        {
            var islevleup = 0;

            if (roleinfo == null || count <= 0)
            {
                return false;
            }
            var oldlevel = roleinfo.role_level; var oldlife = roleinfo.att_life;
            var oldexp = roleinfo.role_exp; var oldpoint = roleinfo.att_points;
            roleinfo.role_exp += count; //经验增加
            var baselv = Variable.BASE_ROLELVUPDATE.FirstOrDefault(q => q.level == roleinfo.role_level);
            var lastelv = Variable.BASE_ROLELVUPDATE.LastOrDefault();
            if (baselv == null || lastelv == null) return false;

            var baseexp = baselv.exp;
            while (baseexp <= roleinfo.role_exp)//等级提升过程
            {
                var baseinfo = Variable.BASE_ROLELVUPDATE.FirstOrDefault(q => q.level == roleinfo.role_level);
                if (baseinfo == null) { return false; }
                if (lastelv.level == baseinfo.level) { roleinfo.role_exp = (int)baseexp; break; }//等级满,不再升级，经验归为最大值
                roleinfo.att_life += baseinfo.AddBlood; //升级增加的生命
                roleinfo.role_level++; islevleup++;
                roleinfo.role_exp -= Convert.ToInt32(baseinfo.exp);
                var nextbase = Variable.BASE_ROLELVUPDATE.FirstOrDefault(q => q.level == roleinfo.role_level);
                if (nextbase == null) break;
                baseexp = nextbase.exp;
                IsGetYin(roleinfo); //是否获取Yin
                VocationTaskPush(userid, roleinfo);
                CheckDaMing(userid, roleinfo);    //检测是否开启大名令
            }
            AddPoint(roleinfo, islevleup, oldlevel);
            //检测有没有开启主线任务
            MainTaskUpdate(roleinfo);
            //格式：武将主键id_基表id_获得经验数|升级前等级_升级后|原始经验_升级后经验|血量_升级后血量|附加属性点_升级后
            var logdata = string.Format("L_{0}_{1}_{2}|{3}_{4}|{5}_{6}|{7}_{8}|{9}_{10}",
                roleinfo.id, roleinfo.role_id, count, oldlevel, roleinfo.role_level, oldexp, roleinfo.role_exp, oldlife, roleinfo.att_life, oldpoint, roleinfo.att_points);
            new Share.Log().WriteLog(userid, (int)LogType.Update, (int)ModuleNumber.ROLE, 0, logdata);
            if (islevleup > 0) //场景数据更新
                SceneInfoUpdate(userid, "level", roleinfo.role_level);
            tg_role.GetLevelExpUpdate(roleinfo.role_level, roleinfo.role_exp, roleinfo.id, roleinfo.att_life, roleinfo.att_points, roleinfo.total_exp);
            var strings = new List<String>() { "experience" };
            int type = 0;
            if (islevleup > 0)
            {
                strings.Add("level");
                strings.Add("life");
                type = 1;
            }
            UserUpdatePush(roleinfo, userid, strings, type, roleinfo.att_points); //向在线玩家发送协议
            return true;
        }

        /// <summary> 主角武将升级 </summary> 
        public bool UserLvUpdate1(Int64 userid, int count, tg_role roleinfo)
        {
            if (roleinfo == null || count <= 0)   return false;
            
            var oldlevel = roleinfo.role_level; var oldlife = roleinfo.att_life;
            var oldexp = roleinfo.role_exp; var oldpoint = roleinfo.att_points;
            roleinfo.total_exp += count; //经验增加
            GetNewExpAndLevel(roleinfo);

            IsGetYin(roleinfo); //是否获取Yin
            VocationTaskPush(userid, roleinfo);
            CheckDaMing(userid, roleinfo);    //检测是否开启大名令

           // AddPoint(roleinfo, islevleup, oldlevel);
            //检测有没有开启主线任务
            MainTaskUpdate(roleinfo);
            //格式：武将主键id_基表id_获得经验数|升级前等级_升级后|原始经验_升级后经验|血量_升级后血量|附加属性点_升级后
            var logdata = string.Format("L_{0}_{1}_{2}|{3}_{4}|{5}_{6}|{7}_{8}|{9}_{10}",
                roleinfo.id, roleinfo.role_id, count, oldlevel, roleinfo.role_level, oldexp, roleinfo.role_exp, oldlife, roleinfo.att_life, oldpoint, roleinfo.att_points);
            new Share.Log().WriteLog(userid, (int)LogType.Update, (int)ModuleNumber.ROLE, 0, logdata);
            if (roleinfo.role_level > oldlevel) //场景数据更新
                SceneInfoUpdate(userid, "level", roleinfo.role_level);
            tg_role.GetLevelExpUpdate(roleinfo.role_level, roleinfo.role_exp, roleinfo.id, roleinfo.att_life, roleinfo.att_points, roleinfo.total_exp);
            var strings = new List<String>() { "experience" };
            int type = 0;
            if (roleinfo.role_level > oldlevel) 
            {
                strings.Add("level");
                strings.Add("life");
                type = 1;
            }
            UserUpdatePush(roleinfo, userid, strings, type, roleinfo.att_points); //向在线玩家发送协议
            return true;
        }

        public bool CheckIsFull(int count, int rolemaincount)
        {


            return false;
        }

        /// <summary> 武将升级 </summary>
        public bool RoleLvUpdate(Int64 userid, int count, tg_role roleinfo)
        {
            var islevleup = 0;

            if (roleinfo == null || count <= 0)
            {
                return false;
            }
            var oldlevel = roleinfo.role_level; var oldlife = roleinfo.att_life;
            var oldexp = roleinfo.role_exp;
            roleinfo.role_exp += count; //经验增加
            var baselv = Variable.BASE_ROLELVUPDATE.FirstOrDefault(q => q.level == roleinfo.role_level);
            var lastelv = Variable.BASE_ROLELVUPDATE.LastOrDefault();
            if (baselv == null || lastelv == null) return false;
            var baseexp = baselv.exp;
            while (baseexp <= roleinfo.role_exp)//等级提升过程
            {
                var baseinfo = Variable.BASE_ROLELVUPDATE.FirstOrDefault(q => q.level == roleinfo.role_level);
                if (baseinfo == null) { return false; }
                if (lastelv.level == baseinfo.level) { roleinfo.role_exp = (int)baseexp; break; }//等级满,不再升级，经验归为最大值
                roleinfo.att_life += baseinfo.AddBlood; //升级增加的生命
                roleinfo.role_level++; islevleup++;
                roleinfo.role_exp -= Convert.ToInt32(baseinfo.exp);
                var nextbase = Variable.BASE_ROLELVUPDATE.FirstOrDefault(q => q.level == roleinfo.role_level);
                if (nextbase == null) break;
                baseexp = nextbase.exp;
            }
            tg_role.GetLevelExpUpdate(roleinfo.role_level, roleinfo.role_exp, roleinfo.id, roleinfo.att_life, roleinfo.total_exp);
            //格式：武将主键id_基表id_获得经验数|升级前等级_升级后|原始经验_升级后经验|血量_升级后血量
            var logdata = string.Format("RL_{0}_{1}_{2}|{3}_{4}|{5}_{6}|{7}_{8}",
                roleinfo.id, roleinfo.role_id, count, oldlevel, roleinfo.role_level, oldexp, roleinfo.role_exp, oldlife, roleinfo.att_life);
            new Share.Log().WriteLog(userid, (int)LogType.Update, (int)ModuleNumber.ROLE, 0, logdata);
            var strings = new List<String>() { "experience" };
            if (islevleup > 0)
            {
                strings.Add("level");
                strings.Add("life");
            }

            SendRoinLevelUp(userid, roleinfo, strings); //向在线玩家发送协议
            return true;

        }

        public bool RoleLvUpdate1(Int64 userid, int count, tg_role roleinfo)
        {
            if (roleinfo == null || count <= 0) return false;
            var oldlevel = roleinfo.role_level; var oldlife = roleinfo.att_life; var oldexp = roleinfo.role_exp;
            roleinfo.total_exp += count; //经验增加
            var mainrole = GetMainRole(userid);
            if (mainrole == null) return false;
            if (!GetNewExpAndLevel(roleinfo, mainrole)) return false;
            tg_role.GetLevelExpUpdate(roleinfo.role_level, roleinfo.role_exp, roleinfo.id, roleinfo.att_life, roleinfo.total_exp);
            //写入日志
            WriteLog(roleinfo.id, roleinfo.role_id, count, oldlevel, oldexp, oldlife, roleinfo);

            var strings = new List<String>() { "experience" };
            if (roleinfo.role_level > oldlevel) { strings.Add("level"); strings.Add("life"); }
            SendRoinLevelUp(userid, roleinfo, strings); //向在线玩家发送协议
            return true;

        }

        /// <summary>
        /// 获取新的等级和经验
        /// </summary>
        /// <param name="roleinfo"></param>
        /// <param name="mainrole"></param>
        /// <returns></returns>
        private bool GetNewExpAndLevel(tg_role roleinfo, tg_role mainrole)
        {
            var baselv = Variable.BASE_ROLELVUPDATE.LastOrDefault(q => q.SumExp <= roleinfo.total_exp);
            var lastelv = Variable.BASE_ROLELVUPDATE.LastOrDefault();
            if (baselv == null || lastelv == null) return false;

            var baseexp = roleinfo.total_exp - baselv.exp;
            if (baselv == lastelv && baselv.exp < baseexp) baseexp = baselv.exp;

            if (baselv.level > mainrole.role_level)//武将等级超过主角武将
            {
                roleinfo.role_level = mainrole.role_level;
                roleinfo.role_exp = mainrole.role_exp;
            }
            else if (baselv.level == mainrole.role_level) //与主角武将等级相同
            {
                roleinfo.role_level = baselv.level;
                if (baseexp > mainrole.role_exp) roleinfo.role_exp = mainrole.role_exp;
            }
            else
            {
                roleinfo.role_level = baselv.level;
                roleinfo.role_exp = baseexp;
            }
            return true;

        }

        private bool GetNewExpAndLevel(tg_role roleinfo)
        {
            var baselv = Variable.BASE_ROLELVUPDATE.LastOrDefault(q => q.SumExp <= roleinfo.total_exp);
            var lastelv = Variable.BASE_ROLELVUPDATE.LastOrDefault();
            if (baselv == null || lastelv == null) return false;

            var baseexp = roleinfo.total_exp - baselv.exp;
            if (baselv == lastelv && baselv.exp < baseexp) baseexp = baselv.exp;
            roleinfo.role_level = baselv.level;
            roleinfo.role_exp = baseexp;
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
        /// 武将升级写入日志
        /// </summary>
        /// <param name="rid">武将主键ID</param>
        /// <param name="baseid">武将基表id</param>
        /// <param name="count">获得经验数</param>
        /// <param name="oldlevel">原来的等级</param>
        /// <param name="oldexp">原来的经验</param>
        /// <param name="oldlife">原来的血量</param>
        /// <param name="role">武将实体</param>
        private void WriteLog(Int64 rid, int baseid, int count, int oldlevel, int oldexp, int oldlife, tg_role role)
        {
            //格式：武将主键id_基表id_获得经验数|升级前等级_升级后|原始经验_升级后经验|血量_升级后血量
            var logdata = string.Format("RL_{0}_{1}_{2}|{3}_{4}|{5}_{6}|{7}_{8}",
               rid, baseid, count, oldlevel, role.role_level, oldexp, role.role_exp, oldlife, role.att_life);
            new Share.Log().WriteLog(role.user_id, (int)LogType.Update, (int)ModuleNumber.ROLE, 0, logdata);

        }

        /// <summary>武将身份更新 </summary>
        public void UserIdentifyUpdate(Int64 userid, int count, tg_role roleinfo, int vocation)
        {
            using (var scope = new TransactionScope())
            {
                if (roleinfo == null || count <= 0) { scope.Complete(); return; }
                var oldident = roleinfo.role_identity; var oldhonor = roleinfo.role_honor;
                var totalhonor = roleinfo.total_honor;
                var isup = 0;
                roleinfo.role_honor = tg_user.IsHonorMax(roleinfo.role_honor, count); //功勋增加
                var lastidentity = Variable.BASE_IDENTITY.LastOrDefault(q => q.vocation == vocation);
                var baseidentify = Variable.BASE_IDENTITY.FirstOrDefault(q => q.id == roleinfo.role_identity);
                if (baseidentify == null || lastidentity == null) { scope.Complete(); return; }
                if (lastidentity.id == baseidentify.id) { scope.Complete(); return; }//最后一个身份不进行更新
                var basehonor = baseidentify.honor;

                while (basehonor <= roleinfo.role_honor)//身份提升过程
                {
                    var baseinfo = Variable.BASE_IDENTITY.FirstOrDefault(q => q.id == roleinfo.role_identity);
                    if (baseinfo == null) { scope.Complete(); return; }
                    if (baseinfo.id == lastidentity.id) { roleinfo.role_honor = basehonor; break; } //最后一个身份,功勋归为最大值
                    roleinfo.role_identity++; isup++;
                    if (roleinfo.role_state == (int)RoleStateType.PROTAGONIST) //主角
                    {
                        SetTaskInit(userid);
                        SetWorkTaskSend(userid, roleinfo.role_identity);
                    }
                    roleinfo.role_honor -= Convert.ToInt32(baseinfo.honor);
                    var basedata = Variable.BASE_IDENTITY.FirstOrDefault(q => q.id == roleinfo.role_identity);
                    if (basedata == null) break;
                    basehonor = basedata.honor;
                }
                roleinfo.total_honor += count;
                roleinfo.Update();
                //格式：武将主键id_基表id_获得功勋数|升级前身份_升级后|原始总功勋_升级后
                var logdata = string.Format("RI_{0}_{1}_{2}|{3}_{4}|{5}_{6}|{7}_{8}|",
                    roleinfo.id, roleinfo.role_id, count, oldident, roleinfo.role_identity, oldhonor, roleinfo.role_honor, totalhonor, roleinfo.total_honor);
                new Share.Log().WriteLog(userid, (int)LogType.Update, (int)ModuleNumber.ROLE, 0, logdata);
                SendRoleAddHonor(userid, roleinfo, isup);
                scope.Complete();
            }
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
        private void VocationTaskPush(Int64 userid, tg_role role)
        {
            if (role.role_state != (int)RoleStateType.PROTAGONIST) return;//主角验证
            var baseinfo = Variable.BASE_MODULEOPEN.FirstOrDefault(q => q.id == 19);//职业任务面板开启等级
            if (baseinfo != null && role.role_level != baseinfo.level) return;
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var newtasks = tg_task.GetTaskQueryByType(userid, (int)TaskType.VOCATION_TASK);
            var aso = BuildVocationData(newtasks);
            var pv = session.InitProtocol((int)ModuleNumber.TASK,
                (int)TaskCommand.TASK_PUSH, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary>主线任务检测</summary>
        private void MainTaskUpdate(tg_role role)
        {
            if (!Variable.OnlinePlayer.ContainsKey(role.user_id)) return;
            var session = Variable.OnlinePlayer[role.user_id] as TGGSession;
            if (session == null) return;
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
                    case "experience": { dic.Add("experience", role.role_exp); break; }
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
        /// <param name="isup">身份提升值</param>
        private void SendRoleAddHonor(Int64 userid, tg_role roleinfo, int isup)
        {
            if (isup > 0) SceneInfoUpdate(userid, "identityId", roleinfo.role_identity);

            if (!Variable.OnlinePlayer.ContainsKey(userid)) { return; }
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) { return; }
            if (roleinfo.role_state == (int)RoleStateType.PROTAGONIST) //主角
            {
                session.Player.Role.Kind = roleinfo;
            }
            if (isup > 0)
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
            if (role.role_level > top) level = top - oldlevle;
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
                Task.Factory.StartNew(m => SendPv(session, aso, (int)SceneCommand.PUSH_PLAYER_VO, (int)ModuleNumber.SCENE, otherid), temp, tokenTest.Token);
            }
        }

        #region Yin操作

        /// <summary> 验证玩家等级是否达到 是否获取印 </summary>
        /// <param name="role">武将信息</param>
        private void IsGetYin(tg_role role)
        {
            if (role.role_state != (int)RoleStateType.PROTAGONIST) return;//主角
            var model = Variable.BASE_YIN.FirstOrDefault(m => m.level == role.role_level);
            if (model == null) return;
            InsertYin(model, role.user_id);
        }

        /// <summary> 插入印数据 </summary>
        /// <param name="model">yin基表集合</param>
        /// <param name="userid">用户Id</param>
        private void InsertYin(BaseYin model, Int64 userid)
        {
            var yin = new tg_fight_yin()
            {
                state = (int)YinStateType.UNUSED,
                user_id = userid,
                yin_id = model.id,
                yin_level = 0
            };
            yin.Save();
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
        private void CheckDaMing(Int64 userid, tg_role role)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;

            if (role.role_state != (int)RoleStateType.PROTAGONIST || role.role_level < 30) return;
            var player = session.Player.CloneEntity();
            player.UserExtend.dml = 1;
            player.UserExtend.Update();
            session.Player = player;
        }
    }
}
