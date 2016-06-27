using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 出征转移目标 </summary>
    public partial class WAR_TRANSFER_TARGET : IDisposable
    {
        //private static WAR_TRANSFER_TARGET _objInstance;

        ///// <summary>WAR_TRANSFER_TARGET单体模式</summary>
        //public static WAR_TRANSFER_TARGET GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_TRANSFER_TARGET());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 出征转移目标 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id"))return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);  //id:[int] 出征Vo的主键Id

            var entity = new WarEntity();
            var userid = session.Player.User.id;
            session.Player.War.WarBattle = new WarEntity();

            var bq = tg_war_battle_queue.GetEntityByUseridAndId(id, userid); //tg_war_battle_queue.FindByid(id);
            if (bq == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (bq.time > time) return CommonHelper.ErrorResult(ResultType.WAR_NOT_ARRIVE);

            var psList = tg_war_partner.GetEntityByUserId(userid);
            var city = (new Share.War()).GetWarCity(bq.end_CityId); //目标据点
            if (city == null) //如果目标据点不等于玩家
            {
                var bcity = Variable.BASE_WARCITY.FirstOrDefault(m => m.id == bq.end_CityId);
                if (bcity == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
                entity = bcity.type != (int)WarLandType.SPACE ?
                    GetBuildWarEntity(bq.last_CityId, bq.end_CityId, userid, psList) :
                    GetBuildWarEntity(new tg_war_city() { base_id = bq.end_CityId, guard_time = 0 }, userid, psList);
            }
            else
            {
                if (city.user_id == userid)//终点为自己的据点
                {
                    entity = GetBuildWarEntity(city, userid, psList);
                }
                else
                {
                    //玩家数据
                    entity = !(new Share.War()).IsExistPartner(psList, city.user_id) ?
                        GetBuildWarEntity(bq.last_CityId, bq.end_CityId, userid, psList) :
                        GetBuildWarEntity(city, userid, psList);
                }
            }

            session.Player.War.WarBattle = entity;
            return BulidData(entity.OperableCityIds, entity.OperableRivalCityIds);
        }

        /// <summary> 己方据点为发出点的情况 </summary>
        /// <param name="city">除敌方外的据点信息</param>
        /// <param name="userid">用户Id</param>
        /// <param name="psList">盟友集合</param>
        private WarEntity GetBuildWarEntity(tg_war_city city, Int64 userid, List<tg_war_partner> psList)
        {
            var cityid = city.base_id;
            var olist = new List<int>();//己方可操作的据点
            var rlist = new List<int>();//敌方可操作的据点
            var map = new List<Core.Common.Util.Map>(); //构建的可操作的己方据点的图型集合
            var _map = new List<Core.Common.Util.Map>();//构建的可操作的敌方据点的图型集合
            var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (city.guard_time < time) _map = (new Share.War()).GetBattleMaps(cityid, psList, userid, new List<int>(), ref rlist);
            map = (new Share.War()).GetMaps(cityid, psList, userid, new List<int>(), ref olist);
            return BuildWarEntity(0, 0, 0, 0, olist, rlist, map, _map);

        }

        /// <summary> 目标据点上一个据点的情况 </summary>
        /// <param name="cityid">目标据点上一个据点Id</param>
        /// <param name="endCityId">目标据点Id</param>
        /// <param name="userid">用户Id</param>
        /// <param name="psList">盟友集合</param>
        private WarEntity GetBuildWarEntity(int cityid, int endCityId, Int64 userid, List<tg_war_partner> psList)
        {
            var olist = new List<int>();//己方可操作的据点
            var rlist = new List<int>();//敌方可操作的据点
            var map = new List<Core.Common.Util.Map>(); //构建的可操作的己方据点的图型集合
            var _map = new List<Core.Common.Util.Map>();//构建的可操作的敌方据点的图型集合

            var temp = (new Share.War()).GetWarCity(cityid); //目标据点的上一个据点信息

            if (temp == null)
            {
                var bcity = Variable.BASE_WARCITY.FirstOrDefault(m => m.id == cityid);
                if (bcity == null) return null;
                if (bcity.type != (int)WarLandType.SPACE)
                {
                    rlist.Add(cityid);
                    _map.AddRange((new Share.War()).BuildMap(endCityId, cityid));
                    return BuildWarEntity(0, 0, 0, 0, olist, rlist, map, _map);
                }
            }
            else
            {
                if (temp.user_id != userid)
                {
                    if (!(new Share.War()).IsExistPartner(psList, temp.user_id)) //如果当前据点不是盟友的据点
                    {
                        var time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
                        if (temp.guard_time < time) //在保护时间
                        {
                            rlist.Add(cityid);
                            _map.AddRange((new Share.War()).BuildMap(cityid, endCityId));
                            return BuildWarEntity(0, 0, 0, 0, olist, rlist, map, _map);
                        }
                    }
                }
                olist.Add(cityid);
            }

            _map = (new Share.War()).GetBattleMaps(cityid, psList, userid, new List<int>(), ref rlist);
            map = (new Share.War()).GetMaps(cityid, psList, userid, new List<int>(), ref olist);
            rlist = rlist.Where(m => m != endCityId).ToList();
            return BuildWarEntity(0, 0, 0, 0, olist, rlist, map, _map);
        }

        /// <summary> 组装出征实体 </summary>
        /// <param name="warGoCityId">出征出发据点Id</param>
        /// <param name="lastCityId">确定出征时的最后一个不是敌方的据点Id</param>
        /// <param name="lockCityId">锁定出征的目标据点</param>
        /// <param name="lockTime">锁定的出征时间(分钟)</param>
        /// <param name="operableCityIds"> 可操作的己方据点（出征地图可选据点）</param>
        /// <param name="operableRivalCityIds">可操作的敌方据点（出征地图可选的敌方据点）</param>
        /// <param name="map">构建的可操作的己方据点的图型集合</param>
        /// <param name="rivalMap">构建的可操作的敌方据点的图型集合</param>
        /// <returns></returns>
        private WarEntity BuildWarEntity(int warGoCityId, int lastCityId, int lockCityId, int lockTime,
            List<int> operableCityIds, List<int> operableRivalCityIds, List<Core.Common.Util.Map> map,
            List<Core.Common.Util.Map> rivalMap)
        {
            return new WarEntity
            {
                Map = map,
                rivalMap = rivalMap,
                LockTime = lockTime,
                lastCityId = lastCityId,
                LockCityId = lockCityId,
                warGoCityId = warGoCityId,
                OperableCityIds = operableCityIds,
                OperableRivalCityIds = operableRivalCityIds,
            };
        }

        /// <summary> 组装数据 </summary>
        /// <param name="ids">可出征的己方城市Id集合</param>
        /// <param name="list">可出征的敌方城市Id集合</param>
        private ASObject BulidData(List<int> ids, List<int> list)
        {
            var dic = new Dictionary<string, object> { 
            { "result", (int)ResultType.SUCCESS },
            { "cityIds", ids },
            { "list",list} 
            };
            return new ASObject(dic);
        }
    }
}
