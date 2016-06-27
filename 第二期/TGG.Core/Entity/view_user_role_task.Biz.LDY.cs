using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class view_user_role_task
    {
        /// <summary>
        /// 查询所有玩家身份和职业信息
        /// </summary>
        public static List<view_user_role_task> GetAll()
        {
            return FindAll();
        }
    }
}
