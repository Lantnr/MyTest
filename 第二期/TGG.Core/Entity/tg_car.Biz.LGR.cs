using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Enum.Type;
using XCode.DataAccessLayer;

namespace TGG.Core.Entity
{
    public partial class tg_car
    {
        /// <summary>根据用户Id获取实体</summary>
        public static List<tg_car> GetEntityList(Int64 userid, int tingid, int state)
        {
            return FindAll(string.Format("user_id={0} and start_ting_id={1} and state={2}", userid, tingid, state), null, null, 0, 0);
        }

        /// <summary>根据时间操作tg_car</summary>
        public static void GetEntityListByTime(Int64 time, Int64 userid)
        {
            var list = FindAll(string.Format("state=1 and time<={0} and user_id={1}", time, userid), null, null, 0, 0);
            foreach (var item in list)
            {
                item.start_ting_id = item.stop_ting_id;
                item.state = (int)CarStatusType.STOP;
                item.stop_ting_id = 0;
                item.distance = 0;
                item.time = 0;
            }
            list.Update();
        }

        /// <summary>根据马车状态获取实体集合</summary>
        public static int GetCarUpdate(tg_car car)
        {
            var _set = string.Format("time={0},start_ting_id={1},stop_ting_id={2},distance={3},state={4} ", car.time, car.start_ting_id, car.stop_ting_id, car.distance, car.state);
            var _where = string.Format("id={0}", car.id);

            return Update(_set, _where);
        }


    }
}
