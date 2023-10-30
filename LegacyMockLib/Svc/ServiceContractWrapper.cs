namespace LegacyMockLib.Svc;

using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
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

    public string Namespace => serviceContract.Namespace ?? XmlNamespaces.org._tempuri;

    public XNamespace XmlNs => Namespace;

    public string Name => serviceContract.Name ?? type.Name;

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
            var baseMethod = serviceContractType == type ? 
                                mi : 
                                serviceContractType.GetMethod(
                                    mi.Name, 
                                    mi.GetParameters()
                                      .Select(x=>x.ParameterType).ToArray()
                                );

            if (null == baseMethod) continue;
            var attr = baseMethod.GetCustomAttribute<OperationContractAttribute>();
            if (null == attr) continue;
            var operation = new OperationContractWrapper(this, mi, attr, baseMethod, app, pathes);
            methods.Add($"\"{UrlUtils.Combine(Namespace, Name, operation.Name)}\"", operation);
        }
    }

    async Task ProcessMethod(HttpContext context) {
        var mediaType = "";
        var charset = "";
        if (null != context.Request.ContentType)
            foreach(var cType in context.Request
                                        .ContentType
                                        .Split(";", StringSplitOptions.TrimEntries & 
                                                    StringSplitOptions.RemoveEmptyEntries)) {
                var nameValue = cType.Split("=");
                switch (nameValue[0]) {
                    case "text/xml":
                    case "application/json":
                        mediaType = nameValue[0];
                        break;
                    case "charset":
                        charset = nameValue[1];
                        break;
                }
            }
        switch(mediaType) {
            case "text/xml":
                await ProcessSoapMethod(context, charset);
                return;
            case "application/json":
                await ProcessJsonMethod(context, charset);
                return;
            default:
                context.Response.StatusCode = 415;
                return;
        }
    }

    async Task ProcessSoapMethod(HttpContext context, string charset) {
        var soapAction = context.Request.Headers["SOAPAction"];

        if (0 == soapAction.Count) {
            return;
        }

        if (!methods.ContainsKey(soapAction!)) {
            // todo: Unknow method error
            return;
        }
        var method = methods[soapAction!];

        var xmlRequest = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, new CancellationToken());
        
        var xmlResult = method.Invoke(xmlRequest);

        context.Response.ContentType = "text/xml;charset=UTF-8";

        var s = xmlResult.ToString();
        
        await context.Response.WriteAsync(s);
    }

    async Task ProcessJsonMethod(HttpContext context, string charset) {
        await context.Response.WriteAsync("not implemented");
    }

    (string type, string content) DescriptionPage(string path) => ("text/html; charset=UTF-8", $$"""
        <html>
            <head>
                <title>{{type.Name}} Service</title>
            </head>
            <body>
                <h1>{{type.Name}} Service</h1>
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