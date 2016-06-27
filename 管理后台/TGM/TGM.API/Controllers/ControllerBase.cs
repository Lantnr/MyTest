using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Providers.Entities;
using NewLife.Reflection;
using TGM.API.Entity;

namespace TGM.API.Controllers
{
    /// <summary>
    /// API控制器基类
    /// 第一步:必须验证Token
    /// 操作后台数据库直接取 tgm_connection 属性字符串设置表连接字符串
    /// 第二步(针对游戏数据可靠)
    /// 必须设置SN(选择服务器)
    /// 操作游戏数据库 直接取db_connection 属性设置数据库表连接字符串
    /// </summary>
    public class ControllerBase : ApiController
    {
        /// <summary>是否会话</summary>
        internal Boolean IsToken(String token)
        {
            tgm_platform.SetDbConnName(DBConnect.GetName(null));
            var entity = tgm_platform.FindByToken(token);
            if (entity == null) return false;
            Token = entity;
            return true; 
        }
        /// <summary>会话</summary>
        internal tgm_platform Token { get; set; }

        /// <summary>后台连接字符串</summary>
        private string _tgm_connection;
        /// <summary>后台连接字符串</summary>
        internal String tgm_connection
        {
            get
            {
                return DBConnect.GetName(null);
            }
            set { _tgm_connection = value; }
        }
        /// <summary>选择服务器</summary>
        internal String SN { get; set; }
        /// <summary>游戏数据库连接字符串</summary>
        private string _db_connection;
        /// <summary>游戏数据库连接字符串</summary>
        internal String db_connection
        {
            get
            {
                return DBConnect.GameConnect(Token.token.ToString(), SN); 
            }
            set { _db_connection = value; }
        }
    }
}