using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>武将招募表</summary>
    public partial class tg_role_recruit
    {
        /// <summary>根据用户id查找</summary>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<tg_role_recruit> GetFindAllByUserId(Int64 userid)
        {
            return FindAll(_.user_id, userid);
        }

        public static EntityList<tg_role_recruit> GetEntityList(List<tg_role_recruit> list)
        {
            var l = new EntityList<tg_role_recruit>();
            l.AddRange(list);
            return l;
        }

        /// <summary>该位置是否已经存在武将</summary>
        public static bool GetFindIsExist(Int64 userid, int position)
        {
            return FindCount(new String[] { _.user_id, _.position }, new Object[] { userid, position }) > 0;

        }

        /// <summary>重置酒馆武将卡牌</summary>
        public static bool ResetCard(Int64 userid)
        {
            return Delete(new String[] { _.user_id }, new Object[] { userid }) > 0;

        }

    }
}
