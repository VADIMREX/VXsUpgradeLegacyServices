namespace LegacyMockLib.Svc;

using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.DataContracts;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

using VXs.Xml;

public class UrlUtils {
    public static string Combine(params string[] args) => 
        null == args || 0 == args.Length ? 
            "" : 
        1 == args.Length ?
            args[0].TrimEnd('/') :
            args.Skip(1)
                .Aggregate(
                    new StringBuilder(args[0].TrimEnd('/')), 
                    (s, e) => 
                        s.Append('/')
                         .Append(e.Trim('/')))
                .ToString();
}

public class OperationContractWrapper
{
    ServiceContractWrapper parent;
    MethodInfo method;
    OperationContractAttribute operationContract;
    MethodInfo operationContractMethod;

    XNamespace? ResultXmlns;

    public string Name => operationContract.Name ?? method.Name;

    public OperationContractWrapper(ServiceContractWrapper parent, MethodInfo method, OperationContractAttribute operationContract, MethodInfo operationContractMethod, IEndpointRouteBuilder app, string[] pathes)
    {
        this.parent = parent;
        this.method = method;
        this.operationContract = operationContract;
        this.operationContractMethod = operationContractMethod;

        var (resAttr, _) = method.ReturnType.GetCustomAttributeRecursevely<DataContractAttribute>();

        ResultXmlns = XConvert.ValueNs(resAttr, method.ReturnType);

        foreach (var path in pathes)
        {
            app.MapGet($"{path}/{Name}", DescriptionPage);
        }
    }

    public XDocument Invoke(XDocument request)
    {
        if (null == request.Root) throw new Exception("request malformed)");

        var xbody = request.Root.Element(XmlNs.E + "Body");
        if (null == xbody) throw new Exception($"request malformed ({XmlNs.E + "Body"} not found)");

        var xargs = xbody.Element(parent.XmlNs + Name);
        if (null == xargs) throw new Exception($"request malformed ({parent.XmlNs + Name} not found)");

        var args = new List<object?>();

        foreach(var (pi, xa) in from pi in method.GetParameters()

                                let xa = xargs.Elements(parent.XmlNs + pi.Name!).ToArray()
                                select (pi, xa)) {         
            args.Add(XConvert.DeserializeObject(xa, pi.ParameterType));
        }

        var result = method.Invoke(parent.Instance, args.ToArray());

        return new XDocument(
            new XElement(
                XmlNs.E + "Envelope",
                new XElement(
                    XmlNs.E + "Body",
                    new XElement(
                        parent.XmlNs + $"{Name}Response",
                        XConvert.SerializeObject((ResultXmlns ?? "") + $"{Name}Result", result)
                    )
                )
            )
        );
    }

    public string DesriptionItem(string basePath) =>
        $$"""<li><a href="{{basePath}}/{{Name}}">{{Name}}</a></li>""";
    
    public string MakeSoapMessage((ParameterInfo parameterInfo, object? value)[] values)
    {
        var doc = new XDocument(
            new XElement(
                XmlNs.E + "Envelope",
                new XElement(XmlNs.E + "Header"
                    // , new XElement(
                    //     addressing.none + "Action", 
                    //     new XAttribute(XmlNs.E + "mustUnderstand", "1"),
                    //     UrlUtils.Combine(parent.Namespace, parent.Name, Name)
                    // )
                ),
                new XElement(XmlNs.E + "Body",
                    new XElement(
                        parent.XmlNs + Name,
                        values.Select(
                            pv => XConvert.SerializeObject(parent.XmlNs + pv.parameterInfo.Name!, pv.value)
                        )
                    )
                )
            )
        );

        return doc.ToString();
    }

    public async Task DescriptionPage(HttpContext context)
    {
        var path = context.Request.Path.Value?.Replace($"/{Name}", "") ?? "";
        await context.Response.WriteAsync($$"""
            <html>
                <head>
                    <title>{{method.Name}}</title>
                    <script>
                        async function send() {
                            let value = document.getElementById("requestBody").value;
                            try {
                                let resp = await fetch("{{path}}", {
                                    method: "POST",
                                    headers: {
                                        "Content-Type": "text/xml;charset=UTF-8",
                                        "SOAPAction": '"{{UrlUtils.Combine(parent.Namespace, parent.Name, Name)}}"'
                                    },
                                    body: value
                                });
                                let respContent = await resp.text();
                                document.getElementById("responseBody").value = respContent
                            }
                            catch (e) {
                                document.getElementById("responseBody").value = e;
                            }
                        }
                    </script>
                    <style>
                        textarea {
                            width: 100%;
                            min-height: 300px;
                        }
                        button {
                            border: 2px solid gray;
                            border-radius: 4px;
                            box-shadow: 0 1px 2px rgba(0,0,0,.1);
                            color: #333;
                            font-family: sans-serif;
                            font-size: 14px;
                            font-weight: 700;
                            padding: 5px 20px;
                        }
                        button.send {
                            background-color: steelblue;
                            border-color: steelblue;
                            color: #fff;
                            width: 100%;
                        }
                    </style>
                </head>
                <body>
                    <h1>{{method.Name}} Operation</h1>
                    <hr>
                    <textarea id="requestBody" oninput="this.style.heigth='';this.style.height=this.scrollHeight + 'px'">{{
                        MakeSoapMessage(method.GetParameters()
                                              .Select(x => (x, typeof(string) == x.ParameterType ? "" : Activator.CreateInstance(x.ParameterType)))
                                              .ToArray()
                    )}}</textarea>
                    <br>
                    <button class="send" onclick="send()">Send</button>
                    <br>
                    <textarea id="responseBody">
                    </textarea>
                </body>
            </html>
        """);
    }
}