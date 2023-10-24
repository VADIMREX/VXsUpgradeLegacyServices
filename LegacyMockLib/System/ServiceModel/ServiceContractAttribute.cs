namespace System.ServiceModel;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=false, Inherited=false)]
public sealed class ServiceContractAttribute : Attribute {
    public Type CallbackContract { get; set; }
    public string ConfigurationName { get; set; }
    public string Name { get; set; }
    public string Namespace { get; set; }
    public SessionMode SessionMode { get; set; }

}
