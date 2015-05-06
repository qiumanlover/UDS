using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UDS.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string PName { get; set; }
        public int EId { get; set; }
        public string EName { get; set; }
        public Position() { }
        public Position(string pname, int eid)
        {
            this.PName = pname;
            this.EId = eid;
        }
        public Position (int id, string pname, int eid)
        {
            this.Id = id;
            this.PName = pname;
            this.EId = eid;
        }

        internal static int AddInfo(Position pos)
        {
            object obj = SQLHelper.ExecuteNonQuery("insert into T_position(pname, employeeid) output inserted.id values(@pname, @eid)", pos.PName, pos.EId);
            return Convert.ToInt32(obj);
        }

        internal static int UpdateInfo(Position pos)
        {
            return SQLHelper.ProcNoQuery("usp_PositionUpdate", new SqlParameter("@id", pos.Id), new SqlParameter("@eid", pos.EId), new SqlParameter("@pname", pos.PName));
        }

        internal static List<SelectListItem> GetSelector()
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_EmployeeSelectorAll");
            List<SelectListItem> typeList = new List<SelectListItem>();
            foreach (DataRow row in typedata.Rows)
            {
                typeList.Add(new SelectListItem { Value = row["id"].ToString(), Text = row["name"].ToString() });
            }
            return typeList;
        }

        internal static Position GetInfoById(int id)
        {
            Position pos = new Position();
            DataTable dt = SQLHelper.ExecuteDataTable("select pname, employeeid from T_position where id=@id", id);
            pos.Id = id;
            pos.PName = dt.Rows[0]["pname"].ToString();
            pos.EId = Convert.ToInt32(dt.Rows[0]["employeeid"]);
            return pos;
        }

        internal static List<Position> GetList()
        {
            DataTable dt = SQLHelper.ProcDataTable("usp_PositionList");
            List<Position> poslist = new List<Position>();
            foreach (DataRow row in dt.Rows)
            {
                Position pos = new Position();
                pos.Id = Convert.ToInt32(row["id"]);
                pos.PName = row["pname"].ToString();
                pos.EId = Convert.ToInt32(row["eid"]);
                pos.EName = row["ename"].ToString();
                poslist.Add(pos);
            }
            return poslist;
        }
    }
}