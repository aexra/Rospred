using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Web.Data;

public class SDGDBContext : DbContext
{
    public DbSet<SDG> SDGs { get; set; }
    public DbSet<SDGTable> SDGTables { get; set; }
    public DbSet<SDGValue> SDGValues { get; set; }

    public string DbPath { get; }

    public SDGDBContext()
    {
        DbPath = "Database/SDGDB.db";
    }

    public async Task CreateTableAsync(string tableName, Dictionary<string, string> columns)
    {
        if (string.IsNullOrWhiteSpace(tableName) || columns == null || !columns.Any())
            throw new ArgumentException("Invalid table name or columns definition.");

        // Формирование SQL-запроса для создания таблицы
        var columnsDefinition = string.Join(", ", columns.Select(c => $"{c.Key} {c.Value}"));
        var createTableSql = $"CREATE TABLE {tableName} ({columnsDefinition});";

        await Database.ExecuteSqlRawAsync(createTableSql);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}
