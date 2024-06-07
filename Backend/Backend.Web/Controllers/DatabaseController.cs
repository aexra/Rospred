using Backend.Web.Data;
using Backend.Web.Dtos.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Web.Controllers;

[Route("api/db")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly SDGDBContext _context;

    public DatabaseController(SDGDBContext context)
    {
        _context = context;
    }

    [HttpGet("sdg")]
    public async Task<ActionResult<IEnumerable<SDG>>> GetSDGs()
    {
        return await _context.SDGs.ToListAsync();
    }

    [HttpGet("sdg/{id}")]
    public async Task<ActionResult<SDG>> GetSDG(int id)
    {
        var sdg = await _context.SDGs.FindAsync(id);

        if (sdg == null)
        {
            return NotFound();
        }

        return sdg;
    }

    [HttpPost("sdg")]
    public async Task<ActionResult<SDG>> PostSDG(SDG sdg)
    {
        try
        {
            _context.SDGs.Add(sdg);
            await _context.SaveChangesAsync();
        } catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}" + (ex.InnerException != null? $"\n{ex.InnerException.Message}" : ""));
        }

        return CreatedAtAction(nameof(GetSDG), new { id = sdg.Id }, sdg);
    }

    [HttpPut("sdg/{id}")]
    public async Task<IActionResult> PutSDG(int id, SDG sdg)
    {
        if (id != sdg.Id)
        {
            return BadRequest();
        }

        _context.Entry(sdg).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SDGExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("sdg/{id}")]
    public async Task<IActionResult> DeleteSDG(int id)
    {
        var sdg = await _context.SDGs.FindAsync(id);
        if (sdg == null)
        {
            return NotFound();
        }

        _context.SDGs.Remove(sdg);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SDGExists(int id)
    {
        return _context.SDGs.Any(e => e.Id == id);
    }
}
