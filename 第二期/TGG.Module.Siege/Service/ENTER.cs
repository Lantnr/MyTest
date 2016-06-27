using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using System;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.Scene;
using TGG.SocketServer;
using TGG.Core.Enum.Command;
using System.Threading.Tasks;
using System.Threading;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 进入活动
    /// </summary>
    public class ENTER
    {
        private static ENTER _objInstance;

        /// <summary>ENTER单体模式</summary>
        public static ENTER GetInstance()
        {
            return _objInstance ?? (_objInstance = new ENTER());
        }

        /// <summary>进入活动</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var user = session.Player.User;
            var role = session.Player.Role.Kind;
            if (!Variable.Activity.Siege.IsOpen) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_NO_OPEN));//验证是否开启活动
            //if (!CheckIdentify(role.role_identity, user.player_vocation)) return new ASObject(Common.GetInstance().BuildData((int)ResultType.SIEGE_IDENTIFY_ERROR)); //玩家身份验证

            var boss1 = GetBoss((int)CampType.East);//东军Boss
            var boss2 = GetBoss((int)CampType.West);//西军Boss
            if (boss1 == null || boss2 == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.BASE_TABLE_ERROR));

            UpdateSence(user.id);  //修改普通场景模块号，并推送玩家离开普通场景
            var playdata = Common.GetInstance().GetSiegePlayer(user.id, user.player_camp);     //获取玩家活动数据
            var scene = Common.GetInstance().UpdateUserScene(user, role.role_level);       //回到出生点

            var otherplays = GetOtherPlayerPush(playdata, scene);  //获取场景其他玩家并Push

            return new ASObject(BuildData((int)ResultType.SUCCESS, playdata, BuildBossData(boss1), BuildBossData(boss2), otherplays));
        }

        #region 私有方法

        /// <summary> 玩家身份验证 </summary>
        private Boolean CheckIdentify(int identity, int vocation)
        {
            //验证玩家身份
            var needidentify = Variable.BASE_IDENTITY.Where(q => q.vocation == vocation).Skip(2).FirstOrDefault();
            if (needidentify == null) return false;
            if (identity < needidentify.id) return false;
            return true;
        }

        /// <summary> 场景指令号修改，并推送玩家离开普通场景 </summary>
        private void UpdateSence(Int64 userid)
        {
            var key = (int)ModuleNumber.SCENE + "_" + userid;
            if (!Variable.SCENCE.ContainsKey(key)) return;
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var scence = Variable.SCENCE[key];
            if (scence == null) return;
            //var session = Variable.OnlinePlayer[userid];
            scence.model_number = (int)ModuleNumber.SIEGE;
            PlayerExit(userid, scence.scene_id);
        }

        /// <summary>推送玩家离开普通场景</summary>
        private void PlayerExit(Int64 userid, Int64 sceneid)
        {
            var otherplays = Core.Common.Scene.GetOtherPlayers(userid, sceneid, (int)ModuleNumber.SCENE);
            foreach (var item in otherplays.Select(m => m.user_id))
            {
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(
                    m =>
                    {
                        dynamic obje = CommonHelper.ReflectionMethods("TGG.Module.Scene", "PLAYER_EXIT_SCENET");
                        obje.CommandStart(userid, Convert.ToInt64(m));
                    }, item, token.Token);
            }
        }

        /// <summary>获取场景内其他玩家数据 并将当前玩家数据推送给其他玩家</summary>
        private List<ScenePlayerVo> GetOtherPlayerPush(SiegePlayer model, view_scene_user scene)
        {
            var otherplays = new List<ScenePlayerVo>();

            var otherplayer = Variable.Activity.ScenePlayer.Where(m =>
                m.Value.model_number == (int)ModuleNumber.SIEGE && m.Key != Common.GetInstance().GetKey(model.user_id)).ToList();
            if (!otherplayer.Any()) return otherplays;

            var aso = new ASObject(BuildData(EntityToVo.ToScenePlayerVo(scene)));

            foreach (var item in otherplayer)
            {
                otherplays.Add(EntityToVo.ToScenePlayerVo(item.Value));
                var token = new CancellationTokenSource();
                Task.Factory.StartNew(
                    m =>
                    {
                        var userid = Convert.ToInt64(m);
                        if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
                        var session = Variable.OnlinePlayer[userid];
                        Common.GetInstance().TrainingSiegeEndSend(session, aso, (int)SiegeCommand.PUSH_PLAYER_ENTER);

                    }, item.Value.user_id, token.Token);
            }
            return otherplays;
        }

        /// <summary> 获取指定阵营Boss数据 </summary>
        /// <param name="camp">阵营</param>
        private Boss GetBoss(int camp)
        {
            var boss = Variable.Activity.Siege.BossCondition.FirstOrDefault(m => m.player_camp == camp);
            if (boss == null) return null;
            return new Boss
            {
                gateHp = boss.GateLife,
                npcHp = boss.BossLife,
                coreHp = boss.BaseLife,
            };
        }

        #endregion

        #region 组装数据

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(ScenePlayerVo model)
        {
            var dic = new Dictionary<string, object>
            {
                {"playerVo", model}
            };
            return dic;
        }

        /// <summary> 组装前端需要的Boss数据 </summary>
        private Dictionary<string, Object> BuildBossData(Boss model)
        {
            var dic = new Dictionary<string, object>
            {
                {"gateHp", model.gateHp},
                {"npcHp", model.npcHp},
                {"coreHp", model.coreHp},
            };
            return dic;
        }

        /// <summary>数据组装</summary>
        private Dictionary<String, Object> BuildData(int result, SiegePlayer player, Dictionary<string, Object> dataA, Dictionary<string, Object> dataB, List<ScenePlayerVo> list)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"ladder", player.count},
                {"fame", player.fame},
                {"dataA", dataA},
                {"dataB", dataB},
                {"playerList", list},
            };
            return dic;
        }

        #endregion

    }

    /// <summary> 前端需要的Boss实体对象 </summary>
    public class Boss
    {
        /// <summary> 城门血量 </summary>
        public int gateHp { get; set; }

        /// <summary> 大将血量 </summary>
        public Int64 npcHp { get; set; }

        /// <summary> 本丸血量 </summary>
        public int coreHp { get; set; }
    }
}
