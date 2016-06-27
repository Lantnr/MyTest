using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;

namespace TGG.Module.Consume
{
    public interface IConsume
    {
        /// <summary>消费执行接口</summary>
        /// <param name="user_id">玩家id</param>
        /// <param name="data">消费接口数据</param>
        ASObject Execute(Int64 user_id, ASObject data);
    }
}
