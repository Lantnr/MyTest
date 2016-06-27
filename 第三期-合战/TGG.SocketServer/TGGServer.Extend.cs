using tg_user_login_log = TGG.Core.Entity.tg_user_login_log;

namespace TGG.SocketServer
{
    public partial class TGGServer
    {
        /// <summary>服务器关闭时扩展方法</summary>
        private void ServerClosed()
        {
            tg_user_login_log.GetServerCloseUpdate();
        }
    }
}
