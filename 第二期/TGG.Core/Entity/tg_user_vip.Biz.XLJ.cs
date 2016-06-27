using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{
    public partial class tg_user_vip
    {
        /// <summary>根据用户Id获取vip</summary>
        public static tg_user_vip GetEntityByUserId(Int64 user_id = 0)
        {
            var entity = Find(new String[] { _.user_id }, new Object[] { user_id });
            return entity;
        }


        /// <summary>更新全局玩家扩展数据</summary>
        public static int GetUpdateByLevel(tg_user_vip model)
        {
            var exp = new ConcatExpression();
            exp &= _.power == model.power;
            exp &= _.bargain == model.bargain;
            exp &= _.buy == model.buy;
            exp &= _.arena_buy == model.arena_buy;
            exp &= _.arena_cd == model.arena_cd;
            exp &= _.train_home == model.train_home;

            var _where = string.Format("[vip_level]={0}", model.vip_level);

            return Update(exp, _where);
        }
    }
}
