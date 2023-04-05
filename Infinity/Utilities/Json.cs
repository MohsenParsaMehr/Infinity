using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Infinity.Utilities
{
    public class Json
    {
        public T deserializeJson<T>(string json){                       

            return new JavaScriptSerializer().Deserialize<T>(json);
        }
    }
}