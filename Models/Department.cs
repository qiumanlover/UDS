using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UDS.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DName { get; set; }
        public string PName { get; set; }
        public int PId { get; set; }
        public Department() { }
        public Department(string dname, int pid)
        {
            this.DName = dname;
            this.PId = pid;
        }
        public Department(int id, string dname, int pid)
        {
            this.Id = id;
            this.DName = dname;
            this.PId = pid;
        }
        internal static int AddInfo(Department info)
        {
            object obj = SQLHelper.ExecuteNonQuery("insert into T_department(name, directorposid) output inserted.id values(@name, @pid)", info.DName, info.PId);
            return Convert.ToInt32(obj);
        }

        internal static int UpdateInfo(Department info)
        {
            return SQLHelper.ProcNoQuery("usp_DepartmentUpdate", new SqlParameter("@id", info.Id), new SqlParameter("@pid", info.PId), new SqlParameter("@dname", info.DName));
        }

        internal static List<SelectListItem> GetSelector()
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_PositionSelectorCharge");
            List<SelectListItem> typeList = new List<SelectListItem>();
            foreach (DataRow row in typedata.Rows)
            {
                typeList.Add(new SelectListItem { Value = row["id"].ToString(), Text = row["name"].ToString() });
            }
            return typeList;
        }

        internal static Department GetInfoById(int id)
        {
            Department info = new Department();
            DataTable dt = SQLHelper.ExecuteDataTable("select name, directorposid from T_department where id=@id", id);
            info.Id = id;
            info.PName = dt.Rows[0]["name"].ToString();
            info.PId = Convert.ToInt32(dt.Rows[0]["directorposid"]);
            return info;
        }

        internal static List<Department> GetList()
        {
            DataTable dt = SQLHelper.ProcDataTable("usp_DepartmentList");
            List<Department> list = new List<Department>();
            foreach (DataRow row in dt.Rows)
            {
                Department info = new Department();
                info.Id = Convert.ToInt32(row["id"]);
                info.DName = row["dname"].ToString();
                info.PName = row["pname"].ToString();
                info.PId = Convert.ToInt32(row["pid"]);
                list.Add(info);
            }
            return list;
        }
    }
}