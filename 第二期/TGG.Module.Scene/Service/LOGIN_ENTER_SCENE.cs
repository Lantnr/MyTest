using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using System.Collections;
using NewLife.Log;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using System.Collections.Concurrent;
using view_scene_user = TGG.Core.Entity.view_scene_user;

namespace TGG.Module.Scene.Service
{
    /// <summary>
    /// 登录后进入当前场景
    /// </summary>
    public class LOGIN_ENTER_SCENE
    {
        private static LOGIN_ENTER_SCENE _objInstance;

        /// <summary>LOGIN_ENTER_SCENE 单体模式</summary>
        public static LOGIN_ENTER_SCENE GetInstance()
        {
            return _objInstance ?? (_objInstance = new LOGIN_ENTER_SCENE());
        }

        /// <summary>登录后进入当前场景</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1} - 玩家:{2}", "LOGIN_ENTER_SCENE", "登录后进入当前场景", session.Player.User.player_name);
#endif
                const int result = (int)ResultType.SUCCESS;
                var userid = session.Player.User.id;
                var userscene = session.Player.Scene;
                if (userscene == null)
                    return new ASObject(Common.GetInstance().BuildData((int)ResultType.SCENE_NOFIND, 0, 0, 0, null));
#if DEBUG
                XTrace.WriteLine("玩家{0}当前场景---{1}", session.Player.User.player_name, userscene.scene_id);
                XTrace.WriteLine("玩家{0}当前坐标---{1},{2}", session.Player.User.player_name, userscene.X, userscene.Y);
#endif

                var key = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, userid);
                Variable.SCENCE.AddOrUpdate(key, userscene, (m, n) => n); //加入到内存中
                if (userscene.model_number != (int)ModuleNumber.SCENE)
                    return new ASObject(Common.GetInstance().BuildData(result, userscene.scene_id, userscene.X, userscene.Y, new List<view_scene_user>()));
                var otherplays = Core.Common.Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);//同场景内的其他玩家
                foreach (var item in otherplays)
                {
#if DEBUG
                    XTrace.WriteLine("向{0}玩家推送{1}进入当前场景---{2}", item.player_name, session.Player.User.player_name, userscene.scene_id);
#endif
                    var temp = new Common.ScenePush()
                    {
                        user_id = userid,
                        other_user_id = item.user_id,
                        user_scene = userscene
                    };
                    var ts = new CancellationTokenSource();
                    Task.Factory.StartNew(m =>
                    {
                        var t = m as Common.ScenePush;
                        if (t == null) return;
                        new PLAYER_ENTER_SCENE().CommandStart(t.user_scene, t.user_id, t.other_user_id);
                        ts.Cancel();
                    }, temp, ts.Token);
                }

                return new ASObject(Common.GetInstance().BuildData(result, userscene.scene_id, userscene.X, userscene.Y, otherplays));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }
    }
}
