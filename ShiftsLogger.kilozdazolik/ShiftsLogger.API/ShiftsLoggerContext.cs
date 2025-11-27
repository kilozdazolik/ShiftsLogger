using Microsoft.EntityFrameworkCore;
using ShiftsLogger.API.Models;

namespace ShiftsLogger.API;

public class ShiftsLoggerContext : DbContext
{
    public ShiftsLoggerContext(DbContextOptions<ShiftsLoggerContext> options) : base(options)
    {
    }
    
    public DbSet<Shift> Shifts { get; set; }
}