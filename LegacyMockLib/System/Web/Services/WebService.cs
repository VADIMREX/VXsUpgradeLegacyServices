namespace System.Web.Services;

public class WebService : ComponentModel.MarshalByValueComponent
{
    [ComponentModel.Browsable(false)]
    public HttpApplicationState Application { get; }
    [ComponentModel.Browsable(false)]
    public HttpContext Context { get; }
    [ComponentModel.Browsable(false)]
    public HttpServerUtility Server { get; }
    [System.ComponentModel.Browsable(false)]
    public SessionState.HttpSessionState Session { get; }
    [ComponentModel.Browsable(false)]
    [Runtime.InteropServices.ComVisible(false)]
    public Protocols.SoapProtocolVersion SoapVersion { get; }
    [ComponentModel.Browsable(false)]
    public Security.Principal.IPrincipal User { get; }
}