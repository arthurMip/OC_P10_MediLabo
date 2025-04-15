using Back.Data;
using Back.Data.Entities;
using Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Services;

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

    public async Task<Patient?> CreateAsync(CreatePatientRequest model)
    {
        var patient = new Patient
        {
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            PostalAddress = model.PostalAddress,
            PhoneNumber = model.PhoneNumber,
        };

        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();

        return patient;
    }

    public async Task<Patient?> UpdateAsync(int id, UpdatePatientRequest model)
    {
        var patient = await context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (patient == null)
        {
            return null;
        }

        patient.Firstname = model.Firstname ?? patient.Firstname;
        patient.Lastname = model.Lastname ?? patient.Lastname;
        patient.BirthDate = model.BirthDate ?? patient.BirthDate;
        patient.Gender = model.Gender ?? patient.Gender;
        patient.PostalAddress = model.PostalAddress ?? patient.PostalAddress;
        patient.PhoneNumber = model.PhoneNumber ?? patient.PhoneNumber;

        await context.SaveChangesAsync();

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
