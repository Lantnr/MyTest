using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo;
using TGG.Core.Vo.Scene;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.Building.Service
{
    public partial class Common
    {
        internal class ScenePush
        {
            /// <summary> 用户id </summary>
            public Int64 user_id { get; set; }

            /// <summary> 用户id </summary>
            public Int64 other_user_id { get; set; }


            /// <summary> 场景数据 </summary>
            public view_scene_user user_scene { get; set; }

            /// <summary> 场景集合的key</summary>
            public string scene_key { get; set; }
        }
        class CityBloodPush
        {
            public Int32 type { get; set; }

            public Int64 user_id { get; set; }

            public Int64 other_user_id { get; set; }

        }

        #region 数据推送
        /// <summary> 推送玩家离开场景 </summary>
        public void PushMyLeaveScene(Int64 userid, view_scene_user userscene)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            if (userscene == null) return;
            userscene.model_number = (int)ModuleNumber.BUILDING;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            if (session == null) return;
            session.Player.Scene.model_number = (int)ModuleNumber.BUILDING;
            var otherplays = Core.Common.Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);

            foreach (var item in otherplays)
            {
                var tokentest = new CancellationTokenSource();
                var temp = new ScenePush()
                {
                    user_id = userid,
                    other_user_id = item.user_id
                };
                Task.Factory.StartNew(m =>
                {
                    var t = m as ScenePush;
                    StartTask(t);
                    tokentest.Cancel();
                }, temp, tokentest.Token);
            }
        }

        private void StartTask(ScenePush temp)
        {
            if (temp == null) return;
            var dic = new Dictionary<string, object>
                {
                    {"result", (int) ResultType.SUCCESS},
                    {"userId", temp.user_id}
                };
            var aso = new ASObject(dic);
            SendPv(temp.other_user_id, aso, (int)SceneCommand.PLAYER_EXIT_SCENET, (int)ModuleNumber.SCENE);
        }

        /// <summary> 推送玩家进入场景并向其他人推送 </summary>
        public void PushMyEnterScene(Int64 userid, view_scene_user userscene)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            if (userscene == null) return;
            userscene.model_number = (int)ModuleNumber.SCENE;
            var key = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, userid);
            if (Variable.SCENCE.ContainsKey(key))
                Variable.SCENCE[key].model_number = (int)ModuleNumber.SCENE;
            var otherplays = Core.Common.Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);//同场景内的其他玩家
            var aso = new ASObject(BuildData((int)ResultType.SUCCESS, userscene.scene_id, userscene.X, userscene.Y, otherplays));

            SendPv(userid, aso, (int)SceneCommand.PUSH_ENTER_SCENET, (int)ModuleNumber.SCENE);
            foreach (var item in otherplays)
            {
                var temp = new ScenePush()
                {
                    user_id = userid,
                    other_user_id = item.user_id,
                    user_scene = userscene.CloneEntity()
                };
                var tokentest = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var t = m as ScenePush;
                    if (t == null) return;
                    var aso1 = new ASObject(BuildData((int)ResultType.SUCCESS, t.user_scene));
                    SendPv(t.other_user_id, aso1, (int)SceneCommand.PLAYER_ENTER_SCENE, (int)ModuleNumber.SCENE);// 向其他玩家推送A玩家退出后的场景
                    tokentest.Cancel();
                }, temp, tokentest.Token);
            }
        }

        /// <summary> 向其他玩家推送A玩家进入活动 </summary>
        public void PushEnterActivity(Int64 userid, view_scene_user myscene)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var otherplays = Core.Common.Scene.ActivityOtherPlayers(userid, (int)ModuleNumber.BUILDING);
            foreach (var item in otherplays)
            {
                var i = item;
                var temp = new ScenePush()
                {
                    user_id = userid,
                    user_scene = myscene,
                    scene_key = i,
                    other_user_id = Variable.Activity.ScenePlayer[i].user_id
                };
                var tokentest = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var t = m as ScenePush;
                    if (t == null) return;
                    var aso = new ASObject(BuildData((int)ResultType.SUCCESS, t.user_scene));
                    SendPv(t.other_user_id, aso, (int)BuildingCommand.PUSH_PLAYER_ENTER, (int)ModuleNumber.BUILDING);
                    tokentest.Cancel();
                }, temp, tokentest.Token);
            }
        }

        /// <summary> 向其他玩家推送A玩家回到活动出生点</summary>
        public void PushRestartActivity(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var otherplays = Core.Common.Scene.ActivityOtherPlayers(userid, (int)ModuleNumber.BUILDING);
            foreach (var item in otherplays)
            {
                var tokenTest = new CancellationTokenSource();
                var temp = new ScenePush()
                {
                    user_id = userid,
                    scene_key = item,
                    other_user_id = Variable.Activity.ScenePlayer[item].user_id
                };
                Task.Factory.StartNew(m =>
                {
                    var t = m as ScenePush;
                    if (t == null) tokenTest.Cancel();
                    var dic = new Dictionary<string, object>() { { "userId", t.user_id } };
                    SendPv(t.other_user_id, new ASObject(dic), (int)BuildingCommand.PUSH_PLAYER_CHANGE, (int)ModuleNumber.BUILDING);
                    tokenTest.Cancel();
                }, temp, tokenTest.Token);
            }
        }

        /// <summary>向其他玩家推送A玩家离开活动 </summary>
        public void PusuLeaveActivity(Int64 userid)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var otherplays = Core.Common.Scene.ActivityOtherPlayers(userid, (int)ModuleNumber.BUILDING); //活动内内其他玩家
            var key = string.Format("{0}_{1}", (int)ModuleNumber.BUILDING, userid);
            view_scene_user scene;
            Variable.Activity.ScenePlayer.TryRemove(key, out scene);
            foreach (var item in otherplays)
            {
                var tokenTest = new CancellationTokenSource();
                var temp = new ScenePush()
                {
                    user_id = userid,
                    scene_key = item,
                    other_user_id = Variable.Activity.ScenePlayer[item].user_id
                };
                Task.Factory.StartNew(m =>
                {
                    var t = m as ScenePush;
                    if (t == null) return;
                    var dic = new Dictionary<string, object>()
                {
                    {"userId",t.user_id }
                };
                    var aso = new ASObject(dic);
                    SendPv(t.other_user_id, aso, (int)BuildingCommand.PUSH_PLAYER_EXIT, (int)ModuleNumber.BUILDING);
                }, temp, tokenTest.Token);
            }

        }

        /// <summary> 向其他玩家推送在监狱移动</summary>
        public void PusuMovingActivity(Int64 userid, view_scene_user scene)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var otherplays = Core.Common.Scene.ActivityOtherPlayers(userid, (int)ModuleNumber.BUILDING);   //监狱内其他玩家
            foreach (var item in otherplays)
            {
                var tokenTest = new CancellationTokenSource();
                var temp = new ScenePush()
                {
                    user_id = userid,
                    scene_key = item,
                    user_scene = scene.CloneEntity(),
                    other_user_id = Variable.Activity.ScenePlayer[item].user_id
                };
                Task.Factory.StartNew(m =>
                {
                    var t = m as ScenePush;
                    if (t == null) return;
                    var aso = new ASObject(BuildData((int)ResultType.SUCCESS, t.user_id, t.user_scene.X, t.user_scene.Y));
                    SendPv(t.other_user_id, aso, (int)BuildingCommand.PUSH_PLAYER_MOVING, (int)ModuleNumber.BUILDING);
                    tokenTest.Cancel();
                }, temp, tokenTest.Token);
            }

        }


        /// <summary> //推送城池耐久更新 </summary>
        public void PushDurability(Int64 userid, int type)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            //推送城池耐久更新
            foreach (var item in Variable.Activity.ScenePlayer.Keys)
            {
                var tokenTest = new CancellationTokenSource();
                var temp = new CityBloodPush()
                {
                    user_id = userid,
                    other_user_id = Variable.Activity.ScenePlayer[item].user_id,
                    type = type
                };
                Task.Factory.StartNew(m =>
                {
                    var t = m as CityBloodPush;
                    if (t == null) return;
                    var dic = new Dictionary<string, object>()
                    {
                        {"type",t.type },
                        {"durability",t.type==1?Variable.Activity.BuildActivity.EastCityBlood:Variable.Activity.BuildActivity.WestCityBlood }
                    };
                    SendPv(t.other_user_id, new ASObject(dic), (int)BuildingCommand.CITY_DURABILITY, (int)ModuleNumber.BUILDING);
                    tokenTest.Cancel();
                }, temp, tokenTest.Token);
            }
        }

        #endregion

        #region 数据组装
        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(object result, view_scene_user sceneplayer)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"playerVo", sceneplayer != null ? EntityToVo.ToScenePlayerVo(sceneplayer)  : null}
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(object result, decimal sceneid, int x, int y, dynamic list_sceneplayers)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"id", sceneid},
                {"x", x},
                {"y", y},
                {  "playerList", list_sceneplayers.Count > 0 ? ConvertListASObject(list_sceneplayers) : null }
            };
            return dic;
        }

        public Dictionary<String, Object> BuildData(object result, decimal userid, int x, int y)
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


        /// <summary>将dynamic对象转换成ASObject对象</summary>
        public List<ASObject> ConvertListASObject(dynamic list)
        {
            var list_aso = new List<ASObject>();
            foreach (var item in list)
            {
                dynamic model = EntityToVo.ToScenePlayerVo(item);
                list_aso.Add(AMFConvert.ToASObject(model));
            }
            return list_aso;
        }

        #endregion

        #region 公共方法

        /// <summary> 获取活动出生坐标 </summary>
        public void GetActivityPoint(view_scene_user scene, int camp)
        {
            if (camp == (int)CampType.East)
            {
                scene.X = Variable.Activity.BuildActivity.EastBornPointX;
                scene.Y = Variable.Activity.BuildActivity.EastBornPointY;
            }
            else
            {
                scene.X = Variable.Activity.BuildActivity.WestBornPointX;
                scene.Y = Variable.Activity.BuildActivity.WestBornPointY;
            }
        }

        /// <summary> 验证boss有没有打死 </summary>
        public bool CheckBossBlood(int camp)
        {
            if (camp == (int)CampType.East)
            {
                if (Variable.Activity.BuildActivity.WestBoosBlood > 0)
                    return false;
            }
            else
            {
                if (Variable.Activity.BuildActivity.EastBoosBlood > 0)
                    return false;
            }
            return true;
        }

        /// <summary>验证放火功能有没有开启 </summary>
        public bool CheckFireOpen(int camp)
        {
            if (camp == (int)CampType.East)
            {
                if (Variable.Activity.BuildActivity.EastBoosBlood > 0)
                    return false;
            }
            else
            {
                if (Variable.Activity.BuildActivity.WestBoosBlood > 0)
                    return false;
            }
            return true;
        }

        /// <summary> 增加声望 </summary>
        public void AddFame(int fame, BuildActivity.UserGoods goods)
        {
            goods.fame += fame;
            if (goods.fame > Variable.Activity.BuildActivity.FameFull)
                goods.fame = Variable.Activity.BuildActivity.FameFull;
        }

        /// <summary> 活动开始 </summary>
        public void ActivityStart()
        {

            #region 开启活动倒计时
            try
            {
                Variable.Activity.BuildActivity.isover = false;
                new Share.Activity().PushActivity(1, 15);
                var token = new CancellationTokenSource();
                var time = Variable.Activity.BuildActivity.PlayTime * 60 * 1000;
#if DEBUG
                time = 1000 * 60 * 2;
#endif
                Task.Factory.StartNew(m =>
                    SpinWait.SpinUntil(() =>
                        {
                            if (Variable.Activity.BuildActivity.isover)
                            {
                                token.Cancel();
                                return true;
                            }
                            return false;
                        }, Convert.ToInt32(m)
                    ), time, token.Token)
                    .ContinueWith(m =>
                    {
                        if (!Variable.Activity.BuildActivity.isover)
                        {
                            END.GetInstance().CommandStart();
                        }
                        token.Cancel();
                    }, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
            #endregion
        }



        /// <summary>发送邮件 </summary>
        public void GetEmail(Int64 userid, tg_activity_building tab)
        {
            var entity = new tg_messages
            {
                receive_id = userid,
                send_id = 0,
                type = 1,
                title = "一夜墨俣活动团队奖励",
                isattachment = 1,
                attachment = GetTeamReward(tab),
                contents = string.Format("尊敬的玩家：您今日参加了一夜墨俣活动,获得奖励如下:" +
                                         "您活动获得声望为{0},团队声望为{1},团队金钱奖励为{2}贯。" +
                                         "活动的团队奖励在附件中，请提取附件领取奖励。",
            tab.fame, tab.team_fame, tab.team_money / 1000),
                create_time = (DateTime.Now.Ticks - 621355968000000000) / 10000,
            };
            entity.Save();
            (new Share.Message()).UnMessage(userid);
        }

        /// <summary> 组装邮件附件数据</summary>
        private string GetTeamReward(tg_activity_building tab)
        {
            var reward = "";
            var fame = tab.team_fame + tab.fame;
            reward += string.Format("{0}_{1}|", (int)GoodsType.TYPE_COIN, tab.team_money);
            reward += string.Format("{0}_{1}", (int)GoodsType.TYPE_FAME, fame);
            return reward;
        }

        /// <summary> 验证坐标 </summary>
        public bool CheckPoint(int x, int y, Int64 userid)
        {
            var key = string.Format("{0}_{1}", (int)ModuleNumber.BUILDING, userid);
            if (!Variable.Activity.ScenePlayer.ContainsKey(key)) return false;
            var scene = Variable.Activity.ScenePlayer[key];
            var distance = Math.Sqrt(Math.Abs(x - scene.X) * Math.Abs(x - scene.X) + Math.Abs(y - scene.Y) * Math.Abs(y - scene.Y));
            return distance < 200;
        }
        #endregion

        #region 推送协议
        /// <summary> 向其他人推送协议 </summary>
        //public void SendPv(Int64 userid, ASObject aso, int commandnumber, Int64 otheruserid, int modulenumber)
        //{
        //    if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
        //    var key = string.Format("{0}_{1}_{2}", modulenumber, commandnumber, otheruserid);
        //    var session = Variable.OnlinePlayer[userid] as TGGSession;
        //    if (session == null) return;
        //    session.SPM.AddOrUpdate(key, aso, (m, n) => aso);
        //}

        /// <summary>
        /// 向增加推送协议
        /// </summary>
        public void SendPv(Int64 other_user_id, ASObject aso, int commandnumber, int modulenumber)
        {
            if (!Variable.OnlinePlayer.ContainsKey(other_user_id)) return;
            var session = Variable.OnlinePlayer[other_user_id] as TGGSession;
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
        #endregion


    }
}
