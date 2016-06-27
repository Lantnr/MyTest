using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_role
    {
        /// <summary>
        /// 根据武将主键id获取武将实体
        /// </summary>
        /// <param name="equipid"></param>
        /// <returns></returns>
        public static tg_role GetRoleById(Int64 id)
        {
            return Find(_.id, id);
        }
        /// <summary>
        /// 更新武将信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool GetRoleUpdate(tg_role model)
        {
            try
            {
                Update(model);
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>更新全部武将信息</summary>
        public static bool GetAllRoleUpdate(int power)
        {
            try
            {
                Update(new string[] { _.power }, new object[] { power }, null, null);
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>插入武将信息</summary>
        public static bool GetRoleInsert(tg_role model)
        {
            try
            {
                model.Insert();
                tg_role_life_skill.InitSkill(model.id);
                return true;
            }
            catch
            { return false; }
        }
    }
}
