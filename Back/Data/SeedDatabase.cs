using PatientApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using PatientApi.Models.Enums;

namespace PatientApi.Data;

public static class SeedDatabase
{
    public static Action<DbContext, bool> SeedPatients = (context, _) =>
    {
        if (context.Set<Patient>().Any())
        {
            return;
        }

        var patients = new List<Patient>()
        {
            new() {
                Firstname = "Test",
                Lastname = "TestNone",
                BirthDate = new DateOnly(1966, 12, 31),
                Gender = Gender.Female,
                PostalAddress = "1 Brookside St",
                PhoneNumber = "100-222-3333"
            },
            new()
            {
                Firstname = "Test",
                Lastname = "TestBorderline",
                BirthDate = new DateOnly(1945, 06, 24),
                Gender = Gender.Male,
                PostalAddress = "2 High St",
                PhoneNumber = "200-333-4444"
            },
            new()
            {
                Firstname = "Test",
                Lastname = "TestInDanger",
                BirthDate = new DateOnly(2004, 06, 18),
                Gender = Gender.Male,
                PostalAddress = "3 Club Road",
                PhoneNumber = "300-444-5555"
            },
            new()
            {
                Firstname = "Test",
                Lastname = "TestEarlyOnset",
                BirthDate = new DateOnly(2002, 06, 28),
                Gender = Gender.Female,
                PostalAddress = "4 Valley Dr",
                PhoneNumber = "400-555-6666"
            }
        };

        context.Set<Patient>().AddRange(patients);
        context.SaveChanges();
    };
}
