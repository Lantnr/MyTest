using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGG.Core.Enum.Type;
using TGG.Core.Global;

namespace TGG.Module.API.Service
{
    /// <summary>
    /// 获取公告
    /// </summary>
    public class GETNOTICE
    {
        private static GETNOTICE _objInstance;

        /// <summary>GETNOTICE 单体模式</summary>
        public static GETNOTICE GetInstance()
        {
            return _objInstance ?? (_objInstance = new GETNOTICE());
        }

        /// <summary> 获取公告</summary>
        public void CommandStart()
        {
            (new Share.Notice()).NewNoticeAddTask();
        }
    }
}
