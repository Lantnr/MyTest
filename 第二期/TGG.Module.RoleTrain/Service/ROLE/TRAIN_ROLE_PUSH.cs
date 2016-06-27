using FluorineFx;
using NewLife.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 修行完成推送
    /// </summary>
    public class TRAIN_ROLE_PUSH
    {
        private static TRAIN_ROLE_PUSH ObjInstance;

        /// <summary> TRAIN_ROLE_PUSH单体模式 </summary>
        public static TRAIN_ROLE_PUSH GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new TRAIN_ROLE_PUSH());
        }

        /// <summary>修行完成推送</summary>
        public void CommandStart(TGGSession session, RoleItem roleitem)
        {
# if DEBUG
            XTrace.WriteLine("{0}:{1}", "TRAIN_ROLE_PUSH", "修行完成推送 ");
#endif
            //RoleTrainPush(session.Player.User.id, roleitem.Kind.id);
# if DEBUG
            XTrace.WriteLine("{0}", "修行完成推送 ");
#endif
        }
        #region  注释

        //        public void RoleTrainPush(Int64 user_id, Int64 rid)
//        {
//            //var role = new RoleItem();
//            var role = tg_role.GetEntityById(rid);
//            if (role == null) return;
//            var user = tg_user.GetUsersById(user_id);
//            if (user == null) return;

//            var train_role = tg_train_role.GetEntityByRid(role.id);
//            if (train_role == null || train_role.state != (int)RoleTrainStatusType.TRAINING || train_role.time == 0) return;
//            var basetrain = Variable.BASE_TRAIN.FirstOrDefault(m => m.id == train_role.type);
//            train_role.state = (int)RoleTrainStatusType.FREE;
//            train_role.time = 0;
//            var att = Common.GetInstance().GetCanAtt(user, role, train_role.attribute, basetrain);
//            var att2 = tg_role.GetSingleCanTrain((RoleAttributeType)train_role.attribute, att, role);
//            role = Common.GetInstance().RoleAtt(role, train_role, att2);
//            tg_train_role.GetUpdate(train_role);
//            tg_role.GetRoleUpdate(role);

//            if (!Variable.OnlinePlayer.ContainsKey(user_id)) return;
//            //向在线玩家推送数据
//            var session = Variable.OnlinePlayer[user_id] as TGGSession;
//            if (session == null) return;
//            if (role.id == session.Player.Role.Kind.id)
//            {
//                session.Player.Role.Kind = role;
//            }
//            TrainRolePush(session, BuildData(rid));
//        }



//        /// <summary>组装数据</summary>
//        private ASObject BuildData(Int64 rid)//, List<int> genretypearr, tg_train_role roletrain)
//        {
//            var dic = new Dictionary<string, object> { { "role", Common.GetInstance().RoleInfo(rid) } };//, genretypearr,roletrain) } };
//            return new ASObject(dic);
//        }

//        /// <summary>发送修行结束协议</summary>
//        private static void TrainRolePush(TGGSession session, ASObject data)
//        {
//#if DEBUG
//            XTrace.WriteLine("{0}:{1}", "TRAIN_ROLE_PUSH", "武将修行结束协议发送");
//#endif
//            var pv = session.InitProtocol((int)ModuleNumber.ROLETRAIN, (int)TGG.Core.Enum.Command.RoleTrainCommand.TRAIN_ROLE_PUSH, (int)ResponseType.TYPE_SUCCESS, data);
//            session.SendData(pv);
        //        }
        #endregion
    }
}
