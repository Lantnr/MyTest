using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Entity;
using TGG.SocketServer;

namespace TGG.Module.War.Service
{
    /// <summary>
    /// 拉取地形和地形设定数据
    /// 开发者：李德雁
    /// </summary>
    public class AREA_JOIN:IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~AREA_JOIN()
        {
            Dispose();
        }
    
        #endregion

        //private static AREA_JOIN _objInstance;

        ///// <summary>AREA_JOIN单体模式</summary>
        //public static AREA_JOIN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new AREA_JOIN());
        //}

        /// <summary> 拉取地形和地形设定数据 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var userid = session.Player.User.id;
            var list = view_user_area_set.GetEntityByUserId(userid);
            var userarea = tg_war_user_area.GetEntityByUserId(userid);

            return BuildData(list, userarea);
        }

        /// <summary>
        /// 组装数据
        /// </summary>
        /// <param name="list">玩家地形设定集合</param>
        /// <param name="userarea">玩家地形集合</param>
        /// <returns></returns>
        private ASObject BuildData(List<view_user_area_set> list, List<tg_war_user_area> userarea)
        {
            var dic = new Dictionary<string, object>()
            {
                {"list", Common.GetInstance().ConvertListASObject(list)},
                {"area", userarea.Select(q => q.base_id).ToList()},
                {"count",list.Select(q=>q.id).Distinct().Count()}
            };
            return new ASObject(dic);
        }
    }
}
