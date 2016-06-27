using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NewLife.Log;
using TGM.API.Entity.Helper;
using XCode;
using XCode.DataAccessLayer;

namespace TGM.API.Entity
{
    /// <summary>角色表</summary>
    public partial class tgm_role
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

        #region 业务

        /// <summary>根据用户名获取角色信息</summary>
        /// <param name="name">用户名</param>
        public static tgm_role GetFindEntity(string name)
        {
            return Find(new string[] { _.name, }, new object[] { name });
        }

        /// <summary>注册</summary>
        /// <param name="name">用户名</param>
        /// <param name="pid">平台编号</param>
        /// <param name="role">角色</param>
        /// <param name="pwd">密码</param>
        public static tgm_role Register(String name, Int32 pid, Int32 role, String pwd)
        {
            var time = DateTime.Now.Ticks;

            //var platform = tgm_platform.FindByid(pid);
            var entity = new tgm_role
            {
                pid = pid,
                createtime = time,
                name = name,
                password = CryptoHelper.Encrypt(pwd, null),
                role = role,

            };
            entity.Save();
            return entity;
        }

        /// <summary> 分页获取用户账户信息 </summary>
        /// <param name="role">角色</param>
        /// <param name="pid">所属id</param>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>

        public static EntityList<tgm_role> GetPageEntity(Int32 role, Int32 pid, Int32 index, Int32 size, out Int32 count)
        {
            var _where = role == 10000 ? "" :
                string.Format("[pid] ={0} and role<={1} ", pid, role);
            count = FindCount(_where, null, null, 0, 0);
            return FindAll(_where, " createtime desc", "*", index * size, size);
        }


        #endregion

        #region 预热初始化数据库

        /// <summary>
        /// 初始化数据及脚本
        /// </summary>
        public static void InitDB()
        {
            //初始化数据
            tgm_platform.Find("id=0");
            Find("id=0");
            //初始化脚本
            var db = DAL.Create(Meta.ConnName);
            db.Session.Execute(SQLResource.tgm_script);
            XTrace.WriteLine("初始化数据库及脚本");
        }

        #endregion

    }
}
