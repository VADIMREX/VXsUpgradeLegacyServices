namespace System.Web.Services;

[AttributeUsage(AttributeTargets.Method)]
public sealed class WebMethodAttribute : Attribute {
    public bool BufferResponse { get; set; }
    public int CacheDuration { get; set; }
    public string? Description { get; set; }
    public bool EnableSession { get; set; }
    public string? MessageName { get; set; }
    public EnterpriseServices.TransactionOption TransactionOption { get; set; }
    public WebMethodAttribute(){}
    public WebMethodAttribute (bool enableSession) => EnableSession = enableSession;
    public WebMethodAttribute (bool enableSession, EnterpriseServices.TransactionOption transactionOption) : this(enableSession) => TransactionOption = transactionOption;
    public WebMethodAttribute (bool enableSession, EnterpriseServices.TransactionOption transactionOption, int cacheDuration) : this(enableSession, transactionOption) => CacheDuration = cacheDuration;
    public WebMethodAttribute (bool enableSession, EnterpriseServices.TransactionOption transactionOption, int cacheDuration, bool bufferResponse) : this(enableSession, transactionOption, cacheDuration) => BufferResponse = bufferResponse;
}