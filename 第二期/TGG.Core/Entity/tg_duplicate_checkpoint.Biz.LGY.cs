using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_duplicate_checkpoint
    {
        /// <summary>插入爬塔实体</summary>
        public static bool GetInsert(tg_duplicate_checkpoint model)
        {
            try
            {
                Insert(model);
                return true;
            }
            catch { return false; }

        }

        /// <summary>根据关卡获取爬塔关卡信息</summary>
        public static tg_duplicate_checkpoint GetEntityByTowerSite(int towersite)
        {
            return Find(new String[] { _.tower_site }, new Object[] { towersite });
        }

        /// <summary>获取所有塔主信息</summary>
        public static List<tg_duplicate_checkpoint> GetTowers()
        {
            var where = string.Format("tower_site > {0}", 0);
            return FindAll(where, null, null, 0, 0);
        }

        /// <summary>爬塔每日重置更新</summary>
        public static void GetUpdate()
        {
            Update("site=0,tower_site=0,state=0,dekaron=0", null);
        }
    }
}
