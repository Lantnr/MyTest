using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using NewLife.Log;
using NewLife.Web;
using XCode;
using XCode.Configuration;

namespace TGM.API.Entity
{
    /// <summary>后台平台表</summary>
    public partial class tgm_platform : Entity<tgm_platform>
    {
		#region 扩展连接
        /// <summary>修改数据库链接字符串</summary>
        /// <param name="name"></param>
        public static  void SetDbConnName(String name)
        {
            Meta.ConnName = name;
        }
        #endregion
		
        #region 对象操作

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);
        }

        

        ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        ///// <returns></returns>
        //public override Int32 Insert()
        //{
        //    return base.Insert();
        //}

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnInsert()
        //{
        //    return base.OnInsert();
        //}
        #endregion

        #region 扩展查询
        /// <summary>根据编号查找</summary>
        /// <param name="__id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static tgm_platform FindByid(Int32 __id)
        {
            //if (Meta.Count >= 1000)
            return Find(_.id, __id);
            //else // 实体缓存
            //return Meta.Cache.Entities.Find(__.id, __id);
            // 单对象缓存
            //return Meta.SingleCache[__id];
        }
        #endregion
		
    }
}