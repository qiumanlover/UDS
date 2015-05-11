using System.Web;
using System.Web.Mvc;
using UDS.Models;

namespace UDS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AboutErrorAttribute(), 1);//自定义的验证特性
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new UDS.Models.LoginActionFilterAttribute());
            filters.Add(new HandleErrorAttribute(), 2);
        }
    }
}