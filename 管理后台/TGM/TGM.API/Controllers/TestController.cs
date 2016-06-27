using System;
using System.Threading;
using System.Web.Http;
using TGM.API.Command;
using TGM.API.Entity;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public class TestController : ControllerBase
    {

        #region Test

        public void GetSend(String token, string sn)
        {
            //if (!IsToken(token)) return;     //验证会话
            //SN = sn;                        //设置游戏服务器




            //var api = new CommandApi("192.168.1.132", 10086, ApiCommand.充值);
            //var entity = api.Recharge("A9s5D2Fr5Tg3H1Yw", new Pay()
            //  {
            //      player_id = 1,
            //      amount = 500,
            //  });

        }

        #endregion



    }
}
