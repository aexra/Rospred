using Backend.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Web.Controllers;

[Route("api/utils")]
[ApiController]
public class SDGUtilityController : ControllerBase
{
    private readonly SDGDBContext _context;

    public SDGUtilityController(SDGDBContext context)
    {
        _context = context;
    }


    /// UTILITY


    [HttpGet("tables/{id}")]
    public async Task<ActionResult<IEnumerable<SDGTable>>> GetSDGTables(int id)
    {
        var sdg = await _context.SDGs.FindAsync(id);

        if (sdg == null)
        {
            return NotFound();
        }

        var tablesIds = sdg.TableIds.Split(",").ToHashSet().Select(int.Parse);

        var tables = _context.SDGTables.Where(t => tablesIds.Contains(t.Id));

        return Ok(tables);
    }
}
