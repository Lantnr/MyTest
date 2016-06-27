using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity.Helper
{
    /// <summary>
    /// 序列号类
    /// </summary>
    public class SerialNumber
    {
        /// <summary>
        /// 生成单个序列号字符串
        /// </summary>
        public static String GenerateString()
        {
            return GenerateString("");
        }

        /// <summary>
        /// 生成单个序列号字符串
        /// </summary>
        /// <param name="prefix">前缀</param>
        public static String GenerateString(String prefix)
        {
            var v = Guid.NewGuid().ToByteArray().Aggregate<byte, long>(1, (current, b) => current * ((int)b + 1));

            return String.Format("{0}{1:X}", prefix, v - DateTime.Now.Ticks);

        }

        /// <summary>
        /// 生成指定个数序列号
        /// </summary>
        /// <param name="count">生成个数</param>
        public static List<String> GenerateStringList(Int32 count)
        {
            return GenerateStringList("", count);
        }

        /// <summary>
        /// 生成指定个数序列号
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="count">生成个数</param>
        public static List<String> GenerateStringList(String prefix, Int32 count)
        {
            var list = new List<String>();
            do
            {
                list.Add(GenerateString(prefix));
                count--;
            } while (count > 0);
            return list;
        }


    }
}
