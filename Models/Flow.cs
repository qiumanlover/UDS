using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UDS.Models
{
    public class Flow
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string TableName { get; set; }
        public string PageName { get; set; }
        public string FlowFlow { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }

        public Flow() { }

        public Flow(string formname, int templateid, string flowflow)
        {
            this.FormName = formname;
            this.TemplateId = templateid;
            this.FlowFlow = flowflow;
        }

        public Flow(int id, string formname, int templateid, string flowflow)
        {
            this.Id = id;
            this.FormName = formname;
            this.TemplateId = templateid;
            this.FlowFlow = flowflow;
        }

        internal static int AddInfo(Flow info)
        {
            return SQLHelper.ExecuteNonQuery("insert into T_flow(formname, templateid, flow) output inserted.id values(@formname, @templateid, @flow)", info.FormName, info.TemplateId, info.FlowFlow);
        }

        internal static int UpdateInfo(Flow info)
        {
            return SQLHelper.ProcNoQuery("usp_FlowUpdate", new SqlParameter("@id", info.Id), new SqlParameter("@formname", info.FormName), new SqlParameter("@tid", info.TemplateId), new SqlParameter("@flow", info.FlowFlow));
        }

        internal static List<SelectListItem> GetSelector()
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_TemplateSelector");
            List<SelectListItem> typeList = new List<SelectListItem>();
            foreach (DataRow row in typedata.Rows)
            {
                typeList.Add(new SelectListItem { Value = row["id"].ToString(), Text = row["name"].ToString() });
            }
            return typeList;
        }

        internal static Flow GetInfoById(int id)
        {
            Flow info = new Flow();
            DataTable dt = SQLHelper.ExecuteDataTable("select f.formname, f.templateid, f.flow from T_flow as f where f.id=@id", id);
            info.Id = id;
            info.FormName = dt.Rows[0]["formname"].ToString();
            info.TemplateId = Convert.ToInt32(dt.Rows[0]["templateid"]);
            info.FlowFlow = dt.Rows[0]["flow"].ToString();
            return info;
        }

        internal static List<Flow> GetList()
        {
            DataTable dt = SQLHelper.ProcDataTable("usp_FlowList");
            List<Flow> list = new List<Flow>();
            foreach (DataRow row in dt.Rows)
            {
                Flow info = new Flow();
                info.Id = Convert.ToInt32(row["id"]);
                info.FormName = row["formname"].ToString();
                info.TemplateName = row["templatename"].ToString();
                info.FlowFlow = row["flow"].ToString();
                list.Add(info);
            }
            return list;
        }
    }
}