using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using UDS.Models;

namespace UDS.Controllers
{
    public class AgentController : Controller
    {
        //
        // GET: /Agent/

        public ActionResult AgentConfig()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                int eid = user.Eid;
                DataTable dt = SQLHelper.ProcDataTable("usp_Grantor", new SqlParameter("@id", eid));
                DataColumnCollection columns = dt.Columns;
                List<AgencyList> data = new List<AgencyList>();
                foreach (DataRow row in dt.Rows)
                {
                    AgencyList info = new AgencyList();
                    info = info.DbDataToAgency(row, columns);
                    data.Add(info);
                }
                ViewData.Model = data;
            }
            return PartialView();
        }

        public ActionResult AgentSign()
        {
            User user = Session["user"] as User;
            if (user != null)
            {
                int eid = user.Eid;
                DataTable dt = SQLHelper.ProcDataTable("usp_GrantorIds", new SqlParameter("@id", eid));
                List<int> idlist = new List<int>();
                foreach (DataRow row in dt.Rows)
                {
                    idlist.Add(Convert.ToInt32(row["grantorid"]));
                }
                string eids = String.Join(",", idlist.ToArray());
                int pagecount;
                int pageindex = int.Parse(Request["pageindex"]);
                if (pageindex < 1) pageindex = 1;
                List<InfoList> data = GetInfoListFromDb("usp_AgentSign", eids, pageindex, 20, out pagecount);
                if (pageindex > pagecount)
                {
                    pageindex = pagecount;
                    data = GetInfoListFromDb("usp_AgentSign", eids, pageindex, 20, out pagecount);
                }
                ViewData.Model = data;
                ViewBag.pageCount = pagecount;
                ViewBag.pageIndex = pagecount == 0 ? 0 : pageindex;
                ViewBag.frtIndex = pagecount == 0 ? 0 : 1;
                ViewBag.preIndex = pagecount == 0 ? 0 : (pageindex == 1 ? 1 : pageindex - 1);
                ViewBag.nxtIndex = pagecount == 0 ? 0 : (pageindex == pagecount ? pageindex : pageindex + 1);
                ViewData["paras"] = string.Format("pageindex={0}", pageindex);
            }
            return PartialView();
        }

        private static List<InfoList> GetInfoListFromDb(string procname, string eids, int pageindex, int pagesize, out int pagecount)
        {
            SqlParameter[] parameters = { new SqlParameter("@pageIndex", SqlDbType.Int), new SqlParameter("@pageSize", SqlDbType.Int), new SqlParameter("@pageCount", SqlDbType.Int), new SqlParameter("@Temp_Array", SqlDbType.VarChar) };
            parameters[0].Value = pageindex;
            parameters[1].Value = pagesize;
            parameters[2].Direction = ParameterDirection.Output;
            parameters[3].Value = eids;
            DataTable dt = SQLHelper.ProcDataTable(procname, parameters);
            pagecount = Convert.ToInt32(parameters[2].Value);
            List<InfoList> data = new List<InfoList>();
            DataColumnCollection columns = dt.Columns;
            foreach (DataRow row in dt.Rows)
            {
                InfoList info = new InfoList();
                info = info.DBDataToInfo(row, columns);
                data.Add(info);
            }
            return data;
        }

        public ActionResult AgentHistory()
        {
            var user = Session["user"] as User;
            if (user != null)
            {
                int eid = user.Eid;
                DataTable dt = SQLHelper.ProcDataTable("usp_Agent", new SqlParameter("@id", eid));
                DataColumnCollection columns = dt.Columns;
                List<AgencyList> data = new List<AgencyList>();
                foreach (DataRow row in dt.Rows)
                {
                    AgencyList info = new AgencyList();
                    info = info.DbDataToAgency(row, columns);
                    data.Add(info);
                }
                ViewData.Model = data;
            }
            return PartialView();
        }

        public ActionResult StopAuth(int id)
        {
            SQLHelper.ProcNoQuery("usp_AgentStop", new SqlParameter("@id", id));
            return RedirectToAction("AgentConfig");
        }

        public ActionResult AgentAdd(AgencyList agency)
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_EmployeeSelectorOn");
            List<SelectListItem> typeList = new List<SelectListItem>();
            foreach (DataRow row in typedata.Rows)
            {
                typeList.Add(new SelectListItem { Value = row["id"].ToString(), Text = row["name"].ToString() });
            }
            ViewData["typelist"] = typeList;
            if (Request["save"] != null)
            {
                var user = Session["user"] as User;
                if (user != null) agency.GrantorId = user.Eid;
                AgencyList.AddInfo(agency);
                return RedirectToAction("AgentConfig");
            }
            else if (Request["back"] != null)
            {
                return RedirectToAction("AgentConfig");
            }
            return PartialView();
        }
    }
}
