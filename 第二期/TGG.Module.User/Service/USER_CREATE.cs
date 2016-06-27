using FluorineFx;
using System;
using System.Linq;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using tg_user_login_log = TGG.Core.Entity.tg_user_login_log;
using tg_user = TGG.Core.Entity.tg_user;
using NewLife.Log;

namespace TGG.Module.User.Service
{
    /// <summary>
    /// 创建玩家
    /// </summary>
    public class USER_CREATE
    {
        private static USER_CREATE ObjInstance;

        /// <summary>USER_CREATE单例模式</summary>
        public static USER_CREATE GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new USER_CREATE());
        }

        /// <summary>创建玩家</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "ROLE_CREATE", "创建玩家");
#endif
            try
            {
                var userName = data.FirstOrDefault(q => q.Key == "userName").Value.ToString();
                var player_camp = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "camp").Value);                 //阵营
                var player_influence = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "influence").Value);       //玩家势力
                var player_vocation = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "vocation").Value);         //玩家职业
                var sex = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "sex").Value);
                var playerName = data.FirstOrDefault(q => q.Key == "playerName").Value.ToString();
#if DEBUG
                XTrace.WriteLine("{0} {1} - (0:东军1:西军) {2} - (1:武田家2:上杉家3:伊达家4:织田家5:岛津家6:德川家) {3} - (1:武士2:剑士3:忍者4:商人) {4} - (1:男2:女) {5}",
                    userName, player_camp, player_influence, player_vocation, sex, playerName);
#endif
                if (string.IsNullOrEmpty(userName)) return ResultError((int)ResultType.USER_SUBMIT_ERROR);
                if (!CommonHelper.CheckActivation(userName)) return ResultError((int)ResultType.ACTIVATION_ERROR);
                if (tg_user.GetUserCodeIsExist(userName))
                    return ResultError((int)ResultType.USER_NAME_ISEXIST);
                if (playerName.Length > 10)
                    return ResultError((int)ResultType.USER_NAME_LEN_MAX);
                if (tg_user.GetPlayerNameExist(playerName))
                    return ResultError((int)ResultType.USER_NAME_ISEXIST);
                if (player_influence == 0 || player_vocation == 0)
                    return ResultError((int)ResultType.USER_SUBMIT_ERROR);

                var rrid = Variable.BASE_ROLE.Where(q => q.jobType == player_vocation).ToList();
                if (!rrid.Any()) return ResultError((int)ResultType.BASE_TABLE_ERROR);
                //性别   SexType 1:男，2：女，0：未知
                var roleid = sex == (int)SexType.Male ? rrid.FirstOrDefault().id : rrid.LastOrDefault().id;

                var rbp = Variable.BASE_RULE.FirstOrDefault(q => q.id == "1001");
                if (rbp == null) return ResultError((int)ResultType.BASE_TABLE_ERROR);
                var birthplace = int.Parse(rbp.value); //读取角色出生地
                var scene = Variable.BASE_SCENE.FirstOrDefault(m => m.id == birthplace);
                if (scene == null) return ResultError((int)ResultType.BASE_TABLE_ERROR);
                var rb = Variable.BASE_ROLEBORNPOINT.FirstOrDefault(m => m.id == scene.bornPoint);
                if (rb == null) return ResultError((int)ResultType.BASE_TABLE_ERROR);
                var xy = rb.coorPoint.Split(',');
                var gold = 1000;    //默认注册账号元宝
                var user = tg_user.InitUser(userName, roleid, Convert.ToInt16(sex), playerName, birthplace, player_vocation,
                    player_influence, player_camp, birthplace, Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1]), gold);

                RecordSeesion(session, user);
                InsertGame(user.id);
                return new ASObject(Common.GetInstance().BuildDataUser((int)ResultType.SUCCESS, session.Player));

            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        #region  初始化私有方法

        private ASObject ResultError(int type)
        {
            return new ASObject(Common.GetInstance().BuildDataUser(type, null));
        }

        /// <summary>插入游艺园数据</summary>
        private void InsertGame(Int64 userid)
        {
            var game = new tg_game { user_id = userid };
            tg_game.Insert(game);
        }

        /// <summary>记录session</summary>
        private void RecordSeesion(TGGSession session, tg_user user)
        {
            session.Player = Common.GetInstance().GetPlayer(user);
            tg_user_login_log.LoginLog(user.id, Common.GetInstance().GetRemoteIP(session));
            Variable.OnlinePlayer.AddOrUpdate(session.Player.User.id, session, (k, v) => session);
        }


        #endregion
    }
}
