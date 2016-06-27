using FluorineFx;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service.HOME
{
    /// <summary>
    /// 茶道顿悟
    /// </summary>
    public class TRAIN_TEA_INSIGHT
    {
        public static TRAIN_TEA_INSIGHT ObjInstance;

        /// <summary>TRAIN_TEA_INSIGHT单体模式</summary>
        public static TRAIN_TEA_INSIGHT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TRAIN_TEA_INSIGHT());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            return CommandStart((int)GoodsType.TYPE_GOLD, session, data);
        }

        /// <summary>茶道顿悟</summary>
        public ASObject CommandStart(int goodType, TGGSession session, ASObject data)
        {
            return (new Consume.TRAIN_TEA_INSIGHT()).Execute(session.Player.User.id, data);
        }
    }
}
