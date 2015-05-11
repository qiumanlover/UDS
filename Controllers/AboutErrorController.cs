using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UDS.Models;

namespace UDS.Controllers
{
    public class AboutErrorController : Controller
    {
        //
        // GET: /AboutError/
        [AboutError]
        public ActionResult About404Error()
        {
            return View();
        }

    }
}
