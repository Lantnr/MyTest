using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 捐献朝廷
    /// </summary>
    public class WAR_DONATION : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    
        #endregion

        //private static WAR_DONATION _objInstance;

        ///// <summary>WAR_DONATION单体模式</summary>
        //public static WAR_DONATION GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DONATION());
        //}

        /// <summary> 捐献朝廷 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id"))
                return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var user = session.Player.User.CloneEntity();
            var extend = session.Player.UserExtend;
            var cityid = session.Player.War.WarCityId;

            if (cityid == id) return CommonHelper.ErrorResult(ResultType.WAR_BASE_CANNOT_DONATE);
            var city = (new Share.War()).GetWarCity(id, user.id);
            if (city == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32024");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var value = Convert.ToInt32(rule.value);

            var time = (DateTime.Now.AddMinutes(value).Ticks - 621355968000000000) / 10000;
            if (city.time < time) return CommonHelper.ErrorResult(ResultType.WAR_CITY_TIME);
            var bcity = Variable.BASE_WARCITY.FirstOrDefault(m => m.id == city.base_id);
            var bcitysize = Variable.BASE_WARCITYSIZE.FirstOrDefault(m => m.id == city.size);
            if (bcity == null || bcitysize == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var count = extend.war_total_own - bcitysize.own;
            if (count < 0) return CommonHelper.ErrorResult(ResultType.WAR_NOT_OWN);

            if (bcity.type == (int)WarLandType.NPCCITY) //之前是NPC据点
            {
                city.user_id = 0;
                city.ownership_type = (int)WarCityOwnershipType.NPC;
                var v = Convert.ToInt32(Variable.BASE_RULE.FirstOrDefault(m => m.id == "32025").value);
                city.guard_time = (DateTime.Now.AddMinutes(v).Ticks - 621355968000000000) / 10000;
                city.Update();
                (new Share.War()).SaveWarCityAll(city);
            }
            else
            {
                city.guard_time = 0;
                city.Delete();
                tg_war_city c;
                Variable.WarCityAll.TryRemove(id, out c);
            }

            var olddonate = user.donate;
            extend.war_total_own = count;
            user.donate += bcitysize.donate;
            extend.Update();
            user.Update();
            session.Player.UserExtend = extend;
            session.Player.User = user;
            (new Share.User()).REWARDS_API((int)GoodsType.TYPE_DONATE, user);

            //日志
            var logdata = string.Format("{0}_{1}_{2}_{3}", "Donate", olddonate, bcitysize.donate, user.donate);
            (new Share.Log()).WriteLog(user.id, (int)LogType.Get, (int)ModuleNumber.WAR, (int)WarCommand.WAR_DONATION, "合战", "捐献朝廷", "贡献度", (int)GoodsType.TYPE_DONATE, bcitysize.donate, user.donate, logdata);

            tg_war_role.DeleteCityNpcRoleByCityId(id); //删除该城市的备大将
            UpdateRolePosition(user.id, id, cityid);
            SendNpcCity(city.base_id, city.guard_time, city.module_number);
            (new Share.War()).CarryClose(id); //移除运输线程
            return SuccessResult();
        }

        /// <summary> 修改武将所在据点 并推送 </summary>
        /// <param name="userid">用户id</param>
        /// <param name="cityid">据点Id</param>
        ///  <param name="cid">主据点基表Id</param>
        private void UpdateRolePosition(Int64 userid, int cityid, int cid)
        {
            var list = tg_war_role.GetEntityListByUserId(userid, cityid);
            var state = (int)WarRoleStateType.IDLE;
            foreach (var item in list)
            {
                item.station = cid;
                item.state = state;
                item.state_end_time = 0;
                item.Update();
                (new Share.War()).SendWarRole(item, "station", "state"); //推送武将当前所在据点
            }
        }

        /// <summary> 推送NPC据点 </summary>
        /// <param name="baseid">NPC据点Id</param>
        /// <param name="time">NPC据点保护时间</param>
        /// <param name="modulenumber">要送送的模块地图</param>
        private void SendNpcCity(int baseid, Int64 time, int modulenumber)
        {
            var list = Variable.WarInUser.Where(m => m.Value == modulenumber).Select(m => m.Key).ToList();
            foreach (var item in list)
            {
                var token = new CancellationTokenSource();
                var obj = new ObjectEntity
                {
                    baseid = baseid,
                    userid = item,
                    time = time,
                };
                Task.Factory.StartNew(m =>
                {
                    var temp = m as ObjectEntity;
                    if (temp == null) return;
                    if (!Variable.OnlinePlayer.ContainsKey(temp.userid)) return;
                    var session = Variable.OnlinePlayer[temp.userid] as TGGSession;
                    var data = BuildData(temp.baseid, temp.time);
                    var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.PUSH_NPC_CITY, (int)ResponseType.TYPE_SUCCESS, data);
                    session.SendData(pv);
                    token.Cancel();
                }, obj, token.Token);
            }
        }

        /// <summary>错误结果返回</summary>
        /// <param name="baseid">npc据点基表Id</param>
        /// <param name="time">npc据点保护时间</param>
        private static ASObject BuildData(int baseid, Int64 time)
        {
            var dic = new Dictionary<string, object> { { "result", (int)ResultType.SUCCESS }, { "id", baseid }, { "time", time } };
            return new ASObject(dic);
        }

        /// <summary>错误结果返回</summary>
        /// <param name="type">数据结构返回值</param>
        private static ASObject SuccessResult()
        {
            var dic = new Dictionary<string, object> { { "result", (int)ResultType.SUCCESS } };
            return new ASObject(dic);
        }
    }
    class ObjectEntity
    {
        public Int64 userid { get; set; }

        public Int64 time { get; set; }

        public int baseid { get; set; }
    }
}
