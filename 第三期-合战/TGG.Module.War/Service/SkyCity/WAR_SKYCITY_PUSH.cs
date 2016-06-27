using System;
using System.Collections.Generic;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.SkyCity
{
    /// <summary>
    /// 内政 军事完成推送
    /// </summary>
    public class WAR_SKYCITY_PUSH:IDisposable
    {
        //private static WAR_SKYCITY_PUSH _objInstance;

        ///// <summary> WAR_SKYCITY_PUSH单体模式 </summary>
        //public static WAR_SKYCITY_PUSH GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_SKYCITY_PUSH());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 内政 军事完成推送资源更新</summary>
        public void ResourcePush(int baseid, tg_war_role role)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_SKYCITY_PUSH", "内政 军事完成推送");
#endif

            if (!Variable.OnlinePlayer.ContainsKey(role.user_id)) return;
            var session = Variable.OnlinePlayer[role.user_id] as TGGSession;
            if (session == null) return;

            if (session.Player.War.PlayerInCityId != role.station) return;
            (new Share.War()).SendCity(baseid, role.user_id);   //推送据点数据更新  

            if (session.Player.War.Status == 0) return;

            //推送天守阁数据更新
            //if (role.state == (int) WarRoleStateType.IDLE)
            //{
            var skyVo = EntityToVo.ToSkyCityVo(role);
            var data = new ASObject(new Dictionary<string, object> { { "skyCity", skyVo } });
            var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.WAR_SKYCITY_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
            //}
        }
    }
}
