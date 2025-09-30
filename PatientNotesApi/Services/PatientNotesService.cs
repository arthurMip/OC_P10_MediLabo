using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using PatientNotesApi.Database;

namespace PatientNotesApi.Services;

public class PatientNotesService
{
    private readonly AppDbContext _context;

    public PatientNotesService(IMongoClient mongoClient)
    {
        var dbContextOptions =
            new DbContextOptionsBuilder<AppDbContext>().UseMongoDB(mongoClient, "PatientNoteStore");

        _context = new AppDbContext(dbContextOptions.Options);
    }

    public Task<List<PatientNote>> GetNotesByPatientIdAsync(int patientId)
    {
        return _context.PatientNotes
            .AsNoTracking()
            .Where(x => x.PatientId == patientId)
            .ToListAsync();
    }

    public Task CreateNoteAsync(PatientNote note)
    {
        _context.PatientNotes.Add(note);
        return _context.SaveChangesAsync();
    }

    public Task<List<PatientNote>> GetAllAsync()
    {
        return _context.PatientNotes
            .AsNoTracking()
            .ToListAsync();
    }
}
