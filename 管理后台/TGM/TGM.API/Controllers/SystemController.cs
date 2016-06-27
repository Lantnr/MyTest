using System;
using System.Collections.Generic;
using System.Linq;
using TGM.API.Entity;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    /// <summary>
    /// 平台系统接口
    /// </summary>
    public partial class SystemController : ControllerBase
    {
        #region 后台登陆
        //  POST api/System?name={name}&password={password}
        /// <summary>后台登陆</summary>
        /// <param name="name">用户名称</param>
        /// <param name="password">密码(非加密密码)</param>
        public User PostLogin(string name, string password)
        {
            tgm_role.SetDbConnName(DBConnect.GetName(null));
            var entity = tgm_role.GetFindEntity(name);

            if (entity == null)
            {
                return new User { result = -1, message = "账号不存在" };
            }

            var pwd = CryptoHelper.Decrypt(entity.password, null);
            if (pwd != password)
            {
                return new User { result = -1, message = "密码错误" };
            }

            entity.password = pwd;
            var user = ToEntity.ToUser(entity);
            user.result = 1;

            return user;
        }

        #endregion

        #region 获取当前用户启服服务器
        //  POST api/System?token={token}
        /// <summary>根据令牌获取当前用户启服服务器</summary>
        /// <param name="token">令牌</param>
        /// <param name="flag">标识</param>
        /// <returns>处理结果信息</returns>
        public IEnumerable<Server> PostFindServers(string token, Boolean flag)
        {
            if (!IsToken(token)) return new List<Server>();   //验证会话

            //设置连接字符串
            tgm_server.SetDbConnName(tgm_connection);
            var list = tgm_server.GetFindServer(token).ToList();

            return list.Select(ToEntity.ToServer).ToList();
        }

        #endregion

        #region 密码重置
        //  POST api/System?token={token}&name={name}&old={old}&newpwd={newpwd}
        /// <summary>密码重置</summary>
        /// <param name="token">令牌</param>
        /// <param name="name">账号</param>
        /// <param name="old">旧密码</param>
        /// <param name="newpwd">新密码</param>
        public User PostResetPassword(String token, String name, String old, String newpwd)
        {
            if (!IsToken(token)) return new User { result = -1, message = "令牌不存在" };   //验证会话

            tgm_role.SetDbConnName(tgm_connection);    //设置数据库连接字符串
            var role = tgm_role.GetFindEntity(name);

            if (role == null)
            {
                return new User { result = -1, message = "账号不存在" };
            }
            var pwd = CryptoHelper.Decrypt(role.password, null);
            if (pwd != old)
            {
                return new User { result = -1, message = "原始密码不正确" };
            }
            var _newpwd = CryptoHelper.Encrypt(newpwd, null);

            role.password = _newpwd;
            role.Save();
            var user = ToEntity.ToUser(role);
            user.result = 1;
            return user;
        }

        #endregion

        #region 账号管理

        //POST api/System?token={token}&role={role}&pid={pid}&index={index}&size={size}
        /// <summary> 账号管理</summary>
        /// <param name="token">令牌</param>
        /// <param name="role">角色</param>
        /// <param name="pid">所属管理</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        public PagerUser PostUsers(String token, Int32 role, Int32 pid, Int32 index = 1, Int32 size = 10)
        {
            if (!IsToken(token)) return null;   //验证会话


            tgm_role.SetDbConnName(tgm_connection);
            var count = 0;
            var entitys = tgm_role.GetPageEntity(role, pid, index - 1, size, out count).ToList();
            var list = new List<User>();
            list.AddRange(entitys.Select(ToEntity.ToUser));
            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };

            return new PagerUser() { Pager = pager, Users = list };
        }

        //  POST api/System?token={token}&name={name}&pid={pid}&role={role}&pwd={pwd}
        /// <summary>后台账号注册</summary>
        /// /// <param name="token">令牌</param>
        /// <param name="name">注册账号名称</param>
        /// <param name="pid">平台编号</param>
        /// <param name="role">注册角色</param>
        /// <param name="pwd">注册密码</param>
        /// <returns>处理结果信息</returns>
        public User PostRegister(String token, String name, Int32 pid, Int32 role, String pwd)
        {
            if (!IsToken(token)) { return new User() { result = -1, message = "令牌不存在" }; }

            tgm_role.SetDbConnName(tgm_connection);
            var entity = tgm_role.GetFindEntity(name);
            if (entity != null) { return new User() { result = -1, message = "账户已存在" }; }

            entity = tgm_role.Register(name, pid, role, pwd);
            var user = ToEntity.ToUser(entity);
            user.result = 1;
            return user;
        }

        //POST api/System/{id}?token={token}
        /// <summary>删除用户</summary>
        /// <param name="token">令牌</param>
        /// <param name="id">账号主键id</param>
        /// <returns></returns>
        public User PostDeleteUser(Int32 id, String token)
        {
            if (!IsToken(token)) return new User { result = -1, message = "令牌不存在" };   //验证会话
            if (id == 0) { return new User() { result = -1, message = "数据错误" }; }

            //操作后台数据库连接字符串
            tgm_role.SetDbConnName(tgm_connection);
            var entity = tgm_role.FindByid(id);

            if (entity == null) { return new User() { result = -1, message = "查询不到该用户信息" }; }
            return entity.Delete() < 0 ? new User() { result = -1, message = "删除用户失败" } : new User() { result = 1 };
        }
        #endregion

        #region 平台管理
        //OST api/System?token={token}&index={index}&size={size}
        /// <summary>获取平台信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        public PagerPlatform PostPlatforms(String token, Int32 index = 1, Int32 size = 10)
        {
            if (!IsToken(token)) return null;   //验证会话


            tgm_role.SetDbConnName(tgm_connection);
            var count = 0;
            var entitys = tgm_platform.GetPageEntity(index - 1, size, out count).ToList();
            var list = new List<Platform>();
            list.AddRange(entitys.Select(ToEntity.ToPlatform));
            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };

            return new PagerPlatform() { Pager = pager, Platforms = list };
        }

        //  POST api/System?token={token}&pname={pname}&name={name}&pwd={pwd}&b={b}
        /// <summary>平台注册</summary>
        /// /// <param name="token">令牌</param>
        /// <param name="pname">平台名称</param>
        /// <param name="name">注册账号名称</param>
        /// <param name="pwd">注册密码</param>
        /// <param name="b">标识</param>
        /// <returns>处理结果信息</returns>
        public Platform PostRegisterPlatform(String token, String pname, String name, String pwd, Boolean b)
        {
            if (!IsToken(token)) { return new Platform() { result = -1, message = "令牌不存在" }; }

            tgm_platform.SetDbConnName(tgm_connection);
            var entity = tgm_platform.GetFindEntity(name);
            if (entity != null) { return new Platform() { result = -1, message = "平台名称已存在" }; }
            tgm_role.SetDbConnName(tgm_connection);
            var role = tgm_role.GetFindEntity(name);
            if (role != null) { return new Platform() { result = -1, message = "账户已存在" }; }

            entity = tgm_platform.Register(pname, name, pwd);
            var platform = ToEntity.ToPlatform(entity);
            platform.result = 1;
            return platform;
        }

        //POST api/System?token={token}&pid={pid}
        /// <summary>删除平台</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台编号</param>
        /// <returns></returns>
        public Platform PostDeletePlatform(String token, Int32 pid)
        {
            if (!IsToken(token)) return new Platform { result = -1, message = "令牌不存在" };   //验证会话
            if (pid == 0) { return new Platform() { result = -1, message = "请求数据错误" }; }

            //操作后台数据库连接字符串
            tgm_platform.SetDbConnName(tgm_connection);
            var entity = tgm_platform.FindByid(pid);

            if (entity == null) { return new Platform() { result = -1, message = "查询不到相应平台信息" }; }
            return tgm_platform.DeleteById(entity.id) < 0 ? new Platform() { result = -1, message = "删除平台数据失败" } : new Platform() { result = 1 };
        }

        #endregion

        #region 启服管理
        //POST api/System?token={token}&role={role}&pid={pid}&type={type}&index={index}&size={size}
        /// <summary>获取平台服务器信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="type">类型</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        /// <param name="role">角色</param>
        /// <param name="pid">平台编号</param>
        public PagerServer PostOpenServer(String token, Int32 role, Int32 pid, String type, Int32 index = 1, Int32 size = 10)
        {
            if (!IsToken(token)) return null;   //验证会话

            tgm_role.SetDbConnName(tgm_connection);
            var count = 0;
            var entitys = tgm_server.GetPageEntity(role, pid, index - 1, size, out count).ToList();
            var list = new List<Server>();
            list.AddRange(entitys.Select(ToEntity.ToServer));
            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };

            return new PagerServer() { Pager = pager, Servers = list };
        }


        //  POST api/System?token={token}&name={name}&ip={ip}&pid={pid}&port={port}&policy={policy}&connect={connect}&tg_route={tg_route}&tg_pay={tg_pay}&game_domain={game_domain}&game_pay={game_pay}
        /// <summary>新服注册</summary>
        /// /// <param name="token">令牌</param>
        /// <param name="name">注册账号名称</param>
        /// <param name="ip">IP</param>
        /// <param name="pid">所属平台</param>
        /// <param name="port">游戏端口</param>
        /// <param name="policy">策略端口</param>
        /// <param name="connect">数据库连接字符串</param>
        /// <param name="tg_route">游戏访问接口</param>
        /// <param name="tg_pay">支付接口</param>
        /// <param name="game_domain">游戏域名</param>
        /// <param name="game_pay">支付路径</param>
        /// <returns>处理结果信息</returns>
        public Server PostRegisterServer(String token, String name, String ip, Int32 pid, Int32 port, Int32 policy
            , String connect, String tg_route, String tg_pay, String game_domain, String game_pay)
        {
            if (!IsToken(token)) { return new Server() { result = -1, message = "令牌不存在" }; }

            tgm_server.SetDbConnName(tgm_connection);
            var isexist = tgm_server.IsExist(name);
            if (isexist) { return new Server() { result = -1, message = "服务器名称已存在" }; }

            var entity = tgm_server.Register(pid, name, ip, port, policy, connect, tg_route, tg_pay, game_domain, game_pay);
            var platform = ToEntity.ToServer(entity);
            platform.result = 1;
            return platform;
        }

        //  POST api/System?token={token}&sid={sid}&b={b}
        /// <summary>获取服务器</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器标识</param>
        /// <param name="b">标识</param>
        public Server PostFindServer(String token, Int32 sid, byte b)
        {
            if (!IsToken(token)) { return new Server() { result = -1, message = "令牌不存在" }; }

            tgm_server.SetDbConnName(tgm_connection);
            var entity = tgm_server.FindByid(sid);
            var server = ToEntity.ToServer(entity);
            server.result = 1;
            return server;
        }

        //  POST api/System?token={token}&id={id}&name={name}&ip={ip}&port={port}&policy={policy}&connect={connect}
        //  &tgroute={tgroute}&tgpay={tgpay}&gamedomain={gamedomain}&gamepay={gamepay}
        /// <summary>服务器信息更新</summary>
        /// /// <param name="token">令牌</param>
        /// <param name="name">注册账号名称</param>
        /// <param name="ip">IP</param>
        /// <param name="id">所属平台</param>
        /// <param name="port">游戏端口</param>
        /// <param name="policy">策略端口</param>
        /// <param name="connect">数据库连接字符串</param>
        /// <param name="tgroute">游戏访问接口</param>
        /// <param name="tgpay">支付接口</param>
        /// <param name="gamedomain">游戏域名</param>
        /// <param name="gamepay">支付路径</param>
        /// <returns>处理结果信息</returns>
        public BaseEntity PostServerEdit(String token, Int32 id, String name, String ip, Int32 port, Int32 policy
            , String connect, String tgroute, String tgpay, String gamedomain, String gamepay)
        {
            if (!IsToken(token)) { return new BaseEntity() { result = -1, message = "令牌不存在" }; }

            tgm_server.SetDbConnName(tgm_connection);
            var entity = tgm_server.FindByid(id);
            if (entity == null) { return new BaseEntity() { result = -2, message = "提交数据错误" }; }
            entity.name = name;
            entity.ip = ip;
            entity.port_server = port;
            entity.port_policy = policy;
            entity.connect_string = connect;
            entity.tg_route = tgroute;
            entity.tg_pay = tgpay;
            entity.game_domain = gamedomain;
            entity.game_pay = gamepay;
            entity.Update();

            return new BaseEntity() { result = 1, message = "更新成功" };
        }

        #endregion
    }
}
