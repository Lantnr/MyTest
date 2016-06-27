using System;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service
{
    /// <summary>
    /// 移除线路
    /// </summary>
    public class WAR_FIGHT_LINE_REMOVE : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_FIGHT_LINE_REMOVE()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_FIGHT_LINE_REMOVE _objInstance;

        ///// <summary>WAR_FIGHT_LINE_REMOVE单体模式</summary>
        //public static WAR_FIGHT_LINE_REMOVE GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_FIGHT_LINE_REMOVE());
        //}

        /// <summary> 移除线路 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "WAR_FIGHT_LINE_REMOVE", "移除线路");
#endif
                if (!data.ContainsKey("roles") || !data.ContainsKey("type")) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);

                var rid = Convert.ToInt64(data.FirstOrDefault(q => q.Key == "roles").Value.ToString());
                var type = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "type").Value.ToString());

                var role = tg_role.FindByid(rid);
                if (role == null) return CommonHelper.ErrorResult((int)ResultType.ROLE_NOT_EXIST);

                Variable.RemoveLine(role.user_id, type, rid);   //移除用户武将合战线路
                return CommonHelper.SuccessResult();
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }
    }
}
