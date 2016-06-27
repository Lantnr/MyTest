using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class view_user_role_family_apply
    {

        /// <summary>
        /// 根据家族id查询家族申请信息
        /// </summary>
        public static List<view_user_role_family_apply> GetEntityByFid(Int64 fid)
        {
            return FindAll(_.fid, fid);
        }

    }
}
