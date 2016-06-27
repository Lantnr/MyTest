using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    public partial class tg_log_operate
    {
        /// <summary>获取今日消耗</summary>
        /// <param name="res_type">资源类型</param>
        /// <param name="type">消耗类型</param>    
        public static Int32 GetTodayCost(Int32 res_type, Int32 type)
        {
            var db = DAL.Create(Meta.ConnName);
            var sql = String.Format("SELECT SUM([count]) AS cost FROM [tg_log_operate] where [resource_type]={0} AND [type]={1} AND  DATEDIFF(hh,[time],GETDATE())<=24 ",res_type, type);
           
            var ds = db.Session.Query(sql);

            var q = ds.Tables[0].AsEnumerable();
            var entity = q.Select(m => Convert.ToInt32(m["cost"])).FirstOrDefault();
            return entity;
        }
    }
}
