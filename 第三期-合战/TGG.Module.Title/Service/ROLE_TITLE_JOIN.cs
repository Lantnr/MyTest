using System;
using System.Collections.Generic;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Title.Service
{
    /// <summary>
    /// 称号信息
    /// </summary>
    public class ROLE_TITLE_JOIN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~ROLE_TITLE_JOIN()
        {
            Dispose();
        }
        #endregion

        //private static ROLE_TITLE_JOIN ObjInstance;

        ///// <summary>ROLE_TITLE_JOIN单体模式</summary>
        //public static ROLE_TITLE_JOIN GetInstance()
        //{
        //    return ObjInstance ?? (ObjInstance = new ROLE_TITLE_JOIN());
        //}

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_TITLE_JOIN", "称号信息");
#endif
                var user = session.Player.User;
                var extend = session.Player.UserExtend;
                var title = tg_role_title.GetTitlesByUserId(user.id, (int)TitleState.HAS_BEEN_REACHED);     //拉取已达成称号信息

                var value = ListCount(extend);
                return new ASObject(Common.GetInstance().BuildTitlesData((int)ResultType.SUCCESS, value, title));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>组装返回的次数list集合</summary>
        private List<int> ListCount(tg_user_extend extend)
        {
            var value = new List<int>
            {
                extend.sword_win_count,
                extend.gun_win_count,
                extend.tea_table_count,
                extend.bargain_success_count
            };
            return value;
        }
    }
}
