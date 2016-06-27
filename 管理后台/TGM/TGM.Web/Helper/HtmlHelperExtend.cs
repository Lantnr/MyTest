using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// HtmlHelper扩展类
    /// </summary>
    public static class HtmlHelperExtend
    {
        /// <summary>分页导航</summary>
        public static HtmlString ShowPageNavigate(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount)
        {
            if (htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url == null) return new HtmlString("");
            var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
          
            #region 显示页数
            output.Append("<div class='span6'><div class='dataTables_info paging_bootstrap pagination'><ul>");
            if (totalPages > 1)
            {
                output.AppendFormat("<li class='prev'><a href='{0}?=1&size={1}'>&lt;&lt;</a></li>", redirectTo, pageSize);
                if (currentPage > 1)
                {
                    //处理上一页的连接
                    //<li class='prev disabled'><a href='#'>← Prev</a></li>  &lt;  <
                    output.AppendFormat("<li class='prev'><a href='{0}?index={1}&size={2}'> &lt; </a></li>",
                        redirectTo, currentPage - 1, pageSize);
                }
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {
                    //一共最多显示10个页码，前面5个，后面5个
                    //<li class='active'><a href='#'>1</a></li>
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {
                            //当前页处理                            
                            output.AppendFormat(
                                "<li class='active'><a href='{0}?index={1}&size={2}'>{3}</a></li>", redirectTo,
                                currentPage, pageSize, currentPage);
                        }
                        else
                        {
                            //一般页处理
                            output.AppendFormat("<li><a href='{0}?index={1}&size={2}'>{3}</a></li>", redirectTo,
                                currentPage + i - currint, pageSize, currentPage + i - currint);
                        }
                    }
                }
                if (currentPage < totalPages)
                {
                    //处理下一页的链接
                    //<li class='next'><a href='#'>Next → </a></li> &gt; >
                    output.AppendFormat("<li class='next'><a href='{0}?index={1}&size={2}'> &gt; </a></li>",
                        redirectTo, currentPage + 1, pageSize);

                    if (currentPage != totalPages)
                    {
                        output.AppendFormat(
                            "<li class='next'><a href='{0}?index={1}&size={2}'> &gt;&gt; </a></li>", redirectTo,
                            totalPages, pageSize);
                    }
                }
                output.Append("</ul></div></div>");
            }

            #endregion

            //这个统计加不加都行
            output.AppendFormat(
                "<div class='span6'><div class='dataTables_paginate' id='editable-sample_info'>{0}/{1}</div></div>",
                currentPage, totalPages);

            return new HtmlString(output.ToString());

        }
    }
}