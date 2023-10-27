using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using LegacyServices.Models;

namespace LegacyServices.Asmx {
    [WebService(Namespace = "http://legacy-services/asmx", Name = "legacy-asmx-service", Description = "Service with customized attributes")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // [System.Web.Script.Services.ScriptService]
    public class CustomService : WebService {
        [WebMethod(MessageName = "get-simple-data", Description = "Get simple data from two parameters")]
        public string GetSimpleData(int number, string suffix) {
            return string.Format("You entered: {0}-{1}", number, suffix);
        }

        [WebMethod(MessageName = "get-base-contract", Description = "Get simple data from base contract")]
        public BaseContract GetBaseContract(BaseContract data) {
            if (null == data) return new BaseContract();
            if (data.NotProperty.Length > 0) data.HidenDataTwo = data.NotProperty[0];
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
                                              .Append(data.Message?.Substring(0, 20) ?? "Х")
                                              .ToString();
            data.HidenDataOne = data.Message;
            data.BinaryData = Encoding.UTF8.GetBytes(data.HidenDataOne);
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            return data;
        }

        [WebMethod(MessageName = "get-custom-contract", Description = "Get simple data from customized contract")]
        public CustomContract GetCustomContract(CustomContract data) {
            if (null == data) return new CustomContract();
            if (data.NotProperty.Length > 0) data.HidenDataTwo = data.NotProperty[0];
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
                                              .Append(data.Message?.Substring(0, 20) ?? "Х")
                                              .ToString();
            data.HidenDataOne = data.Message;
            data.BinaryData = Encoding.UTF8.GetBytes(data.HidenDataOne);
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            return data;
        }

        [WebMethod(MessageName = "get-custom-data", Description = "Get data from custom type")]
        public CustomType GetCustomData(CustomType data) {
            if (null == data) return new CustomType();
            if (data.NotProperty.Length > 0) data.HidenDataTwo = data.NotProperty[0];
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
                                              .Append(data.Message?.Substring(0, 20) ?? "Х")
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