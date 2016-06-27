using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>
    /// tgm_record_hours 逻辑业务类
    /// </summary>
    public partial class tgm_record_hours
    {
        /// <summary>读取存储过程数据</summary>
        /// <param name="conn">连接字符串</param>
        /// <param name="sid">服务器id</param>
        /// <returns></returns>
        public static tgm_record_hours Proc_sp_pay(Int32 sid)
        {
            try
            {
                var db = DAL.Create(Meta.ConnName);

                var now = DateTime.Now;
                var t0 = now.Date.Ticks;
                var t1 = now.Date.AddDays(1).Ticks;
                var d = new DateTime(now.Year, now.Month, 1);
                var m0 = d.Ticks;
                var m1 = d.AddMonths(1).AddDays(-1).Ticks;

                var p = new[]
                { 
                    new SqlParameter("@sid", sid),
                    new SqlParameter("@_begin_time", t0),
                    new SqlParameter("@_end_time", t1),
                    new SqlParameter("@M_BEGIN", m0),
                    new SqlParameter("@M_END", m1)
                };

                var ds = db.Session.Query("sp_pay", CommandType.StoredProcedure, p);

                var q = ds.Tables[0].AsEnumerable();
                var entity = q.Select(m => new tgm_record_hours
                {
                    pay_count = Convert.ToInt32(m["pay_count"]),
                    pay_number = Convert.ToInt32(m["pay_number"]),
                    pay_taday = Convert.ToInt32(m["pay_taday"]),
                    pay_total = Convert.ToInt32(m["pay_total"]),
                    pay_month = Convert.ToInt32(m["pay_month"]),

                }).FirstOrDefault();
                return entity;
            }
            catch (Exception)
            {

                return new tgm_record_hours();
            }
        }

        public static tgm_record_hours GetFindBySidTime(Int32 sid)
        {
            var begin = Convert.ToDateTime(DateTime.Now.ToShortDateString()).Ticks;
            var end = Convert.ToDateTime(DateTime.Now.AddHours(1).ToShortDateString()).Ticks;
            var where = String.Format("sid={0} and createtime>{1} and createtime<{2}", sid, begin, end);
            return Find(where);
        }

        /// <summary>服务器每天记录统计分页</summary>
        /// <param name="sid">服务器编号</param>
        /// <param name="time">时间</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>

        public static EntityList<tgm_record_hours> GetPageEntity(Int32 sid, DateTime time, Int32 index, Int32 size, out Int32 count)
        {
            var begin = Convert.ToDateTime(time.ToShortDateString()).Ticks;
            var end = Convert.ToDateTime(time.AddDays(1).ToShortDateString()).Ticks;
            var _where = String.Format("[sid]={0} AND createtime>{1} AND createtime<{2}", sid, begin, end);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
        }

        /// <summary>获取24小时数据</summary>
        /// <param name="sid">服务器编号</param>
        /// <param name="time">时间</param>
        public static EntityList<tgm_record_hours> GetFind24(Int32 sid, DateTime time)
        {
            var begin = Convert.ToDateTime(time.ToShortDateString()).Ticks;
            var end = Convert.ToDateTime(time.AddDays(1).ToShortDateString()).Ticks;
            var _where = String.Format("[sid]={0} AND createtime>{1} AND createtime<{2}", sid, begin, end);
            return FindAll(_where, " createtime desc", "*", 0, 0);
        }

    }
}
