using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_family_member
    {
        /// <summary>
        /// 根据用户id获取家族成员实体
        /// </summary>
        public static tg_family_member GetEntityById(Int64 userid)
        {
            return Find(_.userid, userid);
        }

        /// <summary>
        /// 根据id获取家族成员实体
        /// </summary>
        public static tg_family_member GetEntityByIdd(Int64 id)
        {
            return Find(_.id, id);
        }

        /// <summary>
        /// 根据家族id获取家族成员实体集合
        /// </summary>
        public static List<tg_family_member> GetAllById(Int64 fid)
        {
            return FindAll(_.fid, fid);
        }

        /// <summary>
        /// 根据家族身份获取家族实体集合
        /// </summary>
        public static List<tg_family_member> GetAllByDegree()
        {
            return FindAll(_.degree, (int)Enum.Type.FamilyPositionType.CHIEF);
        }

        /// <summary>
        /// 根据id删除家族成员
        /// </summary>
        public static bool GetDelete(Int64 id)
        {
            try
            {               
                Delete(new String[]{_.id},new Object[]{id});
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// 查询全部家族实体集合
        /// </summary>
        public static IEnumerable<tg_family_member> GetAll()
        {
            return FindAll();
        }

        /// <summary>插入家族成员实体</summary>
        public static bool GetInsert(tg_family_member model)
        {
            try
            {
                model.Insert();
                return true;
            }
            catch { return false; }

        }

        /// <summary>
        /// 根据家族id删除家族成员信息
        /// </summary>
        public static bool GetMembersDelete(Int64 fid)
        {
            //string where = string.Format("fid = {0}", fid);
            //return tg_family_member.Delete(where) > 0;
            try
            {
                Delete(new String[] { _.fid }, new Object[] { fid });
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 更新家族成员信息
        /// </summary>
        public static bool GetUpdate(tg_family_member model)
        {
            try
            {
                Update(model);
                return true;
            }
            catch { return false; }
        }

        /// <summary>更新家族成员领取状态和捐献金钱信息</summary>
        public static void UpdateStateAndDonate()
        {
            Update("state=0,donate=0", null);
        }
    }
}
