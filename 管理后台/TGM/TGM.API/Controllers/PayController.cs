using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public class PayController : ControllerBase
    {

        //  POST api/Pay?token={token}&pid={pid}&sid={sid}&start={start}&end={end}&index={index}&size={size}

        /// <summary>查询玩家总充值信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台id</param>
        /// <param name="sid">服务器id</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        /// <returns></returns>
        public PagerTotalPay PostPay(string token, int pid, int sid, Int64 start, Int64 end, Int32 index = 1, Int32 size = 10)
        {
            var pager = new PagerTotalPay();
            var paylist = new List<TotalRecordPay>();
            if (!IsToken(token)) return pager;   //验证会话
            tgm_platform.SetDbConnName(tgm_connection);
            var pf = tgm_platform.FindByid(pid);
            if (pf == null) return pager;
            var count = 0;
            if (sid == 0) //全服
            {
                tgm_server.SetDbConnName(tgm_connection);
                var list = tgm_server.GetServerList(pid).ToList();
                if (list.Count == 0) return pager;
                var sids = string.Join(",", list.ToList().Select(m => m.id).ToArray());
                if (start > 0 && end > 0) //查时间的
                {
                    tgm_record_pay.SetDbConnName(tgm_connection); //充值记录
                    //var plist = tgm_record_pay.GetTimeSidsList(start, end, sids);                 
                    var plist = tgm_record_pay.GetTimeSidsList(start, end, sids, index , size, out count).ToList();
                    if (plist.Count == 0) return pager;
                    foreach (var item in list)
                    {
                        var r = GetPayList(item, plist, pf.name);
                        paylist.AddRange(r);
                    }
                }
                else
                {
                    tgm_record_pay.SetDbConnName(tgm_connection); //充值记录
                    var plist = tgm_record_pay.GetListBySids(sids, index, size, out count);
                    if (plist.Count == 0) return pager;
                    foreach (var r in list.Select(item => GetPayList(item, plist, pf.name)))
                    {
                        paylist.AddRange(r);
                    }
                }
            }
            else  //单服
            {
                tgm_server.SetDbConnName(tgm_connection);
                var server = tgm_server.FindByid(Convert.ToInt32(sid));
                if (server == null) return pager;
                if (start > 0 && end > 0)
                {
                    tgm_record_pay.SetDbConnName(tgm_connection); //充值记录
                    var plist = tgm_record_pay.GetTimeSidList(start, end, Convert.ToInt32(sid), index, size, out count);
                    if (plist.Count == 0) return pager;
                    var r = GetPayList(server, plist, pf.name);
                    paylist.AddRange(r);
                }
                else
                {
                    tgm_record_pay.SetDbConnName(tgm_connection); //充值记录
                    var plist = tgm_record_pay.GetListBySid(Convert.ToInt32(sid), index , size, out count);
                    if (plist.Count == 0) return pager;
                    var r = GetPayList(server, plist, pf.name);
                    paylist.AddRange(r);
                }
            }
            pager = new PagerTotalPay()
            {
                result=1,
                Recordpay = paylist,
                Pager = new PagerInfo { CurrentPageIndex = index, PageSize = size, RecordCount = count },
            };           
            return pager;
        }


        /// <summary>查询单个服务器的充值信息集合</summary>
        /// <param name="server">服务器</param>
        /// <param name="plist">充值集合</param>
        /// <param name="platform">平台名称</param>
        /// <returns></returns>
        private IEnumerable<TotalRecordPay> GetPayList(tgm_server server, IEnumerable<tgm_record_pay> plist, string platform)
        {
            var paylist = new List<TotalRecordPay>();
            SN = server.name;
            tg_user_login_log.SetDbConnName(db_connection);
            var loglist = tg_user_login_log.FindAll().ToList();
            if (loglist.Count == 0) return paylist;
            var players = plist.Where(m => m.sid == server.id).ToList();
            foreach (var l in loglist)
            {
                var ps = players.Where(m => m.player_id == l.user_id).ToList();
                if (ps.Count > 0)
                {
                    var pls = ps.OrderByDescending(m => m.createtime).ToList();
                    var count = ps.Count;
                    var paytotal = ps.Select(m => m.money).Sum();
                    var paytime = pls[0].createtime;
                    var record = ToEntity.ToTotalRecordPay(count, platform, server.name, paytotal, l.logout_time,
                        pls[0].player_name, paytime);
                    paylist.Add(record);
                }
            }
            return paylist;
        }

        //  POST api/Pay?token={token}&role={role}&pid={pid}&sign={sign}&index={index}&size={size}
        /// <summary>进入界面时，显示充值所有记录</summary>
        /// <param name="token"></param>
        /// <param name="role"></param>
        /// <param name="pid"></param>
        /// <param name="sign"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public PagerTotalPay PostTotalPay(string token, Int64 role, Int32 pid, bool sign,Int32 index = 1, Int32 size = 10)
        {
            var pager = new PagerTotalPay();
            if (!IsToken(token)) return pager;   //验证会话
            List<tgm_record_pay> paylist;
            List<tgm_server> list;
            var plist = new List<TotalRecordPay>();
            var count = 0;
            if (role == 10000)
            {
                tgm_server.SetDbConnName(tgm_connection);
                list = tgm_server.FindAll().ToList();
                if (list.Count == 0) return pager;
                tgm_record_pay.SetDbConnName(tgm_connection);
                paylist = tgm_record_pay.GetListByPids(index, size, out count);
                var platforms = tgm_platform.FindAll().ToList();
                if (paylist.Count == 0 || platforms.Count == 0) return pager;
                foreach (var item in list)
                {
                    var p = platforms.FirstOrDefault(m => m.id == item.pid);
                    if(p==null)continue;
                    plist.AddRange(GetPayList(item, paylist, p.name));
                }
            }
            else
            {
                tgm_server.SetDbConnName(tgm_connection);
                list = tgm_server.GetServerList(pid).ToList();
                if (list.Count == 0) return pager;
                var sids = string.Join(",", list.ToList().Select(m => m.id).ToArray());
                tgm_record_pay.SetDbConnName(tgm_connection);
                paylist = tgm_record_pay.GetListByPid(index, size, sids, out count);
                if (paylist == null) return pager;
                tgm_platform.SetDbConnName(tgm_connection);
                var platform = tgm_platform.FindByid(pid);
                if (platform == null) return pager;
                foreach (var item in list)
                {
                    plist.AddRange(GetPayList(item, paylist, platform.name));
                }
            }
            pager = new PagerTotalPay()
            {
                result = 1,
                Recordpay = plist,
                Pager = new PagerInfo { CurrentPageIndex = index, PageSize = size, RecordCount = count },
            };
            return pager;
        }

        //  POST api/Pay?token={token}&role={role}&pid={pid}&index={index}&size={size}
        /// <summary>进入界面时，显示充值所有记录</summary>
        /// <param name="token"></param>
        /// <param name="role"></param>
        /// <param name="pid"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public PagerPay PostSinglePay(string token, Int64 role, Int32 pid, Int32 index = 1, Int32 size = 10)
        {
            var pager = new PagerPay();
            if (!IsToken(token)) return pager;   //验证会话
            List<tgm_record_pay> paylist;
            List<tgm_server> list;
            var plist = new List<SingleRecordPay>();
            var count = 0;
            if (role == 10000)
            {
                tgm_server.SetDbConnName(tgm_connection);
                list = tgm_server.FindAll().ToList();
                if (list.Count == 0) return pager;
                tgm_record_pay.SetDbConnName(tgm_connection);
                paylist = tgm_record_pay.GetListByPids(index , size, out count);
                var platforms = tgm_platform.FindAll().ToList();
                if (paylist.Count == 0 || platforms.Count == 0) return pager;
                plist.AddRange(from item in paylist let server = list.FirstOrDefault(m => m.id == item.sid) where server != null let p = platforms.FirstOrDefault(m => m.id == server.pid) where p != null  select ToEntity.ToSingleRecordPay(p.name, server.name, item));
            }
            else
            {
                tgm_server.SetDbConnName(tgm_connection);
                list = tgm_server.GetServerList(pid).ToList();
                if (list.Count == 0) return pager;
                var sids = string.Join(",", list.ToList().Select(m => m.id).ToArray());
                tgm_record_pay.SetDbConnName(tgm_connection);
                paylist = tgm_record_pay.GetListByPid(index , size, sids, out count);
                if (paylist == null) return pager;
                tgm_platform.SetDbConnName(tgm_connection);
                var platform = tgm_platform.FindByid(pid);
                if (platform == null) return pager;
                plist.AddRange(from item in paylist let server = list.FirstOrDefault(m => m.id == item.sid) where server != null select ToEntity.ToSingleRecordPay(platform.name, server.name, item));
            }
            pager = new PagerPay()
            {
                result = 1,
                Recordpay = plist,
                Pager = new PagerInfo { CurrentPageIndex = index, PageSize = size, RecordCount = count },
            };
            return pager;
        }

        //  POST api/Pay?token={token}&pid={pid}&sid={sid}&start={start}&end={end}&type={type}&playname={playname}&userpid={userpid}&role={role}&index={index}&size={size}

        /// <summary>查询玩家充值所有记录</summary>
        /// <param name="token">令牌</param>
        /// <param name="role">角色</param>
        /// <param name="pid">平台id</param>
        /// <param name="sid">服务器id</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="type">玩家昵称/账号</param>
        /// <param name="playname">玩家昵称</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        /// <param name="userpid">用户平台id</param>
        /// <returns></returns>
        public PagerPay PostPlayerPay(string token, int pid, int sid, Int64 start, Int64 end, int type, string playname, Int32 userpid, Int32 role, Int32 index, Int32 size)
        {
            var pager = new PagerPay();
            if (!IsToken(token)) return pager;   //验证会话    //还有一个权限问题

            if (sid == 0 || playname == null) return pager;//服务器和玩家不能为空
            tgm_platform.SetDbConnName(tgm_connection);
            var pf = tgm_platform.FindByid(pid);
            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (pf == null || server == null) return pager;

            List<tgm_record_pay> list;
            var count = 0;
            if (type == 0)
            {
                if (start > 0 && end > 0)
                {
                    tgm_record_pay.SetDbConnName(tgm_connection); //充值记录
                    list = tgm_record_pay.GetTimeSidNameList(start, end, Convert.ToInt32(sid), playname, index , size, out count);
                }
                else
                {
                    tgm_record_pay.SetDbConnName(tgm_connection); //充值记录
                    list = tgm_record_pay.GetListByNameSid(Convert.ToInt32(sid), playname, index , size, out count);
                }
            }
            else
            {
                if (start > 0 && end > 0)
                {
                    tgm_record_pay.SetDbConnName(tgm_connection); //充值记录
                    list = tgm_record_pay.GetTimeSidCodeList(start, end, Convert.ToInt32(sid), playname, index , size, out count);
                }
                else
                {
                    tgm_record_pay.SetDbConnName(tgm_connection); //充值记录
                    list = tgm_record_pay.GetListByCodeSid(Convert.ToInt32(sid), playname, index , size, out count);
                }
            }
            if (!list.Any()) return pager;
            var paylist = (from item in list select ToEntity.ToSingleRecordPay(pf.name, server.name, item)).ToList();

            pager = new PagerPay()
            {
                result = 1,
                Recordpay = paylist,
                Pager = new PagerInfo { CurrentPageIndex = index, PageSize = size, RecordCount = count },
                //total = list.Sum(m => m.pay_total),
                //month_total = list.Sum(m => m.pay_month)
            };
            return pager;

        }





        /// <summary>充值接口</summary>
        /// <param name="key">平台key</param>
        /// <param name="param">充值参数字符串</param>
        /// <returns>充值后状态结果值</returns>
        public Int32 PostPayment(String key, String param)
        {
            //key : 解析param用
            //param:充值封装字符串 格式: token|sid|player_id|order_id|channel|amount

            //解析后调用游戏接口判断是否成功

            //无论成功,存入后台数据库这条数据记录

            return 0;
        }
    }
}
