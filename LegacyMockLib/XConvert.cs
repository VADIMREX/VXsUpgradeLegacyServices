using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.DataContracts;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using LegacyMockLib;
using LegacyMockLib.Svc;

namespace VXs.Xml
{
    public class XConvert
    {

        public static XNamespace? ValueNs(Type valueType) =>
            ValueNs(valueType.GetCustomAttributeRecursevely<DataContractAttribute>().attribute, valueType);

        public static XNamespace? ValueNs(DataContractAttribute? dataContract, Type valueType) =>
            null == dataContract?.Namespace ?
            null == valueType.FullName ?
                null :
                XmlNs.MakeA(valueType.FullName.Remove(valueType.FullName.Length - valueType.Name.Length)) :
            dataContract.Namespace;

        public static List<(MemberInfo member, DataMemberAttribute? dataMember)> GetMembers(Type type)
        {
            List<(MemberInfo, DataMemberAttribute?)> dataMembers = new();
            List<(MemberInfo, DataMemberAttribute?)> members = new();

            foreach (var mi in type.GetMembers())
            {
                if (!(MemberTypes.Field | MemberTypes.Property).HasFlag(mi.MemberType)) continue;
                var dataMember = mi.GetCustomAttribute<DataMemberAttribute>();
                (null == dataMember ? members : dataMembers).Add((mi, dataMember));
            }

            return 0 == dataMembers.Count ? members : dataMembers;
        }

        public static T DeserializeObject<T>(XElement[] objs) => (T)DeserializeObject(objs, typeof(T))!;

        public static T DeserializeObject<T>(XElement[] objs, T sample) => (T)DeserializeObject(objs, typeof(T))!;

        public static object? DeserializeObject(XElement[] objs, Type type)
        {
            if (0 == objs.Length) return null;

            if (!type.IsArray)
            {
                if (1 == objs.Length) return DeserializeObject(objs[0], type);

                // what we gonna do?
                return DeserializeObject(objs[0], type);
            }

            if (typeof(byte[]) == type)
            {
                if (0 == objs.Length) throw new NotImplementedException("binary without any element?");
                if ("true" == objs[0].Attribute(XmlNs.I + "nil")?.Value) return null;
                return Convert.FromBase64String(objs[0].Value);
            }

            var elemType = type.GetElementType()!;
            var arr = Array.CreateInstance(elemType, objs.Length);

            for (var i = 0; i < objs.Length; i++)
                arr.SetValue(DeserializeObject(objs[0], elemType), i);

            return arr;
        }

        public static object? DeserializeObject(XElement obj, Type type)
        {
            if ("true" == obj.Attribute(XmlNs.I + "nil")?.Value) return null;

            if (typeof(string) == type) return obj.Value;

            var parseMethod = type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() });
            if (null != parseMethod)
            {
                var args = new object?[] { obj.Value, null };
                var b = (bool)parseMethod.Invoke(null, args)!;
                if (b) return args[1];
                throw new NotImplementedException("TryParse returns false");
            }

            #region DataContract and DataMember attributes specified logic. 
#warning Todo to extract
            var valueNs = ValueNs(type);
            var targetList = GetMembers(type);
            #endregion

            /* may be for feature
            var targetList = from mi in type.GetMembers()
            where (MemberTypes.Field | MemberTypes.Property).HasFlag(mi.MemberType)
            let datamember = mi.GetCustomAttribute<DataMemberAttribute>()
            let elem = obj.Element(((XNamespace)valueNs) + (datamember?.Name ?? mi.Name))
            select DeserializeObject((mi, datamember, elem));
            */

            var result = Activator.CreateInstance(type);
            foreach (var (member, dataMember) in targetList)
            {
                var elem = obj.Elements((valueNs ?? "") + (dataMember?.Name ?? member.Name)).ToArray();
                if (0 == elem.Length)
                {
                    continue;
                }
                var value = DeserializeObject(elem, member.MemberInstanceType());
                member.SetValue(result, value);
            }
            return result;
        }

        public static string ReservedTypeName(Type type)
        {
            if (typeof(bool) == type) return "bool";
            if (typeof(char) == type) return "char";
            if (typeof(string) == type) return "string";

            if (typeof(float) == type) return "float";
            if (typeof(double) == type) return "double";
            if (typeof(decimal) == type) return "decimal";

            if (typeof(byte) == type) return "byte";
            if (typeof(sbyte) == type) return "sbyte";
            if (typeof(short) == type) return "short";
            if (typeof(ushort) == type) return "ushort";
            if (typeof(int) == type) return "int";
            if (typeof(uint) == type) return "uint";
            if (typeof(long) == type) return "long";
            if (typeof(ulong) == type) return "ulong";

            return type.Name;
        }

        public static XElement SerializeObject(XName name, object? obj)
        {
            // null
            if (null == obj) return new XElement(name, new XAttribute(XmlNs.I + "nil", "true"));
            var type = obj.GetType();
            // value type, what with structures?
            if (type.IsValueType) return new XElement(name, obj);
            // strings
            if (typeof(string) == type) return new XElement(name, obj);
            // byte[]
            if (typeof(byte[]) == type) return new XElement(name, Convert.ToBase64String((byte[])obj));
            // DateTime
            if (typeof(DateTime) == type) return new XElement(name, ((DateTime)obj).ToString("o"));

            // arrays
            if (type.IsArray)
            {
                var baseType = type.GetElementType()!;
                var (dataContract, _) = baseType.GetCustomAttributeRecursevely<DataContractAttribute>();

                var arr = (Array)obj;
                var result = new object[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    var value = arr.GetValue(i);
                    result[i] = SerializeObject(XmlNs.A + (dataContract?.Name ?? ReservedTypeName(baseType)), value);
                }
                return new XElement(name, result);
            }
            else if (type.IsGenericType)
            {
                // List<>
                var genericType = type.GetGenericTypeDefinition();
                if (typeof(List<>) == genericType)
                {
                    var baseType = type.GenericTypeArguments[0]!;
                    var (dataContract, _) = baseType.GetCustomAttributeRecursevely<DataContractAttribute>();

                    var lst = (IList)obj;
                    var result = new object[lst.Count];
                    for (int i = 0; i < lst.Count; i++)
                    {
                        var value = lst[i];
                        result[i] = SerializeObject(XmlNs.A + (dataContract?.Name ?? ReservedTypeName(baseType)), value);
                    }
                    return new XElement(name, result);
                }
                // Dictionary
                else if (typeof(Dictionary<,>) == genericType)
                {
                    var keyType = type.GenericTypeArguments[0]!;
                    var valueType = type.GenericTypeArguments[1]!;
                    var (keyDataContract, _) = keyType.GetCustomAttributeRecursevely<DataContractAttribute>();
                    var (valueDataContract, _) = valueType.GetCustomAttributeRecursevely<DataContractAttribute>();

                    var dic = (IDictionary)obj;
                    var result = new List<object>();
                    foreach (DictionaryEntry kv in dic)
                    {
                        result.Add(new XElement(XmlNs.A + $"KeyValueOf{keyDataContract?.Name ?? ReservedTypeName(keyType)}{valueDataContract?.Name ?? ReservedTypeName(valueType)}",
                            SerializeObject(XmlNs.A + "Key", kv.Key),
                            SerializeObject(XmlNs.A + "Value", kv.Value)
                        ));
                    }
                    return new XElement(name, result);
                }
            }

            {
                // objects
                #region DataContract and DataMember attributes specified logic. 
#warning Todo to extract
                var valueNs = ValueNs(type);
                var targetList = GetMembers(type);
                #endregion

                var result = new List<object>();
                foreach (var (member, dataMember) in targetList)
                {
                    var value = member.GetValue(obj);
                    result.Add(SerializeObject((valueNs ?? "") + (dataMember?.Name ?? member.Name), value));
                }

                return new XElement(name, result.ToArray());
            }
        }
    }

    static class MemberInfoExtension
    {
        public static Func<object?, object?> GetValueMethod(this MemberInfo self) =>
            self is FieldInfo ?
                ((FieldInfo)self).GetValue :
            self is PropertyInfo ?
                ((PropertyInfo)self).GetValue :
            throw new NotSupportedException();

        static Action<object?, object?> SetValueMethod(this MemberInfo self) =>
            self is FieldInfo ?
                ((FieldInfo)self).SetValue :
            self is PropertyInfo ?
                ((PropertyInfo)self).SetValue :
            throw new NotSupportedException();

        public static Type MemberInstanceType(this MemberInfo self) =>
            self is FieldInfo ?
                ((FieldInfo)self).FieldType :
            self is PropertyInfo ?
                ((PropertyInfo)self).PropertyType :
            throw new NotSupportedException();

        public static object? GetValue(this MemberInfo self, object? instance) =>
            GetValueMethod(self)(instance);

        public static void SetValue(this MemberInfo self, object? instance, object? value) =>
            SetValueMethod(self)(instance, value);
    }
}