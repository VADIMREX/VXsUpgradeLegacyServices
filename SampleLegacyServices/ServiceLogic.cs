using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LegacyServices.Models;

namespace LegacyServices {
    public class ServiceLogic {
        static TField GetField<TImpl, TField>(TImpl model, string field) {
            return (TField)typeof(TImpl).GetField(field)
                                        .GetValue(model);
        }

        static void SetField<TImpl, TField>(TImpl model, string field, TField value) {
            typeof(TImpl).GetField(field)
                         .SetValue(model, value);
        }

        public static TImpl DoSomething<TImpl>(TImpl data) where TImpl : ISampleModel, new(){
            if (null == data) return new TImpl(); 
            if (null != GetField<TImpl, decimal[]>(data, "NotProperty") && GetField<TImpl, decimal[]>(data, "NotProperty").Length > 0) data.HidenDataTwo = GetField<TImpl, decimal[]>(data, "NotProperty")[0];
            data.Message = new StringBuilder().Append(data.IsTruth ? "not fake" : "fake")
                                              .AppendLine(" - ")
                                              .Append(data.HidenDataTwo)
                                              .AppendLine(" - ")
                                              .Append(null == data.SubContractOne ? "(T_T)" : "(￣﹃￣)")
                                              .AppendLine(" - ")
                                              .Append(null == data.SubContractTwo ? "😑" : "😌")
                                              .AppendLine(" - ")
                                              .Append(null == data.CustomData ? "-" : "+")
                                              .AppendLine(" - ")
                                              .Append(data.Message?.Substring(0, data.Message.Length < 20 ? data.Message.Length : 20) ?? "Х")
                                              .ToString();
            SetField(data, "HidenDataOne", data.Message);
            data.BinaryData = Encoding.UTF8.GetBytes(GetField<TImpl, string>(data, "HidenDataOne"));
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            return data;
        }
    }
}