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
    /// <summary>武将表</summary>
    public partial class tg_role : Entity<tg_role>
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(tg_role).Name, Meta.Table.DataTable.DisplayName);

        //    var entity = new tg_role();
        //    entity.user_id = 0;
        //    entity.role_id = 0;
        //    entity.power = 0;
        //    entity.role_genre = 0;
        //    entity.role_ninja = 0;
        //    entity.role_level = 0;
        //    entity.role_state = 0;
        //    entity.role_exp = 0;
        //    entity.role_honor = 0;
        //    entity.role_identity = 0;
        //    entity.base_captain = 0;
        //    entity.base_force = 0;
        //    entity.base_brains = 0;
        //    entity.base_charm = 0;
        //    entity.base_govern = 0;
        //    entity.att_points = 0;
        //    entity.att_life = 0;
        //    entity.att_attack = 0;
        //    entity.att_defense = 0;
        //    entity.att_sub_hurtIncrease = 0;
        //    entity.att_sub_hurtReduce = 0;
        //    entity.att_crit_probability = 0;
        //    entity.att_crit_addition = 0;
        //    entity.att_mystery_probability = 0;
        //    entity.att_dodge_probability = 0;
        //    entity.war_att_attack = 0;
        //    entity.war_att_defense = 0;
        //    entity.equip_weapon = 0;
        //    entity.equip_barbarian = 0;
        //    entity.equip_mounts = 0;
        //    entity.equip_armor = 0;
        //    entity.equip_gem = 0;
        //    entity.equip_tea = 0;
        //    entity.equip_craft = 0;
        //    entity.equip_book = 0;
        //    entity.base_captain_life = 0;
        //    entity.base_captain_train = 0;
        //    entity.base_captain_level = 0;
        //    entity.base_captain_spirit = 0;
        //    entity.base_captain_equip = 0;
        //    entity.base_captain_title = 0;
        //    entity.base_force_life = 0;
        //    entity.base_force_train = 0;
        //    entity.base_force_level = 0;
        //    entity.base_force_spirit = 0;
        //    entity.base_force_equip = 0;
        //    entity.base_force_title = 0;
        //    entity.base_brains_life = 0;
        //    entity.base_brains_train = 0;
        //    entity.base_brains_level = 0;
        //    entity.base_brains_spirit = 0;
        //    entity.base_brains_equip = 0;
        //    entity.base_brains_title = 0;
        //    entity.base_charm_life = 0;
        //    entity.base_charm_train = 0;
        //    entity.base_charm_level = 0;
        //    entity.base_charm_spirit = 0;
        //    entity.base_charm_equip = 0;
        //    entity.base_charm_title = 0;
        //    entity.base_govern_life = 0;
        //    entity.base_govern_train = 0;
        //    entity.base_govern_level = 0;
        //    entity.base_govern_spirit = 0;
        //    entity.base_govern_equip = 0;
        //    entity.base_govern_title = 0;
        //    entity.title_sword = 0;
        //    entity.title_gun = 0;
        //    entity.title_tea = 0;
        //    entity.title_eloquence = 0;
        //    entity.buff_power = 0;
        //    entity.total_honor = 0;
        //    entity.total_exp = 0;
        //    entity.character1 = 0;
        //    entity.character2 = 0;
        //    entity.character3 = 0;
        //    entity.art_mystery = 0;
        //    entity.art_cheat_code = 0;
        //    entity.art_war_cheat_code = 0;
        //    entity.art_war_mystery = 0;
        //    entity.art_ninja_cheat_code1 = 0;
        //    entity.art_ninja_cheat_code2 = 0;
        //    entity.art_ninja_cheat_code3 = 0;
        //    entity.art_ninja_mystery = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(tg_role).Name, Meta.Table.DataTable.DisplayName);
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
        /// <summary>根据id查找</summary>
        /// <param name="__id"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static tg_role FindByid(Int64 __id)
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