namespace System.Web.Services;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public sealed class WebServiceBindingAttribute : Attribute
{
    public WsiProfiles ConformsTo { get; set; }
    public bool EmitConformanceClaims { get; set; }
    public string Location { get; set; }
    public string Name { get; set; }
    public string Namespace { get; set; }
    public WebServiceBindingAttribute() {}
    public WebServiceBindingAttribute (string name) { 
        Name = name; 
    }
    public WebServiceBindingAttribute (string name, string ns) : this(name) {
        Namespace = ns;
    }
    public WebServiceBindingAttribute (string name, string ns, string location) : this(name, ns) {
        Location = location;
    }
}