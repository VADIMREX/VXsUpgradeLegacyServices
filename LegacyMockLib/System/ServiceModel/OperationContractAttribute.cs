namespace System.ServiceModel;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OperationContractAttribute : Attribute {
    public string? Action { get; set; }
    public bool AsyncPattern { get; set; }
    public bool IsInitiating { get; set; }
    public bool IsOneWay { get; set; }
    public bool IsTerminating { get; set; }
    public string? Name { get; set; }
    public string? ReplyAction { get; set; }
}
