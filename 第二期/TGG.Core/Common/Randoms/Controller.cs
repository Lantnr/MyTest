using System;
using System.Collections.Generic;
using System.Linq;

namespace TGG.Core.Common.Randoms
{
    /// <summary>
    /// 随机控制器
    /// </summary>
    public class Controller
    {

        /// <summary>随机对象集合 多个概率</summary>
        public Controller(List<Objects> list)
        {
            LO = list;
        }

        /// <summary>默认随机控制器 单个概率</summary>
        /// <param name="probabilities">指定概率 Name:1当前概率 Name:0相反概率</param>
        /// <param name="digit">比例(以10为底数)</param>
        public Controller(int probabilities, params Int32[] digit)
        {
            LO = Init(probabilities, digit);
        }

         /// <summary>获取小数位</summary>
        private static Int32 GetDigit(Int32 digit)
        {
            if (digit < 0) digit = 0;
            return Convert.ToInt32(Math.Pow(10, digit));
        }

        /// <summary>默认初始化</summary>
        /// <param name="probabilities">概率</param>
        /// <param name="digit">比例</param>
        private List<Objects> Init(int probabilities, params Int32[] digit)
        {
            var _digit = 2;
            if (digit.Any()) _digit = digit[0];

            var list = new List<Objects>()
            {
                new Objects() { Name = "1", Probabilities =probabilities },
                new Objects() { Name = "0", Probabilities = (GetDigit(_digit) - probabilities) },
            };
            return list; 
        }

        private List<Objects> LO = new List<Objects>();

        /// <summary>随机函数</summary>
        public Objects RandomFun()
        {
            return (from x in Enumerable.Range(0, 1000000)  //最多支100万次骰子
                    let obj = LO[RNG.Next(LO.Count()-1)]
                    let dice = RNG.Next(0, 99)
                    where dice < obj.Probabilities
                    select obj).First();
        }

        /// <summary>获取多个随机</summary>
        public List<Objects> Range(int count)
        {

            var list = new List<Objects>();
            Enumerable.Range(1, count).ToList().ForEach(x => list.Add(RandomFun()));
            return list;
        }

        /// <summary>获取不重复随机</summary>
        public List<Objects> UniqueRange(int count)
        {
            var list = new List<Objects>();
            Enumerable.Range(1, count).ToList().ForEach(x =>
            {
                var temp = RandomFun();
                if (!Enumerable.Contains(list, temp)) list.Add(temp);
            });
            return list;
        }
    }
}
