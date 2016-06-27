using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using XCode;

namespace TGG.Core.Entity
{
    /// <summary>
    /// tg_user_login_log 业务逻辑
    /// </summary>
    public partial class tg_user_login_log
    {
        /// <summary>根据用户Id获取实体</summary>
        public static tg_user_login_log GetEntityByUserId(Int64 user_id = 0)
        {
            var entity = Find(new String[] { _.user_id }, new Object[] { user_id });
            return entity;
        }

        /// <summary>用户登出用户日志更新</summary>
        public static void GetLoginOutUpdate(Int64 user_id = 0)
        {
            if(user_id==0) return;
            var date = DateTime.Now.Ticks;
            var sb = new StringBuilder();

           
            sb.Append("login_state=0,");
            sb.Append(string.Format("logout_time={0},", date));
            sb.Append("login_time_longer_day+=(logout_time-login_time)/600000000,");
            sb.Append("login_time_longer_total+=(logout_time-login_time)/600000000");
            var _where = string.Format("user_id={0}",user_id);
            Update(string.Format("logout_time={0}", date),_where);
            Update(sb.ToString(), _where);
        }

        /// <summary>服务器关闭用户日志更新</summary>
        public static void GetServerCloseUpdate()
        {
            var date = DateTime.Now.Ticks;
            var sb = new StringBuilder();
            sb.Append("login_state=0,");
            sb.Append(string.Format("logout_time={0},", date));
            sb.Append("login_time_longer_day+=(logout_time-login_time)/600000000,");
            sb.Append("login_time_longer_total+=(logout_time-login_time)/600000000");

            Update(sb.ToString(), "login_state=1");
        }

        /// <summary>根据ID查询</summary>
        public static tg_user_login_log GetFindId(Int64 id = 0)
        {
            return Find(__.id, id);
        }

        /// <summary>用户登录记录</summary>
        public static bool LoginLog(Int64 userid = 0, string ip = "127.0.0.1")
        {
            try
            {
                var entity = GetEntityByUserId(userid);
                if (entity != null)
                {
                    entity.login_state = 1;     
                }
                else
                {
                    entity = new tg_user_login_log
                    {
                        logout_time = DateTime.Now.Ticks,
                        login_state = 1,
                        user_id = userid,
                    };
                }
                entity.login_ip = ip;
                entity.login_time = DateTime.Now.Ticks;
                entity.login_count_day += 1;
                entity.login_count_total += 1;
                entity.Save();
                return true;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return false;
            }
        }


        /// <summary>更新全局玩家扩展数据</summary>
        public static bool GetTimerUpdate()
        {
            return Update("login_time_longer_day=0,login_count_day=0,login_open_time=0 ", null) > 0;
        }
    }
}
