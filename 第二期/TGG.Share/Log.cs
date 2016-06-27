using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TGG.Core.Entity;

namespace TGG.Share
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Log
    {
        /// <summary>写入日志</summary>
        public void WriteLog(Int64 user_id, int type, int mn, int cn, string data)
        {
            var log = new tg_log_operate
            {
                user_id = user_id,
                type = type,
                module_number = mn,
                command_number = cn,
                time = DateTime.Now,
                data = data,
            };
            tg_log_operate.Insert(log);
        }

        /// <summary>启服/关服日志</summary>
        public void WriteServerLog(int type = 0)
        {
            var run = new tg_log_run
            {
                time = DateTime.Now,
                type = type,
            };
            tg_log_run.Insert(run);
        }
    }
}
