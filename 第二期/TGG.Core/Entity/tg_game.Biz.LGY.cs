using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_game
    {
        public static tg_game GetEntityByUserId(Int64 userid)
        {
            return Find(_.user_id, userid);
        }

        /// <summary>插入游艺园实体</summary>
        public static bool GetInsert(tg_game model)
        {
            try
            {
                Insert(model);
                return true;
            }
            catch { return false; }

        }

        /// <summary>游艺园重置更新</summary>
        public static void GetUpdate()
        {
            Update("tea_max_pass=0,ninjutsu_max_pass=0,calculate_max_pass=0,eloquence_max_pass=0,spirit_max_pass=0,week_max_pass=0", null);
        }
    }
}
