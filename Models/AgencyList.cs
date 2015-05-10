using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace UDS.Models
{
    public class AgencyList
    {
        public int Id { get; set; }
        public int GrantorId { get; set; }
        public string GrantorName { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        [Required(ErrorMessage = @"*")]
        public string BeginTime { get; set; }
        [Required(ErrorMessage = @"*")]
        public string EndTime { get; set; }

        public AgencyList DbDataToAgency(DataRow row, DataColumnCollection columns)
        {
            foreach (DataColumn colname in columns)
            {
                switch (colname.ColumnName)
                {
                    case "id":
                        Id = Convert.ToInt32(row["id"] ?? 0);
                        break;
                    case "grantorid":
                        GrantorId = Convert.ToInt32(row["grantorid"] ?? 0);
                        break;
                    case "grantorname":
                        GrantorName = row["grantorname"].ToString();
                        break;
                    case "agentid":
                        AgentId = Convert.ToInt32(row["agentid"] ?? 0);
                        break;
                    case "agentname":
                        AgentName = row["agentname"].ToString();
                        break;
                    case "begintime":
                        BeginTime = Convert.ToDateTime(row["begintime"] ?? "").ToString("yyyy-MM-dd HH:mm");
                        break;
                    case "endtime":
                        EndTime = Convert.ToDateTime(row["endtime"] ?? "").ToString("yyyy-MM-dd HH:mm");
                        break;
                }
            }
            return this;
        }

        public static int AddInfo(AgencyList agency)
        {
            string sql = "insert into T_agency(grantorid, agentid, begintime, endtime)values(@grantorid, @agentid, @begintime, @endtime)";
            return SQLHelper.ExecuteNonQuery(sql, agency.GrantorId, agency.AgentId, agency.BeginTime, agency.EndTime);
        }
    }
}