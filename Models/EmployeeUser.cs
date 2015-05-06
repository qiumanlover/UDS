using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UDS.Models
{
    public class EmployeeUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PId { get; set; }
        public string PName { get; set; }
        public int DId { get; set; }
        public string DName { get; set; }
        public int SPId { get; set; }
        public string SPName { get; set; }
        public bool IsOnJob { get; set; }
        public int UId { get; set; }
        public string LoginName { get; set; }
        public int UserLevel { get; set; }
        public EmployeeUser() { }
        public EmployeeUser(string name, int pid, int did, int spid, string loginname, int userlevel)
        {
            this.Name = name;
            this.PId = pid;
            this.DId = did;
            this.SPId = spid;
            this.LoginName = loginname;
            this.UserLevel = userlevel;
        }
        public EmployeeUser(int id, string name, int pid, int did, int spid, string loginname, int userlevel)
        {
            this.Id = id;
            this.Name = name;
            this.PId = pid;
            this.DId = did;
            this.SPId = spid;
            this.LoginName = loginname;
            this.UserLevel = userlevel;
        }
        internal static int AddInfo(EmployeeUser info)
        {
            object obj = SQLHelper.ExecuteScalar("insert into T_employee(name, positionid, departmentid, superiorposid) output inserted.id values(@name, @positionid, @departmentid, @superiorposid)", info.Name, info.PId, info.DId, info.SPId);
            info.Id = Convert.ToInt32(obj);
            int result = SQLHelper.ExecuteNonQuery("insert into T_user(eid, loginname, userlevel) output inserted.id values(@eid, @loginname, @userlevel)", info.Id, info.LoginName, info.UserLevel);
            if (info.PId != 5)
            {
                result = SQLHelper.ExecuteNonQuery("update T_position set employeeid=@eid where id=@pid", info.Id, info.PId);
            }
            return result;
        }

        internal static int UpdateInfo(EmployeeUser info)
        {
            int result = 0;
            if (info.PId != 5)
            {
                SQLHelper.ProcNoQuery("usp_EmployeeUserOldModify", new SqlParameter("@pid", info.PId));
                result = SQLHelper.ExecuteNonQuery("update T_position set employeeid=@eid where id=@pid", info.Id, info.PId);
            }
            result = SQLHelper.ProcNoQuery("usp_EmployeeUserUpdateInfo", new SqlParameter("@id", info.Id), new SqlParameter("@name", info.Name), new SqlParameter("@pid", info.PId), new SqlParameter("did", info.DId), new SqlParameter("spid", info.SPId), new SqlParameter("@loginname", info.LoginName), new SqlParameter("@userlevel", info.UserLevel));
            return result;
        }

        internal static List<SelectListItem> GetSelector()
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_PositionSelectorAll");
            List<SelectListItem> typeList = new List<SelectListItem>();
            foreach (DataRow row in typedata.Rows)
            {
                typeList.Add(new SelectListItem { Value = row["id"].ToString(), Text = row["name"].ToString() });
            }
            return typeList;
        }

        internal static List<SelectListItem> GetDSelector()
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_DepartmentSelectorAll");
            List<SelectListItem> typeList = new List<SelectListItem>();
            foreach (DataRow row in typedata.Rows)
            {
                typeList.Add(new SelectListItem { Value = row["id"].ToString(), Text = row["name"].ToString() });
            }
            return typeList;
        }

        internal static List<SelectListItem> GetSSelector()
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_PositionSelectorCharge");
            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem { Value = "0", Text = "" });
            foreach (DataRow row in typedata.Rows)
            {
                typeList.Add(new SelectListItem { Value = row["id"].ToString(), Text = row["name"].ToString() });
            }
            return typeList;
        }

        internal static EmployeeUser GetInfoById(int id)
        {
            EmployeeUser info = new EmployeeUser();
            DataTable dt = SQLHelper.ProcDataTable("usp_EmployeeUserGetById", new SqlParameter("@id", id));
            info.Id = id;
            info.Name = dt.Rows[0]["name"].ToString();
            info.PId = Convert.ToInt32(dt.Rows[0]["pid"]);
            info.DId = Convert.ToInt32(dt.Rows[0]["did"]);
            info.SPId = Convert.ToInt32(dt.Rows[0]["spid"]);
            info.UId = Convert.ToInt32(dt.Rows[0]["uid"]);
            info.LoginName = dt.Rows[0]["loginname"].ToString();
            info.UserLevel = Convert.ToInt32(dt.Rows[0]["userlevel"]);
            return info;
        }

        internal static List<EmployeeUser> GetList()
        {
            DataTable dt = SQLHelper.ProcDataTable("usp_EmployeeUserList");
            List<EmployeeUser> list = new List<EmployeeUser>();
            foreach (DataRow row in dt.Rows)
            {
                EmployeeUser info = new EmployeeUser();
                info.Id = Convert.ToInt32(row["id"]);
                info.Name = row["name"].ToString();
                info.PName = row["pname"].ToString();
                info.DName = row["dname"].ToString();
                info.SPName = row["spname"].ToString();
                info.LoginName = row["loginname"].ToString();
                info.IsOnJob = Convert.ToBoolean(row["isonjob"]);
                list.Add(info);
            }
            return list;
        }

        internal static void UpdateCareer(int onjob, int eid)
        {
            SQLHelper.ProcNoQuery("usp_EmployeeUserUpdateCareer", new SqlParameter("@id", eid), new SqlParameter("@onjob", onjob));
        }
    }
}