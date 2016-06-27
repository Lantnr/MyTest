using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGG.Core.Base;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGM.API.Command;
using TGM.API.Entity;
using TGM.API.Entity.Enum;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;
using TGG.Core.Enum.Type;

namespace TGM.API.Controllers
{
    public class ResourceController : ControllerBase
    {
        //  POST api/Resource?token={token}&pid={pid}&sid={sid}&roleid={roleid}&playername={playname}&gift={gift}&gifttype={gifttype}&g1={g1}&g2={g2}&g3={g3}&g4={g4}&g5={g5}&reason={reason}&type={type}&message={message}
        public Resource PostResource(string token, Int64 pid, Int64 sid, Int32 roleid, string playername, string gift, int gifttype, string g1, string g2, string g3, string g4, string g5, string reason, int type,string message)
        {

            if (!IsToken(token)) return new Resource { result = -1, message = "令牌不存在" };   //验证会话
            tgm_role.SetDbConnName(tgm_connection);

            var user = tgm_role.FindByid(roleid);
            if (user == null) return new Resource() { result = -1, message = "没有该操作的权限" };

            tgm_platform.SetDbConnName(tgm_connection);
            var pl = tgm_platform.FindByid(Convert.ToInt32(pid));
            if (user.role != 10000)
            {
                if (pl.id != user.pid) return new Resource() { result = -1, message = "没有权限操作该平台信息" };
            }

            string s = BuildString(g1, g2, g3, g4, g5);
            tgm_resource.SetDbConnName(tgm_connection);
            var tgmentity = new tgm_resource()
            {
                pid = pid,
                sid = sid,
                name = gift,
                player_name = type == 0 ? playername : "",
                user_code = type == 1 ? playername : "",
                time = DateTime.Now.Ticks,
                type = gifttype,
                state = 1,//未审批
                content = reason,
                attachment = s,
                message = message==""?"":message,

            };
            if (tgmentity.Save() < 0) return new Resource() { result = -1, message = "数据格式错误" };
            var server = tgm_server.FindByid(Convert.ToInt32(sid));
            if (server == null) return new Resource() { result = -1, message = "发送服务器信息不存在" };


            var resource = ToEntity.ToResource(tgmentity, GetGoodName(tgmentity.attachment));
            resource.result = 1;
            return resource;
        }

        public string BuildString(string g1, string g2, string g3, string g4, string g5)
        {
            string s = "";
            if (g1 != null)
                s = g1;
            if (g2 != null)
                if (s != "")
                    s += "|" + g2;
                else
                {
                    s = g2;
                }
            if (g3 != null)
                if (s != "")
                    s += "|" + g3;
                else
                {
                    s = g3;
                }
            if (g4 != null)
                if (s != "")
                    s += "|" + g4;
                else
                {
                    s = g4;
                }
            if (g5 != null)
                if (s != "")
                    s += "|" + g5;
                else
                {
                    s = g5;
                }
            return s;
        }


        //POST api/System?token={token}&pid={pid}&role={role}
        /// <summary> 获取平台数据</summary>
        /// <param name="pid">平台编号</param>
        /// <param name="token">令牌</param>
        /// <param name="role">角色</param>>
        public List<Platform> PostPlatformAllList(String token, Int32 pid, Int32 role)
        {
            if (!IsToken(token)) return null;   //验证会话

            tgm_platform.SetDbConnName(tgm_connection);
            var entitys = tgm_platform.GetPlatformList(role, pid).ToList();
            var list = new List<Platform>();
            list.AddRange(entitys.Select(ToEntity.ToPlatform));
            return list;
        }



        /// <summary> 组装邮件实体并推送 </summary>
        /// <param name="userid">接收用户Id</param>
        /// <param name="title">邮件的title</param>
        /// <param name="contents">邮件的contents</param>
        /// <param name="attachment">附件 无附件填""</param>
        public tg_messages BuildMessagesSend(Int64 userid, string title, string contents, string attachment)
        {
            var type = (attachment != String.Empty) ? 1 : 0;
            var model = new tg_messages
            {
                send_id = 0,
                title = title,
                isattachment = type,//type  0：无附件  1：有
                receive_id = userid,
                contents = contents,
                attachment = attachment,
                type = 1, //0：玩家  1：系统
                isread = 0,
                create_time = (DateTime.Now.Ticks - 621355968000000000) / 10000,
            };
            return model;
        }

        //  POST api/Resource?token={token}&roleid={roleid}
        /// <summary>获取所有平台的资源申请列表</summary>
        /// <param name="token">令牌</param>
        /// <param name="roleid">用户id</param>
        /// <returns></returns>
        public List<Resource> PostResourceList(string token, Int32 roleid)
        {
            if (!IsToken(token)) return new List<Resource>(); //{ result = -1, message = "令牌不存在" };   //验证会话

            tgm_role.SetDbConnName(tgm_connection);
            var user = tgm_role.FindByid(roleid);
            if (user == null) return new List<Resource>();
            if (user.role != 10000)
                return new List<Resource>();
            tgm_resource.SetDbConnName(tgm_connection);
            var list = tgm_resource.FindAll().ToList();
            if (!list.Any()) return new List<Resource>();

            var pids = string.Join(",", list.ToList().Select(m => m.pid).ToArray());
            var sids = string.Join(",", list.ToList().Select(m => m.sid).ToArray());
            tgm_platform.SetDbConnName(tgm_connection);
            var plats = tgm_platform.GetEntityName(pids);
            if (!plats.Any()) return new List<Resource>();
            tgm_server.SetDbConnName(tgm_connection);
            var servers = tgm_server.GetEntityName(sids);
            if (!servers.Any()) return new List<Resource>();

            var lists = list.Select(m => ToEntity.ToResource(m, GetGoodName(m.attachment))).ToList();

            foreach (var item in lists)
            {
                var p = plats.FirstOrDefault(m => m.id == item.pid);
                var s = servers.FirstOrDefault(m => m.id == item.sid);
                item.platform = p == null ? "" : p.name;
                item.server = s == null ? "" : s.name;
            }
            return lists;
        }


        //  POST api/Resource?token={token}&pid={pid}&sid={sid}&roleid={roleid}&state={state}
        /// <summary>获取所属平台，所属服务器的资源管理列表</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台id</param>
        /// <param name="sid">服务器id</param>
        /// <param name="roleid">用户id</param>
        /// <param name="state">申请状态</param>
        /// <returns></returns>
        public List<Resource> PostResourceByPidAndSid(string token, Int64 pid, Int64 sid, Int32 roleid, int state)
        {
            if (!IsToken(token)) return new List<Resource>(); //{ result = -1, message = "令牌不存在" };   //验证会话
            tgm_role.SetDbConnName(tgm_connection);
            var user = tgm_role.FindByid(roleid);
            if (user == null) return new List<Resource>();
            if (user.role != 10000)
                return new List<Resource>();

            tgm_platform.SetDbConnName(tgm_connection);
            var platform = tgm_platform.FindByid(Convert.ToInt32(pid));
            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(Convert.ToInt32(sid));

            if (platform == null || server == null)
                return new List<Resource>();

            tgm_resource.SetDbConnName(tgm_connection);
            var list = tgm_resource.GetEntityList(pid, sid, state);
            if (list.Any())
            {

                var lists = list.Select(m => ToEntity.ToResource(m, GetGoodName(m.attachment))).ToList();
                foreach (var item in lists)
                {
                    item.platform = platform.name;
                    item.server = server.name;
                }
                return lists;
            }
            return new List<Resource>();
        }


        //  POST api/Resource?token={token}&id={id}&name={name}
        /// <summary>更改管理资源状态</summary>
        /// <param name="token">令牌</param>
        /// <param name="id">资源主键id</param>
        /// <param name="name">操作人</param>
        /// <returns></returns>
        public Resource PostUpdate(string token, Int64 id, string name)
        {
            if (!IsToken(token)) return new Resource { result = -1, message = "令牌不存在" };   //验证会话
            if (id == 0) { return new Resource() { result = -1, message = "数据错误" }; }

            //操作游戏数据库连接字符串
            tgm_resource.SetDbConnName(tgm_connection);
            var entity = tgm_resource.FindByid(id);

            if (entity == null) { return new Resource() { result = -1, message = "查询不到该资源信息" }; }
            entity.state = 2;
            entity.operation = name;
            entity.Save();

            var server = tgm_server.FindByid(Convert.ToInt32(entity.sid));
            if (server == null) return new Resource() { result = -1, message = " 服务器信息不存在" };

            SN = server.name;
            tg_user.SetDbConnName(db_connection);
            tg_messages.SetDbConnName(db_connection);
            if (entity.type == 2)
            {
                var user1 = new tg_user();
                if (entity.user_code != null)
                {
                    user1 = tg_user.GetEntityByCode(entity.user_code);
                }
                else
                {
                    user1 = tg_user.GetEntityByName(entity.player_name);
                }
                if (user1 != null)
                {
                    var model = BuildMessagesSend(user1.id, entity.name, entity.message, entity.attachment);
                    model.Save();
                }
            }
            else
            {
                var users = tg_user.FindAll().ToList();
                var list = new List<tg_messages>();
                foreach (var item in users)
                {
                    list.Add(BuildMessagesSend(item.id, entity.name, entity.message, entity.attachment));
                }
                tg_messages.GetListInsert(list);
            }

            tg_messages.SetDbConnName(db_connection);

            return new Resource() { result = 1 };

        }

        //  POST api/Resource?token={token}&id={id}&flag={flag}
        /// <summary>删除管理资源</summary>
        /// <param name="token">令牌</param>
        /// <param name="id">资源主键id</param>
        /// <param name="flag">标识</param>
        /// <returns></returns>
        public Resource PostDelete(string token, Int64 id, Boolean flag)
        {
            if (!IsToken(token)) return new Resource { result = -1, message = "令牌不存在" };   //验证会话
            if (id == 0) { return new Resource() { result = -1, message = "数据错误" }; }

            //操作游戏数据库连接字符串
            tgm_resource.SetDbConnName(tgm_connection);
            var entity = tgm_resource.FindByid(id);

            if (entity == null) { return new Resource() { result = -1, message = "查询不到该资源信息" }; }
            entity.Delete();
            return new Resource() { result = 1 };

        }

        //  POST api/Resource?token={token}&id={id}&name={name}&flag={flag}
        /// <summary>更改管理资源状态</summary>
        /// <param name="token">令牌</param>
        /// <param name="id">资源主键id</param>
        /// <param name="name">操作人</param>
        /// <param name="flag">标识</param>
        /// <returns></returns>
        public Resource PostUpdate(string token, Int64 id, string name, Boolean flag)
        {
            if (!IsToken(token)) return new Resource { result = -1, message = "令牌不存在" };   //验证会话
            if (id == 0) { return new Resource() { result = -1, message = "数据错误" }; }

            //操作游戏数据库连接字符串
            tgm_resource.SetDbConnName(tgm_connection);
            var entity = tgm_resource.FindByid(id);

            if (entity == null) { return new Resource() { result = -1, message = "查询不到该资源信息" }; }
            if (entity.state == 2 || entity.state != 1)
                return new Resource() { result = -1, message = "审批已通过" };
            entity.state = 3;
            entity.operation = name;
            entity.Save();
            return new Resource() { result = 1 };

        }

        public string GetGoodName(string s)
        {
            string sv = "";
            if (s == null) return sv;
            if (s.Length > 0)
            {
                if (s.Contains('|'))
                {
                    var v = s.Split('|');
                    foreach (var item in v)
                    {
                        sv += GetGoodNameSingel(item) + "； ";
                    }
                }
                else
                {
                    sv += GetGoodNameSingel(s);
                }
            }
            return sv;
        }

        public string GetGoodNameSingel(string value)
        {
            var v = value.Split('_');
            string s = "";
            switch (Convert.ToInt32(v[0]))
            {
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_GOLD: return "元宝" + v[1];
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_COIN: return "金钱" + v[1];
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_EXP: return "经验" + v[1];
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_HONOR: return "功勋" + v[1];
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_FAME: return "声望" + v[1];
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_SPIRIT: return "魂" + v[1];
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_MERIT: return "战功值" + v[1];
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_DONATE: return "贡献度" + v[1];
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_PROP:
                    {
                        var baseprop = Helper.FixedResources.BASE_PROP.FirstOrDefault(m => m.id == Convert.ToInt32(v[1]));
                        if (baseprop != null)
                            return "道具：" + baseprop.name + v[2] + "个";
                    }
                    break;
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_FUSION:
                    var basefusion = Helper.FixedResources.BASE_FUSION.FirstOrDefault(m => m.id == Convert.ToInt32(v[1]));
                    if (basefusion != null)
                        return "熔炼道具：" + basefusion.name + v[2] + "个";
                    break;
                case (int)TGG.Core.Enum.Type.GoodsType.TYPE_EQUIP:
                    {
                        var baseequip = Helper.FixedResources.BASE_EQUIP.FirstOrDefault(m => m.id == Convert.ToInt32(v[1]));
                        if (baseequip != null)
                            return "装备：" + baseequip.name + v[3] + "个";
                    }
                    break;
            }
            return s;
        }
    }
}
