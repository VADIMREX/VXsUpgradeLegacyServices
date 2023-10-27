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
    public interface IServiceImplicit {
        [OperationContract(Name = "get-simple-data")]
        string GetData(int value);

        [OperationContract(Name = "get-composite-data")]
        CompositeTypeImplicit GetData(CompositeTypeImplicit composite);

        [OperationContract(Name = "get-custom-data")]
        CustomType GetData(CustomType composite);
    }
}

namespace LegacyServices.Models {
    [DataContract]
    public class CompositeTypeImplicit {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    public class CustomType {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}