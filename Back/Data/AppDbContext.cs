using Back.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Back.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Patient> Patients { get; set; }
}
