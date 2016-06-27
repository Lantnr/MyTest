using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using NewLife.Log;
using NewLife.Web;
using XCode;
using XCode.Configuration;

namespace TGG.Core.Entity
{
    /// <summary>据点表</summary>
    public partial class tg_war_city : Entity<tg_war_city>
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

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    base.InitData();

        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    // Meta.Count是快速取得表记录数
        //    if (Meta.Count > 0) return;

        //    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(tg_war_city).Name, Meta.Table.DataTable.DisplayName);

        //    var entity = new tg_war_city();
        //    entity.user_id = 0;
        //    entity.base_id = 0;
        //    entity.state = 0;
        //    entity.type = 0;
        //    entity.name = "abc";
        //    entity.size = 0;
        //    entity.boom = 0;
        //    entity.strong = 0;
        //    entity.peace = 0;
        //    entity.guard_time = 0;
        //    entity.module_number = 0;
        //    entity.defense_id = 0;
        //    entity.plan_1 = 0;
        //    entity.plan_2 = 0;
        //    entity.plan_3 = 0;
        //    entity.time = 0;
        //    entity.own = 0;
        //    entity.fire_time = 0;
        //    entity.destroy_time = 0;
        //    entity.ownership_type = 0;
        //    entity.res_soldier = 0;
        //    entity.res_funds = 0;
        //    entity.res_foods = 0;
        //    entity.res_horse = 0;
        //    entity.res_gun = 0;
        //    entity.res_razor = 0;
        //    entity.res_kuwu = 0;
        //    entity.res_use_soldier = 0;
        //    entity.res_use_funds = 0;
        //    entity.res_use_foods = 0;
        //    entity.res_use_horse = 0;
        //    entity.res_use_gun = 0;
        //    entity.res_use_razor = 0;
        //    entity.res_use_kuwu = 0;
        //    entity.res_morale = 0;
        //    entity.interior_bar = 0;
        //    entity.levy_bar = 0;
        //    entity.train_bar = 0;
        //    entity.residence = 0;
        //    entity.destroy_boom = 0;
        //    entity.destroy_strong = 0;
        //    entity.destroy_peace = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(tg_war_city).Name, Meta.Table.DataTable.DisplayName);
        //}


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
        public static tg_war_city FindByid(Int64 __id)
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