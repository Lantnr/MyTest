using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_fight_yin
    {
        /// <summary>根据用户id查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<tg_fight_yin> GetFindByUserId(Int64 user_id)
        {
            return FindAll(_.user_id, user_id);
        }
    }
}
