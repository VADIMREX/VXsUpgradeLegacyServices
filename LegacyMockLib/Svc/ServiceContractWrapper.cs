namespace LegacyMockLib.Svc;

using System.Reflection;
using System.ServiceModel;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public class ServiceContractWrapper
{
    Type type;
    ServiceContractAttribute serviceContract;
    Type serviceContractType;
    ConstructorInfo constructor;
    Dictionary<string, OperationContractWrapper> methods = new();

    object? instance;

    public object Instance => instance ?? (instance = constructor.Invoke(new object[0]));

    public ServiceContractWrapper(Type type, ServiceContractAttribute serviceContract, Type serviceContractType, IEndpointRouteBuilder app, string[] pathes)
    {
        this.type = type;
        this.serviceContract = serviceContract;
        this.serviceContractType = serviceContractType;
        constructor = type.GetConstructor(new Type[0]) ?? throw new ArgumentException("");

        foreach (var path in pathes)
        {
            app.MapGet(path, DescriptionPage);
        }

        foreach (var mi in type.GetMethods())
        {
            var baseMethod = serviceContractType == type ? mi : serviceContractType.GetMethod(mi.Name);
            if (null == baseMethod) continue;
            var attr = baseMethod.GetCustomAttribute<OperationContractAttribute>();
            if (null == attr) continue;
            methods.Add(mi.Name, new OperationContractWrapper(mi, attr, baseMethod, app, pathes));
        }
    }

    async Task DescriptionPage(HttpContext context)
    {
        var name = serviceContract.Name ?? type.Name;
        var path = context.Request.Path;
        await context.Response.WriteAsync($$"""
            <html>
                <head>
                    <title>{{name}}</title>
                </head>
                <body>
                    <h1>{{name}}</h1>
                    
                    <hr>
                    
                    <a target="_blank" href="{{path}}/wsdl">Get WSDL</a>

                    <h2>Operation contracts</h2>

                    <ul>

                    {{methods.Aggregate(new StringBuilder(), (x, y) => x.AppendLine(y.Value.DesriptionItem(path)))}}

                    </ul>

                </body>
            </html>
        """);
    }

}