namespace LegacyServices.Models {
    public class CustomType : ISampleModel {
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

        public DateTime SomeDate { get; set; }

        public int[] IntArray { get; set; }

        public decimal[] DecimalArray { get; set; }

        public double[] DoubleArray { get; set; }

        public Dictionary<string, string> StringPairs { get; set; }

        public Dictionary<string, BaseContract> KeyValues1 { get; set; }

        public Dictionary<string, CustomContract> KeyValues2 { get; set; }

        public Dictionary<string, CustomType> KeyValues3 { get; set; }

        public List<string> ListOfSomething { get; set; }

        /// <summary> not hidden field </summary>
        public string HidenDataOne;       

        /// <summary> not hidden propery </summary>
        public decimal HidenDataTwo { get; set; }
    }
}