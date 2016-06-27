using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGG.Core.Entity
{

    public partial class tg_log_operate
    {
        /// <summary>查询玩家游戏记录信息</summary>
        public static EntityList<tg_log_operate> GetLogEntity(Int64 playerId, Int32 type, Int32 index, Int32 size, out Int32 count)
        {
            var where = "";
            switch (type)
            {
                case 0: where = string.Format("user_id={0}", playerId); break;
                case 6: where = string.Format("user_id={0} and resource_type in (6,12)", playerId); break;
                case 1:
                case 3:
                case 11: where = string.Format("user_id={0} and resource_type={1}", playerId, type); break;
            }
            count = FindCount(where, null, null, 0, 0);
            return FindAll(where, " time desc", "*", index * size, size);
        }

        /// <summary>查询 玩家元宝消耗日志</summary>
        public static List<tg_log_operate> GetUserGoldLogs(Int64 playerId)
        {
            var where = string.Format("resource_type=3 and type=1000 and user_id={0}", playerId);
            return FindAll(where, null, null, 0, 0);
        }
    }
}
