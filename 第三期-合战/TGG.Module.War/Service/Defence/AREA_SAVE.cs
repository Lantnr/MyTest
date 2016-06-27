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
using TGG.Core.Vo.War;
using TGG.SocketServer;

namespace TGG.Module.War.Service
{
    /// <summary>
    /// 保存地形设定
    /// 开发者：李德雁
    /// </summary>
    public class AREA_SAVE : IDisposable
    {
        //private static AREA_SAVE _objInstance;

        ///// <summary>AREA_SAVE单体模式</summary>
        //public static AREA_SAVE GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new AREA_SAVE());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>析构函数</summary>
        ~AREA_SAVE()
        {
            Dispose();
        }

        #endregion

        /// <summary>  保存地形设定 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id") || !data.ContainsKey("list")) return null;
            var id = Convert.ToInt64(data.FirstOrDefault(m => m.Key == "id").Value); //玩家地形设定vo
            var list = data.FirstOrDefault(m => m.Key == "list").Value as object[]; //玩家地形设定vo
            if (list == null || id <= 0)
                return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);
            //保存空地形
            if (!list.Any())
            {
                return !DataSave(id) ?
                CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR) :
                CommonHelper.SuccessResult();
            }
            //if (tg_war_area.FindByid(id) == null)
            if (tg_war_area.GetEntityByUseridAndId(session.Player.User.id, id) == null)
                return CommonHelper.ErrorResult((int)ResultType.FRONT_DATA_ERROR);
            //解析前端数据并转换成实体集合
            var listentity = GetAreaSet(list, id);
            //验证用户是否有该地形
            if (!CheckArea(listentity, session.Player.User.id))
                return CommonHelper.ErrorResult((int)ResultType.WAR_NO_AREA);
            //验证地形设定的坐标
            if (!CheckPoint(listentity))
                return CommonHelper.ErrorResult((int)ResultType.WAR_POINT_ERROR);

            //保存到数据库
            return !DataSave(listentity) ?
                CommonHelper.ErrorResult((int)ResultType.DATABASE_ERROR) :
                CommonHelper.SuccessResult();
        }

        /// <summary>
        /// 验证地形设定的坐标是否正确
        /// </summary>
        /// <param name="areasetlist"></param>
        /// <returns></returns>
        private bool CheckPoint(List<tg_war_area_set> areasetlist)
        {
            //将地形设定转换成战争地形设定
            var maplist = Common.GetInstance().GetMapArea(areasetlist);

            //山脉地形要验证周围也不能放置地形
            if (!CheckMountainSharp(maplist)) return false;
            if (!CheckTrapSharp(maplist.Where(q => q.type == (int)AreaType.陷阱 && q.type == (int)AreaType.山脉).ToList()))
                return false;
            var test1 = maplist.Where(q => q.X < 4 || q.X > 15 || q.Y < 0 || q.Y > 8 &&
                (q.type != (int)AreaType.陷阱)).ToList(); //测试数据

            var test2 = maplist.GroupBy(q => new { q.X, q.Y }).Where(g => g.Count() > 1).ToList();//测试数据

            maplist = maplist.Where(q => q.type != (int)AreaType.陷阱).ToList();
            return !maplist.Any(q => q.X < 4 || q.X > 15 || q.Y < 0 || q.Y > 8)
                && !maplist.GroupBy(q => new { q.X, q.Y }).Any(g => g.Count() > 1);
        }

        /// <summary>
        /// 验证山脉地形周围是否有其他地形
        /// </summary>
        /// <param name="maplist"></param>
        /// <returns></returns>
        private bool CheckMountainSharp(List<Common.MapArea> maplist)
        {
            var mon_sharp = maplist.Where(q => q.type == (int)AreaType.山脉 && q.position != 5).ToList(); //山脉地形
            foreach (var item in mon_sharp.Where(item => item.X >= 3 && item.X <= 14 && item.Y >= 1 && item.X <= 7))
            {
                #region 验证山脉周围有没有其他山脉
                switch (item.position)
                {
                    case 1: //位置1 验证左、上、左上
                        {
                            if (mon_sharp.Any(q => q.X == item.X - 1 && q.Y == item.Y)) return false; //左
                            if (mon_sharp.Any(q => q.X == item.X && q.Y == item.Y - 1)) return false;//上
                            if (mon_sharp.Any(q => q.X == item.X - 1 && q.Y == item.Y - 1)) return false;//左上
                        }
                        break;
                    case 2: //位置2上
                        {
                            if (mon_sharp.Any(q => q.X == item.X && q.Y == item.Y - 1)) return false;//上
                        }
                        break;
                    case 3: //位置3 验证右、上、右上
                        {
                            if (mon_sharp.Any(q => q.X == item.X + 1 && q.Y == item.Y)) return false; //右
                            if (mon_sharp.Any(q => q.X == item.X && q.Y == item.Y - 1)) return false;//上
                            if (mon_sharp.Any(q => q.X == item.X + 1 && q.Y == item.Y + 1)) return false;//左上
                        }
                        break;
                    case 4: //位置4 验证左
                        {
                            if (mon_sharp.Any(q => q.X == item.X - 1 && q.Y == item.Y)) return false; //左
                        }
                        break;
                    case 6: //位置6 验证右
                        {
                            if (mon_sharp.Any(q => q.X == item.X + 1 && q.Y == item.Y)) return false; //右
                        }
                        break;
                    case 7: //位置7 验证左、左下、下
                        {
                            if (mon_sharp.Any(q => q.X == item.X - 1 && q.Y == item.Y)) return false; //左
                            if (mon_sharp.Any(q => q.X == item.X - 1 && q.Y == item.Y + 1)) return false; //左下
                            if (mon_sharp.Any(q => q.X == item.X && q.Y == item.Y + 1)) return false; //下
                        }
                        break;
                    case 8: //位置8 验证下
                        {
                            if (mon_sharp.Any(q => q.X == item.X && q.Y == item.Y + 1)) return false; //下
                        }
                        break;
                    case 9: //位置9 验证下、右、右下
                        {
                            if (mon_sharp.Any(q => q.X == item.X && q.Y == item.Y + 1)) return false; //下
                            if (mon_sharp.Any(q => q.X == item.X + 1 && q.Y == item.Y)) return false; //右
                            if (mon_sharp.Any(q => q.X == item.X + 1 && q.Y == item.Y + 1)) return false; //右下
                        }
                        break;
                }
                #endregion
            }
            return true;
        }

        /// <summary>
        /// 验证陷阱地形
        /// </summary>
        /// <param name="maplist"></param>
        /// <returns></returns>
        private bool CheckTrapSharp(List<Common.MapArea> maplist)
        {
            return !maplist.Any(q => q.X < 3 || q.X > 15 || q.Y < 0 || q.Y > 8)
             && !maplist.GroupBy(q => new { q.X, q.Y }).Any(g => g.Count() > 1);

        }

        /// <summary>
        /// 保存地形设定数据
        /// </summary>
        /// <param name="entitylist"></param>
        /// <returns></returns>
        private bool DataSave(List<tg_war_area_set> entitylist)
        {
            if (entitylist == null || !entitylist.Any()) return false;
            var areaid = entitylist.FirstOrDefault().area_id;
            //验证有没有这个地形设定
            if (tg_war_area.FindByid(areaid) == null)
                return false;
            //删除老的设定
            if (tg_war_area_set.GetEntieyDelByAreaId(areaid) <= 0)
                return false;
            return tg_war_area_set.GetListInsert(entitylist) > 0;
        }

        /// <summary>
        /// 保存地形设定数据
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns></returns>
        private bool DataSave(Int64 areaid)
        {
            //验证有没有这个地形设定
            if (tg_war_area.FindByid(areaid) == null)
                return false;
            //删除老的设定
            if (tg_war_area_set.GetEntieyDelByAreaId(areaid) <= 0)
                return false;
            var newone = new tg_war_area_set()
            {
                area_id = areaid,
            };
            return newone.Insert() > 0;

        }

        /// <summary>
        /// 验证用户是否有地形设置中的地形
        /// </summary>
        /// <param name="entitylist">地形设置集合</param>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        private bool CheckArea(List<tg_war_area_set> entitylist, Int64 userid)
        {
            var userarea = tg_war_user_area.GetEntityByUserId(userid);
            //验证地形设置中的每一个地形是否都能在用户地形集合中找到
            return !entitylist.Any(item => userarea.All(q => q.base_id != item.base_id));
        }

        #region 解析数据

        /// <summary>
        /// 解析数据并转换成实体
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<tg_war_area_set> GetAreaSet(object[] list, Int64 id)
        {

            return list.Select(AMFConvert.AsObjectToVo<AreaSetVo>).Select(q => VoToEntity(q, id)).ToList();
        }

        /// <summary>
        /// 将Vo的数据转换成实体
        /// </summary>
        /// <param name="vo"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        private tg_war_area_set VoToEntity(AreaSetVo vo, Int64 areaid)
        {
            return new tg_war_area_set()
            {
                area_id = areaid,
                base_point_x = vo.pointX,
                base_point_y = vo.pointY,
                base_id = vo.baseId
            };
        }
        #endregion
    }
}
