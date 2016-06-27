using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 模块激活指令
    /// </summary>
    public class USER_MODULE_ACTIVETION
    {
        private static USER_MODULE_ACTIVETION ObjInstance;


        /// <summary>USER_MODULE_ACTIVETION单例模式</summary>
        public static USER_MODULE_ACTIVETION GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new USER_MODULE_ACTIVETION());
        }

        /// <summary>玩家登陆指令</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "USER_MODULE_ACTIVETION", "模块激活指令");
#endif
            var id = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "id").Value);

            var module = Variable.BASE_MODULEOPEN.FirstOrDefault(m => m.id == id);
            if (module == null) ResultData((int)ResultType.BASE_TABLE_ERROR);

            //等级判断
            if (session.Player.Role.Kind.role_level < module.level) return ResultData((int)ResultType.BASE_PLAYER_LEVEL_ERROR);
            //身份判断
            if (session.Player.Role.Kind.role_identity < module.identity) return ResultData((int)ResultType.BASE_PLAYER_IDENTITY_ERROR);

            var entity = new tg_module_open
            {
                module = id,
                user_id = session.Player.User.id,
            };
            try
            {
                entity.Save();
                return ResultData((int)ResultType.SUCCESS);
            }
            catch { return ResultData((int)ResultType.DATABASE_ERROR); }

        }

        ASObject ResultData(int result)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",  result},
            };
            return new ASObject(dic);
        }
    }
}
