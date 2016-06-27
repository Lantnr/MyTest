using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Module.Scene.Service
{
    /// <summary>
    /// 玩家移动
    /// </summary>
    public partial class MOVING
    {
        private static MOVING _objInstance;

        /// <summary>MOVING 单例模式</summary>
        public static MOVING GetInstance()
        {
            return _objInstance ?? (_objInstance = new MOVING());
        }

        /// <summary>玩家移动</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1} - {2}", "MOVING", "玩家移动", session.Player.User.player_name);
#endif
            try
            {
                var dic = new Dictionary<string, object>();
                var result = (int)ResultType.SUCCESS;
                var x = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "x").Value);
                var y = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "y").Value);
                var userid = session.Player.User.id;
                var userscene = session.Player.Scene;
#if DEBUG
                XTrace.WriteLine("玩家{0}当前场景---{1}\t", session.Player.User.player_name, userscene.scene_id);
                XTrace.WriteLine("玩家{0}当前坐标---{1},{2}\t", session.Player.User.player_name, userscene.X, userscene.Y);
#endif
                if (userscene == null)
                {
                    dic.Add("result", (int)ResultType.SCENE_BASEDATA_WRONG);
                    return new ASObject(dic);
                }

                userscene.X = x;
                userscene.Y = y;
                session.Player.Scene.X = x;
                session.Player.Scene.Y = y;
                #region 测试数据
                //var otherplays = Core.Common.Scene.GetOtherPlayersByArea(userid, userscene.scene_id, (int)ModuleNumber.SCENE, userscene.X, userscene.Y, session.window.X, session.window.Y);//同场景内的其他玩家
                //var otherplays = new Share.TGTask().TestOhters(userid, userscene.scene_id, (int)ModuleNumber.SCENE);
                #endregion
                var otherplays = Core.Common.Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);//同场景内的其他玩家
#if DEBUG
                XTrace.WriteLine("玩家{0}新坐标---{1},{2}\t\n", session.Player.User.player_name, userscene.X, userscene.Y);
#endif

                foreach (var item in otherplays)// 推送玩家进入信息
                {
#if DEBUG
                    XTrace.WriteLine("向{0}玩家推送{1}移动---{2}\t\n", item.player_name, session.Player.User.player_name, userscene.scene_id);
#endif
                    var tokenTest = new CancellationTokenSource();
                    var temp = new Common.ScenePush()
                    {
                        user_id = userid,
                        other_user_id = item.user_id,
                        user_scene = userscene
                    };
                    Task.Factory.StartNew(m =>
                    {
                        var t = m as Common.ScenePush;
                        if (t == null) return;
                        PlayerMoving(t.user_scene.X, t.user_scene.Y, t.user_id, t.other_user_id);
                        tokenTest.Cancel();
                    }, temp, tokenTest.Token);
                }
                dic.Add("result", result);
                return new ASObject(dic);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

    }
}
