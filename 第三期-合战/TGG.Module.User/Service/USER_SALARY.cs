using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 身份俸禄领取指令
    /// </summary>
    public class USER_SALARY:IDisposable
    {
         #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~USER_SALARY()
        {
            Dispose();
        }
    
        #endregion

        //private static USER_SALARY ObjInstance;
        ///// <summary>USER_SALARY单例模式</summary>
        //public static USER_SALARY GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new USER_SALARY());
        //}
        /// <summary>身份俸禄领取指令</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User.CloneEntity();
            var extend = session.Player.UserExtend;
            var role = session.Player.Role.Kind;

            if (extend == null) return new ASObject(BuildData((int)ResultType.DATABASE_ERROR));
            if (extend.salary_state == (int)SalaryStateType.RECEIVE) return new ASObject(BuildData((int)ResultType.USER_RECEIVE));

            var baseident = Variable.BASE_IDENTITY.FirstOrDefault(m => m.vocation == user.player_vocation && m.id == role.role_identity);
            if (baseident == null) return new ASObject(BuildData((int)ResultType.BASE_TABLE_ERROR));

            user.coin = tg_user.IsCoinMax(user.coin, baseident.salary);
            extend.salary_state = (int)SalaryStateType.RECEIVE;

            user.Update();
            extend.Update();

            session.Player.User = user;
            session.Player.UserExtend = extend;

            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_COIN, session.Player.User);
            return new ASObject(BuildData((int)ResultType.SUCCESS, (int)SalaryStateType.RECEIVE));
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result)
        {
            return BuildData(result, (int)SalaryStateType.RECEIVE);
        }

        /// <summary>数据组装</summary>
        public Dictionary<String, Object> BuildData(int result, int state)
        {
            var dic = new Dictionary<string, object>
            {
                {"result",  result},
                {"state", state}
            };
            return dic;
        }
    }
}
