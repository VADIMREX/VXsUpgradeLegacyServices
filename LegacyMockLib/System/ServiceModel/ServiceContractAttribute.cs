namespace System.ServiceModel;

using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public sealed class ServiceContractAttribute : Attribute
{
    public Type CallbackContract { get; set; }
    public string ConfigurationName { get; set; }
    public string Name { get; set; }
    public string Namespace { get; set; } = "http://tempuri.org";
    public SessionMode SessionMode { get; set; }
}
