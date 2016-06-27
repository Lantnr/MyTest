using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.AMF;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.Core.Vo.War;
using TGG.Share;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Defence
{
    /// <summary>
    /// 修改武将防守信息
    /// </summary>
    public class WAR_DEFENCE_PLAN_UPDATE : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DEFENCE_PLAN_UPDATE()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DEFENCE_PLAN_UPDATE _objInstance;

        ///// <summary>WAR_DEFENCE_ROLE_UPDATE单体模式</summary>
        //public static WAR_DEFENCE_PLAN_UPDATE GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DEFENCE_PLAN_UPDATE());
        //}

        /// <summary> 修改武将防守方案信息 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("planId") || !data.ContainsKey("areaSetId") || !data.ContainsKey("frontId") ||
                !data.ContainsKey("roles"))
                return null;
            //玩家防守方案主键id
            var sendplanId = data.FirstOrDefault(q => q.Key == "planId").Value.ToString();
            Int64 planid = 0; Int32 areasetid = 0; Int32 front = 0;
            if (!Int64.TryParse(sendplanId, out planid)) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);
            //获取防守地形
            var sendareasetid = data.FirstOrDefault(m => m.Key == "areaSetId").Value.ToString(); //玩家设置地形主键id
            if (!Int32.TryParse(sendareasetid, out areasetid)) return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);

            //获取防守阵
            var sendfront = data.FirstOrDefault(m => m.Key == "frontId").Value.ToString(); //玩家设置阵基表id
            Int32.TryParse(sendfront, out front);


            var planvo = GetSendVo(planid, front);
            var roles = data.FirstOrDefault(m => m.Key == "roles").Value as object[];
            var listroles = GetSendRoles(roles);

            if (listroles == null || planvo == null) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);


            var areasetlist = view_user_area_set.GetEntityByUserId(session.Player.User.id, areasetid).ToList();
            //验证前端数据数据
            var tuple = CheckData(listroles, planvo, session.Player.User.id, areasetid);
            if (!tuple.Item1)
                return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);
            //验证坐标信息
            if (!CheckPoint(listroles, areasetlist)) return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR);

           // if (!CheckFund(session.Player.War.City)) return CommonHelper.ErrorResult(ResultType.WAR_RES_FUNDS_ERROR);
            return !SavaData(tuple.Item3, tuple.Item2, listroles, planvo, areasetlist) ?
                CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR) :
                CommonHelper.SuccessResult();
        }

        /// <summary>
        /// 解析前端的vo
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="frontid"></param>
        /// <returns></returns>
        private WarDefencePlanVo GetSendVo(Int64 planid, Int32 frontid)
        {
            return new WarDefencePlanVo()
            {
                id = planid,
                roles = null,
                frontId = frontid,
            };
        }

        /// <summary>
        /// 解析前端的防守武将vo集合
        /// </summary>
        /// <returns></returns>
        private List<DefenceRoleVo> GetSendRoles(IEnumerable<object> list)
        {
            return list.Select(AMFConvert.AsObjectToVo<DefenceRoleVo>).ToList();

        }

        #region 验证数据

        /// <summary>
        /// 验证前端发送的数据
        /// </summary>
        /// <param name="listvo">防守武将集合</param>
        /// <param name="planvo">防守方案vo</param>
        /// <param name="userid">用户id</param>
        /// <param name="areaid">地形设定主键id</param>
        /// <returns>bool:数据是否正确 int64:地形id</returns>
        private Tuple<bool, tg_war_city_plan, List<tg_war_city_defense>> CheckData(List<DefenceRoleVo> listvo, WarDefencePlanVo planvo, Int64 userid, Int64 areaid)
        {
            //验证防守方案
            var plan = tg_war_city_plan.GetEntityByPlanId((long)planvo.id,userid);
            if (plan == null) return Tuple.Create(false, plan, new List<tg_war_city_defense>());
            var defenselist = tg_war_city_defense.GetListByPlanId(plan.id);
            //验证阵
            if (tg_war_formation.GetEntityByUserIdAndBaseId(userid, planvo.frontId) == null)
                return Tuple.Create(false, plan, defenselist);
            //验证地形
            //if (tg_war_area.FindByid(areaid) == null)
            //    return Tuple.Create(false, plan, defenselist);
            //验证武将
            return Tuple.Create(!listvo.Any(vo => defenselist.All(q => q.role_id != vo.id)), plan, defenselist);

        }


        /// <summary>
        /// 验证战争地图坐标数据
        /// </summary>
        /// <param name="lsitvo">前端提交防守武将集合</param>
        /// <param name="areaid">地形设定id</param>
        /// <returns></returns>
        private bool CheckPoint(List<DefenceRoleVo> lsitvo, List<view_user_area_set> areasetlist)
        {
            var pointlist = new List<Common.Point>();
            //武将的坐标
            foreach (var item in lsitvo)
            {
                //本丸城门不能设置
                if (item.pointX > 15 || item.pointX < 0 || item.pointY < 0 || item.pointY > 8
                    || item.pointX == 2 || (item.pointX == 0 && item.pointY == 4)) return false;
                pointlist.Add(new Common.Point() { X = item.pointX, Y = item.pointY });
            }

            //将地形设定转换成战争地形设定
            var setlist = new List<tg_war_area_set>();

            foreach (var set in areasetlist)
            {
                setlist.Add(new tg_war_area_set()
                {
                    base_id = set.base_id,
                    base_point_x = set.base_point_x,
                    base_point_y = set.base_point_y,
                    type = set.type

                });
            }

            var maplist = Common.GetInstance().GetMapArea(setlist).Where(q => q.type != (int)AreaType.陷阱);
            pointlist.AddRange(maplist.Where(q => q.type == (int)AreaType.山脉).Select(mapArea => new Common.Point() { X = mapArea.X, Y = mapArea.Y }));
            var test1 = pointlist.GroupBy(q => new { q.X, q.Y }).Where(g => g.Count() > 1).ToList();

            return !pointlist.GroupBy(q => new { q.X, q.Y }).Any(g => g.Count() > 1);
        }

        #endregion

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="roles">防守武将实体集合</param>
        /// <param name="plan">防守方案实体</param>
        /// <param name="rolesvo">前端传输武将集合</param>
        /// <param name="planvo">前端传输方案vo</param>
        /// <param name="areasetid">地形设定主键id</param>
        /// <returns></returns>
        private bool SavaData(List<tg_war_city_defense> roles, tg_war_city_plan plan, List<DefenceRoleVo> rolesvo, WarDefencePlanVo planvo, List<view_user_area_set> areasetlist)
        {
            //更新方案表 
            plan.formation = planvo.frontId;
            plan.Update();
            //更新武将表
            foreach (var vo in rolesvo)
            {
                var entity = roles.FirstOrDefault(q => q.role_id == vo.id);
                if (entity.point_x == vo.pointX && entity.point_y == vo.pointY) continue;
                entity.point_x = vo.pointX;
                entity.point_y = vo.pointY;
            }
            tg_war_city_defense.GetListUpdate(roles);
            //更新地形设定
            SaveAreaSetData(areasetlist, plan.id);
            return true;
        }

        /// <summary>
        /// 保存防守方案地形数据
        /// </summary>
        /// <param name="areasetid">防守地形id</param>
        /// <param name="planid">防守方案主键id</param>
        /// <returns></returns>
        private void SaveAreaSetData(List<view_user_area_set> areasetlist, Int64 planid)
        {

            //删除老的数据
            tg_war_plan_area.GetAreaSetDelete(planid);
            //添加新的数据
            var planarealist = areasetlist.Select(q => new tg_war_plan_area
                {
                    plan_id = planid,
                    base_id = q.base_id,
                    base_point_x = q.base_point_x,
                    base_point_y = q.base_point_y,
                    type = q.type,
                }).ToList();
            tg_war_plan_area.GetListInsert(planarealist);

        }
    }
}
