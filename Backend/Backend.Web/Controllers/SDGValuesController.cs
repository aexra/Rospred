using Backend.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Web.Controllers;

[Route("api/values")]
[ApiController]
public class SDGValuesController : ControllerBase
{
    private readonly SDGDBContext _context;

    public SDGValuesController(SDGDBContext context)
    {
        _context = context;
    }


    /// VALUE


    [HttpGet]
    public async Task<ActionResult<IEnumerable<SDGValue>>> GetValues()
    {
        return await _context.SDGValues.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SDGValue>> GetValue(int id)
    {
        var value = await _context.SDGValues.FindAsync(id);

        if (value == null)
        {
            return NotFound();
        }

        return value;
    }

    [HttpPost]
    public async Task<ActionResult<SDGValue>> PostValue(SDGValue value)
    {
        try
        {
            _context.SDGValues.Add(value);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}" + (ex.InnerException != null ? $"\n{ex.InnerException.Message}" : ""));
        }

        return CreatedAtAction(nameof(GetValue), new { id = value.Id }, value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutValue(int id, SDGValue value)
    {
        if (id != value.Id)
        {
            return BadRequest();
        }

        _context.Entry(value).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.SDGValues.Any(e => e.Id == id))
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteValue(int id)
    {
        var value = await _context.SDGValues.FindAsync(id);
        if (value == null)
        {
            return NotFound();
        }

        _context.SDGValues.Remove(value);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
