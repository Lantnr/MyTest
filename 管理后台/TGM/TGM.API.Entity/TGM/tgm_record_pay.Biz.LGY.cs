using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGM.API.Entity.Model;

namespace TGM.API.Entity
{
    public partial class tgm_record_pay
    {
        /// <summary>查询总充值全服及时间段的集合</summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="sids">某个平台的所有服务器id</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetTimeSidsList(Int64 start, Int64 end, string sids, Int32 index, Int32 size, out Int32 count)
        {
            var _where = string.Format("createtime>={0} and createtime<={1} and sid in ({2}) and pay_state={3}", start, end, sids, 1);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
            //return FindAll(string.Format("createtime>={0} and createtime<={1} and sid in ({2}) and pay_state={3}", start, end, sids,1), null, null, 0, 0);
        }

        /// <summary>查询总充值全服集合</summary>
        /// <param name="sids">某个平台的所有服务器id</param>
        /// <param name="paystate"></param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetListBySids(string sids, Int32 index, Int32 size, out Int32 count)
        {
            var _where = string.Format("sid in ({0}) and pay_state={1}", sids, 1);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
            //return FindAll(string.Format("sid in ({0}) and pay_state={1}", sids, 1), null, null, 0, 0);
        }

        /// <summary>查询总充值单个服务器及时间段的集合</summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="sid">服务器id</param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetTimeSidList(Int64 start, Int64 end, Int32 sid, Int32 index, Int32 size, out Int32 count)
        {
            var _where = string.Format("createtime>={0} and createtime<={1} and sid ={2} and pay_state={3}", start, end,
                sid, 1);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
            //return FindAll(string.Format("createtime>={0} and createtime<={1} and sid ={2} and pay_state={3}", start, end, sid, 1), null, null, 0, 0);
        }

        /// <summary>查询总充值单个服务器的集合</summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetListBySid(Int32 sid, Int32 index, Int32 size, out Int32 count)
        {
            var _where = string.Format("sid = {0} and pay_state={1}", sid,1);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
            //return FindAll(string.Format("sid = {0}", sid), null, null, 0, 0);
        }

        /// <summary>查询单个服务器及时间段的集合，详细记录</summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="sid">服务器id</param>
        /// <param name="playname">玩家名称</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetTimeSidNameList(Int64 start, Int64 end, Int32 sid, string playname, Int32 index, Int32 size, out Int32 count)
        {
            var _where = string.Format("createtime>={0} and createtime<={1} and sid ={2} and player_name='{3}'", start, end, sid, playname);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
            //return FindAll(string.Format("createtime>={0} and createtime<={1} and sid ={2} and player_name='{3}'", start, end, sid,playname), null, null, 0, 0);
        }

        /// <summary>查询单个服务器的集合，详细记录</summary>
        /// <param name="sid">服务器id</param>
        /// <param name="playname">玩家名称</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetListByNameSid(Int32 sid, string playname, Int32 index, Int32 size, out Int32 count)
        {
            var _where = string.Format("sid = {0} and player_name='{1}'", sid, playname);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
            //return FindAll(string.Format("sid = {0} and player_name='{1}'", sid,playname), null, null, 0, 0);
        }

        /// <summary>查询单个服务器及时间段的集合，详细记录</summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="sid">服务器id</param>
        /// <param name="usercode">玩家账号</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetTimeSidCodeList(Int64 start, Int64 end, Int32 sid, string usercode, Int32 index, Int32 size, out Int32 count)
        {
            var _where = string.Format("createtime>={0} and createtime<={1} and sid ={2} and user_code='{3}'", start,
                end, sid, usercode);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
            //return FindAll(string.Format("createtime>={0} and createtime<={1} and sid ={2} and user_code='{3}'", start, end, sid, usercode), null, null, 0, 0);
        }

        /// <summary>查询单个服务器的集合，详细记录</summary>
        /// <param name="sid">服务器id</param>
        /// <param name="usercode">玩家账号</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetListByCodeSid(Int32 sid, string usercode, Int32 index, Int32 size, out Int32 count)
        {
            var _where = string.Format("sid = {0} and user_code='{1}'", sid, usercode);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
            //return FindAll(string.Format("sid = {0} and user_code='{1}'", sid, usercode), null, null, 0, 0);
        }

        /// <summary>进入界面时获取所有平台数据</summary>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetListByPids(Int32 index, Int32 size, out Int32 count)
        {
            count = FindCount(null, null, null, 0, 0);
            return FindAll(null, null, "*", index * size, size);
            //return FindAll(string.Format("sid = {0} and user_code='{1}'", sid, usercode), null, null, 0, 0);
        }

        /// <summary>进入界面时获取用户平台数据</summary>
        /// <param name="size"></param>
        /// <param name="sids">所属平台服务器id</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static List<tgm_record_pay> GetListByPid(Int32 index, Int32 size, string sids,out Int32 count)
        {
            var _where = string.Format("sid in ({0}) ", sids);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, null,"*", index * size, size);
        }

    }
}
