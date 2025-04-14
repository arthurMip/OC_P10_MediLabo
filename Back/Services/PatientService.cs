using Back.Data;

namespace Back.Services;

public class PatientService(AppDbContext context)
{
    private readonly AppDbContext _context = context;
}
