
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Expressions
{
    /// <summary>
    /// 表达式
    /// </summary>
    public class Expressions
    {
        /// <summary>
        /// 获取属性名称
        /// 用法 GetPropertyName&lt;T&gt;(p=>p.name);
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="expr">lambda表达式</param>
        /// <returns>返回属性名称字符串</returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            var result = string.Empty;
            var body = expr.Body as UnaryExpression;
            if (body != null) result = ((MemberExpression)body.Operand).Member.Name;
            else
            {
                var expression = expr.Body as MemberExpression;
                if (expression != null) result = expression.Member.Name;
                else
                {
                    var parameterExpression = expr.Body as ParameterExpression;
                    if (parameterExpression != null) result = parameterExpression.Type.Name;
                }
            }
            return result;
        }

    }
}
