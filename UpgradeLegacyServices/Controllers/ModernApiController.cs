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

    [HttpGet(Name = "Sample Get")]
    public IEnumerable<ListItem> Get() {
        _logger.LogInformation("Sample Get start");
        for(int i = 0; i < 5; i++)
            yield return new ListItem(i + 1, DateTime.Now.AddMonths(i).ToString("MMMM"));
        _logger.LogInformation("Sample Get end");
    }
}