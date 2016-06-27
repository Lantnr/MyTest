using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluorineFx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NewLife.Log;
using TGG.Core.Common.Randoms;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;
using TGG.SocketServer;
using tg_user = TGG.Core.Entity.tg_user;

namespace TGG.Module.Business.Service
{
    /// <summary>
    /// 跑商公共方法类
    /// </summary>
    public partial class Common
    {
        private static Common _objInstance;

        /// <summary>Common单例模式</summary>
        public static Common GetInstance()
        {
            return _objInstance ?? (_objInstance = new Common());
        }

        #region Base
        /// <summary>数据组装</summary>
        public ASObject BuildData(int result)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("result", result);
            return new ASObject(dic);
        }


        /// <summary>解析规则</summary>
        public int RuleData(string rule, int distance, int value)
        {
            var base_rule = Variable.BASE_RULE.FirstOrDefault(q => q.id == rule);
            if (base_rule == null) return 1000;
            var temp = base_rule.CloneEntity().value;
            temp = temp.Replace("distance", distance.ToString("0.00"));
            temp = temp.Replace("speed", value.ToString("0.00"));
            var express = CommonHelper.EvalExpress(temp);
            var result = Convert.ToInt32(express);
            return result;
        }

        /// <summary>根据城市城市id集合，计算城市的距离</summary>
        public Int32 CityDistance(ArrayList arrCity)
        {
            try
            {
                double distance = 0;
                for (var i = 0; i < arrCity.Count - 1; i++)
                {
                    string coorPointA = "", coorPointB = "";
                    var cityA = (int)arrCity[i];
                    var cityB = (int)arrCity[i + 1];
                    foreach (var city in Variable.BASE_TING)
                    {
                        if (cityA == city.id)
                            coorPointA = city.coorPoint;
                        if (cityB == city.id)
                            coorPointB = city.coorPoint;
                    }
                    var coorPointX = Math.Abs(int.Parse(coorPointA.Split(',')[0]) - int.Parse(coorPointB.Split(',')[0]));
                    var coorPointY = Math.Abs(int.Parse(coorPointA.Split(',')[1]) - int.Parse(coorPointB.Split(',')[1]));
                    distance += Math.Sqrt(coorPointX * coorPointX + coorPointY * coorPointY);

                }
                return Convert.ToInt32(distance);
            }
            catch
            {

                return 100;
            }


        }

        #endregion

        #region 20140711 arlen 跑商马车重新加入线程


        /// <summary>跑商马车重新加入线程</summary>
        public void BusinessCarRecovery()
        {
            var time = CurrentMs() + 5000;//+5000毫秒预热
            tg_car.GetUpdateByTime(time);//对该玩家跑商完成未修改的马车进行修改

            var list = tg_car.GetRunning((int)CarStatusType.RUNNING);
            foreach (var item in list)
            {
                var _time = item.time - CurrentMs();
                if (_time < 0) continue;
                BusinssStart(item.user_id, item.id, _time);
            }
        }

        /// <summary>当前毫秒</summary>
        private Int64 CurrentMs()
        {
            // ReSharper disable once PossibleLossOfFraction
            return (DateTime.Now.Ticks - 621355968000000000) / 10000;
        }

        public void LoginCar(Int64 user_id)
        {
            var time = CurrentMs() + 5000;//+5000毫秒预热
            var list = tg_car.GetEntityListByState((int)CarStatusType.RUNNING, user_id, time);
            foreach (var item in list)
            {
                var key = string.Format("{0}_{1}_{2}", (int)CDType.Businss, item.user_id, item.id);
                Variable.CD.AddOrUpdate(key, true, (k, v) => true);
            }
        }

        #endregion

        #region 跑商线程

        /// <summary>开始跑商</summary>
        /// <param name="cid">马车主键</param>
        /// <param name="time">跑商时间</param>
        public void BusinssStart(Int64 user_id, Int64 cid, Int64 time)
        {
            try
            {
                var token = new CancellationTokenSource();
# if DEBUG
                //time = 30000;
#endif
                Object obj = new BusinssObject { user_id = user_id, cid = cid, time = time };
                Task.Factory.StartNew(m =>
                {
                    var t = m as BusinssObject;
                    if (t == null) return;
                    var key = t.GetKey();
                    Variable.CD.AddOrUpdate(key, false, (k, v) => false);
                    SpinWait.SpinUntil(() =>
                    {
                        var b = Convert.ToBoolean(Variable.CD[key]);
                        return b;
                    }, Convert.ToInt32(t.time));
                }, obj, token.Token)
                .ContinueWith((m, n) =>
                {
                    var bo = n as BusinssObject;
                    if (bo == null) { token.Cancel(); return; }

                    BUSINESS_END.GetInstance().CommandStart(bo.cid);
                    //移除全局变量
                    var key = bo.GetKey();
                    bool r;
                    Variable.CD.TryRemove(key, out r);

                    token.Cancel();
                }, obj, token.Token);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        class BusinssObject
        {
            public Int64 user_id { get; set; }
            public Int64 cid { get; set; }
            public Int64 time { get; set; }

            public String GetKey()
            {
                return string.Format("{0}_{1}_{2}", (int)CDType.Businss, user_id, cid);
            }

        }

        #endregion

    }
}
