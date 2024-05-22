using Microsoft.AspNetCore.Mvc;
using Models;

namespace proyecto1.Controllers;

[ApiController]
[Route("[controller]")]
public class IquilinoController : ControllerBase
{
    

    private readonly ILogger<IquilinoController> _logger;

    public IquilinoController(ILogger<IquilinoController> logger)
    {
        _logger = logger;
    }


    
}
