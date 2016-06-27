using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using TGM.API.Entity.Enum;

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
            var ret = new byte[first.Length + second.Length];
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
            var ret = new byte[first.Length + second.Length + third.Length];
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
            var ret = new byte[arrays.Sum(x => x.Length)];
            var offset = 0;
            foreach (var data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }

        /// <summary>byte数组转换int</summary>
        public static int ByteToInt(byte[] b)
        {
            var mask = 0xff;
            var n = 0;
            for (var i = 0; i < 4; i++)
            {
                n <<= 8;
                var temp = b[i] & mask;
                n |= temp;
            }
            return n;
        }

        /// <summary>int转换byte数组</summary>
        public static byte[] IntToBytes(int n)
        {
            var b = new byte[4];
            for (var i = 0; i < 4; i++) { b[i] = (byte)(n >> (24 - i * 8)); }
            return b;
        }

        /// <summary>     
        /// 将对象属性转换为key-value对   
        /// </summary>   
        /// <param name="o"></param>   
        /// <returns></returns>   
        public static Dictionary<String, Object> ToMap(Object o)
        {
            var map = new Dictionary<string, object>();
            var t = o.GetType();
            var pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in pi)
            {
                var mi = p.GetGetMethod();
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
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
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
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
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
            var data = Encoding.Unicode.GetBytes(s);
            var result = new StringBuilder(data.Length * 8);

            foreach (var b in data)
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
            var cs =
                System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            var data = new byte[cs.Count];
            for (var i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }

        /// <summary>MD5加密</summary>
        /// <param name="s">加密字符</param>
        public static String MD5(String s)
        {
            var md5csp = new MD5CryptoServiceProvider();
            var md5s = Encoding.UTF8.GetBytes(s);
            var md5out = md5csp.ComputeHash(md5s);
            var sb = new StringBuilder();
            foreach (var item in md5out)
            {
                sb.Append(item.ToString("x").PadLeft(2, '0'));  
            }
            return sb.ToString();
        }

        /// <summary>加密字符</summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String CryptoString(String strEncode)
        {
            string strReturn = "";//  存储转换后的编码
            foreach (short shortx in strEncode.ToCharArray())
            {
                strReturn += shortx.ToString("X4");
            }
            return strReturn;
        }

        public static Int32 ToGold(Int32 amount, Int32 type)
        {
            var gold = 0;
            switch (type)
            {
                //RMB
                case (int)PayType.RMB: { gold = amount * 10; break; }
                //平台1:1点
                case (int)PayType.RATE_1: { gold = amount; break; }
                //平台1:10点
                case (int)PayType.RATE_10: { gold = amount * 10; break; }
                //平台1:100点
                case (int)PayType.RATE_100: { gold = amount * 100; break; }
                //平台1:1000点
                case (int)PayType.RATE_1000: { gold = amount * 1000; break; }
            }
            return gold;
        }
    }
}
