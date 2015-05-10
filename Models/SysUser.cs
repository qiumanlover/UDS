using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UDS.Models
{
    public class SysUser
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string EName { get; set; }
        public int UserLevel { get; set; }
        public bool IsDelete { get; set; }
        public SysUser() { }

        internal static List<SysUser> GetList()
        {
            DataTable dt = SQLHelper.ProcDataTable("usp_SysUser");
            List<SysUser> userList = new List<SysUser>();
            foreach (DataRow row in dt.Rows)
            {
                SysUser info = new SysUser();
                info.Id = Convert.ToInt32(row["id"]);
                info.EName = row["ename"].ToString();
                info.LoginName = row["loginname"].ToString();
                info.UserLevel = Convert.ToInt32(row["userlevel"]);
                info.IsDelete = Convert.ToBoolean(row["isdelete"]);
                userList.Add(info);
            }
            return userList;
        }

        internal static int StopAccount(int id)
        {
            return SQLHelper.ProcNoQuery("usp_SysUserStop", new SqlParameter("@id", id));
        }

        internal static int ResetPass(int id)
        {
            return SQLHelper.ProcNoQuery("usp_SysUserReset", new SqlParameter("@id", id));
        }

        internal static int AddAdministrator(string loginname)
        {
            return SQLHelper.ProcNoQuery("usp_SysUserAdd", new SqlParameter("@loginname", loginname));
        }
    }
}