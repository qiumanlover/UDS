﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UDS.Models
{
    public class FlowParameter
    {
        public static object GetParaValueFromDb(string paraname, int templateid)
        {
            return SQLHelper.ExecuteScalar(
                "select paravalue from T_parameter where paraname=@name and templateid=@tid", paraname, templateid);
        }

        public static int UpdateParaValue(string paraname, int templateid, string paravalue)
        {
            return SQLHelper.ExecuteNonQuery("update T_parameter set paravalue=@value where paraname=@name and templateid=@tid", paravalue, paraname, templateid);
        }
    }
}