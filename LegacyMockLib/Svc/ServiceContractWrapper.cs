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
            app.MapGet(path, ProcessInfo);
            app.MapPost(path, ProcessMethod);
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

    async Task ProcessMethod(HttpContext context) {
        if ("text/xml" != context.Request.ContentType) {
            context.Response.StatusCode = 415;
            return;
        }
        var soapAction = context.Request.Headers["SOAPAction"];
        await context.Response.WriteAsync($$"""
            <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
              <s:Body>
                <GetDataUsingDataContractResponse xmlns="http://tempuri.org/">
                  <GetDataUsingDataContractResult xmlns:a="http://schemas.datacontract.org/2004/07/ResultType" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
                    
                  </GetDataUsingDataContractResult>
                </GetDataUsingDataContract>
              </s:Body>
            </s:Envelope>
        """);
    }

    (string type, string content) DescriptionPage(string path) => ("text/html; charset=UTF-8", $$"""
        <html>
            <head>
                <title>{{serviceContract.Name}} Service</title>
            </head>
            <body>
                <h1>{{serviceContract.Name}} Service</h1>
                
                <hr>
                
                <span><a target="_blank" href="{{path}}?wsdl">{{path}}?wsdl</a></span>
                
                <br>

                <span><a target="_blank" href="{{path}}?singleWsdl">{{path}}?singleWsdl</a></span>

                <h2>Operation contracts</h2>

                <ul>

                {{methods.Aggregate(new StringBuilder(), (x, y) => x.AppendLine(y.Value.DesriptionItem(path)))}}

                </ul>

            </body>
        </html>
    """);

    async Task ProcessInfo(HttpContext context)
    {
        var path = context.Request.Path;
        var page = "";
        var type = "text/html; charset=UTF-8";
        if (!context.Request.QueryString.HasValue)
            (type, page) = DescriptionPage(path);
        else if (context.Request.Query.ContainsKey("wsdl")) 
            (type, page) = ("text/xml; charset=UTF-8", """
                    <?xml version="1.0" encoding="utf-8"?><wsdl:defenitions xmlns:wsdl="http://schemas.xmlsoap.org/wsdl" />
                """);
        else if (context.Request.Query.ContainsKey("singleWsdl"))
            (type, page) = ("text/xml; charset=UTF-8", """
                    <?xml version="1.0" encoding="utf-8"?><wsdl:defenitions xmlns:wsdl="http://schemas.xmlsoap.org/wsdl" />
                """);
        else if (context.Request.Query.ContainsKey("xsd"))
            (type, page) = ("text/xml; charset=UTF-8", """
                    <?xml version="1.0" encoding="utf-8"?><xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:tns="http://tempuri.org/" elementFromDefault="qualified" targetNamespace="http://tempuri.org/" />
                """);
        
        context.Response.ContentType = type;
        
        await context.Response.WriteAsync(page);
    }

}