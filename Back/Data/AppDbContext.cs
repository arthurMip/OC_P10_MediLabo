using Microsoft.EntityFrameworkCore;
using PatientApi.Data.Entities;

namespace PatientApi.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Patient> Patients { get; set; }
}
