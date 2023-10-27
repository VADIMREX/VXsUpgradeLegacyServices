using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LegacyServices.Models;

namespace LegacyServices.Intefaces {
    [ServiceContract(Namespace = "http://legacy-services/svc", Name = "legacy-svc-service")]
    public interface ICustomService {
        [OperationContract(Name = "get-simple-data")]
        string GetData(int number, string suffix);

        [OperationContract(Name = "get-base-contract")]
        BaseContract GetContract(BaseContract data);

        [OperationContract(Name = "get-custom-contract")]
        CustomContract GetContract(CustomContract data);

        [OperationContract(Name = "get-custom-data")]
        CustomType GetData(CustomType composite);
    }
}

