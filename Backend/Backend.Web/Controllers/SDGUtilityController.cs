using Backend.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;

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


    // Получает таблицы указанной ЦУР
    [HttpGet("sdg-tables/{id}")]
    public async Task<ActionResult<IEnumerable<SDGTable>>> GetSDGTables(int id)
    {
        var sdg = await _context.SDGs.FindAsync(id);

        if (sdg == null)
        {
            return NotFound();
        }

        if (string.IsNullOrEmpty(sdg.TableIds))
        {
            return NotFound($"No tables found for SDG {id} ({sdg.Name})");
        }

        try
        {
            var splitted = sdg.TableIds.Split(",");
            foreach (var tid in splitted)
            {
                if (!int.TryParse(tid, out var _))
                {
                    return StatusCode(500, "Fatal error parsing SDG tables references.");
                }
            }
            var tablesIds = splitted.ToHashSet().Select(int.Parse);
            var tables = _context.SDGTables.Where(t => tablesIds.Contains(t.Id));
            return Ok(tables);
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    // Получает значения указанной таблицы
    [HttpGet("table-values/{id}")]
    public async Task<ActionResult<IEnumerable<SDGTable>>> GetTableValues(int id)
    {
        var table = await _context.SDGTables.FindAsync(id);

        if (table == null)
        {
            return NotFound();
        }

        if (string.IsNullOrEmpty(table.ValuesIds))
        {
            return NotFound($"No values found for table {id} ({table.Name})");
        }

        try
        {
            var splitted = table.ValuesIds.Split(",");
            foreach (var tid in splitted)
            {
                if (!int.TryParse(tid, out var _))
                {
                    return StatusCode(500, "Fatal error parsing SDG tables references.");
                }
            }
            var valuesIds = splitted.ToHashSet().Select(int.Parse);
            var values = _context.SDGValues.Where(v => valuesIds.Contains(v.Id));
            return Ok(values);
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    // Добавляет указатель на таблицу в ЦУРе
    [HttpPut("sdg-tables/s={s}&t={t}")]
    public async Task<ActionResult> AddTableReference(int s, int t)
    {
        var sdg = await _context.SDGs.FindAsync(s);

        if (sdg == null)
        {
            return NotFound($"SDG {s} not found");
        }

        var table = await _context.SDGTables.FindAsync(t);

        if (table == null)
        {
            return NotFound($"Table {t} not found");
        }

        var tablesIds = sdg.TableIds.Split(",").ToHashSet().Select(int.Parse).ToList();

        tablesIds.Add(t);
        sdg.TableIds = string.Join(",", tablesIds);
        await _context.SaveChangesAsync();
        return Ok($"Added table {t} to SDG {s} successfully");
    }

    // Удаляет указатель на таблицу в ЦУРе
    [HttpDelete("sdg-tables/s={s}&t={t}")]
    public async Task<ActionResult> RemoveTableReference(int s, int t)
    {
        var sdg = await _context.SDGs.FindAsync(s);

        if (sdg == null)
        {
            return NotFound();
        }

        var tablesIds = sdg.TableIds.Split(",").ToHashSet().Select(int.Parse).ToList();

        if (tablesIds.Contains(t))
        {
            tablesIds.Remove(t);
            sdg.TableIds = string.Join(",", tablesIds);
            await _context.SaveChangesAsync();
            return Ok($"Removed table {t} from SDG {s} successfully");
        }

        return NotFound($"Table {t} not found in SDG {s}");
    }

    // Добавляет указатель на значение в таблице
    [HttpPut("table-values/t={t}&v={v}")]
    public async Task<ActionResult> AddValueReference(int t, int v)
    {
        var table = await _context.SDGTables.FindAsync(t);

        if (table == null)
        {
            return NotFound($"Table {t} not found");
        }

        var value = await _context.SDGValues.FindAsync(v);

        if (value == null)
        {
            return NotFound($"Value {v} not found");
        }

        var valuesIds = table.ValuesIds.Split(",").ToHashSet().Select(int.Parse).ToList();

        valuesIds.Add(t);
        table.ValuesIds = string.Join(",", valuesIds);
        await _context.SaveChangesAsync();
        return Ok($"Added value {v} to table {t} successfully");
    }

    // Удаляет указатель на значение в таблице
    [HttpDelete("table-values/t={t}&v={v}")]
    public async Task<ActionResult> RemoveValueReference(int t, int v)
    {
        var table = await _context.SDGTables.FindAsync(t);

        if (table == null)
        {
            return NotFound();
        }

        var valuesIds = table.ValuesIds.Split(",").ToHashSet().Select(int.Parse).ToList();

        if (valuesIds.Contains(v))
        {
            valuesIds.Remove(v);
            table.ValuesIds = string.Join(",", valuesIds);
            await _context.SaveChangesAsync();
            return Ok($"Removed value {v} from table {t} successfully");
        }

        return NotFound($"Value {v} not found in table {t}");
    }
}
