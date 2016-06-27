using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TGG.Core.AMF
{
    /// <summary>
    /// Class AutoParseASObject.
    /// 自动转换ASObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoParseAsObject<T>
    {
        /// <summary>解析</summary>
        public static T Parse(ASObject pObj)
        {
            try
            {
                Type tmpType = typeof(T);
                T tmpRstObj = (T)Activator.CreateInstance(tmpType);
                MemberInfo[] tmpMembers = tmpType.GetMembers();
                for (int i = 0; i < tmpMembers.Length; i++)
                {
                    var tmpVar = from value in pObj
                                 where value.Key.ToLower() == tmpMembers[i].Name.ToLower()
                                 select value.Value;
                    using (IEnumerator<object> tmpEnum = tmpVar.GetEnumerator())
                        if (tmpEnum.MoveNext())
                        {
                            switch (tmpMembers[i].MemberType)
                            {
                                case MemberTypes.Field:
                                    tmpType.GetField(tmpMembers[i].Name)
                                        .SetValue(tmpRstObj,
                                        Convert.ChangeType(tmpEnum.Current,
                                        tmpType.GetField(tmpMembers[i].Name).FieldType));
                                    break;
                                case MemberTypes.Property:
                                    if (tmpType.GetProperty(tmpMembers[i].Name).CanWrite)
                                    {
                                        tmpType.GetProperty(tmpMembers[i].Name)
                                            .SetValue(tmpRstObj,
                                            Convert.ChangeType(tmpEnum.Current,
                                            tmpType.GetProperty(tmpMembers[i].Name).PropertyType), null);
                                    }
                                    break;
                            }
                        }
                }
                return tmpRstObj;
            }
            catch { return default(T); }
        }
    }
}
