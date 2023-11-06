using System;
using System.Collections.Generic;

namespace LegacyServices.Models {
    public interface ISampleModel {
        /// <summary> implicit boolean property </summary>
        bool IsTruth { get; set; }

        /// <summary> explicit string property </summary>
        string Message { get; set; }

        /// <summary> contract with type like self </summary>
        BaseContract SubContractOne { get; set; } 

        /// <summary> binary data </summary>
        byte[] BinaryData { get; set; } 

        /// <summary> customized contract </summary>
        CustomContract SubContractTwo { get; set; } 

        /// <summary> custom type </summary>
        CustomType CustomData { get; set; } 

        DateTime SomeDate { get; set; }

        int[] IntArray { get; set; }

        decimal[] DecimalArray { get; set; }

        double[] DoubleArray { get; set; }

        Dictionary<string, string> StringPairs { get; set; }

        Dictionary<string, BaseContract> KeyValues1 { get; set; }

        Dictionary<string, CustomContract> KeyValues2 { get; set; }

        Dictionary<string, CustomType> KeyValues3 { get; set; }

        List<string> ListOfSomething { get; set; }  

        /// <summary> not hidden propery </summary>
        decimal HidenDataTwo { get; set; }
    }
}