using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UDS.Models
{
    public class AboutErrorAttribute : FilterAttribute, IExceptionFilter
    {

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled == true)
            {
                HttpException httpExce = filterContext.Exception as HttpException;
                if (httpExce.GetHttpCode() != 500)//为什么要特别强调500 因为MVC处理HttpException的时候，如果为500 则会自动
                                                    //将其ExceptionHandled设置为true，那么我们就无法捕获异常
                {
                    return;
                }
            }
            HttpException httpException = filterContext.Exception as HttpException;
            if (httpException != null)
            {
                filterContext.Controller.ViewBag.UrlRefer = filterContext.HttpContext.Request.UrlReferrer;
                if (httpException.GetHttpCode() == 404)
                {
                    filterContext.HttpContext.Response.Redirect("~/home/notfound");
                }
                else if (httpException.GetHttpCode() == 500)
                {
                    filterContext.HttpContext.Response.Redirect("~/home/internalError");
                }
            }
            //写入日志 记录
            filterContext.ExceptionHandled = true;//设置异常已经处理
        }
    }
}