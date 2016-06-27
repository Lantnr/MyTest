using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using TGM.API.Entity.Helper;
using XCode;

namespace TGM.API.Entity
{
    /// <summary>后台平台表</summary>
    public partial class tgm_platform
    {
        #region 首次连接数据库时初始化数据
        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitData()
        {
            base.InitData();

            // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
            // Meta.Count是快速取得表记录数
            if (Meta.Count > 0) return;

            // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(tgm_platform).Name, Meta.Table.DataTable.DisplayName);

            var entity = new tgm_platform { token = Guid.NewGuid(), name = "多游网络", createtime = DateTime.Now.Ticks, encrypt = SerialNumber.GenerateString() };
            entity.Insert();

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(tgm_platform).Name, Meta.Table.DataTable.DisplayName);
        }

        #endregion

        /// <summary>注册</summary>
        /// <param name="pname"></param>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        public static tgm_platform Register(String pname,String name, String pwd)
        {
            var time = DateTime.Now.Ticks;
            var entity = new tgm_platform
            {
                createtime = time,
                name = pname,
                token=Guid.NewGuid(),
                encrypt=SerialNumber.GenerateString(),
            };
            entity.Save();
            tgm_role.Register(name, entity.id, 1000, pwd);
            return entity;
        }

        #region 业务
        /// <summary>根据编号查找</summary>
        /// <param name="__token">令牌</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static tgm_platform FindByToken(String __token)
        {
            return Find(_.token, __token);
        }
        #endregion

        /// <summary> 分页获取用户账户信息 </summary>
        /// <param name="index">第几页</param>
        /// <param name="size">分页大小</param>
        /// <param name="count">总数</param>

        public static EntityList<tgm_platform> GetPageEntity(Int32 index, Int32 size, out Int32 count)
        {
            count = FindCount("", null, null, 0, 0);
            return FindAll("", " createtime desc", "*", index * size, size);
        }

        /// <summary>根据平台名称获取平台信息</summary>
        /// <param name="name">平台名称</param>
        public static tgm_platform GetFindEntity(string name)
        {
            return Find(new string[] { _.name, }, new object[] { name });
        }

        /// <summary>删除平台</summary>
        /// <param name="id">平台编号</param>
        public static Int32 DeleteById(Int32 id)
        {
            tgm_server.SetDbConnName(Meta.ConnName);
            tgm_server.Delete(string.Format(" pid={0}", id));
            tgm_role.SetDbConnName(Meta.ConnName);
            tgm_role.Delete(string.Format(" pid={0}", id));
            return Delete(string.Format(" id={0}", id));         
        }

        /// <summary>获取平台数据集合</summary>
        /// <param name="role"></param>
        /// <param name="id">平台编号</param>
        /// <returns></returns>
        public static EntityList<tgm_platform> GetPlatformList(Int32 role,Int32 id)
        {
            var _where = role == 10000 ? "" :
                string.Format("[id] ={0} ", id);
            return FindAll(_where, " createtime desc", "*",0, 0);
        }


    }
}
