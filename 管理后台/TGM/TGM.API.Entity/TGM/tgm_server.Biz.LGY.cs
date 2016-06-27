using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.API.Entity
{
    public partial class tgm_server
    {
        public static List<tgm_server> GetEntityName(string id)
        {
            return FindAll(string.Format("id in ({0})", id), null, null, 0, 0);
        }
    }
}
