using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGM.API.Entity.Model
{
    /// <summary>
    /// 分页查询类
    /// </summary>
    /// <typeparam name="TPager">页面信息</typeparam>
    /// <typeparam name="TEntityList">页面数据集合</typeparam>
    public class PagerQuery<TPager, TEntityList> : BaseEntity
    {
        public PagerQuery(TPager pager, TEntityList entityList)
        {
            Pager = pager;
            EntityList = entityList;
        }
        public TPager Pager { get; set; }
        public TEntityList EntityList { get; set; }

    }

    /// <summary>
    /// 分页页面信息类
    /// </summary>
    public class PagerInfo
    {
        /// <summary>记录总数</summary>
        public int RecordCount { get; set; }

        /// <summary>当前页索引</summary>
        public int CurrentPageIndex { get; set; }
        /// <summary>分页大小</summary>
        public int PageSize { get; set; }

    }
}