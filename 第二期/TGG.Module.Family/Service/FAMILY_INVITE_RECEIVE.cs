using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Family;
using TGG.SocketServer;

namespace TGG.Module.Family.Service
{
    public class FAMILY_INVITE_RECEIVE
    {
        public static FAMILY_INVITE_RECEIVE ObjInstance;

        /// <summary>
        /// FAMILY_INVITE_RECEIVE单体模式
        /// </summary>
        public static FAMILY_INVITE_RECEIVE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new FAMILY_INVITE_RECEIVE());
        }

        /// <summary>接收邀请</summary>
        public void CommandStart(TGGSession session, tg_family family, TGGSession sessionb)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "FAMILY_INVITE_RECEIVE", "接收邀请");
#endif               
                var invitereceivevo = EntityToVo.ToInviteReceiveVo(family, sessionb.Player.User);
                var aso = new ASObject(BuilData(invitereceivevo));
                FamilySend(session, aso);  //推送协议
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary>组装数据</summary>
        private Dictionary<String, Object> BuilData(InviteReceiveVo invitereceivevo)
        {
            var dic = new Dictionary<string, object> {{"invitereceive", invitereceivevo}};
            return dic;
        }

        /// <summary>
        /// 推送家族信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        private void FamilySend(TGGSession session, ASObject data)
        {
#if DEBUG
            session.Logger.Info(string.Format("{0}:{1}", "FAMILY_INVITE_RECEIVE", "接收邀请发送协议"));
#endif
            var pv = session.InitProtocol((int)ModuleNumber.FAMILY, (int)FamilyCommand.FAMILY_INVITE_RECEIVE, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
