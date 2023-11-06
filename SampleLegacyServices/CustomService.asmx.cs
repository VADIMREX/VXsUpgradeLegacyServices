using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using LegacyServices.Models;

namespace LegacyServices.Asmx {
    [WebService(Namespace = "http://legacy-services/asmx", Name = "legacy-asmx-service", Description = "Service with customized attributes")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // [System.Web.Script.Services.ScriptService]
    public class CustomService : WebService {
        [WebMethod(MessageName = "get-simple-data", Description = "Get simple data from two parameters")]
        public string GetSimpleData(int number, string suffix) {
            return string.Format("You entered: {0}-{1}", number, suffix);
        }

        [WebMethod(MessageName = "get-base-contract", Description = "Get simple data from base contract")]
        public BaseContract GetBaseContract(BaseContract data) {
            return ServiceLogic.DoSomething(data);
        }

        [WebMethod(MessageName = "get-custom-contract", Description = "Get simple data from customized contract")]
        public CustomContract GetCustomContract(CustomContract data) {
            return ServiceLogic.DoSomething(data);
        }

        [WebMethod(MessageName = "get-custom-data", Description = "Get data from custom type")]
        public CustomType GetCustomData(CustomType data) {
            return ServiceLogic.DoSomething(data);
        }
    }
}