using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    public partial class view_user_role_family_member
    {
        /// <summary>
        /// 根据家族id获取家族成员实体集合
        /// </summary>
        public static List<view_user_role_family_member> GetAllById(Int64 fid)
        {
            return FindAll(_.fid, fid);
        }

        /// <summary>
        /// 查询全部家族实体集合
        /// </summary>
        public static List<view_user_role_family_member> GetAll()
        {
            return FindAll();
        }

        /// <summary>
        /// 根用户id查询家族成员实体
        /// </summary>
        public static view_user_role_family_member GetEntityByUserId(Int64 userid)
        {
            return Find(_.userid,userid);
        }

        /// <summary>根据ids集合查找</summary>
        public static List<view_user_role_family_member> GetFindByUserIds(IEnumerable<Int64> ids)
        {
            var _ids = string.Join(",", ids.ToArray());
            var where = string.Format("userid in ({0})", _ids);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>
        /// 根据家族身份获取家族实体集合
        /// </summary>
        public static List<view_user_role_family_member> GetAllByDegree()
        {
            return FindAll(_.degree, (int)Enum.Type.FamilyPositionType.CHIEF);
        }
    }
}
