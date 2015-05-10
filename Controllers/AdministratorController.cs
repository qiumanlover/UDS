using System;
using System.Collections.Generic;
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
            ViewData.Model = Flow.GetList();
            ViewData["typelist"] = Flow.GetSelector();
            return PartialView();
        }

        public JsonResult FlowGetInfo(int id)
        {
            Flow flow = Flow.GetInfoById(id);
            object obj = new { id = flow.Id, formname = flow.FormName, templateid = flow.TemplateId, flowflow = flow.FlowFlow };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FlowProc(int id, string formname, int templateid, string flowflow)
        {
            if (id == 0)
            {
                Flow.AddInfo(new Flow(formname, templateid, flowflow));
            }
            else
            {
                Flow.UpdateInfo(new Flow(id, formname, templateid, flowflow));
            }
            return RedirectToAction("FlowManager");
        }

        public ActionResult FormQuery()
        {
            ViewData["ftypelist"] = FormFlow.GetFSelector();
            ViewData["etypelist"] = FormFlow.GetESelector();
            if (Request["pageindex"] != null & Convert.ToInt32(Request["pageindex"]) != 0)
            {
                int pageindex = Convert.ToInt32(Request["pageindex"]);
                int eid = Convert.ToInt32(Request["eid"]);
                int formid = Convert.ToInt32(Request["formid"]);
                DateTime begintime = Convert.ToDateTime(Request["begintime"]);
                DateTime endtime = Convert.ToDateTime(Request["endtime"]);
                int pagecount;
                List<FormFlow> data = FormFlow.GetList(eid, formid, begintime, endtime, pageindex, 20, out pagecount);
                ViewData.Model = data;
                ViewBag.pageCount = pagecount;
                ViewBag.pageIndex = pagecount == 0 ? 0 : pageindex;
                ViewBag.frtIndex = pagecount == 0 ? 0 : 1;
                ViewBag.preIndex = pagecount == 0 ? 0 : (pageindex == 1 ? 1 : pageindex - 1);
                ViewBag.nxtIndex = pagecount == 0 ? 0 : (pageindex == pagecount ? pageindex : pageindex + 1);
                ViewBag.beginTime = begintime.ToString("yyyy-MM-dd hh:mm");
                ViewBag.endTime = endtime.ToString("yyyy-MM-dd hh:mm");
                ViewBag.eid = eid;
                ViewBag.formid = formid;
                ViewData["paras"] = string.Format("pageindex={0},eid={1},formid={2},begintime={3},endtime={4}", pageindex, eid, formid, begintime.ToString("yyyy-MM-dd hh:mm"), endtime.ToString("yyyy-MM-dd hh:mm"));
            }
            else
            {
                ViewData.Model = new List<FormFlow>();
                ViewBag.pageCount = 0;
                ViewBag.pageIndex = 0;
                ViewBag.frtIndex = 0;
                ViewBag.preIndex = 0;
                ViewBag.nxtIndex = 0;
                ViewBag.beginTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                ViewBag.endTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
            }
            return PartialView();
        }

        public ActionResult ForcePassword()
        {
            ViewData.Model = SysUser.GetList();
            return PartialView();
        }
        [HttpPost]
        public ActionResult UserDelMng(int id)
        {
            User user = Session["user"] as User;
            if (user != null && user.Id == id)
            {
                return JavaScript("alert('无法停用当前账号，请使用其他具有权限的账号进行操作！')");
            }
            SysUser.StopAccount(id);
            return RedirectToAction("ForcePassword");
        }

        public JavaScriptResult UserReset(int id, string name)
        {
            User user = Session["user"] as User;
            if (user != null && user.Id == id)
            {
                SysUser.ResetPass(id);
                return JavaScript("alert('当前账号密码已重置, 请重新登录');self.location.href = '/Login/Index';");
            }
            SysUser.ResetPass(id);
            return JavaScript(string.Format("alert('账号：{0} 的密码已重置')", name));
        }

        public ActionResult SysUserAdd(string loginname)
        {
            SysUser.AddAdministrator(loginname);
            return RedirectToAction("ForcePassword");
        }

        public ActionResult FormParameters()
        {
            ViewData["jbtmpname"] = "加班申请模板";
            ViewData["jbdaysbefore"] = -2;
            return PartialView();
        }

        public ActionResult FieldTypePritialAction()
        {
            ViewData.Model = FieldType.GetList();
            ViewData["typelist"] = FieldType.GetSelector();
            return PartialView();
        }

        public JsonResult FieldTypeGetInfo(int id)
        {
            FieldType ft = FieldType.GetInfoById(id);
            object obj = new { id = ft.Id, subname = ft.SubName, typename = ft.TypeName };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldTypeProc(int id, string subname, string typename)
        {
            if (id == 0)
            {
                FieldType.AddInfo(new FieldType(subname, typename));
            }
            else
            {
                FieldType.UpdateInfo(new FieldType(id, subname, typename));
            }
            return RedirectToAction("FormParameters");
        }
        [HttpPost]
        public JavaScriptResult JbParameter(string paravalue)
        {
            SQLHelper.ExecuteNonQuery(
                "update T_parameter set paravalue=@value where templateid=2 and paraname='daysbefore'", paravalue);
            return JavaScript(string.Format("alert('加班申请时间限制设置为: {0}天')", paravalue));
        }
    }
}