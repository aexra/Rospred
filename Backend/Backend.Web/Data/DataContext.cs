using Backend.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Web.Data;

public class DataContext : IdentityDbContext<User>
{
    public string DbPath { get; }

    public static List<IdentityRole> AppRoles = [];

    public DataContext()
    {
        DbPath = "Database/LocalDB.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().HasData(AppRoles);
    }
}
