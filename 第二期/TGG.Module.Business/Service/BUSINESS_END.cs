using FluorineFx;
using System;
using System.Collections.Generic;
using SuperSocket.Common;
using TGG.Core.Common;
using TGG.Core.Entity;
using TGG.Core.Enum;
using TGG.Core.Enum.Command;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using NewLife.Log;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 跑商结束
    /// </summary>
    public class BUSINESS_END
    {
        private static BUSINESS_END _objInstance;

        /// <summary>BUSINESS_END单体模式</summary>
        public static BUSINESS_END GetInstance()
        {
            return _objInstance ?? (_objInstance = new BUSINESS_END());
        }

        /// <summary>跑商结束</summary>
        public void CommandStart(Int64 cid)
        {
            try
            {
# if DEBUG
                XTrace.WriteLine("{0}:{1}", "BUSINESS_END", "跑商结束");
#endif
                var car = tg_car.FindByid(cid);
                var goods = tg_goods_business.GetListEntityByCid(cid); //马车上货物
                if (car.stop_ting_id == 0)
                    XTrace.WriteLine("---- 跑商结束发生错误  用户Id:{0}  马车Id:{1} ----", car.user_id, car.id);
                if (car.state == (int)CarStatusType.STOP) //马车状态验证
                {
                    GetBusinessEnd(car.user_id, BuildData(car, goods));
                    return;
                }
                tg_car.GetTaskCarUpdate(car.id);
                car = tg_car.FindByid(car.id);
                if (car.start_ting_id == 0)
                    XTrace.WriteLine("---- 跑商结束发生没有开始町错误  用户Id:{0}  马车Id:{1} ----", car.user_id, car.id);
                Common.GetInstance().EnterTing(car.user_id, car.start_ting_id);
                GetBusinessEnd(car.user_id, BuildData(car, goods));
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        /// <summary> 跑商结束 </summary>
        /// <param name="session">session</param>
        /// <param name="car">马车信息</param>
        public tg_car CommandStart(TGGSession session, tg_car car)
        {
            car.start_ting_id = car.stop_ting_id.BinaryClone();
            car.state = (int)CarStatusType.STOP;
            car.stop_ting_id = 0;
            car.distance = 0;
            car.time = 0;
            tg_car.Update(car);
            Common.GetInstance().EnterTing(car.user_id, car.start_ting_id);
            return car;
        }

        /// <summary>组装数据</summary>
        public ASObject BuildData(tg_car car, IEnumerable<tg_goods_business> list)
        {
            var dic = new Dictionary<string, object> { { "carVo", EntityToVo.ToBusinessCarVo(car, Common.GetInstance().ConverBusinessGoodsVos(list)) } };
            return new ASObject(dic);
        }

        /// <summary>发送跑商结束协议</summary>
        private static void GetBusinessEnd(Int64 user_id, ASObject data)
        {
            var b = Variable.OnlinePlayer.ContainsKey(user_id);
            if (!b) { return; }
            var session = Variable.OnlinePlayer[user_id] as TGGSession;
            if (session == null) return;
#if DEBUG
            XTrace.WriteLine("{0}:{1}", "BUSINESS_END", "跑商结束协议发送");
#endif
            var pv = session.InitProtocol((int)ModuleNumber.BUSINESS, (int)BusinessCommand.BUSINESS_END, (int)ResponseType.TYPE_SUCCESS, data);
            session.SendData(pv);
        }
    }
}
