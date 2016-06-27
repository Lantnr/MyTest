using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Enum;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public partial class SystemController
    {
        //POST api/System?token={token}&pid={pid}&name={name}&b={b}
        /// <summary>获取平台信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台id</param>
        /// <param name="name">用户名称</param>
        /// <param name="b">标识</param>
        public Platform PostFindPlatform(String token, Int32 pid, String name, byte b)
        {
            if (!IsToken(token)) { return new Platform() { result = -1, message = "令牌不存在" }; }

            tgm_platform.SetDbConnName(tgm_connection);
            var entity = tgm_platform.FindByid(pid);
            var platform = ToEntity.ToPlatform(entity);
            platform.result = 1;
            return platform;
        }

        //POST api/System?token={token}&pid={pid}&pname={pname}&encrypt={encrypt}&b={b}
        /// <summary>平台信息更新</summary>
        /// /// <param name="token">令牌</param>
        /// <param name="pid">平台id</param>
        /// <param name="pname">平台名称</param>
        /// <param name="encrypt">加密字符串</param>
        /// <param name="b">标示</param>
        public BaseEntity PostPlatformEdit(String token, Int32 pid, String pname, String encrypt, byte b)
        {
            if (!IsToken(token)) { return new BaseEntity() { result = -1, message = "令牌不存在" }; }

            tgm_platform.SetDbConnName(tgm_connection);
            var entity = tgm_platform.FindByid(pid);
            if (entity == null) { return new BaseEntity() { result = -2, message = "提交数据错误" }; }
            entity.name = pname;
            entity.encrypt = encrypt;
            entity.Update();
            return new BaseEntity() { result = 1, message = "更新成功" };
        }

        //Post api/System?token={token}&pid={pid}&order={order}&type={type}&index={index}&size={size}
        /// <summary>服务器福利卡激活码数据列表</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">服务器sid</param>
        /// <param name="order">服务器sid</param>
        /// <param name="type">卡牌类型</param>
        /// <param name="index">分页索引值</param>
        /// <param name="size">分页大小</param>
        /// <returns></returns>
        public PagerServerGoodsCode PostServerGoodsCode(String token, Int32 pid, String order, Int32 type, Int32 index = 0, Int32 size = 10)
        {
            if (!IsToken(token)) return new PagerServerGoodsCode() { result = -1, message = "令牌不存在" };

            if (pid == 0) return new PagerServerGoodsCode() { result = -2, message = "平台信息不能为空" };
            tgm_platform.SetDbConnName(tgm_connection);
            var platform = tgm_platform.FindByid(pid);
            if (platform == null) return new PagerServerGoodsCode() { result = -2, message = "平台信息不存在，查询失败" };

            tgm_goods_code.SetDbConnName(tgm_connection);
            int count;
            var entity = tgm_goods_code.GetPageEntity(pid, order, type, index, size, out count).ToList();

            var list = entity.Select(ToEntity.ToServerGoodsCode).ToList();

            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count };
            return new PagerServerGoodsCode() { Pager = pager, GoodsCode = list };
        }

        //POST api/System?token={token}&pid={pid}&data={data}&type={type}&number={number}&b={b}
        /// <summary>生成平台福利卡激活码</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台pid</param>
        /// <param name="data">生成批次</param>
        /// <param name="type">生成类型</param>
        /// <param name="number">生成数量</param>
        /// <param name="b">标示</param>
        public BaseEntity PostCreateCodes(String token, Int32 pid, String data, Int32 type, Int32 number, byte b)
        {
            if (!IsToken(token)) return new BaseEntity() { result = -1, message = "令牌不存在" };

            tgm_platform.SetDbConnName(tgm_connection);
            var platform = tgm_platform.FindByid(pid);
            if (platform == null) return new BaseEntity() { result = -2, message = "平台信息不存在，生成激活码失败" };

            var codes = SerialNumber.GenerateStringList(null, number);
            if (!codes.Any()) return new BaseEntity() { result = -2, message = "生成激活码失败，请重试" };

            tgm_goods_code.SetDbConnName(tgm_connection);
            var create = tgm_goods_code.GetCodesByPid(pid).ToList();  //已经生成激活码
            var list = (from item in codes
                        let key = create.FirstOrDefault(m => m.card_key == item)
                        where key == null
                        select new tgm_goods_code()
                        {
                            card_key = item,
                            type = type,
                            pid = pid,
                            kind = data,
                            platform_name = platform.name,
                        }).ToList();

            if (!tgm_goods_code.InsertCodes(list)) return new BaseEntity() { result = -2, message = "添加数据库激活码失败" };
            return new BaseEntity() { result = 1, message = "发放成功，请前往查看" };
        }

        //POST api/System?token={token}&pid={pid}&sid={sid}&order={order}&type={type}&b={b}    
        /// <summary>发放激活码信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台id</param>
        /// <param name="sid">服务器id</param>
        /// <param name="order">激活码序号</param>
        /// <param name="type">发放类型</param>
        /// <param name="b">标示</param>
        public BaseEntity PostGameGoodsCode(String token, Int32 pid, Int32 sid, String order, Int32 type, byte b)
        {
            if (!IsToken(token)) return new BaseEntity() { result = -1, message = "令牌不存在" };

            tgm_platform.SetDbConnName(tgm_connection);
            var platform = tgm_platform.FindByid(pid);
            if (platform == null) return new BaseEntity() { result = -1, message = "平台信息不存在，发放激活码失败" };

            tgm_goods_code.SetDbConnName(tgm_connection);
            var codes = tgm_goods_code.GetCodesByPidKind(pid, order, type);
            if (!codes.Any()) return new BaseEntity() { result = -1, message = "激活码不存在" };

            tgm_give_log.SetDbConnName(tgm_connection);
            var isgive = tgm_give_log.GetCodesBySidKind(sid, order, type);
            if (isgive != null) return new BaseEntity() { result = -1, message = "该序号激活码已发放，请查看" };

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new BaseEntity() { result = -1, message = "服务器信息不存在，发放激活码失败" };

            var log = new tgm_give_log()
            {
                pid = pid,
                platform_name = platform.name,
                sid = sid,
                server_name = server.name,
                kind = order,
                give_type = type,
                createtime = DateTime.Now,
            };
            if (log.Insert() <= 0) return new BaseEntity() { result = -1, message = "记录激活码发放日志失败" };

            SN = server.name;
            tg_goods.SetDbConnName(db_connection);
            if (!tg_goods.InsertCodes(codes)) return new BaseEntity() { result = -1, message = "发放服务器激活码失败" };
            return new BaseEntity() { result = 1, message = "发放成功，请前往查看" };
        }

        //api/System?token={token}&pid={pid}&b={b}
        /// <summary>查看激活码发放记录</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台pid</param>
        /// <param name="b">标示</param>
        public List<ServerCodeLog> PostCodeLogs(String token, Int32 pid, byte b)
        {
            if (!IsToken(token)) return new List<ServerCodeLog>();

            tgm_give_log.SetDbConnName(tgm_connection);
            var entitys = tgm_give_log.GetCodeLogsByPid(pid).ToList();
            if (!entitys.Any()) return new List<ServerCodeLog>();

            return entitys.Select(ToEntity.ToServerCodeLog).ToList();
        }

        //api/System?token={token}&pid={pid}&role={role}&b={b}
        /// <summary>导出平台激活码</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台pid</param>
        /// <param name="role">权限</param>
        /// <param name="b">标示</param>
        public List<ReportCode> PostCodeLogs(String token, Int32 pid, Int32 role, byte b)
        {
            if (!IsToken(token)) return new List<ReportCode>();

            tgm_goods_code.SetDbConnName(tgm_connection);
            var entitys = tgm_goods_code.GetCodesByPid(pid).ToList();
            if (!entitys.Any()) return new List<ReportCode>();

            return entitys.Select(ToEntity.ToReportCode).ToList();
        }

        //api/System?token={token}&sid={sid}&stateType={stateType}&startTime={startTime}&b={b}
        /// <summary>更改服务器状态</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="stateType">服务器修改状态</param>
        /// <param name="startTime">服务器启服时间</param>
        /// <param name="b">标示</param>
        public BaseEntity PostUpdateServerState(String token, Int32 sid, Int32 stateType, String startTime, byte b)
        {
            if (!IsToken(token)) return new BaseEntity() { result = -1, message = "令牌不存在" };

            tgm_server.SetDbConnName(tgm_connection);
            var entity = tgm_server.FindByid(sid);
            if (entity == null) return new BaseEntity() { result = -1, message = "服务器信息不存在" };

            if (stateType == entity.server_state)
            {
                return new BaseEntity() { result = 1, message = "未更改服务器状态！" };
            }

            if (stateType == (int)ServerOpenState.启服)
            {
                if (string.IsNullOrEmpty(startTime))
                {
                    return new BaseEntity() { result = -1, message = "服务器启服，未设置启服时间！" };
                }
                //var date = DateTime.Now;
                var start = Convert.ToDateTime(startTime);
                //if (start < date)
                //{
                //    return new BaseEntity() { result = -1, message = "启服时间不能小于当前时间，启服时间设置错误！" };
                //}
                entity.server_open = start;
            }
            entity.server_state = stateType;
            entity.Update();
            return new BaseEntity() { result = 1, message = "更改服务器状态成功！" };
        }
    }
}
