using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Share
{
    public partial class War
    {
        /// <summary>关闭天守阁武将线程</summary>
        public void BreakTask(Int64 cityId)
        {
            var city = tg_war_city.FindByid(cityId);  //查询据点信息
            if (city == null) return;

            var states = GetStates();
            var roles = tg_war_role.GetSkyTaskRole(city.user_id, city.base_id, states);

            if (!roles.Any()) return;

            foreach (var key in roles.Select(item => String.Format("{0}_{1}_{2}", (int)CDType.SkyCity, item.user_id, item.id)))
            {
                bool r;
                Variable.CD.TryRemove(key, out r);
            }
        }

        /// <summary>天守阁任务状态集合</summary>
        private List<int> GetStates()
        {
            var states = new List<int>
            {
                (int) WarRoleStateType.MINING,
                (int) WarRoleStateType.PEACE,
                (int) WarRoleStateType.ASSART,
                (int) WarRoleStateType.BUILDING,
                (int) WarRoleStateType.BUILD_ADD,
                (int) WarRoleStateType.TRAIN,
                (int) WarRoleStateType.LEVY
            };
            return states;
        }

        /// <summary>推送最后一个城被攻陷</summary>
        /// <param name="userId">要推送的用户id</param>
        /// <param name="attackid">进攻者id</param>
        public void PushCityAssault(Int64 userId, Int64 attackid)
        {
            var attack = tg_user.FindByid(attackid);
            if (attack == null) return;
            var attackName = attack.player_name;
            if (!Variable.OnlinePlayer.ContainsKey(userId))
            {
                (new Message()).BuildMessagesSend(userId, "据点失陷战报", "您的居城被" + attackName + "玩家攻陷，当前没有据点城市，部队解散", "");
            }
            else
            {
                var session = Variable.OnlinePlayer[userId] as TGGSession;
                if (session == null)
                {
                    (new Message()).BuildMessagesSend(userId, "据点失陷战报", "您的居城被" + attackName + "玩家攻陷，当前没有据点城市，部队解散", "");
                }
                else
                {
                    session.Player.War.WarCityId = 0;   //清除session 主据点信息
                    var state = 0;
                    var iswar = Variable.WarInUser.ContainsKey(userId);
                    if (iswar) state = 1;

                    var name = session.Player.User.player_name;
                    var data = new ASObject(new Dictionary<string, object>()
                            {
                                {"attackerName", name},
                                {"state",state},
                                {"result", (int) ResultType.SUCCESS},
                            });
                    var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.PUSH_WAR_CITY_NOEXIST, (int)ResponseType.TYPE_SUCCESS, data);
                    session.SendData(pv);
                }
            }         
        }
    }
}
