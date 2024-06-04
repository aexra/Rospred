using Backend.Processor;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers;

/// <summary>
/// SDG - Sustainable Development Goal
/// </summary>

[Route("api/ml")]
[ApiController]
public class MachineLearningController : ControllerBase
{
    public MachineLearningController() { }

    [HttpGet]
    [Route("{sdgid}&{tableid}")]
    public async Task<IActionResult> PredictTable([FromRoute] int sdgid, [FromRoute] int tableid)
    {
        var result = IOProcess.Run("python ../Backend.ML/Testing/test.py Hello, web!").Output;

        return Ok(result);
    }
}

