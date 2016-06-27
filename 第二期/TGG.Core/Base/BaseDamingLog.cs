using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Base
{
   public  class BaseDamingLog
    {
       /// <summary>id </summary>
       public int id { get; set; }

       /// <summary>完成需求 </summary>
       public int degree { get; set; }

       /// <summary>奖励 </summary>
       public string reward { get; set; }
    }
}
