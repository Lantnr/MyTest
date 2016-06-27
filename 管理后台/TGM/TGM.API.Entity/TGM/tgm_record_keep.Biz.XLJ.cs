using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    public partial class tgm_record_keep
    {

        #region 实体
        private class KeepItem
        {
            public Double D_1 { get; set; }
            public Double N_1 { get; set; }
            public Double R_1 { get; set; }
            public Double D_3 { get; set; }
            public Double N_3 { get; set; }
            public Double R_3 { get; set; }
            public Double D_5 { get; set; }
            public Double N_5 { get; set; }
            public Double R_5 { get; set; }
            public Double D_7 { get; set; }
            public Double N_7 { get; set; }
            public Double R_7 { get; set; }
            public Double D_30 { get; set; }
            public Double N_30 { get; set; }
            public Double R_30 { get; set; }
        }
        #endregion

        /// <summary>获取服务器留存数据</summary>
        /// <param name="tg_connect">服务器连接字符串</param>
        /// <param name="time">开服时间</param>
        public static tgm_record_keep sp_user_keep(String tg_connect, Int64 time)
        {
            var db = DAL.Create(tg_connect);
            var ds = db.Session.Query("sp_user_keep", CommandType.StoredProcedure, new SqlParameter("@open_time", time));

            var q = ds.Tables[0].AsEnumerable();
            //（第2天至第N天的总登陆人数）去掉重复-第N天的新增用户/N-1(第一天到(N-1)天的总注册人数
            var l = q.Select(m => new KeepItem
            {
                D_1 = Convert.ToDouble(m["D_1"]),
                N_1 = Convert.ToDouble(m["N_1"]),
                R_1 = Convert.ToDouble(m["R_1"]),
                D_3 = Convert.ToDouble(m["D_3"]),
                N_3 = Convert.ToDouble(m["N_3"]),
                R_3 = Convert.ToDouble(m["R_3"]),
                D_5 = Convert.ToDouble(m["D_5"]),
                N_5 = Convert.ToDouble(m["N_5"]),
                R_5 = Convert.ToDouble(m["R_5"]),
                D_7 = Convert.ToDouble(m["D_7"]),
                N_7 = Convert.ToDouble(m["N_7"]),
                R_7 = Convert.ToDouble(m["R_7"]),
                D_30 = Convert.ToDouble(m["D_30"]),
                N_30 = Convert.ToDouble(m["N_30"]),
                R_30 = Convert.ToDouble(m["R_30"]),
            }).FirstOrDefault();

            var entity = new tgm_record_keep();
            if (l != null)
            {
                entity.createtime = time;
                entity.login_30 = Convert.ToInt32(l.D_30);
                entity.keep_1 = l.R_1 == 0 ? 0 : Math.Round((l.D_1 - l.N_1) / l.R_1 * 100, 2);
                entity.keep_3 = l.R_3 == 0 ? 0 : Math.Round((l.D_3 - l.N_3) / l.R_3 * 100, 2);
                entity.keep_5 = l.R_5 == 0 ? 0 : Math.Round((l.D_5 - l.N_5) / l.R_5 * 100, 2);
                entity.keep_7 = l.R_7 == 0 ? 0 : Math.Round((l.D_7 - l.N_7) / l.R_7 * 100, 2);
                entity.keep_30 = l.R_30 == 0 ? 0 : Math.Round((l.D_30 - l.N_30) / l.R_30 * 100, 2);
            }
            return entity;

        }

        /// <summary>根据sid获取实体</summary>
        /// <param name="sid">服务器id</param>
        public static tgm_record_keep GetFindEntityBySid(Int32 sid)
        {
            return Find(String.Format("[sid]={0}", sid));
        }

        /// <summary>根据Pid获取平台存留数据</summary>
        /// <param name="pid">平台编号</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        public static EntityList<tgm_record_keep> GetPageEntity(Int32 pid, Int32 index, Int32 size, out Int32 count)
        {
            var sql = String.Format("[pid]={0}", pid);
            count = FindCount(sql, null, null, 0, 0);
            return FindAll(sql, " createtime desc", "*", index * size, size);
        }


    }
}
