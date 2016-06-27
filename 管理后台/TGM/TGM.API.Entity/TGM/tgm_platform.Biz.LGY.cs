using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCode;

namespace TGM.API.Entity
{
    public partial class tgm_platform
    {

        public static List<tgm_platform> GetEntityName(string ids)
        {
            return FindAll(string.Format("id in ({0})", ids), null, null, 0, 0);            
        }

        public static List<tgm_platform> GetPlatFormList(string token)
        {
            return FindAll(_.token, token);
        }
    }
}
