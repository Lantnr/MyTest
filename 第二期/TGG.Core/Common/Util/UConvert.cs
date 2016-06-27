using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TGG.Core.Common.Util
{
    /// <summary>
    /// Class UConvert.
    /// 公共转换类
    /// </summary>
    public class UConvert
    {
        /// <summary>
        /// byte数组合并
        /// </summary>
        /// <param name="first">第一个byte数组</param>
        /// <param name="second">第二个byte数组</param>
        /// <returns></returns>
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        /// <summary>
        /// byte数组合并
        /// </summary>
        /// <param name="first">第一个byte数组</param>
        /// <param name="second">第二个byte数组</param>
        /// <param name="third">第三个byte数组</param>
        /// <returns></returns>
        public static byte[] Combine(byte[] first, byte[] second, byte[] third)
        {
            byte[] ret = new byte[first.Length + second.Length + third.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            Buffer.BlockCopy(third, 0, ret, first.Length + second.Length,
                             third.Length);
            return ret;
        }

        /// <summary>
        /// byte数组合并
        /// </summary>
        /// <param name="arrays">byte数组集合</param>
        /// <returns></returns>
        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }

        /// <summary>byte数组转换int</summary>
        public static int ByteToInt(byte[] b)
        {
            int mask = 0xff;
            int n = 0;
            for (int i = 0; i < 4; i++)
            {
                n <<= 8;
                int temp = b[i] & mask;
                n |= temp;
            }
            return n;
        }

        /// <summary>int转换byte数组</summary>
        public static byte[] IntToBytes(int n)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++) { b[i] = (byte)(n >> (24 - i * 8)); }
            return b;
        }

        /// <summary>     
        /// 将对象属性转换为key-value对   
        /// </summary>   
        /// <param name="o"></param>   
        /// <returns></returns>   
        public static Dictionary<String, Object> ToMap(Object o)
        {
            Dictionary<String, Object> map = new Dictionary<string, object>();
            Type t = o.GetType();
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();
                if (mi != null && mi.IsPublic) { map.Add(p.Name, mi.Invoke(o, new Object[] { })); }
            }
            return map;
        }

        /// <summary>     
        /// 转全角的函数(SBC case) 
        ///任意字符串
        /// 全角字符串 
        ///全角空格为12288,半角空格为32 
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248 
        ///</summary>     
        public static string ToSbc(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32) { c[i] = (char)12288; continue; }
                if (c[i] < 127) c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>     
        /// 转半角的函数(DBC case) 
        /// 任意字符串
        /// 半角字符串
        /// 全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248 
        ///</summary>   
        public static string ToDbc(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288) { c[i] = (char)32; continue; }
                if (c[i] > 65280 && c[i] < 65375) c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 将 字符串 转成 二进制 “10011100000000011100011111111101”
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Encode(string s)
        {
            byte[] data = Encoding.Unicode.GetBytes(s);
            StringBuilder result = new StringBuilder(data.Length * 8);

            foreach (byte b in data)
            {
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            return result.ToString();
        }
        /// <summary>
        /// 将二进制 “10011100000000011100011111111101” 转成 字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Decode(string s)
        {
            System.Text.RegularExpressions.CaptureCollection cs =
                System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }


    }
}
