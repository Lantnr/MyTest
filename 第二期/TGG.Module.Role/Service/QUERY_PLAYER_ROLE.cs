using System;
using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Vo.Role;
using TGG.SocketServer;

namespace TGG.Module.Role.Service
{

    /// <summary>
    /// 查看玩家信息
    /// </summary>
    public class QUERY_PLAYER_ROLE
    {
        private static QUERY_PLAYER_ROLE _objInstance;

        /// <summary>QUERY_PLAYER_ROLE单体模式</summary>
        public static QUERY_PLAYER_ROLE GetInstance()
        {
            return _objInstance ?? (_objInstance = new QUERY_PLAYER_ROLE());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "QUERY_PLAYER_ROLE", "查看玩家信息");
#endif
                var playerId = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "userId").Value.ToString());

                var user = tg_user.GetUsersById(playerId);
                if (user == null) return Result((int)ResultType.ROLE_QUERY_NOPLAYER);  //验证是否存在该玩家信息
                if (playerId == session.Player.User.id)
                    return Result((int)ResultType.ROLE_QUERY_MYERROR);   //验证要查看的玩家是否为当前玩家本身

                var lists = tg_role.GetFindAllByUserId(playerId);
                if (!lists.Any()) return Result((int)ResultType.ROLE_QUERY_NOINFO);   //验证要查看玩家的武将信息

                var ids = lists.Select(m => m.id).ToList();
                //var listview = view_role.GetRoleById(ids);
                var roles = (new Share.Role()).GetRoleByIds(ids);

                if (!roles.Any()) return Result((int)ResultType.ROLE_QUERY_VIEWERROR);   //验证武将视图信息
                var listroles = roles.Select(item => AMFConvert.ToASObject(ConvertToVo(item))).ToList();

                var equips = tg_bag.GetEquipByUserIdStateType(playerId, (int)LoadStateType.LOAD, (int)GoodsType.TYPE_EQUIP);  //查询玩家武将穿戴的装备
                var listequips = equips.Select(item => AMFConvert.ToASObject(EntityToVo.ToEquipVo(item))).ToList();

                return new ASObject(BuildInfo((int)ResultType.SUCCESS, user.player_name, user.player_camp, user.player_influence, listroles, listequips));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }

        /// <summary>组装武将vo信息</summary>
        private RoleInfoVo ConvertToVo(RoleItem role)
        {
            var equip = (new Share.Role()).GetEquips(role.Kind);     //获取武将装备集合
            var fightskillvo = (new Share.Role()).ConvertFight(role.FightSkill);
            var rolevo = EntityToVo.ToRoleVo(role.Kind, equip, fightskillvo, null, null, null, null);
            return rolevo;
        }

        /// <summary>组装错误信息</summary>
        private ASObject Result(int result)
        {
            return new ASObject(Common.GetInstance().RoleLoadData(result, null));
        }

        /// <summary>组装返回前端信息</summary>
        private Dictionary<String, Object> BuildInfo(int result, string name, int camp, int influence, List<ASObject> listroles, List<ASObject> listequips)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, { "name", name }, { "camp", camp }, { "influence", influence }, 
                { "roles", listroles.Any() ? listroles : null }, { "equips", listequips.Any() ? listequips : null }
            };
            return dic;
        }
    }
}
