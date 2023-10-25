namespace LegacyMockLib;

using System.Reflection;
using Microsoft.AspNetCore.Routing;

public static class LegacyAspExtension {
    public static IEndpointRouteBuilder UseLegacyAsp(this IEndpointRouteBuilder app, Assembly aspAssembly, string aspFolder) {
        var parser = new AspParser(aspAssembly, aspFolder);
        return app;
    }

    public static IEndpointRouteBuilder UseLegacyAsp(this IEndpointRouteBuilder app, string aspFolder)
        => UseLegacyAsp(app, Assembly.GetCallingAssembly(), aspFolder);

    public static IEndpointRouteBuilder UseLegacyAsp(this IEndpointRouteBuilder app, Assembly aspAssembly)
        => UseLegacyAsp(app, aspAssembly, AppDomain.CurrentDomain.BaseDirectory);

    public static IEndpointRouteBuilder UseLegacyAsp(this IEndpointRouteBuilder app)
        => UseLegacyAsp(app, Assembly.GetCallingAssembly(), AppDomain.CurrentDomain.BaseDirectory);
}