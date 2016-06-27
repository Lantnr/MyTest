using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity
{
    public partial class tgm_server
    {
        /// <summary>根据平台主键id查询服务器</summary>
        public static List<tgm_server> GetServers(Int32 pid)
        {
            return FindAll(new String[] { _.pid }, new Object[] { pid });
        }
    }
}
