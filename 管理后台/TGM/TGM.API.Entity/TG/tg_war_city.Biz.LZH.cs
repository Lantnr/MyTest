using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_war_city
    {
        /// <summary>根据用户id查询据点表信息</summary>
        public static List<tg_war_city> GetCityByUserId(Int64 userId)
        {
            return FindAll(new String[] { _.user_id }, new Object[] { userId });
        }
    }
}
