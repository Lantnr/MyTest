using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TGM.API.Entity;
using TGM.API.Entity.Helper;
using TGM.API.Entity.Model;

namespace TGM.API.Controllers
{
    public class GoodsController : ControllerBase
    {
        //api/Goods?token={token}&role={role}&index={index}&size={size}
        /// <summary>福利卡类型信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="role">用户权限</param>
        /// <param name="index">分页索引</param>
        /// <param name="size">分页大小</param>
        public PagerGoodsType PostGoodsType(String token, Int32 role, Int32 index = 1, Int32 size = 10)
        {
            if (!IsToken(token)) return new PagerGoodsType() { result = -1, message = "令牌不存在" };
            if (role != 10000) return new PagerGoodsType() { result = -1, message = "暂无权限查看" };

            tgm_goods_type.SetDbConnName(tgm_connection);
            int count;
            var entitys = tgm_goods_type.GetPageEntity(index - 1, size, out count).ToList();

            var list = new List<GoodsType>();
            list.AddRange(entitys.Select(ToEntity.ToGoodsType));
            var pager = new PagerInfo() { CurrentPageIndex = index, PageSize = size, RecordCount = count, };

            return new PagerGoodsType() { Pager = pager, GoodsType = list };
        }

        //api/Goods?token={token}&type={type}&name={name}
        /// <summary>添加福利卡类型信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="type">福利卡枚举类型ID</param>
        /// <param name="name">福利卡枚举类型名称</param>
        public BaseEntity PostAddType(String token, Int32 type, String name)
        {
            if (!IsToken(token)) return new BaseEntity() { result = -1, message = "令牌不存在" };

            tgm_goods_type.SetDbConnName(tgm_connection);
            var entityId = tgm_goods_type.GetTypeByTypeId(type);
            if (entityId != null) return new BaseEntity() { result = -1, message = "类型枚举ID已存在，请重新输入！" };

            var entityName = tgm_goods_type.GetTypeByName(name);
            if (entityName != null) return new BaseEntity() { result = -1, message = "类型枚举名称已存在，请重新命名！" };

            var entity = new tgm_goods_type()
            {
                type_id = type,
                name = name,
            };
            if (entity.Insert() <= 0) return new BaseEntity() { result = -1, message = "添加数据库失败！" };

            return new BaseEntity() { result = 1, message = "添加成功，请查看！" };
        }

        //POST api/Goods?token={token}&gid={gid}&b={b}
        /// <summary>获取福利卡单条类型信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="gid">福利卡信息id</param>
        /// <param name="b">标识</param>
        public GoodsType PostFindPlatform(String token, Int32 gid, byte b)
        {
            if (!IsToken(token)) return new GoodsType() { result = -1, message = "令牌不存在" };

            tgm_goods_type.SetDbConnName(tgm_connection);
            var entity = tgm_goods_type.FindByid(gid);

            var goodtype = ToEntity.ToGoodsType(entity);
            goodtype.result = 1;
            return goodtype;
        }

        //POST api/System?token={token}&gid={gid}&typeId={typeId}&name={name}&b={b}
        /// <summary>福利卡类型信息更新</summary>
        /// <param name="token">令牌</param>
        /// <param name="gid">福利卡类型主键Id</param> 
        /// <param name="typeId">福利卡类型枚举ID</param>
        /// <param name="name">福利卡类型枚举名称</param>
        /// <param name="b">标示</param>
        public BaseEntity PostPlatformEdit(String token, Int32 gid, Int32 typeId, String name, byte b)
        {
            if (!IsToken(token)) { return new BaseEntity() { result = -1, message = "令牌不存在" }; }

            tgm_goods_type.SetDbConnName(tgm_connection);
            var entityId = tgm_goods_type.GetTypeByTypeId(gid, typeId);
            if (entityId) return new BaseEntity() { result = -1, message = "相同枚举ID已存在，请重新输入！" };

            var entityName = tgm_goods_type.GetTypeByName(gid, name);
            if (entityName) return new BaseEntity() { result = -1, message = "相同名称已存在，请重新命名！" };

            var entity = tgm_goods_type.FindByid(gid);
            if (entity == null) { return new BaseEntity() { result = -2, message = "不存在该福利卡类型信息，数据错误！" }; }
            entity.type_id = typeId;
            entity.name = name;
            entity.Update();
            return new BaseEntity() { result = 1, message = "更新成功" };
        }

        //Post api/Goods?token={token}&gid={gid}&role={role}&b={b}
        /// <summary> 删除福利卡类型信息</summary>
        /// <param name="token">令牌</param>
        /// <param name="gid">福利卡信息主键id</param>
        /// <param name="role">用户权限</param>
        /// <param name="b">标示</param>
        public BaseEntity PostDeleteType(String token, Int32 gid, Int32 role, byte b)
        {
            if (!IsToken(token)) return new BaseEntity() { result = -1, message = "令牌不存在" };

            tgm_goods_type.SetDbConnName(tgm_connection);
            var goodtype = tgm_goods_type.FindByid(gid);
            if (goodtype == null) return new BaseEntity() { result = -1, message = "福利卡类型信息不存在，删除失败！" };

            if (goodtype.Delete() <= 0) return new BaseEntity() { result = -1, message = "删除数据库信息失败！" };
            return new BaseEntity() { result = 1, message = "删除成功！" };
        }

        //  POST api/Goods?token={token}
        /// <summary>获取福利卡类型集合</summary>
        /// <param name="token">令牌</param>
        /// <param name="flag">标识</param>
        /// <returns>处理结果信息</returns>
        public List<GoodsType> PostFindGoodsType(string token, Boolean flag)
        {
            if (!IsToken(token)) return new List<GoodsType>();   //验证会话

            //设置连接字符串
            tgm_goods_type.SetDbConnName(tgm_connection);
            var list = tgm_goods_type.FindAll().ToList();

            return list.Select(ToEntity.ToGoodsType).ToList();
        }
    }
}
