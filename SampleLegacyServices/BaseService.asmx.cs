using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

using LegacyServices.Models;

namespace LegacyServices.Asmx {
    [WebService]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // [System.Web.Script.Services.ScriptService]
    public class BaseService : WebService {
        [WebMethod]
        public string GetSimpleData(int number, string suffix) {
            return string.Format("You entered: {0}-{1}", number, suffix);
        }

        [WebMethod]
        public BaseContract GetBaseContract(BaseContract data) {
            return ServiceLogic.DoSomething(data);
        }

        [WebMethod]
        public CustomContract GetCustomContract(CustomContract data) {
            return ServiceLogic.DoSomething(data);
        }

        [WebMethod]
        public CustomType GetCustomData(CustomType data) {
            return ServiceLogic.DoSomething(data);
        }
    }
}