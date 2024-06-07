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
            if (!_context.SDGs.Any(e => e.Id == id))
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

    [HttpGet("table")]
    public async Task<ActionResult<IEnumerable<SDGTable>>> GetTables()
    {
        return await _context.SDGTables.ToListAsync();
    }

    [HttpGet("table/{id}")]
    public async Task<ActionResult<SDGTable>> GetTable(int id)
    {
        var sdg = await _context.SDGTables.FindAsync(id);

        if (sdg == null)
        {
            return NotFound();
        }

        return sdg;
    }

    [HttpPost("table")]
    public async Task<ActionResult<SDGTable>> PostTable(SDGTable table)
    {
        try
        {
            _context.SDGTables.Add(table);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}" + (ex.InnerException != null ? $"\n{ex.InnerException.Message}" : ""));
        }

        return CreatedAtAction(nameof(GetTable), new { id = table.Id }, table);
    }

    [HttpPut("table/{id}")]
    public async Task<IActionResult> PutTable(int id, SDGTable table)
    {
        if (id != table.Id)
        {
            return BadRequest();
        }

        _context.Entry(table).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.SDGTables.Any(e => e.Id == id))
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

    [HttpDelete("table/{id}")]
    public async Task<IActionResult> DeleteTable(int id)
    {
        var table = await _context.SDGTables.FindAsync(id);
        if (table == null)
        {
            return NotFound();
        }

        _context.SDGTables.Remove(table);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("value")]
    public async Task<ActionResult<IEnumerable<SDGValue>>> GetValues()
    {
        return await _context.SDGValues.ToListAsync();
    }

    [HttpGet("value/{id}")]
    public async Task<ActionResult<SDGValue>> GetValue(int id)
    {
        var value = await _context.SDGValues.FindAsync(id);

        if (value == null)
        {
            return NotFound();
        }

        return value;
    }

    [HttpPost("value")]
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

    [HttpPut("value/{id}")]
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

    [HttpDelete("value/{id}")]
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
