using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGM.Tools
{
    /// <summary>日志委托</summary>
    public delegate void LogEventHandle(object sender, LogEventArgs e);

    /// <summary>
    /// 日志事件
    /// </summary>
    public class LogEventArgs : EventArgs
    {

        public LogEventArgs(String msg)
        {
            Message = msg;
        }

        /// <summary>日志信息</summary>
        public String Message { get; set; }
    }

    /// <summary>
    /// 显示日志
    /// </summary>
    public class DisplayLog
    {
        /// <summary>显示日志事件</summary>
        public event LogEventHandle LogEvent;

        /// <summary>显示日志事件</summary>
        /// <param name="e">日志事件</param>
        private void OnLogEvent(LogEventArgs e)
        {
            if (LogEvent != null) LogEvent(this, e);
        }

        /// <summary>写入日志方法</summary>
        /// <param name="msg">写入信息</param>
        public void Write(String msg)
        {
            OnLogEvent(new LogEventArgs(msg));
        }
    }
    /// <summary>
    /// 全局显示类
    /// </summary>
    public class DisplayGlobal
    {
        public static DisplayLog log = new DisplayLog();
    }
}
