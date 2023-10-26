using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LegacyServices {
    [ServiceContract(Namespace = "http://legacy-services", Name = "")]
    public interface IServiceImplicit {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeTypeImplicit GetDataUsingDataContract(CompositeTypeImplicit composite);
    }

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
}