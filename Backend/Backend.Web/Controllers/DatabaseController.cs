using Backend.Web.Data;
using Backend.Web.Dtos.Tables;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("sdg/{id}")]
    public async Task<IActionResult> GetSDGById([FromRoute] int id)
    {
        var sdg = await _context.SDGs.FindAsync(id);

        if (sdg == null)
        {
            return NotFound($"SDG {id} not found in Database");
        }

        return Ok(sdg);
    }

    [HttpGet("table/{id}")]
    public async Task<IActionResult> GetTableById([FromRoute] int id)
    {
        var table = await _context.SDGTables.FindAsync(id);

        if (table == null)
        {
            return NotFound($"Table {id} not found in Database");
        }

        var dto = new TableDTO() { Id=table.Id, Name=table.Name };

        if (!string.IsNullOrEmpty(table.ValuesIds))
        {
            var ids = table.ValuesIds.Split(",").ToHashSet().Select(int.Parse);

            var values = _context.SDGValues.Where(v => ids.Contains(v.Id));

            foreach (var value in values)
            {
                dto.Headers.Add(value.Name);
                dto.Values.Add(value.Values);
            }
        }

        return Ok(dto);
    }

    [HttpPost("table")]
    public async Task<IActionResult> CreateTable([FromBody] TableDTO dto)
    {
        // Найдем указанный ЦУР
        var sdg = await _context.SDGs.FindAsync(dto.SDG);

        if (sdg == null)
        {
            return NotFound($"SDG {dto.SDG} not found in Database");
        }

        // Сперва создадим все значения
        List<int> valuesIds = [];
        try
        {
            for (var i = 0; i < dto.Headers.Count; i++)
            {
                var v = new SDGValue() { Name = dto.Headers[i], Values = dto.Values[i] };
                var result = await _context.SDGValues.AddAsync(v);
                await _context.SaveChangesAsync();
                valuesIds.Add(result.Entity.Id);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        // Создадим новую таблицу
        SDGTable resultEntity;
        try
        {
            var t = new SDGTable() { Name = dto.Name, ValuesIds = string.Join(",", valuesIds) };
            var result = await _context.SDGTables.AddAsync(t);
            await _context.SaveChangesAsync();
            resultEntity = result.Entity;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        // Добавим указатель на эту таблицу в указанный ЦУР
        if (string.IsNullOrEmpty(sdg.TableIds))
        {
            sdg.TableIds = resultEntity.Id.ToString();
            await _context.SaveChangesAsync();
        }
        else
        {
            var splitted = sdg.TableIds.Split(",").Select(int.Parse).ToHashSet();
            splitted.Add(resultEntity.Id);
            sdg.TableIds = string.Join(",", splitted);
            await _context.SaveChangesAsync();
        }

        return Ok(resultEntity);
    }
}
