using FluorineFx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TGG.Core.AMF
{
    /// <summary>
    /// Class AmfEntityConvert.
    /// AMF实体转换类
    /// </summary>
    public class AmfEntityConvert
    {
        delegate object ConvertAction(object data);

        static Dictionary<Type, ConvertAction> dicTypeRelation = new Dictionary<Type, ConvertAction>();

        static AmfEntityConvert()
        {
            dicTypeRelation[typeof(FluorineFx.AMF3.ByteArray)] = new ConvertAction((object data) =>
            {
                byte[] bytes = null;
                if (data != null)
                {
                    bytes = (data as FluorineFx.AMF3.ByteArray).GetBuffer();
                }
                return bytes;
            });
        }

        public static T ConvertObject<T>(object asObject)
        {
            if (asObject != null)
            {
                Type type = asObject.GetType();
                if (dicTypeRelation.Keys.Contains(type))
                {
                    return (T)dicTypeRelation[type](asObject);
                }
                else
                {
                    return (T)Convert.ChangeType(asObject, typeof(T));
                }
            }
            return default(T);
        }

        public static object ConvertData(Type type, object obj, params object[] pams)
        {
            Type classType = typeof(AmfEntityConvert);
            MethodInfo staticMethod = classType.GetMethod("ConvertObject");
            MethodInfo genericMethod = staticMethod.MakeGenericMethod(type);
            return genericMethod.Invoke(obj, pams);
        }
        /// <summary>
        /// 转换ASObject对象为.Net类型对象
        /// </summary>
        public static object ConvertEntity(ASObject asObjeect, Type type)
        {
            try
            {
                var tempObj = Activator.CreateInstance(type);
                foreach (var asObj in asObjeect)
                {
                    string key = asObj.Key;
                    object value = asObj.Value; MemberInfo[] members = type.GetMember(key);
                    if (null != value && members != null && members.Length > 0)
                    {
                        object newValue = null;
                        Type asValueType = asObj.Value.GetType();                        //.Net类型实体成员
                        MemberInfo member = members[0];
                        Type memberType = null;
                        FieldInfo field = null;
                        PropertyInfo property = null;
                        if (member.MemberType == MemberTypes.Field)
                        {
                            field = type.GetField(member.Name);
                            memberType = field.FieldType;
                        }
                        if (member.MemberType == MemberTypes.Property)
                        {
                            property = type.GetProperty(member.Name);
                            memberType = property.PropertyType;
                        } if (property != null || field != null)
                        {
                            //如果是ASObject对象
                            if (asValueType == typeof(ASObject))
                            {
                                newValue = ConvertEntity((ASObject)value, memberType);
                            }
                            //如果是数组集合
                            else if (asValueType == typeof(Object[]))
                            {
                                Type subtype = Assembly.GetAssembly(memberType).GetType(memberType.FullName.Replace("[]", ""), true); object[] arrobj = (Object[])value;
                                var objData = Array.CreateInstance(subtype, arrobj.Length);
                                for (int nn = 0; nn < arrobj.Length; nn++)
                                {
                                    FluorineFx.ASObject asData = (FluorineFx.ASObject)arrobj[nn]; var val = ConvertEntity(asData, subtype);
                                    objData.SetValue(val, nn);
                                }
                                newValue = objData;
                            }
                            //基本类型
                            else
                            {
                                newValue = ConvertData(memberType, null, value);
                            }
                            if (field != null)
                            {
                                field.SetValue(tempObj, Convert.ChangeType(newValue, memberType));
                            }
                            else if (property != null && property.CanWrite)
                            {
                                property.SetValue(tempObj, Convert.ChangeType(newValue, memberType), null);
                            }
                        }
                    }

                }
                return tempObj;
            }
            catch { return null; }
        }


    }
}
