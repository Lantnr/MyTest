using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.SocketServer;

namespace TGG.Share
{
    public class Prison
    {
        #region 数据推送
        /// <summary>
        /// 向其他玩家推送A玩家进入出狱后的场景
        /// </summary>
        public void PushEnterScene(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var key = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, userid);
            if (!Variable.SCENCE.ContainsKey(key)) return;
            var userscene = Variable.SCENCE[key];
            if (userscene == null) return;
            var otherplays = Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);
            foreach (var item in otherplays)
            {
                var aso = new ASObject(BuildData((int)ResultType.SUCCESS, userscene));
                SendPv(aso, (int)SceneCommand.PLAYER_ENTER_SCENE, item.user_id, (int)ModuleNumber.SCENE);
            }

        }

        /// <summary>
        /// 推送玩家进入场景
        /// </summary>
        public void PushMyEnterScene(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var key = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, userid);
            if (!Variable.SCENCE.ContainsKey(key)) return;
            var userscene = Variable.SCENCE[key];
            if (userscene == null) return;
            userscene.model_number = (int)ModuleNumber.SCENE;  //模块号改为场景模块
            key = string.Format("{0}_{1}", (int)ModuleNumber.PRISON, userscene.user_id);
            var scene = new view_scene_user();
            if (Variable.SCENCE.ContainsKey(key))
                Variable.SCENCE.TryRemove(key, out scene);
            var otherplays = Core.Common.Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);
            var aso = new ASObject(BuildData((int)ResultType.SUCCESS, userscene.scene_id, userscene.X, userscene.Y, otherplays));
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            var pv = session.InitProtocol((int)ModuleNumber.SCENE,
                     (int)SceneCommand.PUSH_ENTER_SCENET, (int)ResponseType.TYPE_SUCCESS, aso);
            session.SendData(pv);
        }

        /// <summary> 推送玩家离开场景 </summary>
        public void PushMyLeaveScene(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var key = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, userid);
            if (!Variable.SCENCE.ContainsKey(key)) return;
            var userscene = Variable.SCENCE[key];
            if (userscene == null) return;
            userscene.model_number = (int)ModuleNumber.PRISON;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.Player.Scene.model_number = (int)ModuleNumber.PRISON;
            var userids = Variable.OnlinePlayer.Keys.Where(m => m != userid).ToList();
            if (!userids.Any()) return;
            var otherplays = Core.Common.Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);
            foreach (var item in otherplays)
            {
                var dic = new Dictionary<string, object>
                {
                    {"result", (int) ResultType.SUCCESS},
                    {"userId", userid}
                };
                var aso = new ASObject(dic);
                SendPv(aso, (int)SceneCommand.PLAYER_EXIT_SCENET, item.user_id, (int)ModuleNumber.SCENE);
            }
        }

        /// <summary>
        /// 向其他玩家推送A玩家进入监狱
        /// </summary>
        public void PushEnterPrison(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var myscene = Scene.GetSceneInfo((int)ModuleNumber.PRISON, userid);
            if (myscene == null) return;
            var otherplays = GetPrisonOhters(userid); //监狱内其他玩家
            if (otherplays == null) return;
            foreach (var item in otherplays)
            {
                var aso = new ASObject(BuildData((int)ResultType.SUCCESS, myscene));
                SendPv(aso, (int)PrisonCommand.PUSH_PLAYER_ENTER, item.user_id, (int)ModuleNumber.PRISON);
            }
        }


        /// <summary>
        /// 推送玩家进入监狱
        /// </summary>
        public void PushmyEnterPrison(Int64 userid, tg_prison prison)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            PushMyLeaveScene(userid);
            var key = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, userid);
            if (!Variable.SCENCE.ContainsKey(key)) return;
            var prisonscene = Variable.SCENCE[key].CloneEntity();
            GetPrisonPoint(prisonscene);
            key = string.Format("{0}_{1}", (int)ModuleNumber.PRISON, userid);
            Variable.SCENCE.AddOrUpdate(key, prisonscene, (m, n) => n);
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var doubledate = date.Ticks;
            var count = tg_prison_messages.GetUserMessageCount(doubledate, userid); //今日留言次数
            var list = GetPrisonOhters(userid);
            var aso = BuildData((int)ResultType.SUCCESS, Convert.ToDouble(prison.prison_time), count, list);
            SendPv(userid, aso, (int)PrisonCommand.PUSH_NETER, (int)ModuleNumber.PRISON);
        }

        /// <summary>
        /// 向监狱内玩家推送出狱
        /// </summary>
        public void PusuLeavePrison(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var otherplays = GetPrisonOhters(userid); //监狱内其他玩家
            var key = string.Format("{0}_{1}", (int)ModuleNumber.PRISON, userid);
            if (!Variable.SCENCE.ContainsKey(key)) return;
            var scene = new view_scene_user();
            Variable.SCENCE.TryRemove(key, out scene);

            foreach (var item in otherplays)
            {
                var dic = new Dictionary<string, object>()
                {
                    {"userId", userid}
                };
                var aso = new ASObject(dic);
                SendPv(aso, (int)PrisonCommand.PUSH_PLAYER_EXIT, item.user_id, (int)ModuleNumber.PRISON);
            }
        }

        /// <summary>
        /// 向其他玩家推送在监狱移动
        /// </summary>
        public void PusuMovingPrison(view_scene_user myscene)
        {
            var userid = myscene.user_id;
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var otherplays = GetPrisonOhters(userid);
            //监狱内其他玩家
            if (!otherplays.Any()) return;
            foreach (var item in otherplays)
            {
                var aso = new ASObject(BuildData((int)ResultType.SUCCESS, userid, myscene.X, myscene.Y));
                SendPv(aso, (int)PrisonCommand.PUSH_PLAYER_MOVING, item.user_id, (int)ModuleNumber.PRISON);
            }
        }

        #endregion

        #region 组装数据

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(object result, decimal sceneid, int x, int y, dynamic list_sceneplayers)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"id", sceneid},
                {"x", x},
                {"y", y},
                { "playerList", list_sceneplayers.Count > 0 ? ConvertListASObject(list_sceneplayers, "ScenePlayer") : null
                }
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(object result, decimal userid, int x, int y)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"userId", userid}, 
                {"x", x}, 
                {"y", y}
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(object result, view_scene_user sceneplayer)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"playerVo", sceneplayer != null ? EntityToVo.ToScenePlayerVo(sceneplayer)  : null}
            };
            return dic;
        }

        public ASObject BuildData(int result, double time, int count, List<view_scene_user> sceneplayer)
        {
            List<ASObject> player;
            if (sceneplayer == null) player = null;
            else
                player = sceneplayer.Any() ? ConvertListASObject(sceneplayer, "ScenePlayer") : null;
            var dic = new Dictionary<string, object>()
            {
                {"result", result},
                {"time",time},
                {"count",count},
                {"playerList",player }
            };
            return new ASObject(dic);
        }

        /// <summary>将dynamic对象转换成ASObject对象</summary>
        public List<ASObject> ConvertListASObject(dynamic list, string classname)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model;
                switch (classname)
                {
                    case "ScenePlayer": model = EntityToVo.ToScenePlayerVo(item); break;
                    default: model = null; break;
                }
                list_aso.Add(AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        #endregion

        #region 推送协议

        public void SendPv(ASObject aso, int commandnumber, Int64 otheruserid, int modulenumber)
        {
            if (!Variable.OnlinePlayer.ContainsKey(otheruserid)) return;
            var session = Variable.OnlinePlayer[otheruserid] as TGGSession;
            if (session == null) return;
            Send(session, aso, commandnumber, modulenumber);
        }

        public void Send(TGGSession session, ASObject data, int commandNumber, int modulenumber)
        {
            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = modulenumber,
                commandNumber = commandNumber,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }

        public void SendPv(Int64 userid, ASObject aso, int commandnumber, int modulenumber)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            Send(session, aso, commandnumber, modulenumber);
        }
        #endregion

        #region 公共方法
        /// <summary> 获取监狱坐标 </summary>
        public bool GetPrisonPoint(view_scene_user scene)
        {
            var baseinfo = Variable.BASE_RULE.FirstOrDefault(q => q.id == "25004");
            if (baseinfo == null) return false;
            var temp = baseinfo.value;
            if (!temp.Contains(',')) return false;
            var split = temp.Split(',');
            scene.X = Convert.ToInt32(split[0]);
            scene.Y = Convert.ToInt32(split[1]);
            return true;
        }

        /// <summary> 开启新线程 </summary>
        public void NewTaskStart(decimal time, Int64 userid)
        {
            try
            {
                if (Variable.PrisonTask.ContainsKey(userid)) return;
                Variable.PrisonTask.TryAdd(userid, true);
                var token = new CancellationTokenSource();
                var task = new System.Threading.Tasks.Task(() => SpinWait.SpinUntil(() => false, (int)time), token.Token);
                task.Start();
                task.ContinueWith(m =>
                {
                    if (Variable.PrisonTask.ContainsKey(userid))
                    {
                        var a = true;
                        Variable.PrisonTask.TryRemove(userid, out a);
                        PrisonOut(userid);
                    }
                    token.Cancel();
                }, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary> 玩家出狱 </summary>
        private void PrisonOut(Int64 userid)
        {
            var prison = tg_prison.GetPrisonByUserId(userid);
            if (prison == null) return;
            prison.Delete();
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            //推送玩家离开监狱
            PusuLeavePrison(userid);
            //推送玩家进入场景 
            PushMyEnterScene(userid);
            //向其他玩家推送玩家进入场景
            PushEnterScene(userid);
        }

        /// <summary>玩家进入监狱 </summary>
        public void PutInPrison(Int64 userid)
        {
            var prison = tg_prison.GetPrisonByUserId(userid);
            if (prison != null) return;
            var myprison = new tg_prison { user_id = userid };
            var user_extend = tg_user_extend.GetByUserId(userid);
            if (user_extend == null) return;
            if (!CheckEnterPrison(user_extend.steal_fail_count, userid)) return;
            var now = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            var baserule = Variable.BASE_RULE.FirstOrDefault(q => q.id == "25003"); //入狱时间
            if (baserule == null) return;
            var basetime = Convert.ToInt64(baserule.value);
            myprison.prison_time = basetime * 1000 + now;
            myprison.Insert();
            NewTaskStart(basetime * 1000, userid);
            if (user_extend.steal_fail_count >= 2)
            {
                user_extend.steal_fail_count = 0;
                user_extend.Update();
            }
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.Player.UserExtend = user_extend;
            PushmyEnterPrison(userid, myprison);
            PushEnterPrison(userid);//向监狱内玩家推送进入监狱
        }

        /// <summary>验证入狱</summary>
        private bool CheckEnterPrison(int count, Int64 userid)
        {
            if (count >= 2) return true;
            var task = Variable.TaskInfo.FirstOrDefault(m => m.userid == userid);
            if (task != null)
            {
                if (task.RumorFailCount >= 3) { task.RumorFailCount = 0; return true; }
                if (task.FireFailCount >= 3) { task.FireFailCount = 0; return true; }
                if (task.BreakFailCount >= 3) { task.BreakFailCount = 0; return true; }
                if (task.SellFailCount >= 3) { task.SellFailCount = 0; return true; }
                if (task.SearchFailTimes >= 3) { task.SearchFailTimes = 0; return true; }
            }
            var work = Variable.WorkInfo.FirstOrDefault(m => m.userid == userid);
            if (work == null) return false;
            if (work.RumorFailCount >= 3) { work.RumorFailCount = 0; return true; }
            if (work.FireFailCount >= 3) { work.FireFailCount = 0; return true; }
            if (work.BreakFailCount >= 3) { work.BreakFailCount = 0; return true; }
            if (work.SellFailCount >= 3) { work.SellFailCount = 0; return true; }
            if (work.SearchFailTimes >= 3) { work.SearchFailTimes = 0; return true; }
            return false;
        }

        /// <summary> 监狱内其他在线玩家数据</summary>
        public List<view_scene_user> GetPrisonOhters(Int64 userid)
        {
            var others = Scene.GetOtherPlayers(userid, (int)ModuleNumber.PRISON);
            return others.Select(item => Variable.SCENCE[item]).ToList();
        }

        #endregion
    }
}
