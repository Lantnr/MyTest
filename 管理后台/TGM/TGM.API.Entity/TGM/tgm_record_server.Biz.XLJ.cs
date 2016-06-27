using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>
    /// tgm_record_server逻辑类
    /// </summary>
    public partial class tgm_record_server
    {
        public static tgm_record_server GetFindBySid(Int32 sid)
        {
            var where = String.Format("sid={0}", sid);
            return Find(where);
        }

        /// <summary>所以总充值</summary>
        /// <param name="pid">平台编号</param>
        /// <param name="sid">服务器编号</param>
        public static Int32 GetServerTotalPay(Int32 pid, Int32 sid)
        {
            var db = DAL.Create(Meta.ConnName);
            var sql = "SELECT SUM(pay_total) AS pay_total FROM [tgm_record_server] ";
            if (pid != 0)
            {
                sql = String.Format("{0} WHERE [pid]={1}", sql, pid);
                if (sid != 0)
                {
                    sql = String.Format("{0} and [sid]={1}", sql, sid);
                }
            }
            var ds = db.Session.Query(sql);

            var q = ds.Tables[0].AsEnumerable();
            var entity = q.Select(m => Convert.ToInt32(m["pay_total"])).FirstOrDefault();
            return entity;
        }

        /// <summary>月总充值</summary>
        /// <param name="pid">平台编号</param>
        /// <param name="sid">服务器编号</param>
        public static Int32 GetServerMonthPay(Int32 pid, Int32 sid)
        {
            var db = DAL.Create(Meta.ConnName);
            var sql = "SELECT SUM(pay_month) AS pay_month FROM [tgm_record_server] ";
            if (pid != 0)
            {
                sql = String.Format("{0} WHERE [pid]={1}", sql, pid);
                if (sid != 0)
                {
                    sql = String.Format("{0} and [sid]={1}", sql, sid);
                }
            }
            var ds = db.Session.Query(sql);

            var q = ds.Tables[0].AsEnumerable();
            var entity = q.Select(m => Convert.ToInt32(m["pay_month"])).FirstOrDefault();
            return entity;
        }

        /// <summary>服务器记录统计分页</summary>
        /// <param name="pid">平台编号</param>
        /// <param name="sid">服务器编号</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        public static EntityList<tgm_record_server> GetPageEntity(Int32 pid, Int32 sid, Int32 index, Int32 size, out Int32 count)
        {
            var _where = string.Empty;
            if (pid != 0)
            {
                _where = String.Format("{0} [pid]={1}", _where, pid);
                if (sid != 0)
                {
                    _where = String.Format("{0} and [sid]={1}", _where, sid);
                }
            }
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
        }



    }
}
