using System;
using System.Linq;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using System.Collections.Generic;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Vo.War;

namespace TGG.Module.War.Service.Map
{
    /// <summary>
    /// 同意建交
    /// </summary>
    public class WAR_OK_DIPLOMACY : IDisposable
    {
        //private static WAR_OK_DIPLOMACY _objInstance;

        ///// <summary>WAR_OK_DIPLOMACY单体模式</summary>
        //public static WAR_OK_DIPLOMACY GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_OK_DIPLOMACY());
        //}

        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary> 同意建交 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id") || !data.ContainsKey("type"))
                return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);//type:[int] 0:拒绝 1:同意

            var temp = tg_war_partner.GetEntityById(id,session.Player.User.id);
            if (temp == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            var time = Common.GetInstance().CurrentTime();
            if (temp.time > time) return CommonHelper.ErrorResult(ResultType.WAR_YES_PARTNER);
            if (temp.state == (int)WarPertnerType.ALLIANCE_IN) return CommonHelper.ErrorResult(ResultType.WAR_YES_RESULT);

            var count = session.Player.UserExtend.war_total_own;
            var of = session.Player.User.office;
            var bof = Variable.BASE_OFFICE.FirstOrDefault(m => m.id == of);
            if (bof == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var c = Convert.ToDouble(count) / Convert.ToDouble(bof.total_own);  //占有率

            switch (type)
            {
                case 0: { return Refuse(temp, c); }
                case 1: { return Agree(temp, c); }
                default: { return CommonHelper.ErrorResult(ResultType.UNKNOW_ERROR); }
            }
        }

        /// <summary> 同意建交 </summary>
        /// <param name="temp">己方盟友数据</param>
        /// <param name="count">占有率</param>
        /// <returns></returns>
        private ASObject Agree(tg_war_partner temp, double count)
        {
            if (IsFailure(temp)) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_ERROR);
            bool flag = false;
            var number = 0;
            switch (temp.state)
            {
                case (int)WarPertnerType.REQUEST_ALLIANCE_IN: //同盟请求
                    {
                        if (temp.friendly < 100) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_NOTFULL);
                        var rv = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32016");
                        if (rv == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
                        var value = Convert.ToInt32(rv.value);
                        temp.time = (DateTime.Now.AddMinutes(value).Ticks - 621355968000000000) / 10000;
                        temp.state = (int)WarPertnerType.ALLIANCE_IN;
                        flag = true;
                        break;
                    }
                case (int)WarPertnerType.REQUEST_DIPLOMACY_IN: //外交请求
                    {
                        if (temp.friendly >= 100) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_FULL);
                        var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32037");
                        if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
                        var str = rule.value.Replace("charm", Convert.ToString(temp.charm));
                        var express = CommonHelper.EvalExpress(str);

                        number = Convert.ToInt32((new Share.War()).GetTactics(temp.user_id, (int)WarTacticsType.FOREIGN)); //内政策略外交效果提升

                        var c = Convert.ToInt32(express) + number;
                        temp.friendly += c;
                        temp.charm = 0;
                        if (temp.friendly > 100) temp.friendly = 100;
                        temp.state = (int)WarPertnerType.DIPLOMACY_IN;
                        break;
                    }
                default: { return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR); }
            }
            temp.request_end_time = 0;
            temp.Update();

            if (!SaveWarPartner(temp, number, flag)) return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);//更新对方对己方的盟友信息

            if (flag)
            {
                //推送同盟据点信息
                SendPartnerCitys(temp.user_id, temp.partner_id, temp.time); //将盟友的据点推给自己
                SendPartnerCitys(temp.partner_id, temp.user_id, temp.time); //将自己的据点推给盟友
            }

            var wp = view_war_partner.GetEntityById(temp.id);
            var d = EntityToVo.ToDiplomacyVo(wp, count);
            return BulidData(d, 1);
        }

        /// <summary> 推送据点信息给盟友 </summary>
        /// <param name="userid">自己的用户Id</param>
        /// <param name="puid">盟友的用户Id</param>
        /// <param name="time">同盟结束时间</param>
        private void SendPartnerCitys(Int64 userid, Int64 puid, Int64 time)
        {
            if (!Variable.WarInUser.ContainsKey(puid)) return;
            var mn = Variable.WarInUser[puid]; //取到地图模块号
            var session = Variable.OnlinePlayer[puid];
            var list = view_war_city.GetEntityByMoudleNumber(userid, mn); //获取自己在该模块的据点集合
            if (!list.Any()) return;
            var vos = new List<WarCityVo>();
            int state = (int)WarCityCampType.PARTNER;
            int ow = (int)WarCityOwnershipType.PLAYER;
            foreach (var item in list)
            {
                var temp = EntityToVo.ToWarCityVo(item, state, ow, time);
                vos.Add(temp);
            }
            var data = (new Share.War()).BulidData(vos);
            var pv = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.PUSH_CITY, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }

        /// <summary> 检测请求是否失效 </summary>
        private bool IsFailure(tg_war_partner temp)
        {
            var time = Common.GetInstance().CurrentTime();
            if (temp.request_end_time >= time) return false;
            var istrue = tg_war_partner.GetPartnerExist(temp.user_id, temp.partner_id);
            if (!istrue) return temp.Delete() > 0;
            temp.state = (int)WarPertnerType.DIPLOMACY_IN; //标记处理过
            return temp.Update() > 0;
        }

        /// <summary>根据己方盟友数据保存对方盟友数据 </summary>
        /// <param name="model">己方盟友数据</param>
        /// <returns></returns>
        private bool SaveWarPartner(tg_war_partner model, int count, bool flag)
        {
            var number = Convert.ToInt32((new Share.War()).GetTactics(model.partner_id, (int)WarTacticsType.FOREIGN)); //内政策略外交效果提升
            var temp = tg_war_partner.GetEntityByUserId(model.partner_id, model.user_id);
            if (temp == null)
            {
                temp = new tg_war_partner
                {
                    time = model.time,
                    user_id = model.partner_id,
                    friendly = model.friendly - count + number,
                    partner_id = model.user_id,
                };
                if (flag) temp.state = model.state;
                return temp.Insert() > 0;
            }
            temp.time = model.time;
            if (flag) temp.state = model.state;
            temp.friendly = model.friendly - count + number;
            return temp.Update() > 0;
        }

        /// <summary> 拒绝同盟 </summary>
        /// <param name="temp">盟友信息</param>
        /// <returns></returns>
        private ASObject Refuse(tg_war_partner temp, double count)
        {
            var istrue = tg_war_partner.GetPartnerExist(temp.user_id, temp.partner_id); //验证对方是否有数据
            if (istrue)
            {
                temp.state = (int)WarPertnerType.DIPLOMACY_IN; //标记处理过
                temp.request_end_time = 0;
                if (temp.Update() > 0)
                {
                    var wp = view_war_partner.GetEntityById(temp.id);
                    var d = EntityToVo.ToDiplomacyVo(wp, count);
                    return BulidData(d, 1);
                }
            }
            else
            {
                var wp = view_war_partner.GetEntityById(temp.id);
                var d = EntityToVo.ToDiplomacyVo(wp, count);
                if (temp.Delete() > 0) return BulidData(d, 0);
            }
            return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);
        }

        /// <summary> 组装数据 </summary>
        /// <param name="model">同盟Vo</param>
        /// <param name="type">0:删除 1:不删</param>
        /// <returns></returns>
        public ASObject BulidData(DiplomacyVo model, int type)
        {
            var dic = new Dictionary<string, object> { 
            { "result", (int)ResultType.SUCCESS },
             { "type", type },
            { "vo",model} 
            };
            return new ASObject(dic);
        }
    }

}
