using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using NewLife.Log;
using TGG.Core.AMF;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{

    /// <summary>
    /// 进入合战武将宅
    /// arlen
    /// </summary>
    public class WAR_HOME_JOIN : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
        //private static WAR_HOME_JOIN _objInstance;

        ///// <summary>WAR_HOME_JOIN单体模式</summary>
        //public static WAR_HOME_JOIN GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_HOME_JOIN());
        //}

        /// <summary> 进入合战武将宅开始指令处理 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "WAR_HOME_JOIN", "进入合战武将宅");
#endif
            if (!data.ContainsKey("id")) return null;
            var station = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);        //主键id
            if (station == 0) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);

            var user_id = session.Player.User.id;

            var list = view_war_role.GetFindRole(user_id, station);
            var vo = new List<view_war_role>();
            foreach (var item in list)
            {
                var entity = item;
                if (entity.type == 1) //备大将
                {
                    //
                    var ide = Variable.BASE_IDENTITY.FirstOrDefault(m => m.vocation == (int)VocationType.Roles);
                    entity.role_id = Convert.ToInt32(entity.rid);
                    entity.role_identity = ide != null ? ide.id : 1038;
                    entity.role_state = (int)RoleStateType.READY;
                    entity.power = 0;
                    var role = Variable.BASE_ROLE.FirstOrDefault(m => m.id == entity.role_id);
                    entity.player_name = role != null ? role.name : "--";
                }
                else
                {
                    if (item.role_state != 1) //不是主角武将
                    {
                        var role = Variable.BASE_ROLE.FirstOrDefault(m => m.id == entity.role_id);
                        entity.player_name = role != null ? role.name : "--";

                    }
                }
                vo.Add(entity);
            }
            return new ASObject(BuildData((int)ResultType.SUCCESS, vo));
        }

        /// <summary>仓库物品信息集合</summary>
        private List<ASObject> TolistRoles(IEnumerable<view_war_role> depots)
        {
            var war_role = new List<ASObject>();
            war_role.AddRange(depots.Select(item => AMFConvert.ToASObject(EntityToVo.ToViewWarRoleVo(item))));
            return war_role;
        }

        private Dictionary<String, Object> BuildData(int result, List<view_war_role> list)
        {
            var dic = new Dictionary<string, object>
            {
                { "result", result }, 
                { "list", list.Any() ? TolistRoles(list) : null }, 
            };
            return dic;
        }



    }
}
