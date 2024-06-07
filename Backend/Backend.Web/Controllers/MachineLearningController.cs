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
    [Route("{label}&{model}&{horizon}")]
    public async Task<IActionResult> PredictTable([FromRoute] string label, [FromRoute] string model, [FromRoute] int horizon)
    {
        var result = IOProcess.Run($"python ../Backend.ML/Scripts/predict.py {label} {model} {horizon}").Output;
        return Ok(result);
    }
}

