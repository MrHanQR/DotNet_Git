using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        //自定义分页Helper扩展
        public static HtmlString ShowPageNavigate(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount)
        {
            var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            pageSize = pageSize <= 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            if (totalPages > 1)
            {
                //if (currentPage != 1)
                {//处理首页连接
                    output.AppendFormat("<a class='pageLink' href='{0}?pageIndex=1&pageSize={1}'>首页</a> ", redirectTo, pageSize);
                }
                if (currentPage > 1)
                {//处理上一页的连接
                    output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>上一页</a> ", redirectTo, currentPage - 1, pageSize);
                }
                else
                {
                    // output.Append("<span class='pageLink'>上一页</span>");
                }

                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理
                            //output.Append(string.Format("[{0}]", currentPage));
                            output.AppendFormat("<a class='cpb' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a> ", redirectTo, currentPage, pageSize, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a> ", redirectTo, currentPage + i - currint, pageSize, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>下一页</a> ", redirectTo, currentPage + 1, pageSize);
                }
                else
                {
                    //output.Append("<span class='pageLink'>下一页</span>");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>末页</a> ", redirectTo, totalPages, pageSize);
                }
                output.Append(" ");
            }
            output.AppendFormat("第{0}页 / 共{1}页", currentPage, totalPages);//这个统计加不加都行

            return new HtmlString(output.ToString());
        }
        /// <summary>
        /// 创建一个input_text标签，用于Add表单
        /// id=name,class默认form-control
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="lableText">input前的lable所显示的字符</param>
        /// <param name="tagId">input标签的id属性</param>
        /// <param name="placeHolderText">placeholder属性的显示文字</param>
        /// <returns></returns>
        public static MvcHtmlString InPutTag(this HtmlHelper htmlHelper, string lableText, string tagId,  string placeHolderText)
        {
            string str = string.Format("<div class='form-group'>" +
                        "<label for='{0}'>{1}</label>" +
                        "<input type='text' class='form-control' id='{0}' name='{0}' required='required' placeholder='{2}'>" +
                        "</div>", tagId, lableText,  placeHolderText);
            return new MvcHtmlString(str);
        }
        /// <summary>
        /// 创建一个input标签，用于Add表单
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="lableText">input前的lable所显示的字符</param>
        /// <param name="tagId">input标签的id属性</param>
        /// <param name="placeHolderText">placeholder属性的显示文字</param>
        /// <param name="tagType">input标签的type属性</param>
        /// <returns>返回HtmlString</returns>
        public static MvcHtmlString InPutTag(this HtmlHelper htmlHelper, string lableText, string tagId, string placeHolderText,string tagType)
        {
            string str = string.Format("<div class='form-group'>" +
                        "<label for='{0}'>{1}</label>" +
                        "<input type='{2}' class='form-control' id='{0}' name='{0}'  required='required' placeholder='{3}'>" +
                        "</div>", tagId, lableText, tagType,placeHolderText);
            return new MvcHtmlString(str);
        }
        /// <summary>
        /// 创建一个input标签，用于Add表单
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="lableText">input前的lable所显示的字符</param>
        /// <param name="tagId">input标签的id属性</param>
        /// <param name="placeHolderText">placeholder属性的显示文字</param> 
        /// <param name="tagType">input标签的type属性</param>
        /// <param name="tagName">input标签的name属性</param>
        /// <param name="tagClass">input标签的class属性</param>
        /// <returns>返回HtmlString</returns>
        public static MvcHtmlString InPutTag(this HtmlHelper htmlHelper, string lableText, string tagId, string placeHolderText, string tagType, string tagName, string tagClass)
        {
            string str = string.Format("<div class='form-group'>" +
                        "<label for='{0}'>{1}</label>" +
                        "<input type='{2}' class='{3}' id='{0}' name='{4}'  required='required' placeholder='{5}'>" +
                        "</div>", tagId, lableText, tagType, tagClass, tagName, placeHolderText);
            return new MvcHtmlString(str);
        }
       
        /// <summary>
        /// 创建一个input标签，用于Edit表单
        /// 缺省为text标签，id=name,class=form-control
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="lableText">input前的lable所显示的字符</param>
        /// <param name="tagId">input标签的id属性</param>
        /// <returns>返回HtmlString</returns>
        public static MvcHtmlString EditInPutTag(this HtmlHelper htmlHelper, string lableText, string tagId)
        {
            string str = string.Format("<div class='form-group'>" +
                        "<label for='{0}'>{1}</label>" +
                        "<input type='text' class='form-control' id='{0}' name='{0}'  required='required'>" +
                        "</div>", tagId, lableText);
            return new MvcHtmlString(str);
        }
        /// <summary>
        /// 创建一个input标签，用于Edit表单
        /// 缺省为text标签，id=name,class=form-control
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="tagType">input的type属性</param>
        /// <param name="lableText">input前的lable所显示的字符</param>
        /// <param name="tagId">input标签的id属性</param>
        /// <returns>返回HtmlString</returns>
        public static MvcHtmlString EditInPutTag(this HtmlHelper htmlHelper, string lableText, string tagId, string tagType)
        {
            string str = string.Format("<div class='form-group'>" +
                        "<label for='{0}'>{1}</label>" +
                        "<input type='{2}' class='form-control' id='{0}' name='{0}'  required='required'>" +
                        "</div>", tagId, lableText,tagType);
            return new MvcHtmlString(str);
        }
        /// <summary>
        /// 创建一个input标签，用于Edit表单
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="lableText">input前的lable所显示的字符</param>
        /// <param name="tagId">input标签的id属性</param>
        /// <param name="tagType">input标签的type属性</param>
        /// <param name="tagClass">input标签的class属性</param>
        /// <param name="tagName">input标签的name属性</param>
        /// <returns>返回HtmlString</returns>
        public static MvcHtmlString EditInPutTag(this HtmlHelper htmlHelper, string lableText, string tagId, string tagType, string tagClass, string tagName)
        {
            string str = string.Format("<div class='form-group'>" +
                        "<label for='{0}'>{1}</label>" +
                        "<input type='{2}' class='{3}' id='{0}' name='{4}'  required='required'>" +
                        "</div>", tagId, lableText, tagType, tagClass, tagName);
            return new MvcHtmlString(str);
        }
        /// <summary>
        /// 创建一个select标签
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="lableText">标签前lable显示的文字</param>
        /// <param name="tagId">select标签的id</param>
        /// <param name="keyValue">option键值对</param> 
        /// <param name="tagName">select标签的name</param>
        /// <param name="tagClass">select标签的class</param>
        /// <returns></returns>
        public static MvcHtmlString SelectTag(this HtmlHelper htmlHelper,string lableText,string tagId, Dictionary<string, string> keyValue,string tagName,string tagClass)
        {
            StringBuilder sb=new StringBuilder();
            sb.Append(string.Format("<div class='form-group'>" +
                                    "<label for='{1}'>{0}</label>" +
                                    "<select id={1} name='{2}' class='{3}'>",lableText,tagId,tagName,tagClass));
            if (keyValue!=null)
            {
                foreach (KeyValuePair<string, string> item in keyValue)
                {
                    sb.Append(string.Format("<option value='{0}'>{1}</option>", item.Value, item.Key));
                }
            }
            sb.Append("</select></div>");
            return new MvcHtmlString(sb.ToString());
        }
        /// <summary>
        /// 创建一个select标签
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="lableText">标签前lable显示的文字</param>
        /// <param name="tagId">select标签的id</param>
        /// <param name="keyValue">option键值对</param>
        /// <returns></returns>
        public static MvcHtmlString SelectTag(this HtmlHelper htmlHelper, string lableText, string tagId, Dictionary<string, string> keyValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<div class='form-group'>" +
                                    "<label for='{1}'>{0}</label>" +
                                    "<select id={1} name='{1}' class='form-control'>", lableText, tagId));
            if (keyValue != null)
            {
                foreach (KeyValuePair<string, string> item in keyValue)
                {
                    sb.Append(string.Format("<option value='{0}'>{1}</option>", item.Value, item.Key));
                }
            }
            sb.Append("</select></div>");
            return new MvcHtmlString(sb.ToString());
        }
    }
}