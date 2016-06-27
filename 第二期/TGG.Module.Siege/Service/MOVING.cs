using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 主角移动
    /// </summary>
    public class MOVING
    {
        public static MOVING ObjInstance;

        /// <summary>MOVING单体模式</summary>
        public static MOVING GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new MOVING());
        }

        /// <summary>主角移动</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var x = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "x").Value);
            var y = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "y").Value);
            var user = session.Player.User;
            if (!Variable.Activity.ScenePlayer.ContainsKey(Common.GetInstance().GetKey(user.id)))
                return new ASObject(Common.GetInstance().BuildData((int)ResultType.NO_DATA));

            var userscene = Variable.Activity.ScenePlayer[Common.GetInstance().GetKey(user.id)];
            userscene.X = x;
            userscene.Y = y;

            PushMoving(user.id, x, y); // 场景其他玩家数据
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }

        /// <summary> 推送玩家移动 </summary>
        /// <param name="userid">当前玩家id</param>
        /// <param name="x">当前玩家x</param>
        /// <param name="y">当前玩家y</param>
        private void PushMoving(Int64 userid, int x, int y)
        {
            var list = Common.GetInstance().GetOtherSceneUsers(userid);
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(m =>
                {
                    var uid = Convert.ToInt64(m);
                    if (!Variable.OnlinePlayer.ContainsKey(uid)) return;
                    var session = Variable.OnlinePlayer[uid] as TGGSession;
                    PUSH_PLAYER_MOVING.GetInstance().SendCommandStart(session, userid, x, y);
                }, item.user_id, token.Token);
            }
        }
    }
}
