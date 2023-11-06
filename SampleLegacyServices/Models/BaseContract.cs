using System.Runtime.Serialization;

namespace LegacyServices.Models {
    /// <summary> Model with empty DataContract/DataMember attributes </summary>
    [DataContract]
    public class BaseContract : ISampleModel {
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

        
        [DataMember]
        public DateTime SomeDate { get; set; }

        [DataMember]
        public int[] IntArray { get; set; }

        [DataMember]
        public decimal[] DecimalArray { get; set; }

        [DataMember]
        public double[] DoubleArray { get; set; }

        [DataMember]
        public Dictionary<string, string> StringPairs { get; set; }

        [DataMember]
        public Dictionary<string, BaseContract> KeyValues1 { get; set; }

        [DataMember]
        public Dictionary<string, CustomContract> KeyValues2 { get; set; }

        [DataMember]
        public Dictionary<string, CustomType> KeyValues3 { get; set; }

        [DataMember]
        public List<string> ListOfSomething { get; set; }

        /// <summary> public but not serializable field </summary>
        public string HidenDataOne;       

        /// <summary> public but not serializable property </summary>
        public decimal HidenDataTwo { get; set; }
    }
}