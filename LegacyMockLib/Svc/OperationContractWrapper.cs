namespace LegacyMockLib.Svc;

using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public class OperationContractWrapper
{
    ServiceContractWrapper parent;
    MethodInfo method;
    OperationContractAttribute operationContract;
    MethodInfo operationContractMethod;

    public string Name => operationContract.Name ?? method.Name;

    public OperationContractWrapper(ServiceContractWrapper parent, MethodInfo method, OperationContractAttribute operationContract, MethodInfo operationContractMethod, IEndpointRouteBuilder app, string[] pathes)
    {
        this.parent = parent;
        this.method = method;
        this.operationContract = operationContract;
        this.operationContractMethod = operationContractMethod;

        foreach (var path in pathes)
        {
            app.MapGet($"{path}/{Name}", DescriptionPage);
        }
    }

    public XDocument Invoke(XDocument request)
    {


        return XDocument.Parse($$"""
            <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
              <s:Body>
                <GetDataUsingDataContractResponse xmlns="">
                  <GetDataUsingDataContractResult xmlns:a="http://schemas.datacontract.org/2004/07/ResultType" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
                    
                  </GetDataUsingDataContractResult>
                </GetDataUsingDataContractResponse>
              </s:Body>
            </s:Envelope>
        """);
    }

    public string DesriptionItem(string basePath) =>
        $$"""<li><a href="{{basePath}}/{{Name}}">{{Name}}</a></li>""";

    object SerializeObject(object? obj) {
        XNamespace i = "http://www.w3.org/2001/XMLSchema-instance";
        if (null == obj) return new XAttribute(i + "nil", "true");
        var type = obj.GetType();
        if (type.IsValueType) return obj;
        var attr = type.GetCustomAttribute<DataContractAttribute>();
        return new XElement("dummy", 123);
    }
    
    public string MakeSoapMessage((ParameterInfo parameterInfo, object? value)[] values)
    {
        XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
        XNamespace i = "http://www.w3.org/2001/XMLSchema-instance";
        XNamespace unknown = "http://schemas.datacontract.org/2004/07/" + method.ReturnType.FullName.Remove(method.ReturnType.FullName.Length - method.ReturnType.Name.Length);
        XNamespace service = parent.Namespace;
        XNamespace msAddressing = "http://schemas.microsoft.com/ws/2005/05/addressing/none";

        var doc = new XDocument(
            new XElement(
                soapenv + "Envelope",
                new XElement(soapenv + "Header"
                    // , new XElement(
                    //     msAddressing + "Action", 
                    //     new XAttribute(soapenv + "mustUnderstand", "1"),
                    //     Path.Combine(parent.Namespace, parent.Name, Name)
                    // )
                ),
                new XElement(soapenv + "Body",
                    new XElement(
                        service + Name,
                        values.Select(
                            pv => new XElement(
                                service + pv.parameterInfo.Name, 
                                SerializeObject(pv.value)
                            )
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
                                        "SOAPAction": '"{{Path.Combine(parent.Namespace, parent.Name, Name)}}"'
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
                </head>
                <body>
                    <h1>Operation contract {{method.Name}}</h1>
                    <hr>
                    <textarea id="requestBody" oninput="this.style.heigth='';this.style.height=this.scrollHeight + 'px'">
                    {{MakeSoapMessage(method.GetParameters()
                                            .Select(x => (x, Activator.CreateInstance(x.ParameterType)))
                                            .ToArray()
                                     )}}
                    </textarea>
                    <br>
                    <button onclick="send()">Send</button>
                    <br>
                    <textarea id="responseBody">
                    </textarea>
                </body>
            </html>
        """);
    }
}