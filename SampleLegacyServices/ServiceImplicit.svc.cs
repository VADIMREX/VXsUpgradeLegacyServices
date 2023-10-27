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
    public class ServiceImplicit : IServiceImplicit
    {
        public string GetData(int value) {
            return string.Format("You entered: {0}", value);
        }

        public CompositeTypeImplicit GetData(CompositeTypeImplicit composite)
        {
            if (composite == null) {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue) {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public CustomType GetData(CustomType composite) {
            if (composite == null) {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue) {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}