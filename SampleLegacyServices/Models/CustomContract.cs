using System.Runtime.Serialization;

namespace LegacyServices.Models {
    /// <summary> </summary>
    [DataContract(Name = "custom-contract", Namespace = "http://legacy-services/models")]
    public class CustomContract {
        /// <summary> hiden field for property <see cref="IsTruth"/></summary>
        bool hidenDataZero_isTruth = true;

        /// <summary> implicit boolean property </summary>
        [DataMember(Name = "is-truth", Order = 0)]
        public bool IsTruth {
            get { return hidenDataZero_isTruth; }
            set { hidenDataZero_isTruth = value; }
        }

        /// <summary> explicit string property </summary>
        [DataMember(Name = "message", Order = 1)]
        public string Message { get; set; } = "Hello ";

        /// <summary> decimal array field </summary>
        [DataMember(Name = "field", Order = 3)]
        public decimal[] NotProperty;

        /// <summary> contract with type like self </summary>
        [DataMember(Name = "sub-contract-one", Order = 4)]
        public BaseContract SubContractOne { get; set; } 

        /// <summary> binary data </summary>
        [DataMember(Name = "binary-data", Order = 2)]
        public byte[] BinaryData { get; set; } 

        /// <summary> customized contract </summary>
        [DataMember(Name = "sub-contract-two", Order = 5)]
        public CustomContract SubContractTwo { get; set; } 

        /// <summary> custom type </summary>
        [DataMember(Name = "custom-type", Order = 6)]
        public CustomType CustomData { get; set; } 

        /// <summary> public but not serializable field </summary>
        public string HidenDataOne;       

        /// <summary> public but not serializable property </summary>
        public decimal HidenDataTwo { get; set; }
    }
}