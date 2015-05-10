using System;
using System.Data;

namespace UDS.Models
{
    public class BaseInfo
    {
        public string Formname { get; set; }
        public string Applier { get; set; }
        public string Department { get; set; }
        public string Posotion { get; set; }
        public string Writetime { get; set; }
        public string State { get; set; }
        public string Nextname { get; set; }

        public BaseInfo DbToObject(DataTable dtBaseInfo)
        {
            Formname = dtBaseInfo.Rows[0]["formname"].ToString();
            Applier = dtBaseInfo.Rows[0]["name"].ToString();
            Department = dtBaseInfo.Rows[0]["department"].ToString();
            Posotion = dtBaseInfo.Rows[0]["position"].ToString();
            Writetime = Convert.ToDateTime(dtBaseInfo.Rows[0]["writetime"]).ToString("yyyy-MM-dd HH:mm:ss");
            State = dtBaseInfo.Rows[0]["state"].ToString();
            Nextname = dtBaseInfo.Rows[0]["nextname"].ToString();

            return this;
        }
    }
}