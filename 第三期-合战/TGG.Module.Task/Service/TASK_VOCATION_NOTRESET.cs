using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluorineFx;
using TGG.Core.Enum.Type;
using TGG.SocketServer;

namespace TGG.Module.Task.Service
{
    /// <summary>
    /// 职业任务提升后不重置任务
    /// </summary>
    //public class TASK_VOCATION_NOTRESET
    //{
    //    #region IDisposable 成员
    //    public void Dispose()
    //    {
    //        GC.SuppressFinalize(this);
    //    }

    //     /// <summary>析构函数</summary>
    //    ~TASK_VOCATION_BUY()
    //    {
    //        Dispose();
    //    }
    
    //    #endregion
    //    private static TASK_VOCATION_NOTRESET objInstance = null;

    //    /// <summary> TASK_VOCATION_NOTRESET单体模式 </summary>
    //    public static TASK_VOCATION_NOTRESET getInstance()
    //    {
    //        return objInstance ?? (objInstance = new TASK_VOCATION_NOTRESET());
    //    }

    //    public ASObject CommandStart(TGGSession session, ASObject data)
    //    {
    //        var userextend = session.Player.UserExtend.CloneEntity();
    //        userextend.task_vocation_isgo = 1;
    //        userextend.Save();
    //        session.Player.UserExtend = userextend;
    //        var dic = new Dictionary<string, object>() { { "result", (int)ResultType.SUCCESS } };
    //        return new ASObject(dic);
    //    }
    //}
}
