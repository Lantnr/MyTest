using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Base;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Entity;

namespace TGG.Module.Scene.Service
{
    /// <summary>
    /// 切换场景
    /// </summary>
    public class ENTER_SCENE
    {
        private static ENTER_SCENE _objInstance;

        /// <summary>ENTER_SCENE 单体模式</summary>
        public static ENTER_SCENE GetInstance()
        {
            return _objInstance ?? (_objInstance = new ENTER_SCENE());
        }

        /// <summary>切换场景</summary>
        public ASObject CommandStart(SocketServer.TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1} - {2}", "ENTER_SCENE", "切换场景", session.Player.User.player_name);
#endif
            var result = (int)ResultType.SUCCESS;
            var sceneid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var userid = session.Player.User.id;
            var userscene = session.Player.Scene;
#if DEBUG
            XTrace.WriteLine("玩家{0}当前场景---{1}\t", session.Player.User.player_name, userscene.scene_id);
            XTrace.WriteLine("玩家{0}当前坐标---{1},{2}\t", session.Player.User.player_name, userscene.X, userscene.Y);
#endif
            var basescene = Variable.BASE_SCENE.FirstOrDefault(q => q.id == sceneid);
            if (basescene == null || userscene == null) return BuildData((int)ResultType.SCENE_BASEDATA_WRONG, null);
            if (userscene.scene_id == sceneid) return BuildData((int)ResultType.SCENE_ENTER_SAME, null);
            //1.表示开启 0表示未开启
            if (basescene.enabled == 0) return BuildData((int)ResultType.SCENE_CITY_UNOPEN, null);
            var basebornpoint = Variable.BASE_ROLEBORNPOINT.FirstOrDefault(q => q.id == basescene.bornPoint);
            if (basebornpoint == null) return BuildData((int)ResultType.SCENE_BASEDATA_WRONG, null);

            var otherplays = Core.Common.Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);//同场景内的其他玩家
            //推送玩家离开场景信息
            foreach (var item in otherplays) //调用推送
            {
#if DEBUG
                XTrace.WriteLine("向{0}玩家推送{1}离开场景---{2}\t\n", item.player_name, session.Player.User.player_name, userscene.scene_id);
#endif
                var tokenTest = new CancellationTokenSource();
                var temp = new Common.ScenePush()
                {
                    user_id = userid,
                    other_user_id = item.user_id
                };
                Task.Factory.StartNew(m =>
                    {
                        var t = m as Common.ScenePush;
                        if (t == null) return;
                        new PLAYER_EXIT_SCENET().CommandStart(t.user_id, t.other_user_id);
                        tokenTest.Cancel();
                    }, temp, tokenTest.Token);
            }
            SceneDataSave(sceneid, userscene, session.Player.Scene, basebornpoint);
            var newotherplays = Core.Common.Scene.GetOtherPlayers(userid, userscene.scene_id, (int)ModuleNumber.SCENE);//同场景内的其他玩家

            foreach (var item in newotherplays)   //向新场景玩家推送玩家进入
            {
#if DEBUG
                XTrace.WriteLine("玩家{0}当前坐标---{1},{2}\t", session.Player.User.player_name, userscene.X, userscene.Y);
                XTrace.WriteLine("向{0}玩家推送{1}进入当前场景---{2}\t", item.player_name, session.Player.User.player_name, userscene.scene_id);
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
                    new PLAYER_ENTER_SCENE().CommandStart(t.user_scene, t.user_id, t.other_user_id);
                    tokenTest.Cancel();
                }, temp, tokenTest.Token);
            }
            return BuildData(result, newotherplays);
        }

        /// <summary>新数据更新到内存中</summary>
        private void SceneDataSave(long sceneid, view_scene_user userscene, view_scene_user sessionscene, BaseRoleBornPoint _base)
        {
            var key = string.Format("{0}_{1}", (int)ModuleNumber.SCENE, userscene.user_id);
            userscene.scene_id = sceneid;
            if (Variable.SCENCE.ContainsKey(key))
                Variable.SCENCE.TryUpdate(key, userscene, userscene);
            var v = _base.coorPoint.Split(',');
            userscene.X = int.Parse(v[0]);
            userscene.Y = int.Parse(v[1]);
            sessionscene.scene_id = sceneid;
            sessionscene.X = userscene.X;
            sessionscene.Y = userscene.Y;

        }

        private ASObject BuildData(object result, IEnumerable<view_scene_user> sceneplayer)
        {
            List<ASObject> players;
            if (sceneplayer == null) players = null;
            else players = sceneplayer.Any() ? Common.GetInstance().ConvertListASObject(sceneplayer, "ScenePlayer") : null;
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"playerList", players}
            };
            return new ASObject(dic);
        }
    }
}
