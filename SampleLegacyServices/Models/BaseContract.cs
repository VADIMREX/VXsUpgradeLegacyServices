using System.Runtime.Serialization;

namespace LegacyServices.Models {
    /// <summary> Model with empty DataContract/DataMember attributes </summary>
    [DataContract]
    public class BaseContract {
        /// <summary> hiden field for property <see cref="IsTruth"/></summary>
        bool hidenDataZero_isTruth = true;

        /// <summary> implicit boolean property </summary>
        [DataMember]
        public bool IsTruth {
            get { return hidenDataZero_isTruth; }
            set { hidenDataZero_isTruth = value; }
        }

        /// <summary> explicit string property </summary>
        [DataMember]
        public string Message { get; set; } = "Hello ";

        /// <summary> decimal array field </summary>
        [DataMember]
        public decimal[] NotProperty;

        /// <summary> contract with type like self </summary>
        [DataMember]
        public BaseContract SubContractOne { get; set; } 

        /// <summary> binary data </summary>
        [DataMember]
        public byte[] BinaryData { get; set; } 

        /// <summary> customized contract </summary>
        [DataMember]
        public CustomContract SubContractTwo { get; set; } 

        /// <summary> custom type </summary>
        [DataMember]
        public CustomType CustomData { get; set; } 

        /// <summary> public but not serializable field </summary>
        public string HidenDataOne;       

        /// <summary> public but not serializable property </summary>
        public decimal HidenDataTwo { get; set; }
    }
}