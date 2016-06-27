using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>Fight_Personal</summary>
    /// <remarks></remarks>
    public partial class tg_fight_personal
    {
        /// <summary>根据用户id查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static tg_fight_personal GetFindByUserId(Int64 user_id)
        {
            return Find(_.user_id, user_id);
        }

        /// <summary> 插入阵 </summary>
        public static tg_fight_personal PersonalInsert(Int64 userid, Int64 roleid)
        {
            var model = new tg_fight_personal
            {
                yid = 0,
                user_id = userid,
                matrix1_rid = roleid,
                matrix2_rid = 0,
                matrix3_rid = 0,
                matrix4_rid = 0,
                matrix5_rid = 0,
            };
            model.Insert();
            return model;
        }
    }
}
