using FluorineFx;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 武将刷新
    /// </summary>
    public class TRAIN_HOME_NPC_REFRESH
    {
        private static TRAIN_HOME_NPC_REFRESH _objInstance;

        /// <summary>TRAIN_HOME_NPC_REFRESH单体模式</summary>
        public static TRAIN_HOME_NPC_REFRESH GetInstance()
        {
            return _objInstance ?? (_objInstance = new TRAIN_HOME_NPC_REFRESH());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return (new Consume.TRAIN_HOME_NPC_REFRESH()).Execute(session.Player.User.id, data);
        }
    }
}
