using Microsoft.EntityFrameworkCore;

namespace AppService1.Services;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Models.Table1> table1 { get; set; }
}
