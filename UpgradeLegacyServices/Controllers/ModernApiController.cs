using System.Text;
using LegacyServices;
using LegacyServices.Models;
using Microsoft.AspNetCore.Mvc;

namespace UpgradeLegacyServices;

public record ListItem(int no, string name);

[ApiController]
[Route("[controller]")]
public class ModernApiController : ControllerBase
{
    private readonly ILogger<ModernApiController> _logger;

    public ModernApiController(ILogger<ModernApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet("sample", Name = "Sample Get")]
    public IEnumerable<ListItem> Get()
    {
        _logger.LogInformation("Sample Get start");
        for (int i = 0; i < 5; i++)
            yield return new ListItem(i + 1, DateTime.Now.AddMonths(i).ToString("MMMM"));
        _logger.LogInformation("Sample Get end");
    }

    [HttpGet("get-simple-data", Name = "Get simple data")]
    public string GetData(int value) => $"You entered: {value}";

    [HttpPost("get-simple-data", Name = "Get simple data")]
    public string GetData(int value, [FromBody] string suffix) => $"You entered: {value}-{suffix}";

    [HttpPost("get-base-contract", Name = "Post base contract")]
    public BaseContract GetContract([FromBody] BaseContract data)
    {
        return ServiceLogic.DoSomething(data);
    }

    [HttpPost("get-custom-contract", Name = "Post custom contract")]
    public CustomContract GetContract(CustomContract data)
    {
        return ServiceLogic.DoSomething(data);
    }

    [HttpPost("get-custom-data", Name = "Post custom data")]
    public CustomType GetData(CustomType data)
    {
        return ServiceLogic.DoSomething(data);
    }
}