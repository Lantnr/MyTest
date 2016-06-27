using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using TGG.Core.Entity;
using TGM.API.Entity;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public partial class RecordController
    {
        //Post api/Record?token={token}&sid={sid}&index={index}&size={size}
        /// <summary>服务器元宝数据列表</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="index">分页索引值</param>
        /// <param name="size">分页大小</param>
        /// <returns></returns>
        public PagerServerGold PostServerGold(String token, Int32 sid, Int32 index = 0, Int32 size = 10)
        {
            if (!IsToken(token)) return new PagerServerGold() { result = -1, message = "令牌不存在" };

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new PagerServerGold() { result = -2, message = "服务器信息不存在，请重新选择" };

            tgm_gold_record.SetDbConnName(tgm_connection);
            int count;
            var entity = tgm_record_day.GetPageEntity(sid, index, size, out count).ToList();
            var list = new List<ServerGoldConsume>();
            list.AddRange(entity.Select(ToEntity.ToServerGoldConsume));

            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count };
            return new PagerServerGold() { Pager = pager, GoldConsumes = list };
        }

        //Post api/Record?token={token}&sid={sid}&role={role}&index={index}&size={size}
        /// <summary>服务器身份数据列表信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="role">用户名</param>
        /// <param name="index">分页索引值</param>
        /// <param name="size">分页大小</param>
        /// <returns></returns>
        public PagerServerIdentity PostServerIdentity(String token, Int32 sid, Int32 role, Int32 index = 0, Int32 size = 10)
        {
            if (!IsToken(token)) return new PagerServerIdentity() { result = -1, message = "令牌不存在" };

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new PagerServerIdentity() { result = -2, message = "服务器信息不存在，请重新选择" };

            SN = server.name;
            report_identity_day.SetDbConnName(db_connection);
            int count;
            var entity = report_identity_day.GetPageEntity(index, size, out count).ToList();
            var list = new List<IdentitySpread>();
            list.AddRange(entity.Select(ToEntity.ToIdentitySpread));

            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count };
            return new PagerServerIdentity() { Pager = pager, ServerIdentitys = list };
        }

        //Post api/Record?token={token}&pid={pid}&sid={sid}&name={name}&index={index}&size={size}
        /// <summary>等级分布信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="pid">平台pid</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="name">权限</param>
        /// <param name="index">分页索引值</param>
        /// <param name="size">分页大小</param>
        /// <returns></returns>
        public PagerServerLevel PostLevel(String token, Int32 pid, Int32 sid, String name, Int32 index = 0, Int32 size = 10)
        {
            if (!IsToken(token)) return new PagerServerLevel() { result = -1, message = "令牌不存在" };

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new PagerServerLevel() { result = -2, message = "服务器信息不存在" };

            SN = server.name;
            report_level_day.SetDbConnName(db_connection);

            int count;
            var entity = report_level_day.GetPageEntity(index, size, out count).ToList();
            var list = new List<LevelSpread>();
            list.AddRange(entity.Select(ToEntity.ToLevelSpread));

            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count };
            return new PagerServerLevel() { Pager = pager, ServerLevels = list };
        }

        //Post api/Record?token={token}&name={name}&sid={sid}&type={type}&value={value}&index={index}&size={size}
        /// <summary>查询服务器玩家信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="name">用户名</param>
        /// <param name="sid">服务器sid</param>
        /// <param name="type">查询类型</param>
        /// <param name="value">查询值</param>
        /// <param name="index">分页索引值</param>
        /// <param name="size">分页大小</param>
        /// <returns></returns>
        public PageServerPlayer PostPlayer(String token, String name, Int32 sid, Int32 type, String value, Int32 index = 0, Int32 size = 10)
        {
            if (!IsToken(token)) return new PageServerPlayer() { result = -1, message = "令牌不存在" };

            tgm_server.SetDbConnName(tgm_connection);
            var server = tgm_server.FindByid(sid);
            if (server == null) return new PageServerPlayer() { result = -2, message = "服务器信息不存在" };

            SN = server.name;
            view_player_detail.SetDbConnName(db_connection);

            int count;
            var entity = view_player_detail.GetPagerEntity(type, value, index, size, out count).ToList();
            var list = new List<ServerPlayer>();
            foreach (var item in entity)
            {
                var identity = Helper.FixedResources.BASE_IDENTITY.FirstOrDefault(m => m.id == item.role_identity);
                if (identity == null) continue;
                list.Add(ToEntity.ToServerPlayer(item, identity.name));
            }

            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count };
            return new PageServerPlayer() { Pager = pager, Players = list };
        }
    }
}
