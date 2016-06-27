using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_car
    {
        /// <summary>根据用户Id获取实体</summary>
        public static List<tg_car> GetEntityByUserId(Int64 user_id)
        {
            return tg_car.FindAll(__.user_id, user_id);
        }
    }
}
