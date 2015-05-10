using System.Web.Mvc;
using UDS.Models;

namespace UDS.Controllers
{
    [LoginActionFilter]
    public class NavigationController : Controller
    {
        //
        // GET: /Navigater/

        public ActionResult UserPage()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                int userlevel = user.Userlevel;
                ViewBag.userName = user.Ename == "" ? user.Loginname : user.Ename;
                ViewBag.id = user.Eid.ToString();
                ViewBag.guid = Session[user.Eid.ToString()].ToString();
                switch (userlevel)
                {
                    case 0:
                        ViewBag.PageName = "LevelZero";
                        break;
                    case 1:
                        ViewBag.PageName = "LevelOne";
                        break;
                    case 2:
                        ViewBag.PageName = "LevelTwo";
                        break;
                    case 3:
                        ViewBag.PageName = "LevelThree";
                        break;
                    default:
                        return RedirectToAction("Index", "Login");
                }
            }
            return View();
        }
    }
}
