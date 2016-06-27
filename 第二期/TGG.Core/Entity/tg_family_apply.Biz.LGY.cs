using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_family_apply
    {
        /// <summary>插入家族申请实体</summary>
        public static bool GetInsert(tg_family_apply model)
        {
            try
            {
                Insert(model);
                return true;
            }
            catch { return false; }

        }

        /// <summary>根据用户id和家族id删除家族申请实体</summary>
        public static bool GetDelete(Int64 userid, Int64 fid)
        {
            try
            {
                Delete(new String[] { _.userid, _.fid }, new Object[] { userid, fid });
                return true;
            }
            catch { return false; }             
        }
        /// <summary>
        /// 根据家族id删除家族申请信息
        /// </summary>
        public static bool GetDelete(Int64 id)
        {
            try
            {
                Delete(new String[] { _.fid}, new Object[] { id });
                return true;
            }
            catch { return false; }  
        }

        /// <summary>
        /// 根据家族id查询家族申请信息
        /// </summary>
        public static List<tg_family_apply> GetEntityByFid(Int64 fid)
        {
            return FindAll(_.fid,fid);
        }


        /// <summary>
        /// 根据用户id查询家族申请信息
        /// </summary>
        public static List<tg_family_apply> GetEntityByUserId(Int64 userid)
        {
            return FindAll(_.userid, userid);
        }
    }
}
