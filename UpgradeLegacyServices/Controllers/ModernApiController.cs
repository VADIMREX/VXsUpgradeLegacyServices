using Microsoft.AspNetCore.Mvc;

namespace UpgradeLegacyServices;

[ApiController]
[Route("[controller]")]
public class ModernApiController : ControllerBase {
    private readonly ILogger<ModernApiController> _logger;

    public ModernApiController(ILogger<ModernApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Sample Get")]
    public IEnumerable<(int no, string name)> Get() {
        _logger.LogInformation("Sample Get start");
        foreach(var data in Enumerable.Range(1, 5).Select(index => (index, DateTime.Now.AddMonths(index).ToString("MMMM")))) 
            yield return data;
        _logger.LogInformation("Sample Get end");
    }
}