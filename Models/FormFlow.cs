using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UDS.Models
{
    public class FormFlow
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string FormName { get; set; }
        public int EId { get; set; }
        public string EName { get; set; }
        public string WriteTime { get; set; }
        public int State { get; set; }
        public string StateDesc { get; set; }
        public FormFlow() { }

        internal static List<FormFlow> GetList(int eid, int formid, DateTime begintime, DateTime endtime, int pageindex, int pagesize, out int pagecount)
        {
            DataTable dt;
            if (eid == 0 && formid == 0)
            {
                SqlParameter[] parameters = { new SqlParameter("@pageIndex", SqlDbType.Int), new SqlParameter("@pageSize", SqlDbType.Int), new SqlParameter("@pageCount", SqlDbType.Int), new SqlParameter("@begintime", SqlDbType.DateTime), new SqlParameter("@endtime", SqlDbType.DateTime) };
                parameters[0].Value = pageindex;
                parameters[1].Value = pagesize;
                parameters[2].Direction = ParameterDirection.Output;
                parameters[3].Value = begintime;
                parameters[4].Value = endtime;
                dt = SQLHelper.ProcDataTable("usp_FormFlowRecord", parameters);
                pagecount = Convert.ToInt32(parameters[2].Value);
            }
            else if (eid == 0 && formid != 0)
            {
                SqlParameter[] parameters = { new SqlParameter("@pageIndex", SqlDbType.Int), new SqlParameter("@pageSize", SqlDbType.Int), new SqlParameter("@pageCount", SqlDbType.Int), new SqlParameter("@formid", SqlDbType.Int), new SqlParameter("@begintime", SqlDbType.DateTime), new SqlParameter("@endtime", SqlDbType.DateTime) };
                parameters[0].Value = pageindex;
                parameters[1].Value = pagesize;
                parameters[2].Direction = ParameterDirection.Output;
                parameters[3].Value = formid;
                parameters[4].Value = begintime;
                parameters[5].Value = endtime;
                dt = SQLHelper.ProcDataTable("usp_FormFlowRecordF", parameters);
                pagecount = Convert.ToInt32(parameters[2].Value);
            }
            else if (eid != 0 && formid == 0)
            {
                SqlParameter[] parameters = { new SqlParameter("@pageIndex", SqlDbType.Int), new SqlParameter("@pageSize", SqlDbType.Int), new SqlParameter("@pageCount", SqlDbType.Int), new SqlParameter("@eid", SqlDbType.Int), new SqlParameter("@begintime", SqlDbType.DateTime), new SqlParameter("@endtime", SqlDbType.DateTime) };
                parameters[0].Value = pageindex;
                parameters[1].Value = pagesize;
                parameters[2].Direction = ParameterDirection.Output;
                parameters[3].Value = eid;
                parameters[4].Value = begintime;
                parameters[5].Value = endtime;
                dt = SQLHelper.ProcDataTable("usp_FormFlowRecordE", parameters);
                pagecount = Convert.ToInt32(parameters[2].Value);
            }
            else
            {
                SqlParameter[] parameters = { new SqlParameter("@pageIndex", SqlDbType.Int), new SqlParameter("@pageSize", SqlDbType.Int), new SqlParameter("@pageCount", SqlDbType.Int), new SqlParameter("@eid", SqlDbType.Int), new SqlParameter("@formid", SqlDbType.Int), new SqlParameter("@begintime", SqlDbType.DateTime), new SqlParameter("@endtime", SqlDbType.DateTime) };
                parameters[0].Value = pageindex;
                parameters[1].Value = pagesize;
                parameters[2].Direction = ParameterDirection.Output;
                parameters[3].Value = eid;
                parameters[4].Value = formid;
                parameters[5].Value = begintime;
                parameters[6].Value = endtime;
                dt = SQLHelper.ProcDataTable("usp_FormFlowRecordEF", parameters.ToArray());
                pagecount = Convert.ToInt32(parameters[2].Value);
            }

            List<FormFlow> list = new List<FormFlow>();
            foreach (DataRow row in dt.Rows)
            {
                FormFlow info = new FormFlow();
                info.Id = Convert.ToInt32(row["id"]);
                info.FormName = row["formname"].ToString();
                info.EName = row["ename"].ToString();
                info.WriteTime = Convert.ToDateTime((row["writetime"] ?? "")).ToString("yyyy-MM-dd HH:mm:ss");
                info.StateDesc = row["statedesc"].ToString();
                list.Add(info);
            }
            return list;
        }

        internal static List<SelectListItem> GetFSelector()
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_FlowSelector");
            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem { Value = "0", Text = "全部" });
            foreach (DataRow row in typedata.Rows)
            {
                typeList.Add(new SelectListItem { Value = row["id"].ToString(), Text = row["name"].ToString() });
            }
            return typeList;
        }

        internal static List<SelectListItem> GetESelector()
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_EmployeeSelector");
            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem { Value = "0", Text = "全部" });
            foreach (DataRow row in typedata.Rows)
            {
                typeList.Add(new SelectListItem { Value = row["id"].ToString(), Text = row["name"].ToString() });
            }
            return typeList;
        }
    }
}