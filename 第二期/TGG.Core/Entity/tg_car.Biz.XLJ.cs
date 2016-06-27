using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGG.Core.Global;
using TGG.Core.Enum.Type;

namespace TGG.Core.Entity
{
    /// <summary>
    /// 马车部分类
    /// </summary>
    public partial class tg_car
    {
        /// <summary>初始马车</summary>
        public static bool InitCar(Int64 user_id)
        {
            try
            {
                var rbp = Variable.BASE_RULE.FirstOrDefault(q => q.id == "1003");
                var rp = Variable.BASE_RULE.FirstOrDefault(q => q.id == "3009");
                if (rp == null || rbp == null) return false;
                var birthplace = Convert.ToInt32(rbp.value);    //马车出生地
                var packet = Convert.ToInt32(rp.value);         //默认马车开启车厢数量
                var list = Variable.BASE_PART.Where(m => m.vip == 0).ToList();
                foreach (var item in list)
                {
                    if (item.id <= 0) continue;
                    var car = new tg_car
                    {
                        car_id = item.id,
                        distance = 0,
                        packet = packet,
                        rid = 0,
                        speed = item.speed,
                        start_ting_id = birthplace,
                        state = (int)CarStatusType.STOP,
                        stop_ting_id = 0,
                        time = 0,
                        user_id = user_id,
                    };
                    car.Save();
                }
                return true;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return false;
            }
        }

        /// <summary>跑商中马车集合</summary>
        public static List<tg_car> GetRunning(int state)
        {
            return FindAll(string.Format("state={0} ", state), null, null, 0, 0);
        }

        /// <summary>根据时间操作tg_car</summary>
        public static void GetUpdateByTime(Int64 time)
        {
            var list = FindAll(string.Format("state=1 and time<={0}", time), null, null, 0, 0);
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
        public static List<tg_car> GetEntityListByState(int state, Int64 userid, Int64 endtime)
        {
            return FindAll(string.Format("state={0} and user_id={1} and time<{2}", state, userid, endtime), null, null, 0, 0);
        }

        /// <summary>马车是否跑商</summary>
        public static bool GetCanBusiness(Int64 id)
        {
            return FindCount(new String[] { _.id, _.state }, new Object[] { id, (int)CarStatusType.STOP }) > 0;
        }

        /// <summary>马车是否跑商</summary>
        public static bool GetStopTing(Int64 user_id, Int32 tid)
        {
            return FindCount(new String[] { _.user_id, _.start_ting_id, _.state }, new Object[] { user_id, tid, (int)CarStatusType.STOP }) > 0;
        }

        /// <summary>根据马车状态获取实体集合</summary>
        public static int GetTaskCarUpdate(Int64 id)
        {
            var _set = string.Format("start_ting_id=stop_ting_id,stop_ting_id=0,time=0,distance=0,state=0 ");
            var _where = string.Format("id={0}  and stop_ting_id<>0", id);

            return Update(_set, _where);
        }

    }
}
