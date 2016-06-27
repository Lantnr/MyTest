using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_duplicate_checkpoint 业务逻辑操作类
    /// </summary>
    public partial class tg_duplicate_checkpoint
    {
        /// <summary>根据用户id获取爬塔关卡信息</summary>
        public static tg_duplicate_checkpoint GetEntityByUserId(Int64 userid)
        {
            return Find(new String[] { _.user_id }, new Object[] { userid });
        }

        /// <summary>根据关卡实体更新关卡信息</summary>
        public static bool UpdateSite(tg_duplicate_checkpoint model)
        {
            try
            {
                model.Update();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
