using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
            return (from DataRow row in typedata.Rows select new SelectListItem {Value = row["id"].ToString(), Text = row["name"].ToString()}).ToList();
        }

        internal static Department GetInfoById(int id)
        {
            DataTable dt = SQLHelper.ExecuteDataTable("select name, directorposid from T_department where id=@id", id);
            Department info = new Department
            {
                Id = id,
                PName = dt.Rows[0]["name"].ToString(),
                PId = Convert.ToInt32(dt.Rows[0]["directorposid"])
            };
            return info;
        }

        internal static List<Department> GetList()
        {
            var dt = SQLHelper.ProcDataTable("usp_DepartmentList");
            return (from DataRow row in dt.Rows
                select new Department
                {
                    Id = Convert.ToInt32(row["id"]), DName = row["dname"].ToString(), PName = row["pname"].ToString(), PId = Convert.ToInt32(row["pid"])
                }).ToList();
        }
    }
}