using System.Text;
using LegacyServices.Models;
using Microsoft.AspNetCore.Mvc;

namespace UpgradeLegacyServices;

public record ListItem(int no, string name);

[ApiController]
[Route("[controller]")]
public class ModernApiController : ControllerBase {
    private readonly ILogger<ModernApiController> _logger;

    public ModernApiController(ILogger<ModernApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet("sample", Name = "Sample Get")]
    public IEnumerable<ListItem> Get() {
        _logger.LogInformation("Sample Get start");
        for(int i = 0; i < 5; i++)
            yield return new ListItem(i + 1, DateTime.Now.AddMonths(i).ToString("MMMM"));
        _logger.LogInformation("Sample Get end");
    }

    [HttpGet("get-simple-data", Name = "Get simple data")]
    public string GetData(int value) => $"You entered: {value}";

    [HttpPost("get-simple-data", Name = "Get simple data")]
    public string GetData(int value, [FromBody] string suffix) => $"You entered: {value}-{suffix}";

    [HttpPost("get-base-contract", Name = "Post base contract")]
    public BaseContract GetContract([FromBody]BaseContract data) {
            if (null == data) return new BaseContract();
            if (data.NotProperty.Length > 0) data.HidenDataTwo = data.NotProperty[0];
            data.Message = new StringBuilder().Append(data.IsTruth ? "not fake" : "fake")
                                              .AppendLine(" - ")
                                              .Append(data.HidenDataTwo)
                                              .AppendLine(" - ")
                                              .Append(null == data.SubContractOne ? "(T_T)" : "(￣﹃￣)")
                                              .AppendLine(" - ")
                                              .Append(null == data.SubContractTwo ? "😑" : "😌")
                                              .AppendLine(" - ")
                                              .Append(null == data.CustomData ? "-" : "+")
                                              .AppendLine(" - ")
                                              .Append(data.Message?.Substring(0, 20) ?? "Х")
                                              .ToString();
            data.HidenDataOne = data.Message;
            data.BinaryData = Encoding.UTF8.GetBytes(data.HidenDataOne);
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            return data;
        }

        [HttpPost("get-custom-contract", Name = "Post custom contract")]
        public CustomContract GetContract(CustomContract data) {
            if (null == data) return new CustomContract();
            if (data.NotProperty.Length > 0) data.HidenDataTwo = data.NotProperty[0];
            data.Message = new StringBuilder().Append(data.IsTruth ? "not fake" : "fake")
                                              .AppendLine(" - ")
                                              .Append(data.HidenDataTwo)
                                              .AppendLine(" - ")
                                              .Append(null == data.SubContractOne ? "(T_T)" : "(￣﹃￣)")
                                              .AppendLine(" - ")
                                              .Append(null == data.SubContractTwo ? "😑" : "😌")
                                              .AppendLine(" - ")
                                              .Append(null == data.CustomData ? "-" : "+")
                                              .AppendLine(" - ")
                                              .Append(data.Message?.Substring(0, 20) ?? "Х")
                                              .ToString();
            data.HidenDataOne = data.Message;
            data.BinaryData = Encoding.UTF8.GetBytes(data.HidenDataOne);
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            return data;
        }

        [HttpPost("get-custom-data", Name = "Post custom data")]
        public CustomType GetData(CustomType data) {
            if (null == data) return new CustomType();
            if (data.NotProperty.Length > 0) data.HidenDataTwo = data.NotProperty[0];
            data.Message = new StringBuilder().Append(data.IsTruth ? "not fake" : "fake")
                                              .AppendLine(" - ")
                                              .Append(data.HidenDataTwo)
                                              .AppendLine(" - ")
                                              .Append(null == data.SubContractOne ? "(T_T)" : "(￣﹃￣)")
                                              .AppendLine(" - ")
                                              .Append(null == data.SubContractTwo ? "😑" : "😌")
                                              .AppendLine(" - ")
                                              .Append(null == data.CustomData ? "-" : "+")
                                              .AppendLine(" - ")
                                              .Append(data.Message?.Substring(0, 20) ?? "Х")
                                              .ToString();
            data.HidenDataOne = data.Message;
            data.BinaryData = Encoding.UTF8.GetBytes(data.HidenDataOne);
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            return data;
        }
}