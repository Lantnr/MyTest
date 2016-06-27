using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using FluorineFx.Util;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 回到出生点 
    /// </summary>
    public class GO_BACK
    {
        private static GO_BACK _objInstance;

        /// <summary>GO_BACK单体模式</summary>
        public static GO_BACK GetInstance()
        {
            return _objInstance ?? (_objInstance = new GO_BACK());
        }

        /// <summary>回到出生点</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User;
            var role = session.Player.Role.Kind;
            var playdata = Common.GetInstance().GetSiegePlayer(user.id, user.player_camp);
            playdata.state = (int)SiegePlayerType.EXIT_DEFEND;
            Common.GetInstance().UpdateUserScene(user, role.role_level);
            PushBack(session);
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }

        /// <summary> 推送玩家回到出生地</summary>
        /// <param name="session">当前玩家session</param>
        private void PushBack(TGGSession session)
        {
            var list = Variable.Activity.ScenePlayer.Where(m => m.Value.model_number == (int)ModuleNumber.SIEGE && m.Key != Common.GetInstance().GetKey(session.Player.User.id));
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(
                    m => PUSH_PLAYER_POS.GetInstance().CommandStart(session,Convert.ToInt64(m) , (int)SiegePointType.BIRTHPLACE, 0), item.Value.user_id, token.Token);
            }
        }
    }
}
