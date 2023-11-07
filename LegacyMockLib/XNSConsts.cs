using System.Xml.Linq;
using LegacyMockLib.Svc;

namespace LegacyMockLib
{
    public static class XmlNs
    {
        /// <summary> XML Namespace - xmlns:e="http://schemas.xmlsoap.org/soap/envelope/" </summary>
        public static readonly XNamespace E = XmlNamespaces.org.xmlsoap.soap.envelope;
        /// <summary> XML Namespace - xmlns:i="http://www.w3.org/2001/XMLSchema-instance" </summary>
        public static readonly XNamespace I = XmlNamespaces.org.w3._2001.XMLSchema.instance;

        public static readonly XNamespace T = XmlNamespaces.org.tempuri;

        /// <summary> Generate DataContract XML Namespace - xmlns:a="http://schemas.datacontract.org/2004/07/:ns" </summary>
        public static XNamespace MakeA(string ns) => XmlNamespaces.org.datacontract._2004._07.Make(ns);

        /// <summary> xmlns:a="http://schemas.datacontract.org/2004/07/" </summary>
        public static readonly XNamespace A = XmlNamespaces.com.microsoft._2003._10.Serialization.Arrays;
    }

    internal static class XmlNamespaces
    {
        internal static class @org
        {
            internal const string _tempuri = "http://tempuri.org";
            internal static readonly XNamespace tempuri = _tempuri;

            internal static class _xmlsoap
            {
                internal const string _wsdl = "http://schemas.xmlsoap.org/wsdl";
            }

            internal static class @xmlsoap
            {
                public static class @soap
                {
                    internal const string _envelope = "http://schemas.xmlsoap.org/soap/envelope/";
                    internal static readonly XNamespace envelope = _envelope;
                }
            }

            internal static class w3
            {
                internal static class _2001
                {
                    internal static class XMLSchema
                    {
                        internal const string _instance = "http://www.w3.org/2001/XMLSchema-instance";
                        internal static readonly XNamespace instance = _instance;
                    }
                }
            }

            internal static class @datacontract
            {
                internal static class _2004
                {
                    internal static class _07
                    {
                        internal const string _template = "http://schemas.datacontract.org/2004/07/";
                        internal static XNamespace Make(string ns) => UrlUtils.Combine(_template, ns);
                    }
                }
            }
        }
        internal static class @com
        {
            internal static class @microsoft {
                internal static class @ws {
                    internal static class _2005 {
                        internal static class _05 {
                            internal static class @addressing {
                                internal const string _none = "http://schemas.microsoft.com/ws/2005/05/addressing/none";
                                internal static readonly XNamespace none = _none;
                            }
                        }
                    }
               }
               internal static class _2003 {
                    internal static class _10 {
                        internal static class Serialization {
                            internal const string _Arrays = "http://schemas.microsoft.com/2003/10/Serialization/Arrays";
                            internal static readonly XNamespace Arrays = _Arrays;
                        }
                    }
               }
            }
        }
    }
}
