using System;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 退出活动
    /// </summary>
    public class EXIT
    {
        public static EXIT ObjInstance;

        /// <summary>EXIT单体模式</summary>
        public static EXIT GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new EXIT());
        }

        /// <summary>退出活动</summary>
        public ASObject CommandStart(TGGSession session)
        {
            var user = session.Player.User;
            var playerdata = Common.GetInstance().GetSiegePlayer(user.id, user.player_camp);
            playerdata.state = (int)SiegePlayerType.EXIT_DEFEND;

            PlayerExit(session);          //(SendData)
            RemoveActivityScene(user.id); //移除玩家活动场景数据

            PUSH_PLAYER_EXIT.GetInstance().CommandStart(session);//给其他玩家推送  该玩家离开美浓活动
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }

        /// <summary> 移除玩家活动场景数据 </summary>
        /// <param name="userid">用户Id</param>
        private void RemoveActivityScene(Int64 userid)
        {
            view_scene_user scene;
            Variable.Activity.ScenePlayer.TryRemove(Common.GetInstance().GetKey(userid), out scene);
        }

        /// <summary> 场景指令号修改 </summary>
        private void UpdateSence(TGGSession session)
        {
            var user = session.Player.User;
            var key = (int)ModuleNumber.SCENE + "_" + user.id;
            if (!Variable.SCENCE.ContainsKey(key)) return;
            var scence = Variable.SCENCE[key];
            if (scence == null) return;
            scence.model_number = (int)ModuleNumber.SCENE;
        }

        /// <summary>(SendData)玩家登录场景</summary>
        private void PlayerExit(TGGSession session)
        {
            UpdateSence(session);
            dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Scene", "LOGIN_ENTER_SCENE");
            var aso = obje.CommandStart(session, new ASObject());
            TrainingRoleEndSend(session, aso);
        }

        /// <summary>推送发送</summary>
        private void TrainingRoleEndSend(TGGSession session, ASObject data)
        {
            var pv = session.InitProtocol((int)ModuleNumber.SCENE, (int)SceneCommand.LOGIN_ENTER_SCENE, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
