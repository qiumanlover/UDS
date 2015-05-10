using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace UDS.Models
{
    public class FieldType
    {
        public int Id { get; set; }
        public string SubName { get; set; }
        public string TypeName { get; set; }

        public FieldType(string subname, string typename)
        {
            SubName = subname;
            TypeName = typename;
        }

        public FieldType(int id, string subname, string typename)
        {
            Id = id;
            SubName = subname;
            TypeName = typename;
        }

        private FieldType() { }

        internal static List<FieldType> GetList()
        {
            DataTable dt = SQLHelper.ProcDataTable("usp_FieldTypeList");
            return (from DataRow row in dt.Rows
                select new FieldType
                {
                    Id = Convert.ToInt32(row["id"]), SubName = row["subname"].ToString(), TypeName = row["typename"].ToString()
                }).ToList();
        }

        internal static List<SelectListItem> GetSelector()
        {
            DataTable typedata = SQLHelper.ProcDataTable("usp_FieldTypeSelector");
            return (from DataRow row in typedata.Rows select new SelectListItem { Value = row["typename"].ToString(), Text = row["typename"].ToString() }).ToList();
        }

        internal static int AddInfo(FieldType info)
        {
            return SQLHelper.ExecuteNonQuery("insert into T_fieldtype(subname, typename) output inserted.id values(@subname, @typename)", info.SubName, info.TypeName);
        }

        internal static int UpdateInfo(FieldType info)
        {
            return SQLHelper.ExecuteNonQuery("update T_fieldtype set subname=@subname, typename=@typename where id=@id", info.SubName, info.TypeName, info.Id);
        }

        internal static FieldType GetInfoById(int id)
        {
            DataTable dt = SQLHelper.ExecuteDataTable("select subname, typename from T_fieldtype where id=@id", id);
            FieldType info = new FieldType
            {
                Id = id,
                SubName = dt.Rows[0]["subname"].ToString(),
                TypeName = dt.Rows[0]["typename"].ToString()
            };
            return info;
        }
    }
}