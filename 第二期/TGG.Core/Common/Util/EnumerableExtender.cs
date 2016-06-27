using System;
using System.Collections.Generic;

namespace TGG.Core.Common
{

    /// <summary>
    /// Enumerable 扩展方法
    /// </summary>
    public static class EnumerableExtender
    {
        /// <summary>
        /// Linq使用Distinct删除重复数据
        /// var data = datas.Distinct(m => m.X);
        /// Group方式
        /// var data  = from data in datas group data by data.X into g select g.First();
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                var elementValue = keySelector(element);
                if (seenKeys.Add(elementValue))
                {
                    yield return element;
                }
            }
        }
    }
}
