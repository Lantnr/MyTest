using System.Configuration;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    public class DBConnect
    {
        /// <summary>获取后台数据库连接名称 </summary>
        /// <param name="model">启服表实体</param>
        /// <returns>连接名称</returns>
        public static string GetName(tgm_server model)
        {
            var connName = string.Empty;
            if (model == null)
            {
                connName = "tgm";
                var db = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
                DAL.AddConnStr(connName, db, null, "MSSQL");
                return connName;
            }
            connName = string.Format("{0}_{1}_{2}", model.id, model.pid, model.name);
            DAL.AddConnStr(connName, model.connect_string, null, "MSSQL");
            return connName;
        }

        /// <summary>游戏服务器连接名称</summary>
        /// <param name="token">令牌</param>
        /// <param name="sn">服务器名称</param>
        public static string GameConnect(string token, string sn)
        {
            tgm_server.SetDbConnName(GetName(null));
            var server = tgm_server.GetFindEntity(token, sn);
            return GetName(server);
        }
    }
}