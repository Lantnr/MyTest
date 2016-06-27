using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_family_log
    {
        /// <summary>
        /// 根据家族id删除家族日志
        /// </summary>
        public static bool GetDelete(Int64 fid)
        {
            try
            {
                Delete(new String[] { _.fid }, new Object[] { fid });
                return true;
            }
            catch { return false; } 
        }

        /// <summary>
        /// 根据家族id查询家族日志
        /// </summary>
        public static List<tg_family_log> GetEntityByFid(Int64 fid)
        {
            return FindAll(_.fid,fid);
        }

        /// <summary>
        /// 插入家族日志
        /// </summary>
        public static bool GetInsert(tg_family_log model)
        {
            try
            {
                Insert(model);
                return true;
            }
            catch { return false; }
        }
    }
}
