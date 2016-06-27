using System;
using System.Collections.Generic;
using System.Linq;
using TGG.Core.Entity;
using TGM.API.Command;
using TGM.API.Entity;
using TGM.API.Entity.Enum;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public class GMController : ControllerBase
    {
        //api/GM?token={token}&name={name}&pid={pid}&sid={sid}&search={search}&value={value}&type={type}&time={time}&reason={reason}
        /// <summary>Gm管理</summary>
        /// <param name="token">令牌</param>
        /// <param name="name">用户名</param>
        /// <param name="pid">平台id</param>
        /// <param name="sid">服务器</param>
        /// <param name="search">查询方式</param>
        /// <param name="value">查询值</param>
        /// <param name="type">操作类型</param>
        /// <param name="time">时间限制</param>
        /// <param name="reason">操作原因</param>
        /// <returns></returns>
        public GmManage PostGmManage(String token, String name, Int32 pid, Int32 sid, Int32 search, String value, Int32 type, Int64 time, String reason)
        {
            if (!IsToken(token)) return new GmManage() { result = -1, message = "令牌不存在" }; //验证会话

            tgm_role.SetDbConnName(tgm_connection);
            var user = tgm_role.GetFindEntity(name);
            if (user == null) return new GmManage() { result = -1, message = "没有该操作的权限" };

            tgm_platform.SetDbConnName(tgm_connection);
            var pl = tgm_platform.FindByid(pid);
            if (user.role != 10000)
            {
                if (pl.id != user.pid) return new GmManage() { result = -1, message = "没有权限操作该平台信息" };
            }

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new GmManage() { result = -1, message = "服务器不存在" };

            SN = server.name;
            tg_user.SetDbConnName(db_connection);

            var player = search == 1
                ? tg_user.GetEntityByCode(value) //根据账号查询
                : tg_user.GetEntityByName(value); //根据玩家名查询

            if (player == null) return new GmManage() { result = -1, message = "没有该玩家信息" };
            if (player.state != 0)
            {
                switch (player.state)
                {
                    case 1: return new GmManage() { result = -1, message = "该玩家已冻结，不能继续操作" };
                    case 2: return new GmManage() { result = -1, message = "该玩家已封号，不能继续操作" };
                }
            }
            player.state = type;
            player.state_end_time = DateTime.Now.Ticks + time * 60 * 1000;
            if (player.Save() < 0) return new GmManage() { result = -1, message = "操作玩家数据失败" };

            tgm_gm.SetDbConnName(tgm_connection);
            var entity = new tgm_gm()
            {
                pid = pl.id,
                sid = server.id,
                player_id = player.id,
                limit_time = time * 60 * 1000,
                state = type,
                player_name = player.player_name,
                player_code = player.user_code,
                platform_name = pl.name,
                server_name = server.name,
                describe = reason,
                createtime = DateTime.Now.Ticks,
                operate = name,
            };
            entity.Save();

            var ip = server.ip;
            var port = server.port_server;
            //解析后调用游戏接口判断是否成功
            var api = new CommandApi(ip, port, ApiCommand.冻结封号);
            var state = api.Gmoperate(player.id);
            api.Dispose();
            if (state != (int)ApiType.OK) return new GmManage() { result = (int)ApiType.OK, message = "操作玩家信息失败" };

            var gm = ToEntity.ToGmManage(entity);
            gm.result = 1;
            return gm;
        }

        //api/GM?token={token}&role={role}&pid={pid}&index={index}&size={size}
        /// <summary>Gm记录操作</summary>
        /// <param name="token">令牌</param>
        /// <param name="role">用户权限</param>
        /// <param name="pid">平台pid</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        public PageGmManage PostGmRecords(String token, Int32 role, Int32 pid, Int32 index = 1, Int32 size = 10)
        {
            if (!IsToken(token)) return new PageGmManage() { result = -1, message = "令牌不存在" };  //验证会话

            tgm_gm.SetDbConnName(tgm_connection);
            int count;
            var entitys = tgm_gm.GetPageEntity(role, pid, index - 1, size, out count).ToList();

            var list = new List<GmManage>();
            list.AddRange(entitys.Select(ToEntity.ToGmManage));
            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };

            return new PageGmManage() { result = 1, Pager = pager, Gms = list };
        }

        //api?GM/token={token}&name={name}&pid={pid}&sid={sid}&state={state}&type={type}&value={value}
        /// <summary>查询玩家GM记录信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="name">用户名</param>
        /// <param name="pid">平台pid</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="state">查询状态  1：冻结  2：封号</param>
        /// <param name="type">查询类型  1：账号  2：名称</param>
        /// <param name="value">查询值</param>
        public PageGmManage PostPlayerGm(String token, String name, Int32 pid, Int32 sid, Int32 state, Int32 type, String value)
        {
            var pg = new PageGmManage();
            var list = new List<GmManage>();
            if (!IsToken(token)) return new PageGmManage() { result = -1, message = "令牌不存在" };  //验证会话

            tgm_role.SetDbConnName(tgm_connection);
            var user = tgm_role.GetFindEntity(name);
            if (user == null) return new PageGmManage() { result = -1, message = "没有该操作的权限" };

            tgm_platform.SetDbConnName(tgm_connection);
            var pl = tgm_platform.FindByid(pid);
            if (user.role != 10000)
            {
                if (pl.id != user.pid) return new PageGmManage() { result = -1, message = "没有权限操作该平台信息" };
            }

            tgm_gm.SetDbConnName(tgm_connection);
            if (!string.IsNullOrEmpty(value))
            {
                int count;
                var entitys = tgm_gm.GetPlayerEntity(pid, sid, state, type, value, out count).ToList();

                list.AddRange(entitys.Select(ToEntity.ToGmManage));
                var pager = new PagerInfo() { CurrentPageIndex = 1, PageSize = count, RecordCount = count, };
                pg.result = 1;
                pg.Pager = pager;
                pg.Gms = list;
            }
            else
            {
                if (pid == 0)  //查询全部平台
                {
                    int count;
                    var entitys = tgm_gm.GetPlayerEntity(state, out count).ToList();
                    list.AddRange(entitys.Select(ToEntity.ToGmManage));
                    var pager = new PagerInfo() { CurrentPageIndex = 1, PageSize = count, RecordCount = count, };
                    pg.result = 1;
                    pg.Pager = pager;
                    pg.Gms = list;
                }
                else
                {
                    if (sid == 0)  //选定平台所有服务器
                    {
                        int count;
                        var entitys = tgm_gm.GetPlayerEntity(pid, state, out count).ToList();
                        list.AddRange(entitys.Select(ToEntity.ToGmManage));
                        var pager = new PagerInfo() { CurrentPageIndex = 1, PageSize = count, RecordCount = count, };
                        pg.result = 1;
                        pg.Pager = pager;
                        pg.Gms = list;
                    }
                    else
                    {
                        int count;
                        var entitys = tgm_gm.GetPlayerEntity(pid, sid, state, out count).ToList();
                        list.AddRange(entitys.Select(ToEntity.ToGmManage));
                        var pager = new PagerInfo() { CurrentPageIndex = 1, PageSize = count, RecordCount = count, };
                        pg.result = 1;
                        pg.Pager = pager;
                        pg.Gms = list;
                    }
                }
            }
            return pg;
        }

        /// <summary>解冻  解封</summary>
        /// <param name="token">令牌</param>
        /// <param name="gid">Gm记录主键id</param>
        public GmManage PostRescueGm(String token, Int32 gid)
        {
            if (!IsToken(token)) return new GmManage() { result = -1, message = "令牌不存在" };  //验证会话
            if (gid == 0) return new GmManage() { result = -1, message = "请求数据错误" };

            tgm_gm.SetDbConnName(tgm_connection);
            var entity = tgm_gm.FindByid(gid);

            if (entity == null) return new GmManage() { result = -1, message = "查询不到该条数据信息" };
            if (entity.state == 0) return new GmManage() { result = -1, message = "操作失效，数据错误" };
            entity.state = 0;
            entity.limit_time = 0;
            entity.Save();

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(entity.sid);
            if (server == null) return new GmManage() { result = -1, message = "服务器信息不存在" };
            SN = server.name;

            tg_user.SetDbConnName(db_connection);
            var player = tg_user.FindByid(entity.player_id);
            if (player == null) return new GmManage() { result = -1, message = "玩家信息不存在，操作失败" };

            if (player.state == 0) return new GmManage() { result = -1, message = "玩家数据信息已恢复正常" };
            player.state = 0;
            player.state_end_time = 0;
            if (player.Save() < 0) return new GmManage() { result = -1, message = "更新玩家数据失败" };
            return new GmManage() { result = 1 };
        }
    }
}
