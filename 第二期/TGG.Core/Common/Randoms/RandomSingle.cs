using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;

namespace TGG.Core.Common.Randoms
{
    /// <summary>
    /// 单随机类
    /// </summary>
    public class RandomSingle
    {
        /// <summary>随机最大值</summary>
        public Double Max { get; set; }

        /// <summary>随机最小值</summary>
        public Double Min { get; set; }

        /// <summary>扩大倍数</summary>
        public Int32 Ratio { get; set; }

        /// <summary>构造函数</summary>
        /// <param name="ratio">扩大倍数</param>
        public RandomSingle(Int32 ratio)
        {
            Ratio = ratio + 1;
            Max = 99;
            Min = 0;
        }
        /// <summary>构造函数</summary>
        public RandomSingle() : this(0) { }

        /// <summary>单个概率计算</summary>
        /// <param name="probabilities">概率</param>
        public bool IsTrue(Int32 probabilities)
        {
            probabilities = probabilities > 100 ? 100 : probabilities;
            var matter = RNG.Next(Convert.ToInt32(Min), Convert.ToInt32(Max), 100).ToList();
            var list = RNG.Next(probabilities, matter);
            var rd = RNG.Next(Convert.ToInt32(Min), Convert.ToInt32(Max));
            return list.Contains(rd);
        }

        /// <summary>单个概率计算</summary>
        /// <param name="probabilities">概率</param>
        public bool IsTrue(Double probabilities, params Int32[] digit)
        {
            probabilities = probabilities > 100 ? 100 : probabilities;
            Int32 _digit = 0;
            if (digit.Any()) _digit = digit[0];
            var number = Convert.ToInt32(probabilities);
            var matter = RNG.NextDouble(100, Min, Max, _digit).ToList();
            var list = RNG.NextDouble(number, matter);
            var rd = RNG.NextDouble(Min, Max, _digit);
            return list.Contains(rd);
        }


        /// <summary>随机方法</summary>
        /// <param name="probabilities">概率</param>
        public Int32 RandomFun(params Int32[] probabilities)
        {
            if (!probabilities.Any()) return 0;
            var order = probabilities.OrderByDescending(m => m);
            foreach (var item in order)
            {
                //var list = RandomList(item);
                if (IsTrue(item)) return item;
            }
            return order.Count() > 1 ? order.FirstOrDefault() : 0;
        }

        /// <summary>随机方法</summary>
        /// <param name="count">随机获取多个</param>
        /// <param name="probabilities">概率</param>
        public List<Int32> RandomFun(Int32 count, Int32[] probabilities)
        {
            var list = new List<Int32>();
            for (var i = 0; i < count; i++)
            {
                list.Add(RandomFun(probabilities));
            }
            return list;
        }
        #region 扩展
        /// <summary>随机方法</summary>
        /// <param name="count">随机获取多个</param>
        /// <param name="probabilities">概率对象</param>
        public List<Objects> RandomFun(Int32 count, List<Objects> probabilities)
        {
            var list = new List<Objects>();
            for (var i = 0; i < count; i++)
            {
                list.Add(RandomFun(probabilities));
            }
            return list;
        }

        /// <summary>随机方法</summary>
        /// <param name="probabilities">概率对象</param>
        public Objects RandomFun(List<Objects> probabilities)
        {
            if (!probabilities.Any()) return new Objects();
            var order = probabilities.OrderByDescending(m => m.Probabilities);
            foreach (var item in order)
            {
                //var list = RandomList(item);
                if (IsTrue(item.Probabilities)) return item;
            }
            return order.Count() > 1 ? order.FirstOrDefault() : new Objects();
        }

        #endregion

        /// <summary>获取小数位</summary>
        private static Double GetDigit(Int32 digit)
        {
            if (digit < 0) digit = 0;
            return Math.Pow(10, digit);
        }

        /// <summary>随机方法</summary>
        /// <param name="probabilities">概率</param>
        public Double RandomFunDouble(params Double[] probabilities)
        {
            if (!probabilities.Any()) return 0;
            var order = probabilities.OrderByDescending(m => m);
            foreach (var item in order)
            {
                //var list = RandomList(item);
                var number = ((Max - Min) * item) / 100;
                var list = RNG.NextDouble(Convert.ToInt32(Min), Convert.ToInt32(Max), number);
                var rd = RNG.NextDouble(Min, Max);
                if (list.Contains(rd)) return item;
            }
            return order.FirstOrDefault();
        }

        /// <summary>随机方法</summary>
        /// <param name="count">随机获取多个</param>
        /// <param name="probabilities">概率</param>
        public List<Double> RandomFunDouble(Int32 count, Double[] probabilities)
        {
            var list = new List<Double>();
            for (var i = 0; i < count; i++)
            {
                list.Add(RandomFunDouble(probabilities));
            }
            return list;
        }

        /// <summary>随机一个数组</summary>
        /// <param name="probabilities"></param>
        private IEnumerable<Int32> RandomList(Int32 probabilities)
        {
            var number = Convert.ToInt32(((Max - Min) * probabilities) / 100);
            return from x in Enumerable.Range(Convert.ToInt32(Min), Convert.ToInt32(Max))
                        .OrderBy(m => Guid.NewGuid()).Take(number)
                   select x;
        }


        #region 扩展 Double
        /// <summary>随机方法</summary>
        /// <param name="count">随机获取多个</param>
        /// <param name="probabilities">概率对象</param>
        public List<ObjectsDouble> RandomFun(Int32 count, List<ObjectsDouble> probabilities)
        {
            var list = new List<ObjectsDouble>();
            for (var i = 0; i < count; i++)
            {
                list.Add(RandomFun(probabilities));
            }
            return list;
        }

        /// <summary>随机方法</summary>
        /// <param name="probabilities">概率对象</param>
        public ObjectsDouble RandomFun(List<ObjectsDouble> probabilities)
        {
            if (!probabilities.Any()) return new ObjectsDouble();
            var order = probabilities.OrderByDescending(m => m.Probabilities);
            foreach (var item in order)
            {
                //var list = RandomList(item);
                if (IsTrue(item.Probabilities)) return item;
            }
            return order.Count() > 1 ? order.FirstOrDefault() : new ObjectsDouble();
        }

        #endregion

    }
}
