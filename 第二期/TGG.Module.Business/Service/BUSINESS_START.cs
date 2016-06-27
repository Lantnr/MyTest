using System;
using System.Collections.Generic;
using FluorineFx;
using System.Collections;
using System.Linq;
using TGG.Core.Common;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.SocketServer;
using NewLife.Log;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 开始跑商
    /// </summary>
    public class BUSINESS_START
    {
        private static BUSINESS_START ObjInstance;

        /// <summary>BUSINESS_START单体模式</summary>
        public static BUSINESS_START GetInstance()
        {
            return ObjInstance ?? (ObjInstance = new BUSINESS_START());
        }

        /// <summary>开始跑商</summary>
        public ASObject CommandStart(TGGSession session, ASObject data)
        {
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_START", "开始跑商");
#endif

            var destinationid = new ArrayList();

            var carid = Convert.ToInt32(data.FirstOrDefault(q => q.Key == "carId").Value);//跑商马车Id
            var destination = data.FirstOrDefault(q => q.Key == "idList").Value as object[];
            if (!FrontData(destination)) return Common.GetInstance().BuildData((int)ResultType.FRONT_DATA_ERROR);

            var car = tg_car.FindByid(carid);

            if (car.state == (int)CarStatusType.RUNNING) //马车状态验证
            {
                //return Common.getInstance().BuildData((int)ResultType.CAR_RUNNING);
                var _bglist = tg_goods_business.GetListEntityByCid(carid); //马车上货物
                return new ASObject(BuildData((int)ResultType.SUCCESS, car, _bglist));//返回马车当前信息
            }

            //体力判断
            if (!PowerOperate(session.Player.Role.Kind, session)) return ErrorResult((int)ResultType.BASE_ROLE_POWER_ERROR);

            foreach (var item in destination)
            {
                destinationid.Add(item);
            }
            var distance = Common.GetInstance().CityDistance(destinationid); //城市距离
            var bglist = tg_goods_business.GetListEntityByCid(carid); //马车上货物

            Int64 time = Common.GetInstance().RuleData("3003", distance, car.speed) * 1000;
            if (time == 0) time = 1000 * 1000;
            var ticks = (DateTime.Now.Ticks - 621355968000000000) / 10000;
            car.time = ticks + time;
            car.start_ting_id = Convert.ToInt32(destination.FirstOrDefault());
            car.stop_ting_id = Convert.ToInt32(destination.LastOrDefault());
            car.state = (int)CarStatusType.RUNNING;
            car.distance = distance;
            if (car.stop_ting_id == 0)
                XTrace.WriteLine("---- 跑商发车发生错误  用户Id:{0}  马车Id:{1} ----", car.user_id, car.id);
            tg_car.GetCarUpdate(car);
            Common.GetInstance().BusinssStart(car.user_id, car.id, time);
            TaskCheck(bglist, session.Player.User.id);
            (new Share.DaMing()).CheckDaMing(session.Player.User.id, (int)DaMingType.跑商);
            return new ASObject(BuildData((int)ResultType.SUCCESS, car, bglist));
        }

        /// <summary> 验证前端数据 </summary>
        /// <param name="data">要验证的数据</param>
        private bool FrontData(Object[] data)
        {
            if (!data.Any()) return false;
            if (data.Contains(0)) return false;
            return true;
        }

        /// <summary>体力操作</summary>
        private bool PowerOperate(tg_role role, TGGSession session)
        {
            var userid = session.Player.User.id;
            var power = RuleConvert.GetCostPower();
            var totalpower = tg_role.GetTotalPower(role);
            if (totalpower < power) return false;
            new Share.Role().PowerUpdateAndSend(role, power, userid);
            return true;
        }

        private ASObject ErrorResult(int error)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", error}
            };
            return new ASObject(dic);
        }

        /// <summary>玩家所有马车组装数据</summary>
        public Dictionary<String, Object> BuildData(object result, tg_car car, IEnumerable<tg_goods_business> list)
        {
            var dic = new Dictionary<string, object>
            {
                {"result", result},
                {"carVo",EntityToVo.ToBusinessCarVo(car, Common.GetInstance().ConverBusinessGoodsVos(list))}
            };
            return dic;
        }

        /// <summary>
        /// 主线任务更新验证
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userid"></param>
        private void TaskCheck(List<tg_goods_business> list, Int64 userid)
        {
            //foreach (var business in list)
            //{
            //    new Share.TGTask().MainTaskUpdate(TaskStepType.TYPE_BUSINESS, userid, (int)business.goods_id, business.goods_number);
            //}
            if (list.Any())
                new Share.TGTask().MainTaskUpdate(TaskStepType.TYPE_BUSINESS, userid);

        }
    }
}
