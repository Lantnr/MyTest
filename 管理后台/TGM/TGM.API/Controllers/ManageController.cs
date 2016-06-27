using System;
using System.Web.Http;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    /// <summary>
    /// 管理接口
    /// </summary>
    public class ManageController : ControllerBase
    {
        /*
       /// <summary>
       /// 发送系统邮件
       /// </summary>
       /// <param name="token">令牌</param>
       /// <param name="sn">启服服务器名称</param>
       /// <param name="data">邮件实体</param>
       /// <returns></returns>  
       public ResultMessage Email(string token, string sn, [FromBody]string data)
       {
           var rm = new ResultMessage();
           //设置连接字符串
           tg_messages.SetDbConnName(DBConnect.GameConnect(token, sn));
           var entity = tg_messages.FromJson(data);
           if (entity == null)
           {
               rm.message = "参数为空";
               return rm;
           }
           if (entity.title == "")
           {
               rm.message = "邮件标题为空"; return rm;
           }
           if (entity.contents == "")
           {
               rm.message = "邮件内容为空"; return rm;
           }
           entity.Save();
           rm.result = 1;
           rm.data = entity.ToJson();
           return rm;
       }
       */
        /*
        /// <summary>
        /// 发送系统公告
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="sn">启服服务器名称</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="space">时间间隔</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
         public ResultMessage Notice(string token, string sn, Int64 start, Int64 end, int space, string content)
         {
             var rm = new ResultMessage();

             var now = DateTime.Now.Ticks;
             if (space <= 0 || end < now || start <= 0)
             {
                 rm.message = "时间设置有误";
                 return rm;
             }
             if (content == "")
             {
                 rm.message = "公告内容为空"; return rm;
             }
             //设置连接字符串
             tg_system_notice.SetDbConnName(DBConnect.GameConnect(token, sn));
             var entity = new tg_system_notice() { content = content, }; //时间格式不统一，暂不处理
             entity.Save();
             return rm;
         }
         */

        /// <summary>发送系统邮件</summary>
        /// <param name="token">令牌</param>
        /// <param name="sn">启服服务器名称</param>
        /// <param name="title">邮件标题</param>
        /// /// <param name="content">邮件内容</param>
        public Email PostEmail(string token, string sn, string title, string content)
        {
            if (!IsToken(token)) return new Email { result = -1, message = "令牌不存在" }; //验证会话
            SN = sn;

            //设置连接字符串
            tg_messages.SetDbConnName(db_connection);

            if (string.IsNullOrEmpty(title)) { return new Email() { result = -1, message = "邮件标题为空" }; }
            if (string.IsNullOrEmpty(content)) { return new Email() { result = -1, message = "邮件内容为空" }; }

            var entity = new tg_messages()
            {
                attachment = "",
                create_time = DateTime.Now.Ticks,
                type = 1,
                isread = 0,
                title = title,
                contents = content,
                isattachment = 0,
            };

            entity.Save();
            var email = ToEntity.ToEmail(entity);
            email.result = 1;
            return email;
        }

        /// <summary>
        /// 发送系统公告
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="sn">启服服务器名称</param>
        /// <param name="playid">玩id</param>
        /// <param name="sid">服务器主键id</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="space">时间间隔</param>
        /// <param name="content">内容</param>
        /// <param name="pid">平台主键id</param>
        /// <returns></returns>
        public Notice PostNotice(string token, string sn,Int64 playid,Int64 pid,Int64 sid, Int64 start, Int64 end, int space, string content)
        {
            if (!IsToken(token)) return new Notice { result = -1, message = "令牌不存在" };   //验证会话
            SN = sn;
            var notice = new Notice();
            var now = DateTime.Now.Ticks;
            if (Convert.ToInt32(space) <= 0 || end < now || start <= 0)
                return new Notice { result = -1, message = "时间设置有误" };
            //设置连接字符串
            tgm_notice.SetDbConnName(tgm_connection);
            var entity = new tgm_notice()
            {
                //start_time = start,
                //end_time = end,
                //time_interval = space,
                content = content, 
                player_id = playid,
                pid = pid,
                sid = sid,
            };
            entity.Save();
            notice = ToEntity.ToNotice(entity);
            notice.result = 1;
            return notice;
        }
    }
}
