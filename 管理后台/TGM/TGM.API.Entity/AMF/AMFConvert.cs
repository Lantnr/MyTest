using FluorineFx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TGG.Core.AMF
{
    /// <summary>
    /// Class AMFConvert.
    ///  AMF转换类
    /// </summary>
    public class AMFConvert
    {
        /// <summary>AMF序列化byte数组</summary>
        public static byte[] AMF_Serializer(object obj)
        {
            var byteArray = new FluorineFx.AMF3.ByteArray();
            byteArray.WriteObject(obj);
            var buffer = new byte[byteArray.Length];
            byteArray.Position = 0;
            byteArray.ReadBytes(buffer, 0, byteArray.Length);
            return buffer;

        }

        /// <summary>AMF序列化为对象</summary>
        public static T AMF_Deserializer<T>(byte[] buffer, int length)
        {
            var stream = new MemoryStream(buffer, 0, length);
            var byteArray = new FluorineFx.AMF3.ByteArray(stream);
            var asobj = (ASObject)byteArray.ReadObject();
            var obj = AmfEntityConvert.ConvertEntity(asobj, typeof(T));

            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }

        /// <summary>将对象属性转换为key-value对</summary>   
        public static Dictionary<String, Object> ToMap(Object o)
        {
            var map = new Dictionary<string, object>();
            var t = o.GetType();
            var pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in pi)
            {
                var mi = p.GetGetMethod();
                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke(o, new Object[] { }));
                }
            }
            return map;
        }

        /// <summary>对象转换成ASObject</summary>
        public static ASObject ToASObject(object obj)
        {
            return new ASObject(ToMap(obj));
        }

        /// <summary>
        /// ASObject转换对象类
        /// </summary>
        public static T AsObjectToVo<T>(dynamic _class)
        {
            var obj = _class as ASObject;
            if (obj != null)
            {
                var cn = obj.FirstOrDefault(q => q.Key == "className").Value.ToString();
                var asm = Assembly.Load(assemblyString: "TGG.Core");
                if (asm != null)
                {
                    var t = asm.GetTypes().FirstOrDefault(m => m.Name == cn);
                }
            }
            var toclass = AMF.AutoParseAsObject<T>.Parse(obj);
            return (T)toclass;
        }
    }
}
