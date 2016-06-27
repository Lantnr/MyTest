using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace TGM.Web.Helper
{
    public class ExcelHelper
    {
        /// <summary>表格内容</summary>
        public static String ToHtmlTable<T>(List<T> list)
        {
            var sb = new StringBuilder();
            sb.Append("<table>");

            //标题
            sb.Append("<tr>");
            sb.Append(ToTDTitle(list.FirstOrDefault()));
            sb.Append("</tr>");

            //内容
            foreach (var item in list)
            {
                sb.Append("<tr>");
                sb.Append(ToTDContent(item));
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        /// <summary>td</summary>
        private static String ToTDContent<T>(T entity)
        {
            var sb = new StringBuilder();

            var properts = entity.GetType().GetProperties();
            foreach (var propertyInfo in properts)
            {
                var pi = propertyInfo;
                var strValue = pi.GetValue(entity, null) == null ? string.Empty : pi.GetValue(entity, null).ToString();
                if (!pi.PropertyType.IsGenericType)
                {
                    sb.Append("<td>");
                    sb.Append(strValue);
                    sb.Append("</td>");
                }
                else
                {
                    sb.Append("<td>");
                    sb.Append("");
                    sb.Append("</td>");
                }

            }
            return sb.ToString();
        }


        /// <summary>表格标题</summary>
        private static String ToTDTitle<T>(T entity)
        {
            var sb = new StringBuilder();

            var properts = entity.GetType().GetProperties();
            foreach (var propertyInfo in properts)
            {
                var pi = propertyInfo;
                var strName = pi.Name.Replace("_", " ");
                sb.Append("<td>");
                sb.Append(strName);
                sb.Append("</td>");
            }
            return sb.ToString();
        }
    }
}