using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_system_notice
    {
       /// <summary> 根据状态获取除该状态的实体集合 </summary>
       /// <param name="state">除此状态</param>
       /// <returns></returns>
        public static List<tg_system_notice> GetEntitys(int state)
        {
            return FindAll(string.Format("state!={0} ", state), null, null, 0, 0);
        }

        /// <summary>根据状态获取实体集合</summary>
        public static List<tg_system_notice> GetEntityByState(int state)
        {
            return FindAll(_.state, state);
        }
    }
}
