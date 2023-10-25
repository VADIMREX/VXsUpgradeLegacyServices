namespace LegacyMockLib;

using System.Reflection;

public static class ReflectionExtension
{
    public static (T, Type)? GetCustomAttributeRecursevely<T>(this Type type) where T : Attribute
    {
        if (typeof(object) == type) return null;
        var attrs = type.GetCustomAttributes<T>(true).ToArray();
        if (1 == attrs.Length) return (attrs[0], type);
        if (1 < attrs.Length) throw new AmbiguousMatchException();

        if (null != type.BaseType)
        {
            var attr = type.BaseType.GetCustomAttributeRecursevely<T>();
            if (null != attr) return attr;
        }

        foreach (var ii in type.GetInterfaces())
        {
            var attr = ii.GetCustomAttributeRecursevely<T>();
            if (null != attr) return attr;
        }

        return null;
    }

    public static IEnumerable<(T, Type)> GetCustomAttributesRecursevely<T>(this Type type) where T : Attribute
    {
        if (typeof(object) == type) return new (T, Type)[0];

        var attrs = type.GetCustomAttributes<T>(true).Select(t => (t, type));

        if (null != type.BaseType)
            attrs.Union(type.BaseType.GetCustomAttributesRecursevely<T>());

        foreach (var ii in type.GetInterfaces())
            attrs.Union(ii.GetCustomAttributesRecursevely<T>());

        return attrs;
    }
}
