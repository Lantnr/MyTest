using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGG.Core.Entity
{
    public partial class tg_task
    {
        public static void GetInsert(tg_task task)
        {
            tg_task.Insert(task);
        }
    }
}
