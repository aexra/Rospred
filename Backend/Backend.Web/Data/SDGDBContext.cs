using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}
