using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Entity;

namespace TGG.Module.RoleTrain.Service
{
    /// <summary>
    /// 武将难度选择
    /// </summary>
    public class TRAIN_HOME_LEVEL_SELECT
    {
        private static TRAIN_HOME_LEVEL_SELECT _objInstance;

        /// <summary>TRAIN_HOME_LEVEL_SELECT单体模式</summary>
        public static TRAIN_HOME_LEVEL_SELECT GetInstance()
        {
            return _objInstance ?? (_objInstance = new TRAIN_HOME_LEVEL_SELECT());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "TRAIN_HOME_LEVEL_SELECT", "武将难度选择");
#endif
                var lv = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "lv").Value.ToString());  //将册难度
                var level = session.Player.Role.Kind.role_level;
                var userid = session.Player.User.id;
                var sid = session.Player.Scene.scene_id;

                if (!(new Share.RoleTrain()).IsLevelOk(lv, level))     //判断用户等级
                    return Error((int)ResultType.BASE_PLAYER_LEVEL_ERROR);

                var npcs = tg_train_home.GetByUserIdCityIdLevel(userid, sid, lv);
                if (npcs.Any()) return new ASObject(Common.GetInstance().NpcBulidData((int)ResultType.SUCCESS, npcs));

                //获取当前居城难度将册NPC信息
                var basenpcs = (new Share.RoleTrain()).RandomNpc(Convert.ToInt32(sid), lv);
                if (!basenpcs.Any()) return Error((int)ResultType.BASE_TABLE_ERROR);

                if (!(new Share.RoleTrain()).InsertNpc(basenpcs, userid)) return Error((int)ResultType.DATABASE_ERROR);

                var listsnpc = tg_train_home.GetByUserIdCityIdLevel(userid, sid, lv);
                return new ASObject(Common.GetInstance().NpcBulidData((int)ResultType.SUCCESS, listsnpc));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>返回错误信息</summary>
        private ASObject Error(int error)
        {
            return new ASObject(new Dictionary<string, object> { { "result", error } });
        }
    }
}
