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

    public string MakeSoapMessage((ParameterInfo parameterInfo, object? value)[] values)
    {
        return $$""""
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
              <soapenv:Header/>
              <soapenv:Body>
                <tem:{{method.Name}}>
                  {{values.Aggregate(new StringBuilder(), 
                                    (x, y)=> null == y.value ? 
                                             x.Append($$"""<tem:{{y.parameterInfo.Name}} />""") :
                                             x.Append($$"""<tem:{{y.parameterInfo.Name}}>{{y.value}}</tem:{{y.parameterInfo.Name}}>""")         
                                    )}}
                </tem:{{method.Name}}>
              </soapenv:Body>
            </soapenv:Envelope>
        """";
    }

    public async Task DescriptionPage(HttpContext context)
    {
        var path = context.Request.Path.Value?.Replace($"/{method.Name}", "") ?? "";
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
                                        "SOAPAction": '"http://tempuri.org/IService1/{{method.Name}}"'
                                    },
                                    body: value
                                });
                                let respContent = await resp.text();
                                console.log(resp);
                            }
                            catch (e) {
                                console.log(e);
                            }
                        }
                    </script>
                </head>
                <body>
                    <h1>Operation contract {{method.Name}}</h1>
                    
                    <hr>
                    
                    <textarea id="requestBody" oninput="this.style.heigth='';this.style.height=this.scrollHeight + 'px'">
                    {{MakeSoapMessage(method.GetParameters()
                                            .Select(x => (x, x.ParameterType
                                                          .IsValueType ? Activator.CreateInstance(x.ParameterType) :
                                                                         null)
                                            ).ToArray()
                                     )}}
                    </textarea>
                    
                    <br>

                    <button onclick="send()">Send</button>

                    <br>

                    <div>

                    </div>
                    
                </body>
            </html>
        """);
    }
}