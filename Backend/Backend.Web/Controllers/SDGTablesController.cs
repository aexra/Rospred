using Backend.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Web.Controllers;

[Route("api/tables")]
[ApiController]
public class SDGTablesController : ControllerBase
{
    private readonly SDGDBContext _context;

    public SDGTablesController(SDGDBContext context)
    {
        _context = context;
    }


    /// TABLE


    [HttpGet]
    public async Task<ActionResult<IEnumerable<SDGTable>>> GetTables()
    {
        return await _context.SDGTables.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SDGTable>> GetTable(int id)
    {
        var sdg = await _context.SDGTables.FindAsync(id);

        if (sdg == null)
        {
            return NotFound();
        }

        return sdg;
    }

    [HttpPost]
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

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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
}

