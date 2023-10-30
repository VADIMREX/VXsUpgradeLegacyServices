namespace LegacyMockLib;

using System.Reflection;
using System.ServiceModel;
using System.Web.Services;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;

using LegacyMockLib.Svc;
using LegacyMockLib.Asmx;

public static class LegacyAspExtension {
    public static IEndpointRouteBuilder UseLegacyAsp(this IEndpointRouteBuilder app, Assembly aspAssembly, string aspFolder) {
        var fileClasses = AspParser.ParseDirectory(aspFolder);
        foreach(var type in aspAssembly.GetTypes()) {
            if (type.IsInterface) continue;
            if (type.IsGenericParameter) continue;
            if (null == type.FullName) continue;
            var (serviceContract, serviceContractType) = type.GetCustomAttributeRecursevely<ServiceContractAttribute>();
            if (null != serviceContract) {
                new ServiceContractWrapper(type, serviceContract, serviceContractType!, app, fileClasses.ContainsKey(type.FullName) ? fileClasses[type.FullName].ToArray() : new string[0]) ;
            }
            var webServices = type.GetCustomAttributesRecursevely<WebServiceAttribute>().ToArray();
            if (0 != webServices.Length) {
                new WebServiceWrapper(type, webServices, fileClasses.ContainsKey(type.FullName) ? fileClasses[type.FullName].ToArray() : new string[0]);
            }
        }
        return app;
    }

    public static IEndpointRouteBuilder UseLegacyAsp(this IEndpointRouteBuilder app, string aspFolder)
        => UseLegacyAsp(app, Assembly.GetCallingAssembly(), aspFolder);

    public static IEndpointRouteBuilder UseLegacyAsp(this IEndpointRouteBuilder app, Assembly aspAssembly)
        => UseLegacyAsp(app, aspAssembly, AppDomain.CurrentDomain.BaseDirectory);

    public static IEndpointRouteBuilder UseLegacyAsp(this IEndpointRouteBuilder app)
        => UseLegacyAsp(app, Assembly.GetCallingAssembly(), AppDomain.CurrentDomain.BaseDirectory);
}