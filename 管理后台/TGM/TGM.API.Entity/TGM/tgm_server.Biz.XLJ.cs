using System;
using System.Text;
using System.Xml.Serialization;
using XCode;

namespace TGM.API.Entity
{
    public partial class tgm_server
    {

        #region 扩展属性
        /// <summary>平台</summary>
        [NonSerialized]
        private tgm_platform _platform;
        /// <summary>平台</summary>
        [XmlIgnore]
        public tgm_platform Platform
        {
            get
            {
                if (_platform != null || pid <= 0 || Dirtys.ContainsKey("Platform")) return _platform;
                _platform = tgm_platform.FindByid(pid);
                Dirtys["Platform"] = true;
                return _platform;
            }
            set { _platform = value; }
        }

        #endregion

        /// <summary>根据回话获取服务器</summary>
        /// <param name="token">回话</param>
        /// <param name="server">启服服务器</param>
        public static tgm_server GetFindEntity(string token, string server)
        {
            var sb = new StringBuilder();
            sb.Append(" name='");
            sb.Append(server);
            sb.Append("' AND ");
            sb.Append(" pid in ");
            sb.Append(" (select id from tgm_platform where token='");
            sb.Append(token);
            sb.Append("')");
            return Find(sb.ToString());
        }

        /// <summary>根据回话获取服务器是否存在</summary>
        /// <param name="name">服务器名称</param>
        public static Boolean IsExist(string name)
        {
            return FindCount(new string[] { _.name, }, new object[] { name }) > 0;
        }

        /// <summary>根据令牌货物玩家所有启服服务器</summary>
        /// <param name="token">令牌</param>
        public static EntityList<tgm_server> GetFindServer(string token)
        {
            var sb = new StringBuilder();
            sb.Append(" pid in ");
            sb.Append(" (select id from tgm_platform where token='");
            sb.Append(token);
            sb.Append("')");
            return FindAll(sb.ToString(), null, null, 0, 0);
        }

        /// <summary> 分页获取服务器信息 </summary>
        /// <param name="role">角色</param>
        /// <param name="pid">所属id</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>
        public static EntityList<tgm_server> GetPageEntity(Int32 role, Int32 pid, Int32 index, Int32 size, out Int32 count)
        {
            var _where = role == 10000 ? "" :
                string.Format("[pid] ={0} ", pid);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
        }

        /// <summary>注册新服</summary>
        /// <param name="name">用户名</param>
        /// <param name="pid">平台编号</param>
        /// <param name="ip">游戏IP</param>
        /// <param name="port_server">游戏端口</param>
        /// <param name="port_policy">游戏策略端口</param>
        /// <param name="connect_string">游戏数据库连接字符串</param>
        /// <param name="tg_route">游戏访问接口</param>
        /// <param name="tg_pay">游戏支付接口</param>
        /// <param name="game_pay">支付路径</param>
        /// <param name="game_domain">访问域名</param>
        public static tgm_server Register(Int32 pid, String name, String ip, Int32 port_server, Int32 port_policy
            , String connect_string, String tg_route, String tg_pay, String game_domain, String game_pay)
        {
            var time = DateTime.Now.Ticks;

            //var platform = tgm_platform.FindByid(pid);
            var entity = new tgm_server
            {
                pid = pid,
                name = name,
                ip = ip,
                port_server = port_server,
                port_policy = port_policy,
                connect_string = connect_string,
                tg_route = tg_route,
                tg_pay = tg_pay,
                game_domain = game_domain,
                game_pay = game_pay,
                createtime = time,
                server_state=0,
                server_open = DateTime.Now.AddHours(1),
            };
            entity.Save();
            return entity;
        }

        /// <summary>获取服务器数据集合</summary>
        /// <param name="pid">平台编号</param>
        public static EntityList<tgm_server> GetServerList(Int32 pid)
        {
            return FindAll(pid == 0 ? "" : string.Format("[pid] ={0} ", pid), " createtime desc", "*", 0, 0);
        }

        /// <summary>获取服务器数据集合</summary>
        /// <param name="game_domain">访问域名</param>
        public static tgm_server GetGameServer(String game_domain)
        {
            return Find(_.game_domain, game_domain);
        }

        /// <summary>获取服务器数据集合</summary>
        /// <param name="pid">平台编号</param>
        public static EntityList<tgm_server> GetOpenServerList()
        {
            return FindAll("server_state=3", null, null, 0, 0);
        }
    }
}