using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UDS.Models;

namespace UDS.Controllers
{
    public class AdministratorController : Controller
    {
        public ActionResult PositionManager()
        {
            ViewData.Model = Position.GetList();
            ViewData["typelist"] = Position.GetSelector();
            return PartialView();
        }
        public JsonResult PositionGetInfo(int id)
        {
            Position pos = Position.GetInfoById(id);
            object obj = new { id = pos.Id, pname = pos.PName, eid = pos.EId };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PositionProc(int id, string pname, int eid)
        {
            if (id == 0)
            {
                Position.AddInfo(new Position(pname, eid));
            }
            else
            {
                Position.UpdateInfo(new Position(id, pname, eid));
            }
            return RedirectToAction("PositionManager");
        }

        public ActionResult DepartmentManager()
        {
            ViewData.Model = Department.GetList();
            ViewData["typelist"] = Department.GetSelector();
            return PartialView();
        }
        public JsonResult DepartmentGetInfo(int id)
        {
            Department department = Department.GetInfoById(id);
            object obj = new { id = department.Id, pname = department.PName, pid = department.PId };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DepartmentProc(int id, string dname, int pid)
        {
            if (id == 0)
            {
                Department.AddInfo(new Department(dname, pid));
            }
            else
            {
                Department.UpdateInfo(new Department(id, dname, pid));
            }
            return RedirectToAction("DepartmentManager");
        }

        public ActionResult EmployeeManager()
        {
            ViewData.Model = EmployeeUser.GetList();
            ViewData["typelist"] = EmployeeUser.GetSelector();
            ViewData["dtypelist"] = EmployeeUser.GetDSelector();
            ViewData["stypelist"] = EmployeeUser.GetSSelector();
            return PartialView();
        }

        public JsonResult EmployeeUserGetInfo(int id)
        {
            EmployeeUser eu = EmployeeUser.GetInfoById(id);
            object obj = new { id = eu.Id, name = eu.Name, pid = eu.PId, did = eu.DId, spid = eu.SPId, uid = eu.UId, loginname = eu.LoginName, userlevel = eu.UserLevel };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeUserProc(int id, string name, int pid, int did, int spid, string loginname, int userlevel)
        {
            if (id == 0)
            {
                EmployeeUser.AddInfo(new EmployeeUser(name, pid, did, spid, loginname, userlevel));
            }
            else
            {
                EmployeeUser.UpdateInfo(new EmployeeUser(id, name, pid, did, spid, loginname, userlevel));
            }
            return RedirectToAction("EmployeeManager");
        }

        public ActionResult EmployeeUserCareer(int onjob, int eid)
        {
            EmployeeUser.UpdateCareer(onjob, eid);
            return RedirectToAction("EmployeeManager");
        }

        public ActionResult FlowManager()
        {
            return PartialView();
        }
    }
}