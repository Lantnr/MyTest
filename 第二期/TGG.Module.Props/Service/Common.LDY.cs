using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Base;
using TGG.Core.Entity;
using TGG.Core.Global;

namespace TGG.Module.Props.Service
{
    public partial class Common
    {
        public tg_bag BaseToEntity(BaseProp baseinfo, Int64 userid, int count)
        {
            return new tg_bag()
            {
                base_id = baseinfo.id,
                type = baseinfo.type,
                bind = baseinfo.bind,
                user_id = userid,
                count = count,
            };

        }
    }
}
