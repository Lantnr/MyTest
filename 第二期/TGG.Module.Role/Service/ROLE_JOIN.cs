using System.Collections.Generic;
using System.Linq;
using FluorineFx;
using System;
using FluorineFx.Messaging.Rtmp.SO;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using TGG.Core.Entity;
using NewLife.Log;
namespace TGG.Module.Role.Service
{
    /// <summary>
    /// 武将信息
    /// </summary>
    public class ROLE_JOIN
    {
        private static ROLE_JOIN _objInstance;

        /// <summary>ROLE_JOIN单体模式</summary>
        public static ROLE_JOIN GetInstance()
        {
            return _objInstance ?? (_objInstance = new ROLE_JOIN());
        }

        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            try
            {
#if DEBUG
                XTrace.WriteLine("{0}:{1}", "ROLE_JOIN", "加载武将数据");
#endif
                var player = session.Player;
                var list = tg_role.GetFindAllByUserId(player.User.id);       //获取该用户当前所有武将信息

                var listroles = new List<ASObject>();
                var ids = list.Select(m => m.id).ToList();
                //var roles = view_role.GetRoleById(ids);                  //所有武将视图信息
                var roles = (new Share.Role()).GetRoleByIds(ids);
                if (roles.Count == 0) return new ASObject(Common.GetInstance().RoleLoadData((int)ResultType.DATABASE_ERROR, null));

                var listtrainrs = tg_train_role.GetEntityByIds(ids);

                foreach (var item in roles)
                {
                    var trainrole = listtrainrs.FirstOrDefault(m => m.rid == item.Kind.id);
                    var genre = (new Share.Role()).LearnGenreOrNinja(item.FightSkill, item.Kind.role_genre, item.Kind.role_ninja);  //开启流派

                    listroles.Add(trainrole != null ?
                        AMFConvert.ToASObject((new Share.Role()).BuildRole(item, genre, trainrole)) :
                        AMFConvert.ToASObject((new Share.Role()).BuildRole(item, genre, null)));
                }
                return new ASObject(Common.GetInstance().RoleJoinData((int)ResultType.SUCCESS, player.UserExtend.train_bar, listroles,
                    EntityToVo.ToRoleHomeHireVo(player.UserExtend), player.UserExtend.power_buy_count));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return new ASObject();
            }
        }
    }
}