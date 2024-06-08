using Backend.Processor;
using Backend.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Backend.Web.Controllers;

/// <summary>
/// SDG - Sustainable Development Goal
/// </summary>

[Route("api/ml")]
[ApiController]
public class MachineLearningController : ControllerBase
{
    private readonly SDGDBContext _context;

    public MachineLearningController(SDGDBContext context) 
    { 
        _context = context;
    }

    [HttpGet]
    [Route("predict/v={vid}&m={model}&h={horizon}")]
    public async Task<IActionResult> PredictTable([FromRoute] string vid, [FromRoute] string model, [FromRoute] int horizon)
    {
        var result = IOProcess.Run($"python ../Backend.ML/Scripts/predict.py {vid} {model} {horizon}").Output;
        return Ok(result);
    }

    [HttpGet]
    [Route("learn")]
    public async Task<IActionResult> LearnModels()
    {
        var columns = (await _context.SDGValues.ToListAsync()).Select(v => $"{v.Id}&{v.Values}");
        var str = string.Join("|", columns);

        var result = IOProcess.Run($"python ../Backend.ML/Scripts/learn.py \"{str}\"").Output;

        return Ok(result);
    }
}

