using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;


namespace UDS.Models
{
    public class FlowParameter
    {
        public int Id { get; set; }
        public string ParamsValue { get; set; }
        public string ParamsName { get; set; }
        public string ParamsMean { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }

        public static object GetParaValueFromDb(string paraname, int templateid)
        {
            return SQLHelper.ExecuteScalar(
                "select paravalue from T_parameter where paraname=@name and templateid=@tid", paraname, templateid);
        }

        public static int UpdateParaValueByName(string paraname, int templateid, string paravalue)
        {
            return SQLHelper.ExecuteNonQuery("update T_parameter set paravalue=@value where paraname=@name and templateid=@tid", paravalue, paraname, templateid);
        }

        public static int UpdateParaValueById(int id, string paravalue)
        {
            return SQLHelper.ExecuteNonQuery("update T_parameter set paravalue=@value where id=@id", paravalue, id);
        }

        internal static List<FlowParameter> GetList()
        {
            var dt = SQLHelper.ProcDataTable("usp_ParameterList");
            return (from DataRow row in dt.Rows
                    select new FlowParameter
                    {
                        Id = Convert.ToInt32(row["id"]),
                        ParamsName = row["paraname"].ToString(),
                        ParamsMean = row["paramean"].ToString(),
                        ParamsValue = row["paravalue"].ToString()
                    }).ToList();
        }
    }
}