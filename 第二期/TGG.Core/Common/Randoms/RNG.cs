using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace TGG.Core.Common.Randoms
{
    /// <summary>
    /// 使用RNGCryptoServiceProvider 产生由密码编译服务供应者(CSP) 提供的乱数产生器
    /// </summary>
    public static class RNG
    {
        private static RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[8];

        #region Int32
        /// <summary>产生一个非负数的乱数</summary>
        public static Int32 Next()
        {
            rngp.GetBytes(rb);
            var value = BitConverter.ToInt32(rb, 0);
            return (value < 0) ? -value : value;

        }
        /// <summary>产生一个非负数且最大值max 以下的乱数</summary>
        /// <param name="max">最大值</param>
        public static Int32 Next(Int32 max)
        {
            return Next() % (max + 1);
        }
        /// <summary>产生一个非负数且最小值在min 以上最大值在max 以下的乱数</summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static Int32 Next(Int32 min, Int32 max)
        {
            return Next(max - min) + min;

        }

        /// <summary>产生指定个数非负数且最小值在min 以上最大值在max 以下的乱数</summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="number">输出个数</param>
        public static IEnumerable<Int32> Next(Int32 min, Int32 max, Int32 number)
        {
            var list = new List<Int32>();
            var less = max - min + 1;
            number = number > less ? less : number;
            var i = 0;
            while (i < number)
            {
                var value = Next(max - min) + min;
                if (Enumerable.Contains(list, value)) continue;
                list.Add(value);
                i++;
            }
            return list;
        }

        #endregion

        #region Int32 Less 扩展
        /// <summary>产生一个负数且最大值max 以下的乱数</summary>
        /// <param name="max">最大值</param>
        public static Int32 NextLess(Int32 max)
        {
            rngp.GetBytes(rb);
            var value = BitConverter.ToInt32(rb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return -value;
        }

        /// <summary>产生指定个数负数且最小值在min 以上最大值在max 以下的乱数</summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="number">输出个数</param>
        public static IEnumerable<Int32> NextLess(Int32 min, Int32 max, Int32 number)
        {
            var list = new List<Int32>();
            var less = max - min + 1;
            number = number > less ? less : number;
            var i = 0;
            while (i < number)
            {
                var value = NextLess(max - min) + min;
                if (Enumerable.Contains(list, value)) continue;
                list.Add(value);
                i++;
            }
            return list;
        }
        #endregion

        #region

        /// <summary>获取小数位</summary>
        private static Double GetDigit(Int32 digit)
        {
            if (digit < 0) digit = 0;
            return Math.Pow(10, digit);
        }

        /// <summary>产生一个非负浮点数的乱数</summary>
        public static Double NextDouble(params Int32[] digit)
        {
            var _digit = 2;
            if (digit.Any()) _digit = digit[0];
            rngp.GetNonZeroBytes(rb);
            var value = Convert.ToDouble(BitConverter.ToInt32(rb, 0)) / GetDigit(_digit);
            return (value < 0) ? -value : value;
        }

        /// <summary>产生一个非负浮点数且最大值max 以下的乱数</summary>
        /// <param name="max">最大值</param>
        public static Double NextDouble(Double max, params Int32[] digit)
        {
            Int32 _digit = 2;
            if (digit.Any()) _digit = digit[0];
            var value = NextDouble(_digit);
            value = value % (max + 1);
            value = Math.Round(value, _digit);
            return (value < 0) ? -value : value;
        }

        /// <summary>产生一个非负浮点数且最小值在min 以上最大值在max 以下的乱数</summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static Double NextDouble(Double min, Double max, params Int32[] digit)
        {
            Int32 _digit = 2;
            if (digit.Any()) _digit = digit[0];
            var value = NextDouble(max - min, _digit) + min;
            return value;
        }

        /// <summary>产生指定个数非负浮点数且最小值在min 以上最大值在max 以下的乱数</summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="number">输出个数</param>
        public static IEnumerable<Double> NextDouble(Int32 number, Double min, Double max, params Int32[] digit)
        {
            Int32 _digit = 2;
            if (digit.Any()) _digit = digit[0];
            var list = new List<Double>();
            var less = Convert.ToInt32(max - min) + 1;
            number = number > less ? less : number;
            var i = 0;
            while (i < number)
            {
                var value = NextDouble(max - min, _digit) + min;
                if (Enumerable.Contains(list, value)) continue;
                list.Add(value);
                i++;
            }
            return list;
        }



        #endregion

        #region 扩展

        /// <summary>从对应数组中随机对应数集合</summary>
        /// <param name="number">输出个数</param>
        /// <param name="list">原随机数集合</param>
        /// <returns>随机数集合</returns>
        public static IEnumerable<Int32> Next(Int32 number, List<Int32> list)
        {
            var result = new List<Int32>();
            var less = list.Count;
            number = number > less ? less : number;
            var i = 0;
            while (i < number)
            {
                var index = Next(list.Count - 1);
                var value = list[index];
                if (Enumerable.Contains(result, value)) continue;
                result.Add(value);
                i++;
            }
            return result;
        }

        /// <summary>从对应数组中随机对应数集合</summary>
        /// <param name="number">输出个数</param>
        /// <param name="list">原随机数集合</param>
        /// <returns>随机数集合</returns>
        public static IEnumerable<Double> NextDouble(Int32 number, List<Double> list)
        {
            var result = new List<Double>();
            var less = list.Count;
            number = number > less ? less : number;
            var i = 0;
            while (i < number)
            {
                var index = Next(list.Count - 1);
                var value = list[index];
                if (Enumerable.Contains(result, value)) continue;
                result.Add(value);
                i++;
            }
            return result;
        }

        #endregion

    }
}
