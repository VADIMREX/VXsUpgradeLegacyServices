using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using LegacyServices.Intefaces;
using LegacyServices.Models;

namespace LegacyServices.Svc {
    public class CustomService : ICustomService
    {
        public string GetData(int number, string suffix) {
            return string.Format("You entered: {0}-{1}", number, suffix);
        }

        public BaseContract GetContract(BaseContract data) {
            return ServiceLogic.DoSomething(data);
        }

        public CustomContract GetContract(CustomContract data) {
            return ServiceLogic.DoSomething(data);
        }

        public CustomType GetData(CustomType data) {
            return ServiceLogic.DoSomething(data);
        }
    }
}