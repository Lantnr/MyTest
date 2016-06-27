using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.Siege.Service
{
    /// <summary>
    /// 防守或退出防守
    /// </summary>
    public class DEFEND
    {
        public static DEFEND ObjInstance;

        /// <summary>DEFEND单体模式</summary>
        public static DEFEND GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new DEFEND());
        }

        /// <summary> 防守或退出防守</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);
            var user = session.Player.User;
            switch (type)
            {
                case (int)SiegePlayerType.DEFEND:
                case (int)SiegePlayerType.EXIT_DEFEND: { return UpdatePlayState(user, type); }
                default: { return new ASObject(Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR)); }
            }
        }

        /// <summary> 修改玩家状态 </summary>
        /// <param name="user">用户信息</param>
        /// <param name="type">状态类型</param>
        /// <returns></returns>
        private ASObject UpdatePlayState(tg_user user, int type)
        {
            //if (!IsPosition(user)) return new ASObject(Common.GetInstance().BuildData((int)ResultType.POSITION_ERROR));
            var playerdata = Variable.Activity.Siege.PlayerData.FirstOrDefault(m => m.user_id == user.id);
            if (playerdata == null) return new ASObject(Common.GetInstance().BuildData((int)ResultType.NO_DATA));
            lock (this) { playerdata.state = type; }
            return new ASObject(Common.GetInstance().BuildData((int)ResultType.SUCCESS));
        }

        ///// <summary> 验证玩家位置 </summary>
        ///// <param name="user">用户信息</param>
        //private bool IsPosition(tg_user user)
        //{
        //    var npcsiege = Variable.BASE_NPCSIEGE.FirstOrDefault(m => m.type == (int)SiegeNpcType.GATE && m.camp == user.player_camp);
        //    if (npcsiege == null) return false;
        //    var xy = npcsiege.coorPoint.Split(',');
        //    if (xy.Length != 2) return false;
        //    var scene = Variable.Activity.ScenePlayer.FirstOrDefault(m => m.user_id == user.id);
        //    return Common.GetInstance().IsCoorPoint(xy, scene);
        //}
    }
}
