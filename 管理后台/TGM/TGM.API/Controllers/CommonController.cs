using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TGG.Core.Common.Util;
using TGG.Core.Entity;
using TGM.API.Command;
using TGM.API.Entity;
using TGM.API.Entity.Enum;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;
using XCode;

namespace TGM.API.Controllers
{
    /// <summary>
    /// 公共API
    /// </summary>
    public class CommonController : ControllerBase
    {
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

        //POST api/Common?token={token}&pid={pid}&flag={flag}
        /// <summary>获取平台服务器数据</summary>
        /// <param name="pid">服务器编号</param>
        /// <param name="token">令牌</param>
        /// <param name="flag">标识</param>
        /// <returns></returns>
        public List<Server> PostServerAllList(String token, Int32 pid, Boolean flag)
        {
            if (!IsToken(token)) return null;   //验证会话

            tgm_server.SetDbConnName(tgm_connection);
            var entitys = tgm_server.GetServerList(pid).ToList();
            var list = new List<Server>();
            list.AddRange(entitys.Select(ToEntity.ToServer));
            return list;
        }

        /// <summary>充值接口</summary>
        /// <param name="param">充值参数字符串
        /// token|sid|user_code|order_id|channel|type|amount 
        /// 令牌|游戏服编号|玩家账号|订单号|渠道|充值类型|充值数值 
        /// </param>
        /// <param name="checksum">param的MD5校验和 
        /// MD5字符串token|sid|user_code|order_id|channel|type|amount|key 
        /// 令牌|游戏服编号|玩家账号|订单号|渠道|充值类型|充值数值|平台加密字符串
        /// </param>
        /// <returns>充值后状态结果值</returns>

        public BaseEntity PostPayment(String param, String checksum)
        {
            //key : 解析param用
            //param:充值封装字符串 格式: token|sid|user_code|order_id|channel|type|amount
            //checksum: MD5字符串token|sid|user_code|order_id|channel|type|amount|key

            //var t = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", "0c372ec3-1b00-4286-84a8-9216e7ab59e3", 1, "", 1, "", 1, 1, 100);
            //var _t0 = string.Format("{0}|{1}", t, "123456");

            //解析
            var s = param.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length == 7)
            {
                var _token = s[0];
                var _sid = Convert.ToInt32(s[1]);
                var _user_code = s[2];
                var _order_id = s[3];
                var _channel = s[4];
                var _type = Convert.ToInt32(s[5]);
                var _amount = Convert.ToInt32(s[6]);

                if (!IsToken(_token)) return new BaseEntity { result = (int)ApiType.FAIL, message = "令牌错误" };   //验证会话
                var ck = string.Format("{0}|{1}", param, Token.encrypt);
                var md5 = UConvert.MD5(ck);
                if (checksum == md5)//参数校验
                {
                    //获取游戏服信息
                    tgm_server.SetDbConnName(tgm_connection);
                    var server = tgm_server.FindByid(_sid);
                    if (server == null) return new BaseEntity { result = (int)ApiType.FAIL, message = "游戏服务器不存在" };
                    SN = server.name;
                    tg_user.SetDbConnName(db_connection);
                    var user = tg_user.GetEntityByCode(_user_code);
                    if (user == null) return new BaseEntity { result = (int)ApiType.FAIL, message = "玩家账号不存在" };

                    var ip = server.ip;
                    var port = server.port_server;
                    //var conn = server.connect_string;
                    //解析后调用游戏接口判断是否成功

                    var gold = UConvert.ToGold(_amount, _type);
                    var api = new CommandApi(ip, port, ApiCommand.充值);
                    var state = api.Recharge(user.id, gold);
                    api.Dispose();
                    if (state == (int)ApiType.OK)
                    {
                        tgm_record_pay.SetDbConnName(tgm_connection);
                        var entity = new tgm_record_pay()
                        {
                            sid = _sid,
                            user_code = _user_code,
                            player_id = user.id,
                            player_name = user.player_name,
                            order_id = _order_id,
                            channel = _channel,
                            pay_type = _type,
                            amount = gold,
                            pay_state = state,
                            createtime = DateTime.Now.Ticks,
                            money = _amount,
                        };
                        //无论成功,存入后台数据库这条数据记录
                        entity.Save();
                        //同步数据
                        tgm_record_pay.Proc_sp_pay_syn(entity.id);
                        return new BaseEntity { result = (int)ApiType.OK, message = "充值成功" };
                    }
                    return new BaseEntity { result = state, message = "充值未达到玩家账号" };
                }
                else
                {
                    return new BaseEntity { result = (int)ApiType.FAIL, message = "参数校验错误,传递参数被修改" };
                }
            }
            return new BaseEntity { result = (int)ApiType.FAIL, message = "传递参数解析错误" };
        }


        //api/Common?pid={pid}&type={type}&order={order}
        /// <summary>导出平台激活码</summary>
        /// <param name="pid">平台pid</param>
        /// <param name="type">权限</param>
        /// <param name="order">标示</param>
        public List<ReportCode> PostExcel(Int32 pid, Int32 type, String order)
        {
            if (pid == 0) return new List<ReportCode>();

            tgm_goods_code.SetDbConnName(tgm_connection);
            var entitys = tgm_goods_code.GetExcel(pid, type, order);
            if (!entitys.Any()) return new List<ReportCode>();

            return entitys.Select(ToEntity.ToReportCode).ToList();
        }

        //POST api/Common?token={token}&pid={pid}&sid={sid}&type={type}
        /// <summary>获取平台激活码序列号集合</summary>    
        /// <param name="token">令牌</param>
        /// <param name="pid">平台编号</param>
        /// <param name="sid">服务器编号</param>
        /// <param name="type">福利卡类型</param>
        public List<String> PostPlatformCodes(String token, Int32 pid, Int32 sid, Int32 type)
        {
            if (!IsToken(token)) return null;   //验证会话

            tgm_goods_code.SetDbConnName(tgm_connection);
            var entitys = tgm_goods_code.GetCodesByPidType(pid, type);

            var codes = entitys.GroupBy(m => m.kind);
            var list = codes.Select(item => item.Key).ToList();

            tgm_give_log.SetDbConnName(tgm_connection);
            var give = tgm_give_log.GetCodesBySidType(sid, type);
            if (give.Any())
            {
                var gtype = give.GroupBy(m => m.kind);
                foreach (var item in gtype)
                {
                    if (list.Contains(item.Key))
                    {
                        list.Remove(item.Key);
                    }
                }
            }
            return list;
        }

        //POST api/Common?token={token}&name={name}&pid={pid}&b={b}
        /// <summary>获取平台已发放激活码类型集合</summary>    
        /// <param name="token">令牌</param>
        /// <param name="name">用户名称</param>
        /// <param name="pid">平台编号</param>
        /// <param name="b">标识</param>
        public List<GoodsType> PostCodeType(String token, String name, Int32 pid, byte b)
        {
            if (!IsToken(token)) return null;   //验证会话

            tgm_goods_code.SetDbConnName(tgm_connection);
            var entitys = tgm_goods_code.GetCodesByPid(pid);

            var codes = entitys.GroupBy(m => m.type);
            var list = codes.Select(item => item.Key).ToList();  //获取已生成激活码类型

            if (!list.Any()) return null;
            tgm_goods_type.SetDbConnName(tgm_connection);
            var codestype = tgm_goods_type.FindAll().ToList();

            return (from item in list
                    select codestype.FirstOrDefault(m => m.type_id == item)
                        into type
                        where type != null
                        select new GoodsType()
                        {
                            type_id = type.type_id,
                            name = type.name,
                        }).ToList();
        }
    }
}
