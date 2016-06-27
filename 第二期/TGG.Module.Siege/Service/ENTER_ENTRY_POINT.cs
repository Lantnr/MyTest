using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 进入传递点
    /// </summary>
    public class ENTER_ENTRY_POINT
    {
        private static ENTER_ENTRY_POINT _objInstance;

        /// <summary>ENTER_ENTRY_POINT单体模式</summary>
        public static ENTER_ENTRY_POINT GetInstance()
        {
            return _objInstance ?? (_objInstance = new ENTER_ENTRY_POINT());
        }

        /// <summary> 进入传递点</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User;
            var level = session.Player.Role.Kind.role_level;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);

            var ep = Variable.BASE_ENTRYPOINTSIEGE.FirstOrDefault(m => m.id == id);
            if (ep == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.BASE_TABLE_ERROR)); //验证传送点基表
            if (user.player_camp != ep.camp) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_NO_OWN_PORT)); //验证传送点阵营

            //var xy = ep.coorPoint.Split(',');
            //if (xy.Length != 2) return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR)); //验证传送点数据;

            var key = Common.GetInstance().GetKey(user.id);
            if (!Variable.Activity.ScenePlayer.ContainsKey(key))
            {
                var s = Common.GetInstance().BuildSceneUser(user, level);
                Variable.Activity.ScenePlayer.AddOrUpdate(key, s, (m, n) => n); //加入到内存中
            }
            var scene = Variable.Activity.ScenePlayer[key];
            if (scene == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_POSITION_ERROR)); //验证玩家是否在传送点位置;
            
            //if (!Common.GetInstance().IsCoorPoint(xy, scene))
            //    return new ASObject(Common.GetInstance().BuildData((int)ResultType.POSITION_ERROR)); //验证玩家是否在传送点位置;

            var xy = ep.coorPoint2.Split(',');
            if (xy.Length != 2) return new ASObject(Common.GetInstance().BuildData((int)ResultType.DATABASE_ERROR)); //验证传送点数据;

            lock (this)
            {
                scene.X = Convert.ToInt32(xy[0]);
                scene.Y = Convert.ToInt32(xy[1]);
            }

            OtherPlayerPush(session, id);
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }

        /// <summary>场景内其他玩家数据 并将当前玩家数据推送给其他玩家</summary>
        private void OtherPlayerPush(TGGSession session, int entryid)
        {
            var user = session.Player.User;
            //var otherplayer = Variable.Activity.Siege.PlayerData.Where(m => m.user_id != user.id).ToList();
            var otherplayer = Common.GetInstance().GetOtherSceneUsers(user.id);
            foreach (var item in otherplayer)
            {
                //if (!Common.GetInstance().IsActivities(item.user_id)) continue;
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(
                    m => PUSH_PLAYER_POS.GetInstance().CommandStart(session, Convert.ToInt64(m), (int)SiegePointType.POINT, entryid), item.user_id, token.Token);
            }
        }
    }
}
