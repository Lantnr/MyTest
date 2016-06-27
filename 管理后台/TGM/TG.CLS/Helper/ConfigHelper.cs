using System;

namespace TG.CLS
{
    /// <summary>
    /// 配置文件类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>Api路径</summary>
        public string ApiUrl { get; set; }

        /// <summary>获取Api路径</summary>
        public static String GetApiUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["api"].ToString();
        }
        public static String GetAppSettings(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }
       
    }
}