using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LegacyServices.Models;

namespace LegacyServices.Intefaces {
    [ServiceContract]
    public interface IBaseService {
        [OperationContract]
        string GetSimpleData(int number, string suffix);

        [OperationContract]
        BaseContract GetBaseContract(BaseContract data);

        [OperationContract]
        CustomContract GetCustomContract(CustomContract data);

        [OperationContract]
        CustomType GetCustomData(CustomType composite);
    }
}