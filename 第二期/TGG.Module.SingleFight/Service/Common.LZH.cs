using System;
using System.Linq;
using TGG.Core.Common.Randoms;

namespace TGG.Module.SingleFight.Service
{
    /// <summary>
    /// 一将讨公共方法
    /// </summary>
    public partial class Common
    {
        /// <summary>随机获得残卷的数量</summary>
        /// <param name="value">范围</param>
        /// <returns>数量</returns>
        public int AcquireCount(string value)
        {
            if (!value.Contains("_")) return Convert.ToInt32(value);
            var item = value.Split("_").ToList();
            var count = RNG.Next(Convert.ToInt32(item[0]), Convert.ToInt32(item[1]));
            return Convert.ToInt32(count);
        }

    }
}
