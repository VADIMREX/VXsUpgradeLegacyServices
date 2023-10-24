
namespace System.Web.Services;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class WebServiceAttribute : Attribute {
    public const string DefaultNamespace = "http://tempuri.org";
    public string Description { get; set; }
    public string Name { get; set; }
    public string Namespace { get; set; } = DefaultNamespace;
}