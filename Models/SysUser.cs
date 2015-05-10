using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace UDS.Models
{
    public class SysUser
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string EName { get; set; }
        public int UserLevel { get; set; }
        public bool IsDelete { get; set; }

        internal static List<SysUser> GetList()
        {
            DataTable dt = SQLHelper.ProcDataTable("usp_SysUser");
            return (from DataRow row in dt.Rows
                select new SysUser
                {
                    Id = Convert.ToInt32(row["id"]), EName = row["ename"].ToString(), LoginName = row["loginname"].ToString(), UserLevel = Convert.ToInt32(row["userlevel"]), IsDelete = Convert.ToBoolean(row["isdelete"])
                }).ToList();
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