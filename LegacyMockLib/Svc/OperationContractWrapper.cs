namespace LegacyMockLib.Svc;

using System.Reflection;
using System.ServiceModel;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public class OperationContractWrapper
{
    MethodInfo method;
    OperationContractAttribute operationContract;
    MethodInfo operationContractMethod;

    public OperationContractWrapper(MethodInfo method, OperationContractAttribute operationContract, MethodInfo operationContractMethod, IEndpointRouteBuilder app, string[] pathes)
    {
        this.method = method;
        this.operationContract = operationContract;
        this.operationContractMethod = operationContractMethod;

        foreach (var path in pathes)
        {
            app.MapGet($"{path}/{method.Name}", DescriptionPage);
        }
    }

    public string DesriptionItem(string basePath)
    {
        var name = operationContract.Name ?? method.Name;
        return $$"""<li><a href="{{basePath}}/{{method.Name}}">{{name}}</a></li>""";
    }

    public string MakeSoapMessage(object?[] values)
    {
        return "";
    }

    public async Task DescriptionPage(HttpContext context)
    {
        var name = operationContract.Name ?? method.Name;
        var path = context.Request.Path;
        await context.Response.WriteAsync($$"""
            <html>
                <head>
                    <title>{{name}}</title>
                </head>
                <body>
                    <h1>Operation contract {{name}}</h1>
                    
                    <hr>
                    
                    <textarea id="body">
                    {{MakeSoapMessage(method.GetParameters()
                                            .Select(x => x.ParameterType
                                                          .IsValueType ? (object?) Activator.CreateInstance(x.ParameterType) : 
                                                                         (object?)null
                                            ).ToArray()
                                     )}}
                    </textarea>
                    
                    <br>

                    <button onclick="send()">Send</button>

                    <br>
                    
                </body>
            </html>
        """);
    }
}