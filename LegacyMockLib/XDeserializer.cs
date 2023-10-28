using System.Xml.Linq;

namespace VXs.Xml {
    public class XConvert {
        public static T DeserializeObject<T>(XElement obj) => (T)DeserializeObject(obj, typeof(T));

        public static T DeserializeObject<T>(XElement obj, T sample) => (T)DeserializeObject(obj, typeof(T));

        public static object DeserializeObject(XElement obj, Type type) {
            if (typeof(string) == type) return obj.Value;
            if (type.IsPrimitive) {
                var parseMethod = type.GetMethod("TryParse", new [] { typeof(string), type.MakeByRefType() });
                if (null != parseMethod) {
                    var args = new object[] { obj.Value, null };
                    var b = (bool)parseMethod.Invoke(null, args);
                    if (b) return args[1];
                }
            }
            
            return default;
        }
    }
}