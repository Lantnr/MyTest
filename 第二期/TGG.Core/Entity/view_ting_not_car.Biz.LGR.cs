using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class view_ting_not_car
    {
        /// <summary>根据町状态获取实体集合</summary>
        public static List<view_ting_not_car> GetEntityByState(int state)
        {
            return FindAll(string.Format("state={0}", state), null, null, 0, 0);
        }
    }
}
