﻿using System;
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
    /// <summary>Role</summary>
    /// <remarks></remarks>
    public partial class view_role : Entity<view_role>
    {
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(view_role).Name, Meta.Table.DataTable.DisplayName);

        //    var entity = new view_role();
        //    entity.id = 0;
        //    entity.user_id = 0;
        //    entity.role_id = 0;
        //    entity.power = 0;
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
        //    entity.att_life = 0;
        //    entity.att_attack = 0;
        //    entity.att_defense = 0;
        //    entity.att_sub_hurtIncrease = 0;
        //    entity.att_sub_hurtReduce = 0;
        //    entity.art_mystery = 0;
        //    entity.art_cheat_code = 0;
        //    entity.equip_weapon = 0;
        //    entity.equip_barbarian = 0;
        //    entity.equip_mounts = 0;
        //    entity.equip_armor = 0;
        //    entity.equip_gem = 0;
        //    entity.equip_craft = 0;
        //    entity.equip_tea = 0;
        //    entity.equip_book = 0;
        //    entity.att_points = 0;
        //    entity.lid = 0;
        //    entity.sub_tea = 0;
        //    entity.sub_calculate = 0;
        //    entity.sub_build = 0;
        //    entity.sub_eloquence = 0;
        //    entity.sub_equestrian = 0;
        //    entity.sub_reclaimed = 0;
        //    entity.sub_ashigaru = 0;
        //    entity.sub_artillery = 0;
        //    entity.sub_mine = 0;
        //    entity.sub_craft = 0;
        //    entity.sub_archer = 0;
        //    entity.sub_etiquette = 0;
        //    entity.sub_martial = 0;
        //    entity.sub_tactical = 0;
        //    entity.sub_medical = 0;
        //    entity.sub_ninjitsu = 0;
        //    entity.sub_tea_progress = 0;
        //    entity.sub_calculate_progress = 0;
        //    entity.sub_build_progress = 0;
        //    entity.sub_eloquence_progress = 0;
        //    entity.sub_equestrian_progress = 0;
        //    entity.sub_reclaimed_progress = 0;
        //    entity.sub_ashigaru_progress = 0;
        //    entity.sub_artillery_progress = 0;
        //    entity.sub_mine_progress = 0;
        //    entity.sub_craft_progress = 0;
        //    entity.sub_archer_progress = 0;
        //    entity.sub_etiquette_progress = 0;
        //    entity.sub_martial_progress = 0;
        //    entity.sub_tactical_progress = 0;
        //    entity.sub_medical_progress = 0;
        //    entity.sub_ninjitsu_progress = 0;
        //    entity.sub_tea_time = 0;
        //    entity.sub_calculate_time = 0;
        //    entity.sub_build_time = 0;
        //    entity.sub_eloquence_time = 0;
        //    entity.sub_equestrian_time = 0;
        //    entity.sub_reclaimed_time = 0;
        //    entity.sub_ashigaru_time = 0;
        //    entity.sub_artillery_time = 0;
        //    entity.sub_mine_time = 0;
        //    entity.sub_craft_time = 0;
        //    entity.sub_archer_time = 0;
        //    entity.sub_etiquette_time = 0;
        //    entity.sub_martial_time = 0;
        //    entity.sub_tactical_time = 0;
        //    entity.sub_medical_time = 0;
        //    entity.sub_ninjitsu_time = 0;
        //    entity.sub_tea_level = 0;
        //    entity.sub_calculate_level = 0;
        //    entity.sub_build_level = 0;
        //    entity.sub_eloquence_level = 0;
        //    entity.sub_equestrian_level = 0;
        //    entity.sub_reclaimed_level = 0;
        //    entity.sub_ashigaru_level = 0;
        //    entity.sub_artillery_level = 0;
        //    entity.sub_mine_level = 0;
        //    entity.sub_craft_level = 0;
        //    entity.sub_archer_level = 0;
        //    entity.sub_etiquette_level = 0;
        //    entity.sub_martial_level = 0;
        //    entity.sub_tactical_level = 0;
        //    entity.sub_medical_level = 0;
        //    entity.sub_ninjitsu_level = 0;
        //    entity.sub_tea_state = 0;
        //    entity.sub_calculate_state = 0;
        //    entity.sub_build_state = 0;
        //    entity.sub_eloquence_state = 0;
        //    entity.sub_equestrian_state = 0;
        //    entity.sub_reclaimed_state = 0;
        //    entity.sub_ashigaru_state = 0;
        //    entity.sub_artillery_state = 0;
        //    entity.sub_mine_state = 0;
        //    entity.sub_craft_state = 0;
        //    entity.sub_archer_state = 0;
        //    entity.sub_etiquette_state = 0;
        //    entity.sub_martial_state = 0;
        //    entity.sub_tactical_state = 0;
        //    entity.sub_medical_state = 0;
        //    entity.sub_ninjitsu_state = 0;
        //    entity.fid = 0;
        //    entity.skill_id = 0;
        //    entity.skill_type = 0;
        //    entity.skill_level = 0;
        //    entity.skill_time = 0;
        //    entity.skill_genre = 0;
        //    entity.role_genre = 0;
        //    entity.att_crit_probability = 0;
        //    entity.att_crit_addition = 0;
        //    entity.att_dodge_probability = 0;
        //    entity.att_mystery_probability = 0;
        //    entity.skill_state = 0;
        //    entity.role_ninja = 0;
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
        //    entity.type_sub = 0;
        //    entity.title_sword = 0;
        //    entity.title_gun = 0;
        //    entity.title_tea = 0;
        //    entity.title_eloquence = 0;
        //    entity.buff_power = 0;
        //    entity.total_honor = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(view_role).Name, Meta.Table.DataTable.DisplayName);
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

        #region 扩展属性
        #endregion

        #region 扩展查询
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        ///// <summary>
        ///// 查询满足条件的记录集，分页、排序
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>实体集</returns>
        //[DataObjectMethod(DataObjectMethodType.Select, true)]
        //public static EntityList<view_role> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindAll(SearchWhere(key), orderClause, null, startRowIndex, maximumRows);
        //}

        ///// <summary>
        ///// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>记录数</returns>
        //public static Int32 SearchCount(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindCount(SearchWhere(key), null, null, 0, 0);
        //}

        /// <summary>构造搜索条件</summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        private static String SearchWhere(String key)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代
            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索
            var exp = SearchWhereByKeys(key, null);

            // 以下仅为演示，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符（第4行）
            //if (userid > 0) exp &= _.OperatorID == userid;
            //if (isSign != null) exp &= _.IsSign == isSign.Value;
            //if (start > DateTime.MinValue) exp &= _.OccurTime >= start;
            //if (end > DateTime.MinValue) exp &= _.OccurTime < end.AddDays(1).Date;

            return exp;
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        #endregion
    }
}