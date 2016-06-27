using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity
{
    /// <summary>资源管理表</summary>
    public partial class tgm_resource
    {
        /// <summary>根据平台id与服务器id获取资源列表</summary>
        /// <param name="pid">平台id</param>
        /// <param name="sid">服务器id</param>
        /// <param name="state">申请状态</param>
        /// <returns></returns>
        public static List<tgm_resource> GetEntityList(Int64 pid, Int64 sid,int state)
        {
            return FindAll(string.Format("pid={0} and sid={1} and state={2}", pid, sid,state), null, null, 0, 0);
        }
    }
}
