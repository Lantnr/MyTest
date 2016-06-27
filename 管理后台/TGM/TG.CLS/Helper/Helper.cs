using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace TG.CLS
{
    public class CommonHelper
    {
        ///<summary>
        /// 把json字符串转成对象
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="data">json字符串</param> 
        public static T Deserialize<T>(string data)
        {
            var json = new JavaScriptSerializer();
            return json.Deserialize<T>(data);
        }

        /// <summary>定时查询方法</summary>
        public void TimerQuery()
        {

        }
    }
}