using System;
using System.Linq;
using FluorineFx;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;

namespace TGG.Module.War.Service.Map
{
    /// <summary> 派遣建交 </summary>
    public class WAR_DIPLOMACY : IDisposable
    {
        #region IDisposable 成员
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

         /// <summary>析构函数</summary>
        ~WAR_DIPLOMACY()
        {
            Dispose();
        }
    
        #endregion

        //private static WAR_DIPLOMACY _objInstance;

        ///// <summary>WAR_DIPLOMACY单体模式</summary>
        //public static WAR_DIPLOMACY GetInstance()
        //{
        //    return _objInstance ?? (_objInstance = new WAR_DIPLOMACY());
        //}

        /// <summary> 派遣建交 </summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
            if (!data.ContainsKey("id") || !data.ContainsKey("roleid") || !data.ContainsKey("type"))
                return null;
            var id = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "id").Value);//id:[double] 据点基表id
            var rid = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "roleid").Value);//roleid:[double] 武将编号id
            var type = Convert.ToInt32(data.FirstOrDefault(m => m.Key == "type").Value);//type:[int] 1 建交 2 同盟
            var userid = session.Player.User.id;
            var count = session.Player.UserExtend.war_total_own;

            var city = (new Share.War()).GetWarCity(id);//目标城市
            if (city == null) return CommonHelper.ErrorResult(ResultType.WAR_CITY_NOEXIST);

            var role = tg_war_role.GetEntityByUserIdAndId(userid, rid);
            if (role == null) return CommonHelper.ErrorResult(ResultType.NO_DATA);
            if (!tg_war_role.RoleIsIdle(role))
                return CommonHelper.ErrorResult(ResultType.WAR_ROLE_STATE_ERROR); //验证武将是否空闲

            //var r = tg_role.FindByid(role.rid);
            var r = tg_role.GetRoleByIdAndUser(role.rid, session.Player.User.id);
            if (r == null) return CommonHelper.ErrorResult(ResultType.DATA_NULL_ERROR);
            var rl = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32046");
            if (rl == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var totalpower = tg_role.GetTotalPower(r);
            var power = Convert.ToInt32(rl.value);
            if (totalpower < power) return CommonHelper.ErrorResult(ResultType.BASE_PLAYER_POWER_ERROR); //验证体力

            var of = session.Player.User.office;
            var bof = Variable.BASE_OFFICE.FirstOrDefault(m => m.id == of);
            if (bof == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);

            int _type; double charm = 0;
            var p = tg_war_partner.GetEntityByUserId(city.user_id, userid);

            switch (type)
            {
                case 1://type:[int] 1 建交
                    {
                        if (p != null)
                        {
                            if (p.friendly >= 100) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_FULL);
                            var time = Common.GetInstance().CurrentTime();
                            if (p.request_end_time > time) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_IN);
                            if (p.time > time) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_TIME_IN);
                        }
                        _type = (int)WarPertnerType.REQUEST_DIPLOMACY_IN;
                        charm = Math.Round(tg_role.GetSingleTotal(RoleAttributeType.ROLE_CHARM, r), 2);
                        break;
                    }
                case 2://type:[int] 2 同盟
                    {
                        if (p == null) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_NOTFULL);
                        var time = Common.GetInstance().CurrentTime();
                        if (p.request_end_time > time) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_IN);
                        if (p.time > time) return CommonHelper.ErrorResult(ResultType.WAR_DIPLOMACY_TIME_IN);
                        _type = (int)WarPertnerType.REQUEST_ALLIANCE_IN;
                        break;
                    }
                default: { return CommonHelper.ErrorResult(ResultType.FRONT_DATA_ERROR); }
            }

            var rule = Variable.BASE_RULE.FirstOrDefault(m => m.id == "32036");
            if (rule == null) return CommonHelper.ErrorResult(ResultType.BASE_TABLE_ERROR);
            var ms = Convert.ToInt32(rule.value) * 60 * 1000;
            var t = Common.GetInstance().CurrentTime() + ms;

            var model = GetWarPartner(city.user_id, userid, _type, charm, t);
            if (model == null) return CommonHelper.ErrorResult(ResultType.DATABASE_ERROR);

            var role2 = r.CloneEntity();
            (new Share.Role()).PowerUpdateAndSend(r, power, r.user_id);
            (new Share.Role()).LogInsert(role2, power, ModuleNumber.WAR, (int)WarCommand.PUSH_DIPLOMACY, "合战", "建交");
            Common.GetInstance().SaveRoleStateAndSend(role, (int)WarRoleStateType.DIPLOMATIC_RELATIONS, "32015");

            var d = EntityToVo.ToDiplomacyVo(model, Convert.ToDouble(count) / Convert.ToDouble(bof.total_own));
            SendPartnerRequest(city.user_id, Common.GetInstance().BulidData(d));    //推送建交请求
            return CommonHelper.SuccessResult();
        }



        /// <summary> 获取盟友对自己的信息 </summary>
        /// <param name="userid">盟友的用户Id</param>
        /// <param name="puid">自己用户Id</param>
        /// <param name="type">请求同盟还是外交</param>
        /// <param name="charm">要锁定的魅力值</param>
        /// <param name="time">请求过期时间</param>
        private view_war_partner GetWarPartner(Int64 userid, Int64 puid, int type, double charm, Int64 time)
        {

            var mp = tg_war_partner.GetEntityByUserId(userid, puid);
            if (mp != null)
            {
                mp.state = type;
                mp.charm = charm;
                mp.request_end_time = time;
                mp.Update();
            }
            else
            {
                mp = Insert(userid, puid, type, charm, time);
            }
            var temp = view_war_partner.GetEntityById(mp.id);
            return temp;
        }

        /// <summary> 插入盟友数据 </summary>
        /// <param name="userid">接受方用户id</param>
        /// <param name="partnerid">发送请求方用户id</param>
        /// <param name="type">请求同盟还是外交</param>
        ///  <param name="time">请求过期时间</param>
        /// <returns></returns>
        private tg_war_partner Insert(Int64 userid, Int64 partnerid, int type, double charm, Int64 time)
        {
            var model = new tg_war_partner
            {
                partner_id = partnerid,
                state = type,
                user_id = userid,
                charm = charm,
                request_end_time = time,
            };
            model.Insert();
            return model;
        }

        /// <summary> 推送建交申请 </summary>
        /// <param name="userid">要推送的用户</param>
        /// <param name="data">请求数据</param>
        private void SendPartnerRequest(Int64 userid, ASObject data)
        {
            if (!Variable.OnlinePlayer.ContainsKey(userid)) return;
            var session = Variable.OnlinePlayer[userid] as TGGSession;
            var d = session.InitProtocol((int)ModuleNumber.WAR, (int)WarCommand.PUSH_DIPLOMACY, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(d);
        }
    }
}
