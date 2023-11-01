using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.DataContracts;
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

        public static List<(MemberInfo member, DataMemberAttribute? dataMember)> GetMembers(Type type) {
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

        public static object? DeserializeObject(XElement[] objs, Type type) {
            if (0 == objs.Length) return null;

            if (!type.IsArray) {
                if (1 == objs.Length) return DeserializeObject(objs[0], type);

                // what we gonna do?
                return DeserializeObject(objs[0], type);
            }

            if (typeof(byte[]) == type) {
                if (0 == objs.Length) throw new NotImplementedException("binary without any element?");
                if ("true" == objs[0].Attribute(XmlNs.I + "nil")?.Value) return null;
                return Convert.FromBase64String(objs[0].Value);
            }

            var elemType = type.GetElementType()!;
            var arr = Array.CreateInstance(elemType, objs.Length);

            for(var i = 0; i < objs.Length; i++)
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
                if (0 == elem.Length) {
                    continue;
                }
                var value = DeserializeObject(elem, member.MemberInstanceType());
                member.SetValue(result, value);
            }
            return result;
        }

        public static object SerializeObject(XName name, object? obj) {
            // null
            if (null == obj) return new XElement(name, new XAttribute(XmlNs.I + "nil", "true"));
            var type = obj.GetType();
            // value type, what with structures?
            if (type.IsValueType) return new XElement(name, obj);
            // strings
            if (typeof(string) == type) return new XElement(name, obj);
            // byte[]
            if (typeof(byte[]) == type) return new XElement(name, Convert.ToBase64String((byte[])obj));

            
            List<object> result = new ();
            // arrays
            if (type.IsArray) {
                foreach(var a in (Array)obj)
                    result.Add(SerializeObject(name, a));
                return result.ToArray();
            }

            // objects
            #region DataContract and DataMember attributes specified logic. 
            #warning Todo to extract
            var valueNs = ValueNs(type);
            var targetList = GetMembers(type);
            #endregion

            foreach (var (member, dataMember) in targetList)
            {
                var value = member.GetValue(obj);
                result.Add(SerializeObject((valueNs ?? "") + (dataMember?.Name ?? member.Name), value));
            }

            return new XElement(name, result.ToArray());
        }
    }

    static class MemberInfoExtension {
        public static Func<object?, object?> GetValueMethod(this MemberInfo self) =>
            self is FieldInfo ? 
                ((FieldInfo)self).GetValue :
            self is PropertyInfo? 
                ((PropertyInfo)self).GetValue :
            throw new NotSupportedException();

        static Action<object?, object?> SetValueMethod(this MemberInfo self) =>
            self is FieldInfo ? 
                ((FieldInfo)self).SetValue :
            self is PropertyInfo? 
                ((PropertyInfo)self).SetValue :
            throw new NotSupportedException();

        public static Type MemberInstanceType(this MemberInfo self) =>
            self is FieldInfo ? 
                ((FieldInfo)self).FieldType :
            self is PropertyInfo? 
                ((PropertyInfo)self).PropertyType :
            throw new NotSupportedException();

        public static object? GetValue(this MemberInfo self, object? instance) =>
            GetValueMethod(self)(instance);

        public static void SetValue(this MemberInfo self, object? instance, object? value) =>
            SetValueMethod(self)(instance, value);
    }
}