using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_car 业务逻辑
    /// </summary>
    public partial class tg_car
    {
        /// <summary>根据马车主键id获取马车实体</summary>
        public static tg_car GetEntityById(Int64 id)
        {
            return Find(new String[] { _.id }, new Object[] { id });
        }
    }
}
