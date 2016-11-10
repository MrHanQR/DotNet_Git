using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DotNet.Web.Attributes
{
    /// <summary>
    /// 幂等性过滤器
    /// </summary>
    public class IdempotentAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public const string IdempotentFlag = "IdempotentRequestId";
        private static readonly ConcurrentDictionary<string, IdempotentItem> RequestHashtable = new ConcurrentDictionary<string, IdempotentItem>();
        private static readonly ConcurrentQueue<Tuple<string, DateTime>> Queue = new ConcurrentQueue<Tuple<string, DateTime>>();

        static IdempotentAttribute()
        {
            var t = new Timer(obj =>
            {
                Tuple<string, DateTime> item;
                while (Queue.TryPeek(out item))
                {
                    if (item.Item2 <= DateTime.Now)
                    {
                        Queue.TryDequeue(out item);
                        IdempotentItem temp;
                        RequestHashtable.TryRemove(item.Item1, out temp);
                    }
                    else
                    {
                        break;
                    }
                }
            });
            t.Change(0, 1000 * 2);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var paramList = new SortedDictionary<string, string>();
            var requset = filterContext.HttpContext.Request;
            foreach (var key in requset.Params.AllKeys)
            {
                if (key != null)
                    paramList.Add(key, key + "=" + requset.Params[key]);
            }
            var requestId =DotNet.Common.DESEncryptHelper.GetStringMD5(string.Join("&", paramList.Select(kv => kv.Value)));
            filterContext.HttpContext.Items[IdempotentFlag] = requestId;
            var result = RequestHashtable.AddOrUpdate(requestId, key =>
            {
                Queue.Enqueue(new Tuple<string, DateTime>(key, DateTime.Now.AddDays(1)));
                return new IdempotentItem() { Status = 1 };
            }, (key, item) =>
            {
                if (item.Status == 3)
                    return item;
                item.Status = 2;
                return item;
            });
            switch (result.Status)
            {
                case 3://已完成返回结果
                    filterContext.Result = result.Result;
                    return;
                case 2://正在处理则返回结果
                    filterContext.Result = filterContext.HttpContext.Request.IsAjaxRequest()
                        ? new JsonResult() { Data = new { isok = true, msg = "系统正在处理中……" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet }
                        : (ActionResult)new ContentResult { Content = "<script text='text/javascript'>alert('系统正在处理中……')</script>", ContentType = "text/html" };
                    return;
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var requestId = filterContext.HttpContext.Items[IdempotentFlag].ToString();
            if (!string.IsNullOrEmpty(requestId))
            {
                RequestHashtable.AddOrUpdate(requestId,
                    key =>
                    {
                        Queue.Enqueue(new Tuple<string, DateTime>(key, DateTime.Now.AddSeconds(10)));
                        return new IdempotentItem() { Status = 3, Result = filterContext.Result };
                    },
                    (key, item) =>
                    {
                        item.Status = 3;
                        item.Result = filterContext.Result;
                        return item;
                    });
            }
            base.OnActionExecuted(filterContext);
        }
        public void OnException(ExceptionContext filterContext)
        {
            string requestId = filterContext.HttpContext.Request[IdempotentFlag];
            if (string.IsNullOrEmpty(requestId))
                return;
            IdempotentItem temp;
            RequestHashtable.TryRemove(requestId, out temp);
        }
        private class IdempotentItem
        {
            /// <summary>
            /// 处理中/已完成
            /// </summary>
            public int Status { get; set; }
            public ActionResult Result { get; set; }
        }
    }

    public static class IdempotentHelper
    {
        /// <summary>
        /// 生成幂等性请求唯一Id
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString AntiIdempotentToken(this HtmlHelper helper)
        {
            return new MvcHtmlString(string.Format("<input type='hidden' name='{0}' value='{1}' />", IdempotentAttribute.IdempotentFlag, Guid.NewGuid().ToString()));
        }
    }
}