using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FluorineFx;
using TGG.Core.Entity;
using TGM.API.Command;
using TGM.API.Entity;
using TGM.API.Entity.Enum;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public class MessageController : ControllerBase
    {
        //  POST api/Message?token={token}&roleid={roleid}&role={role}&pid={pid}&sid={sid}&start={start}&end={end}&space={space}&content={content}
        /// <summary> 发送系统公告 </summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器id</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="space">时间间隔</param>
        /// <param name="content">内容</param>
        /// <param name="roleid">玩家id</param>
        /// <param name="role">角色权限</param>
        /// <param name="pid">平台id</param>
        /// <returns></returns>
        public Notice PostNotice(string token, Int32 roleid, Int32 role, Int32 pid, Int32 sid, string start, string end, int space, string content)
        {
            if (!IsToken(token)) return new Notice { result = -1, message = "令牌不存在" };   //验证会话

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new Notice() { result = -1, message = "发送服务器信息不存在" };

            SN = server.name;
            var now = DateTime.Now.Ticks;
            var stick = string.IsNullOrEmpty(start) ? now : DateTime.Parse(start).Ticks;
            var etick = DateTime.Parse(end).Ticks;
            if (Convert.ToInt32(space) <= 0 || etick < now || stick < 0)
                return new Notice { result = -1, message = "时间设置有误" };
            //设置连接字符串

            tgm_role.SetDbConnName(tgm_connection);
            var user = tgm_role.FindByid(Convert.ToInt32(roleid));
            if (user == null) return new Notice() { result = -1, message = "没有该操作的权限" };

            tgm_platform.SetDbConnName(tgm_connection);
            var pl = tgm_platform.FindByid(Convert.ToInt32(pid));
            if (role != 10000)
            {
                if (pl.id != user.pid) return new Notice() { result = -1, message = "没有权限操作该平台信息" };
            }
            tg_system_notice.SetDbConnName(db_connection);
            var entity = new tg_system_notice()
            {
                start_time = stick,
                end_time = etick,
                time_interval = space,
                content = content,
                base_Id = 0,
                level = 2,
                state = 0,
            };
            entity.Save();

            tgm_notice.SetDbConnName(tgm_connection);
            var tgmentity = new tgm_notice()
            {
                start_time = stick,
                end_time = etick,
                content = content,
                player_id = roleid,
                pid = pid,
                sid = sid,
                gameid = entity.id,
            };
            tgmentity.Save();
            var notice = ToEntity.ToNotice(tgmentity);

            if (stick == now)
            {
                var ip = server.ip;
                var port = server.port_server;

                //解析后调用游戏接口判断是否成功
                var api = new CommandApi(ip, port, ApiCommand.公告);
                var state = api.NoticePush();
                api.Dispose();
                if (state != (int)ApiType.OK) return new Notice() { result = -1, message = "发送公告失败！" };
            }

            notice.result = 1;
            return notice;
        }

        //  POST api/Message?id={id}&token={token}
        /// <summary>删除公告</summary>
        /// <param name="id">公告主键id</param>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        public Notice PostDeleteNotice(Int32 id, String token)
        {
            if (!IsToken(token)) return new Notice { result = -1, message = "令牌不存在" };   //验证会话
            if (id == 0) { return new Notice() { result = -1, message = "数据错误" }; }

            //操作游戏数据库连接字符串
            tgm_notice.SetDbConnName(tgm_connection);
            var entity = tgm_notice.FindByid(id);
            if (entity == null) { return new Notice() { result = -1, message = "查询不到该公告信息" }; }

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(Convert.ToInt32(entity.sid));
            SN = server.name;

            tg_system_notice.SetDbConnName(db_connection);
            var notice = tg_system_notice.FindByid(Convert.ToInt32(entity.gameid));
            if (notice == null) return new Notice() { result = (int)ApiType.OK, message = "更改公告状态失败" };
            if (notice.state == 1)
            {

                var ip = server.ip;
                var port = server.port_server;
                //解析后调用游戏接口判断是否成功
                var api = new CommandApi(ip, port, ApiCommand.停止公告);
                var state = api.StopNotice(entity.gameid);
                api.Dispose();
                if (state != (int) ApiType.OK) return new Notice() {result = (int) ApiType.OK, message = "操作公告状态失败"};
                notice.state = -1;
                notice.Save();
            }
            else
            {
                if (notice.Delete() < 0)
                {
                    return new Notice() { result = -1, message = "删除公告失败" };
                }
            }            

            if (entity.Delete() < 0)
            {
                return new Notice() { result = -1, message = "删除公告失败" };
            }

            return new Notice() { result = 1 };
        }

        //  POST api/Message?token={token}&roleid={roleid}&flag={flag}
        /// <summary>查询公告</summary>
        /// <param name="token">令牌</param>
        /// <param name="roleid">玩家id</param>
        /// <param name="flag">标识</param>
        ///// <returns></returns>
        public IEnumerable<Notice> PostFindNotices(string token, Int64 roleid, Boolean flag)
        {
            if (!IsToken(token)) return new List<Notice>();   //验证会话
            if (roleid == 0) return new List<Notice>();

            //设置连接字符串
            tgm_notice.SetDbConnName(tgm_connection);
            var list = tgm_notice.GetEntityList(roleid).ToList();
            if (!list.Any()) return new List<Notice>();


            var pids = string.Join(",", list.ToList().Select(m => m.pid).ToArray());
            var sids = string.Join(",", list.ToList().Select(m => m.sid).ToArray());
            tgm_platform.SetDbConnName(tgm_connection);
            var plats = tgm_platform.GetEntityName(pids);
            if (!plats.Any()) return new List<Notice>();
            tgm_server.SetDbConnName(tgm_connection);
            var servers = tgm_server.GetEntityName(sids);
            if (!servers.Any()) return new List<Notice>();

            var lists = list.Select(item => ToEntity.ToNotice(item)).ToList();

            foreach (var item in lists)
            {
                var p = plats.FirstOrDefault(m => m.id == item.pid);
                var s = servers.FirstOrDefault(m => m.id == item.sid);
                item.platform = p == null ? "" : p.name;
                item.server = s == null ? "" : s.name;
            }
            return lists;

        }
    }
}
