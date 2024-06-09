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

        var dto = new TableDTO() { Id=table.Id, Name=table.Name, SDG = table.SDG };

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
    public async Task<IActionResult> CreateTable(TableDTO dto)
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
            var t = new SDGTable() { Name = dto.Name, ValuesIds = string.Join(",", valuesIds), SDG = dto.SDG };
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
        }
        else
        {
            var splitted = sdg.TableIds.Split(",").Select(int.Parse).ToHashSet();
            splitted.Add(resultEntity.Id);
            sdg.TableIds = string.Join(",", splitted);
            await _context.SaveChangesAsync();
        }

        await _context.SaveChangesAsync();
        return Ok(resultEntity);
    }

    [HttpDelete("table/{id}")]
    public async Task<IActionResult> DeleteTable(int id)
    {
        // Получим существующую таблицу по этому индексу
        var table = await _context.SDGTables.FindAsync(id);
        if (table == null) return NotFound($"Not found table {id} to replace");

        // Сохраним номер ЦУР и имя таблицы
        var sdgid = table.SDG;
        var name = table.Name;

        // Удалим все значения связанные со старой таблицей
        if (!string.IsNullOrEmpty(table.ValuesIds))
        {
            var ids = table.ValuesIds.Split(",").Select(int.Parse).ToHashSet();
            foreach (var vid in ids)
            {
                var value = await _context.SDGValues.FindAsync(vid);
                if (value == null) continue;
                _context.SDGValues.Remove(value);
            }
        }

        // Удалим указатель на эту таблицу в указанном ЦУРе
        var sdg = await _context.SDGs.FindAsync(sdgid);

        if (sdg == null)
        {
            return NotFound($"SDG {sdgid} not found in Database");
        }

        var splitted = sdg.TableIds.Split(",").Select(int.Parse).ToHashSet();
        splitted.Remove(id);
        sdg.TableIds = string.Join(",", splitted);

        // Удалим старую таблицу
        _context.SDGTables.Remove(table);

        await _context.SaveChangesAsync();

        return Ok($"Removed table {id} ({name}) successfully");
    }

    [HttpPut("table")]
    public async Task<IActionResult> PutTable(TableDTO dto)
    {
        // Получим существующую таблицу по этому индексу
        var table = await _context.SDGTables.FindAsync(dto.Id);
        if (table == null) return NotFound($"Not found table {dto.Id} to replace");

        // Удалим все значения связанные со старой таблицей
        if (!string.IsNullOrEmpty(table.ValuesIds))
        {
            var ids = table.ValuesIds.Split(",").Select(int.Parse).ToHashSet();
            foreach (var id in ids)
            {
                var value = await _context.SDGValues.FindAsync(id);
                if (value == null) continue;
                _context.SDGValues.Remove(value);
            }
        }

        // Удалим указатель на эту таблицу в указанном ЦУРе
        var sdg = await _context.SDGs.FindAsync(dto.SDG);

        if (sdg == null)
        {
            return NotFound($"SDG {dto.SDG} not found in Database");
        }

        var splitted = sdg.TableIds.Split(",").Select(int.Parse).ToHashSet();
        splitted.Remove(dto.SDG);
        sdg.TableIds = string.Join(",", splitted);

        // Удалим старую таблицу
        _context.SDGTables.Remove(table);

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
            var t = new SDGTable() { Name = dto.Name, ValuesIds = string.Join(",", valuesIds), SDG = dto.SDG };
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
        }
        else
        {
            splitted.Add(resultEntity.Id);
            sdg.TableIds = string.Join(",", splitted);
            await _context.SaveChangesAsync();
        }

        await _context.SaveChangesAsync();
        return Ok(resultEntity);
    }
}
