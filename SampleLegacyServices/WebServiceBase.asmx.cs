using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace LegacyServices.Asmx {
    [WebService]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceBase : WebService {
        [WebMethod]
        public string HelloWorld() {
            return "Hello World";
        }
    }
}