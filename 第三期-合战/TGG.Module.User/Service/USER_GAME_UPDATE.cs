using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    public class USER_GAME_UPDATE : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~USER_GAME_UPDATE()
        {
            Dispose();
        }

        #endregion

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "USER_GAME_UPDATE", "点击游戏更新图标");
#endif
            var userextend = session.Player.UserExtend.CloneEntity();
            if (userextend.game_update == 1)
                userextend.game_update = 0;
            userextend.Save();
            session.Player.UserExtend = userextend;

            return CommonHelper.SuccessResult();
        }
    }
}
