using PatientApi.Data;
using PatientApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Contracts.Requests;

namespace PatientApi.Services;

public class PatientService(AppDbContext context)
{
    private readonly AppDbContext context = context;

    public Task<List<Patient>> GetAllAsync()
    {
        return context.Patients
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<Patient?> GetByIdAsync(int id)
    {
        return context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> CreateAsync(Patient patient)
    {
        await context.Patients.AddAsync(patient);

        var result = await context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<Patient?> UpdateAsync(Patient patient)
    {
        if (!await ExistAsync(patient.Id))
        {
            return null;
        }

        context.Patients.Update(patient);

        bool updated = await context.SaveChangesAsync() > 0;
        if (!updated)
        {
            return null;
        }

        return patient;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (patient == null)
        {
            return false;
        }

        context.Patients.Remove(patient);

        return await context.SaveChangesAsync() > 0;
    }


    public Task<bool> ExistAsync(int id)
    {
        return context.Patients
            .AsNoTracking()
            .AnyAsync(p => p.Id == id);
    }
}
