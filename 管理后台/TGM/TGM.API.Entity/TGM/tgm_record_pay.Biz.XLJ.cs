using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    public partial class tgm_record_pay
    {
        /// <summary>保存实体</summary>
        /// <param name="model">实体</param>
        public static void SaveEntity(tgm_record_pay model)
        {
            model.Save();
        }

        public static void Proc_sp_pay_syn(Int64 pid)
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
                    new SqlParameter("@pid", pid),
                    new SqlParameter("@_begin_time", t0),
                    new SqlParameter("@_end_time", t1),
                    new SqlParameter("@M_BEGIN", m0),
                    new SqlParameter("@M_END", m1)
                };

            var ds = db.Session.Query("sp_pay_syn", CommandType.StoredProcedure, p);
        }
    }
}
