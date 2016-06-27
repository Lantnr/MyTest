using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public class log
    {
        public static void BagInsertLog(List<tg_bag> bags, int mn, int cn, int getcoin)
        {
            foreach (var item in bags)
            {
                BagInsertLog(item, mn, cn, getcoin);
            }
        }

        /// <summary> 背包日志记录 </summary>
        public static void BagInsertLog(tg_bag bag, int mn, int cn, int getcoin)
        {
            if (bag == null) return;
            var entity = new tg_bag_log()
            {
                bid = bag.id,
                base_id = bag.base_id,
                attribute1_spirit_level = bag.attribute1_spirit_level,
                attribute1_spirit_value = bag.attribute1_spirit_value,
                attribute1_type = bag.attribute1_type,
                attribute2_spirit_level = bag.attribute2_spirit_level,
                attribute2_spirit_value = bag.attribute2_spirit_value,
                attribute2_type = bag.attribute2_type,
                attribute3_spirit_level = bag.attribute3_spirit_level,
                attribute3_spirit_value = bag.attribute3_spirit_value,
                attribute3_type = bag.attribute3_type,
                attribute1_spirit_lock = bag.attribute1_spirit_lock,
                attribute1_value = bag.attribute1_value,
                attribute2_spirit_lock = bag.attribute2_spirit_lock,
                attribute2_value = bag.attribute2_value,
                attribute3_spirit_lock = bag.attribute3_spirit_lock,
                attribute3_value = bag.attribute3_value,
                bind = bag.bind,
                count = bag.count,
                equip_type = bag.equip_type,
                user_id = bag.user_id,
                type = bag.type,
                state = bag.state,
                sell_time = DateTime.Now.Ticks,
                module_number = mn,
                command_number = cn,
                get_coin = getcoin,
            };
            entity.Save();
        }

        /// <summary> 金币消费记录 </summary>
        /// <param name="gold">金币消费数量</param>
        /// <param name="userid">用户Id</param>
        /// <param name="mn">模块号</param>
        /// <param name="cn">指令号</param>
        public static void GoldInsertLog(int gold, Int64 userid, int mn, int cn)
        {
            var model = new tg_gold_log
            {
                consume = gold,
                command_number = cn,
                module_number = mn,
                time = DateTime.Now.Ticks,
                user_id = userid,
            };
            model.Save();
        }
    }
}
