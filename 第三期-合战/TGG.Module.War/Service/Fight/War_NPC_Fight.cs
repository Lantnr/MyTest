using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Entity;
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Fight
{
    ///// <summary>
    ///// NPC城战斗
    ///// </summary>
    //public class War_NPC_Fight
    //{
    //    private static WAR_FIGHT _objInstance;
    //    /// <summary>WAR_FIGHT单体模式</summary>
    //    public static WAR_FIGHT GetInstance()
    //    {
    //        return _objInstance ?? (_objInstance = new WAR_FIGHT());
    //    }

    //    /// <summary> 城战斗 </summary>
    //    public ASObject CommandStart(TGGSession session, ASObject data)
    //    {
    //        if (!data.ContainsKey("roles") || !data.ContainsKey("frontId")) return null;
    //        var roles = data.FirstOrDefault(m => m.Key == "roles").Value as object[];
    //        var sendfrontid = data.FirstOrDefault(m => m.Key == "frontId").Value.ToString();
    //        var baseid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "cityid").Value);
    //        Int32 frontid;
    //        Int32.TryParse(sendfrontid, out frontid);
    //        var lines = GetSendRoles(roles);
    //        if (frontid == 0 || !lines.Any()) return null;
    //        //验证坐标
    //        // if (!Common.GetInstance().CheckPoint(lines, planid)) return null;
    //        //验证武将
    //        var tuple = CheckRoles(lines); if (!tuple.Item1) return null;

    //        //得到战斗过程 
    //        var fightvo = new FightProcess().GetFightProcess(planid, lines, tuple.Item2, session.Player.User.id, queue.end_CityId, frontid, queue.morale);
    //    }

    //    /// <summary>
    //    /// 解析前端的数据
    //    /// </summary>
    //    /// <param name="list">武将线路集合</param>
    //    /// <returns></returns>
    //    private List<WarRolesLinesVo> GetSendRoles(IEnumerable<object> list)
    //    {
    //        return list.Select(AMFConvert.AsObjectToVo<WarRolesLinesVo>).ToList();

    //    }


        ///// <summary>    
        ////验证武将是否存在
        ///// </summary>
        ///// <param name="lines"></param>
        ///// <returns></returns>
        //private Tuple<bool, List<tg_war_role>> CheckRoles(List<WarRolesLinesVo> lines)
        //{
        //    //查询武将实体
        //    var rids = lines.Select(q => q.rid).Distinct().ToList();
        //    var warrolelist = tg_war_role.GetEntityListByIds(rids);

        //    if (warrolelist.Count != rids.Count) return Tuple.Create(false, warrolelist);
        //    return Tuple.Create(true, warrolelist);

        //}

   // }
}
