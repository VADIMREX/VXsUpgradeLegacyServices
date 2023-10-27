namespace LegacyServices.Models {
    public class CustomType {
        /// <summary> hiden field for property <see cref="IsTruth"/></summary>
        bool hidenDataZero_isTruth = true;

        /// <summary> implicit boolean property </summary>
        public bool IsTruth {
            get { return hidenDataZero_isTruth; }
            set { hidenDataZero_isTruth = value; }
        }

        /// <summary> explicit string property </summary>
        public string Message { get; set; } = "Hello ";

        /// <summary> decimal array field </summary>
        public decimal[] NotProperty;

        /// <summary> contract with type like self </summary>
        public BaseContract SubContractOne { get; set; } 

        /// <summary> binary data </summary>
        public byte[] BinaryData { get; set; } 

        /// <summary> customized contract </summary>
        public CustomContract SubContractTwo { get; set; } 

        /// <summary> custom type </summary>
        public CustomType CustomData { get; set; } 

        /// <summary> not hidden field </summary>
        public string HidenDataOne;       

        /// <summary> not hidden propery </summary>
        public decimal HidenDataTwo { get; set; }
    }
}