using System;
using System.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluorineFx.Messaging.Rtmp.SO;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Global;
using TGG.Core.Queue;
using FluorineFx;
using TGG.Core.Vo;
using TGG.Core.Enum.Type;
using tg_user_login_log = TGG.Core.Entity.tg_user_login_log;
using System.Collections.Concurrent;
using TGG.Core.Enum.Command;
using System.Threading;

namespace TGG.SocketServer
{
    /// <summary>
    /// TGG Session会话类
    /// </summary>
    public partial class TGGSession
    {
        #region Session变量属性声明

        /// <summary>
        /// 推送数据模块
        /// key_value
        /// key:moduleNumber_commandNumber_userid
        /// value:ASObject(推送协议对象data)
        /// </summary>
        public ConcurrentDictionary<string, ASObject> SPM { get; set; }

        /// <summary>当前指令集合 </summary>
        //private static ConcurrentDictionary<string, bool> CCI { get; set; }

        /// <summary>玩家信息</summary>
        public Player Player { get; set; }

        /// <summary>玩家战斗信息</summary>
        public FightItem Fight { get; set; }

        /// <summary>玩家任务项</summary>
        public List<TaskItem> TaskItems { get; set; }

        /// <summary>世界聊天时间</summary>
        public Int64 CharTime { get; set; }

        /// <summary>窗口</summary>
        public Window window { get; set; }


        /// <summary>主线任务</summary>
        public tg_task MainTask { get; set; }
        #endregion

        /// <summary>扩展新连接方法</summary>
        private void SessionStartedExtend()
        {

#if DEBUG
            XTrace.WriteLine("{0}", "新连接");
#endif
            //初始化
            Init();

        }
        /// <summary>扩展连接关闭</summary>
        private void SessionClosedExtend()
        {
            GetScene();
            CloseTask();
            RemoveUser();
            RemoveCCI();
            GameUpdate();

#if DEBUG
            XTrace.WriteLine("{0}", "连接关闭");
#endif
        }


        #region 私有方法

        /// <summary>初始化</summary>
        private void Init()
        {
            window = new Window();
            Player = new Player();
            Fight = new FightItem();
            TaskItems = new List<TaskItem>();
            SPM = new ConcurrentDictionary<string, ASObject>();
            //CCI = new ConcurrentDictionary<string, bool>();
            CharTime = (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }

        /// <summary>移除在线用户</summary>
        private void RemoveUser()
        {
            try
            {
                if (Player.User.id != 0)
                    tg_user_login_log.GetLoginOutUpdate(Player.User.id);
                if (Player == null || !Variable.OnlinePlayer.ContainsKey(Player.User.id)) return;
                dynamic dy;
                var b = Variable.OnlinePlayer.TryRemove(Player.User.id, out dy);
                if (!b)
                {
#if DEBUG
                    XTrace.WriteLine("Thread.Sleep 10 TryRemove:{0} ", b);
#endif
                    Thread.Sleep(1);
                    RemoveUser();
                }
#if DEBUG
                XTrace.WriteLine("TryRemove:{0} ", b);
#endif
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary>移除指令</summary>
        private void RemoveCCI()
        {
            var key = string.Format("{0}_", Player.User.id);
            var keys = Variable.CCI.Keys.Where(q => q.Contains(key)).ToList();
            foreach (var item in keys)
            {
                bool b;
                Variable.CCI.TryRemove(item, out b);
            }    
        }

        /// <summary>场景处理</summary>
        private void GetScene()
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("Close场景处理 ");
#endif
                if (Player == null || Player.User.id == 0) return;
                PlayerOffline(Player.Scene);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary>推送其他session对象数据</summary>
        private void OtherSessionPush(long userid, int mn, int cn, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var s = Variable.OnlinePlayer[userid] as TGGSession;
            if (s == null) return;
            var pv_push = s.InitProtocol(mn, cn, (int)ResponseType.TYPE_SUCCESS, data);
            s.SendData(pv_push, false);
        }

        #endregion

        #region 公共方法

        /// <summary>指令结束</summary>
        public void CommandEnd(ProtocolQueue model) { CommandEnd(model.Protocol.moduleNumber, model.Protocol.commandNumber); }
        /// <summary>指令结束</summary>
        public void CommandEnd(int modulenumber, int commandnumber)
        {
            //var key = string.Format("{0}_{1}_{2}", Player.User.id, modulenumber, commandnumber);
            //var f = Variable.CCI.ContainsKey(key);
            //if (Variable.CCI.Count == 0) return;
            //if (!f) return;
            //Thread.Sleep(5000);
            //bool b;
            //var c = Variable.CCI.TryRemove(key, out b);

            //XTrace.WriteLine("End Command Key:{0} total:{1} {2} {3}", key, Variable.CCI.Count, c, f);
        }

        /// <summary>初始化协议</summary>
        public ProtocolVo InitProtocol(ProtocolQueue model, int status, ASObject data)
        {
            return InitProtocol(model.Protocol.serialNumber, model.Protocol.moduleNumber, model.Protocol.commandNumber, model.Protocol.verificationCode, status, data);
        }

        /// <summary>初始化协议</summary>
        public ProtocolVo InitProtocol(int modulenumber, int commandnumber, int status, ASObject data)
        {
            return InitProtocol(1, modulenumber, commandnumber, 0, status, data);
        }

        /// <summary>初始化协议</summary>
        public ProtocolVo InitProtocol(int sn, int modulenumber, int commandnumber, int vc, int status, ASObject data)
        {
            return new ProtocolVo
            {
                serialNumber = sn,
                moduleNumber = modulenumber,
                commandNumber = commandnumber,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = status,
                verificationCode = vc,
                data = data,
            };
        }

        /// <summary>发送数据</summary>
        public void SendData(ProtocolVo pv)
        {
            SendData(pv, true);
        }

        /// <summary>发送数据</summary>
        /// <param name="pv">通讯协议对象</param>
        /// <param name="ispush">是否推送</param>
        public void SendData(ProtocolVo pv, bool ispush)
        {
            var protocol = new Protocol(pv);
            var send_data = protocol.AFMData();
            Send(send_data, 0, send_data.Length); //发送数据
            if (ispush) PushData();  //推送数据
           // CommandEnd(pv.moduleNumber, pv.commandNumber);

        }

        /// <summary>数据推送</summary>
        public void PushData()
        {
            PushDataExtend();
        }

        private void PushDataExtend()
        {
            try
            {
                //推送数据
                foreach (var item in SPM)
                {
                    lock (item.Key)
                    {
                        var mc = item.Key.Split('_');
                        var mn = int.Parse(mc[0]);
                        var cn = int.Parse(mc[1]);
                        var data_push = item.Value;
                        if (mc.Length == 3)
                        {
                            var userid = Convert.ToInt64(mc[2]);
                            var _p = new PushSendOtherPlayer { user_id = userid, mn = mn, cn = cn, data = data_push, key = item.Key };

                            var token_test = new CancellationTokenSource();
                            Task.Factory.StartNew(m =>
                            {
                                var temp = m as PushSendOtherPlayer;
                                if (temp == null) return;
                                OtherSessionPush(temp.user_id, temp.mn, temp.cn, temp.data);
                                ASObject _obj;
                                SPM.TryRemove(temp.key, out _obj);
                            }, _p, token_test.Token);

                        }
                        else
                        {
                            var pv_push = InitProtocol(mn, cn, (int)ResponseType.TYPE_SUCCESS, data_push);
                            SendData(pv_push, false);
                        }


                    }
                }
                SPM.Clear();
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        class PushSendOtherPlayer
        {
            public Int64 user_id { get; set; }
            public Int32 mn { get; set; }
            public Int32 cn { get; set; }

            public string key { get; set; }
            public ASObject data { get; set; }
        }

        #endregion

        #region 退出场景

        /// <summary> 玩家下线操作 </summary>
        private void PlayerOffline(view_scene_user scene)
        {
            try
            {
                var userid = scene.user_id;
                var scenekey = (int)ModuleNumber.SCENE + "_" + userid;
                view_scene_user scenevalue;
                Variable.SCENCE.TryRemove(scenekey, out scenevalue); //从内存中移除

#if DEBUG
                XTrace.WriteLine("玩家{0}下线时的场景:{1}  坐标:{1},{2}", Player.User.player_name, scene.scene_id, scene.X, scene.Y);
#endif
                var prison = tg_prison.GetPrisonByUserId(userid);
                if (prison != null)
                {
                    PrisonOut(userid);
                    tg_scene.GetSceneUpdate(scene); //保存到数据库
                    return;   //监狱内下线
                }
                if (Variable.Activity.ScenePlayer.Keys.Contains(string.Format("{0}_{1}", (int)ModuleNumber.BUILDING, userid)) ||
                    Variable.Activity.ScenePlayer.Keys.Contains(string.Format("{0}_{1}", (int)ModuleNumber.SIEGE, userid)))
                {
                    ActivityOut(userid);
                    scene.model_number = (int)ModuleNumber.SCENE;
                    tg_scene.GetSceneUpdate(scene); //保存到数据库
                    return;
                }
                scene.model_number = (int)ModuleNumber.SCENE;
                tg_scene.GetSceneUpdate(scene); //保存到数据库
                NormalOut(scene); //正常场景下线
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }
        /// <summary>
        /// 发送协议
        /// </summary>
        public void SendPushCommand(TGGSession session, ASObject data, int commandNumber, int mn)
        {
            var pv = new ProtocolVo
            {
                serialNumber = 1,
                verificationCode = 1,
                moduleNumber = mn,
                commandNumber = commandNumber,
                sendTime = 1000,
                serverTime = (DateTime.Now.Ticks - 621355968000000000) / 10000,
                status = (int)ResponseType.TYPE_SUCCESS,
                data = data,
            };
            session.SendData(pv);
        }

        /// <summary>监狱内下线 </summary>
        private void PrisonOut(Int64 userid)
        {
            //推送玩家离开监狱
            var prisoners = Scene.GetOtherPlayers(userid, (int)ModuleNumber.PRISON);
            var key = string.Format("{0}_{1}", (int)ModuleNumber.PRISON, userid);
            view_scene_user scene;
            Variable.SCENCE.TryRemove(key, out scene);
            if (!prisoners.Any()) return;  //监狱内其他玩家
            foreach (var item in prisoners)
            {
                var other = (string)item.Clone();
                var obj = new OnlineObject { OffLine = userid, OtherPlayer = Convert.ToInt32(other.Split("_")[1]) };
                var tokenTest = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var temp = m as OnlineObject;
                    if (temp == null) return;
                    var aso = BuildData(temp.OffLine);
                    if (!Variable.OnlinePlayer.ContainsKey(temp.OtherPlayer)) return;
                    var othersession = Variable.OnlinePlayer[temp.OtherPlayer] as TGGSession;
                    if (othersession == null) return;
                    SendPushCommand(othersession, aso, (int)PrisonCommand.PUSH_PLAYER_EXIT, (int)ModuleNumber.PRISON);
                }, obj, tokenTest.Token);
            }
        }

        /// <summary>活动内下线 </summary>
        private void ActivityOut(Int64 userid)
        {
            var mn = 0;
            if (Variable.Activity.ScenePlayer.Keys.Contains(string.Format("{0}_{1}", (int)ModuleNumber.SIEGE, userid)))
                mn = (int)ModuleNumber.SIEGE;
            if (Variable.Activity.ScenePlayer.Keys.Contains(string.Format("{0}_{1}", (int)ModuleNumber.BUILDING, userid)))
                mn = (int)ModuleNumber.BUILDING;
            var otherplays = Scene.ActivityOtherPlayers(userid, mn);//活动内其他玩家
            var key = string.Format("{0}_{1}", mn, userid);
            view_scene_user scene;
            Variable.Activity.ScenePlayer.TryRemove(key, out scene);
            foreach (var item in otherplays)
            {
                var obj = new OnlineObject { OffLine = userid, OtherPlayer = Variable.Activity.ScenePlayer[item].user_id };
                var tokenTest = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var temp = m as OnlineObject;
                    if (temp == null) return;
                    var aso = BuildData(temp.OffLine);
                    if (!Variable.OnlinePlayer.ContainsKey(temp.OtherPlayer)) return;
                    var othersession = Variable.OnlinePlayer[temp.OtherPlayer] as TGGSession;
                    if (othersession == null) return;
                    if (mn == (int)ModuleNumber.BUILDING)
                        SendPushCommand(othersession, aso, (int)BuildingCommand.PUSH_PLAYER_EXIT, (int)ModuleNumber.BUILDING);
                    if (mn == (int)ModuleNumber.SIEGE)
                        SendPushCommand(othersession, aso, (int)SiegeCommand.PUSH_PLAYER_EXIT, (int)ModuleNumber.SIEGE);
                }, obj, tokenTest.Token);

            }
        }

        /// <summary>正常下线</summary>
        private void NormalOut(view_scene_user userscene)
        {
            var otherplays = Scene.GetOtherPlayers(userscene.user_id, userscene.scene_id, (int)ModuleNumber.SCENE);

            if (!otherplays.Any()) return;
            foreach (var item in otherplays)
            {
                var obj = new OnlineObject { OffLine = userscene.user_id, OtherPlayer = item.user_id };
                var tokenTest = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var temp = m as OnlineObject;

                    if (temp == null) return;
                    if (!Variable.OnlinePlayer.ContainsKey(temp.OtherPlayer)) return;
                    var otherplayerssession = Variable.OnlinePlayer[temp.OtherPlayer] as TGGSession;
                    if (otherplayerssession == null) return;
                    var aso = BuildData(temp.OffLine);
                    SendPushCommand(otherplayerssession, aso, (int)SceneCommand.PLAYER_EXIT_SCENET, (int)ModuleNumber.SCENE);
                }, obj, tokenTest.Token);
            }
        }

        /// <summary> 组装数据 </summary>
        private ASObject BuildData(Int64 userid)
        {
            var dic = new Dictionary<string, object>();
            const int result = (int)ResultType.SUCCESS;
            dic.Add("result", result);
            dic.Add("userId", userid);
            return new ASObject(dic);
        }

        class OnlineObject
        {
            public Int64 OffLine { get; set; }
            public Int64 OtherPlayer { get; set; }
        }

        /// <summary>玩家下线更新全局任务对应值，关闭线程</summary>
        private void CloseTask()
        {
            var task = Variable.TaskInfo.FirstOrDefault(m => m.userid == Player.User.id);
            var worktask = Variable.WorkInfo.FirstOrDefault(m => m.userid == Player.User.id);
            if (task != null)
            {
                task.ArrestRumorSceneId = 0;
                task.ArrestBreakSceneId = 0;
                task.ArrestFireSceneId = 0;
                task.ArrestSellSceneId = 0;
                task.GuardSceneId = 0;
                task.GuardCamp = 0;
                task.WatchState = (int)TaskKillType.LOSE;
                task.P_State = 0;
            }
            if (worktask == null) return;
            worktask.ArrestRumorSceneId = 0;
            worktask.ArrestBreakSceneId = 0;
            worktask.ArrestFireSceneId = 0;
            worktask.ArrestSellSceneId = 0;
            worktask.GuardSceneId = 0;
            worktask.GuardCamp = 0;
            worktask.WatchState = (int)TaskKillType.LOSE;
            worktask.P_State = 0;
        }

        /// <summary>玩家下线，处理游艺园信息</summary>
        private void GameUpdate()
        {
            var game = Variable.GameInfo.FirstOrDefault(m => m.userid == Player.User.id);
            if (game == null || game.type == 0) return;

            CheckDay(Player.User.id, game.type);
            game.type = 0;
        }

        /// <summary>处理每日完成度</summary>
        private void CheckDay(Int64 userid, int gt)
        {
            var ex = tg_user_extend.GetByUserId(userid);
            if (ex == null) return;

            switch (gt)
            {
                case (int)GameEnterType.辩驳游戏: ex.eloquence_count++; break;
                case (int)GameEnterType.老虎机: ex.calculate_count++; break;
                case (int)GameEnterType.花月茶道: ex.tea_count++; break;
                case (int)GameEnterType.猜宝游戏: ex.ninjutsu_count++; break;
                case (int)GameEnterType.猎魂: ex.ball_count++; break;
            }
            CheckReward(ex);   //每日完成度统计
        }

        /// <summary>统计每日完成度</summary>
        private void CheckReward(tg_user_extend ex)
        {
            var finish = Variable.BASE_RULE.FirstOrDefault(m => m.id == "30004");
            if (finish == null) return;
            var total = Convert.ToInt32(finish.value);

            if (ex.game_finish_count >= total || ex.game_receive != (int)GameRewardType.TYPE_UNREWARD) { ex.Update(); return; }

            ex.game_finish_count++;

            if (ex.game_finish_count < total)   //未达到领奖条件
            { ex.Update(); return; }

            ex.game_receive = (int)GameRewardType.TYPE_CANREWARD;
            ex.Update();
        }
        #endregion
    }
}
