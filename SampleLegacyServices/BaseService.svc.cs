using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using LegacyServices.Intefaces;
using LegacyServices.Models;

namespace LegacyServices.Svc {
    public class BaseService : IBaseService
    {
        public string GetSimpleData(int number, string suffix) {
            return string.Format("You entered: {0}-{1}", number, suffix);
        }

        public BaseContract GetBaseContract(BaseContract data) {
            if (null == data) return new BaseContract();
            if (null != data.NotProperty && data.NotProperty.Length > 0) data.HidenDataTwo = data.NotProperty[0];
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
            data.HidenDataOne = data.Message;
            data.BinaryData = Encoding.UTF8.GetBytes(data.HidenDataOne);
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            return data;
        }

        public CustomContract GetCustomContract(CustomContract data) {
            if (null == data) return new CustomContract();
            if (null != data.NotProperty && data.NotProperty.Length > 0) data.HidenDataTwo = data.NotProperty[0];
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
            data.HidenDataOne = data.Message;
            data.BinaryData = Encoding.UTF8.GetBytes(data.HidenDataOne);
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            return data;
        }

        public CustomType GetCustomData(CustomType data) {
            if (null == data) return new CustomType();
            if (null != data.NotProperty && data.NotProperty.Length > 0) data.HidenDataTwo = data.NotProperty[0];
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
            data.HidenDataOne = data.Message;
            data.BinaryData = Encoding.UTF8.GetBytes(data.HidenDataOne);
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            return data;
        }
    }
}