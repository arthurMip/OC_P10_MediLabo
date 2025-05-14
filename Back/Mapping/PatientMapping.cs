using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PatientApi.Data.Entities;

namespace PatientApi.Mapping;

public static class ContractMapping
{
    public static Patient MapToPatient(this CreatePatientRequest request)
    {
        return new Patient
        {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            PostalAddress = request.PostalAddress ?? string.Empty,
            PhoneNumber = request.PhoneNumber ?? string.Empty,
        };
    }

    public static Patient MapToPatient(this UpdatePatientRequest request, int id)
    {
        return new Patient
        {
            Id = id,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            PostalAddress = request.PostalAddress ?? string.Empty,
            PhoneNumber = request.PhoneNumber ?? string.Empty,
        };
    }

    public static PatientResponse MapToResponse(this Patient patient)
    {
        return new PatientResponse
        {
            Id = patient.Id,
            Firstname = patient.Firstname,
            Lastname = patient.Lastname,
            BirthDate = patient.BirthDate,
            Gender = patient.Gender,
            PostalAddress = string.IsNullOrEmpty(patient.PostalAddress) ? null : patient.PostalAddress,
            PhoneNumber = string.IsNullOrEmpty(patient.PhoneNumber) ? null : patient.PhoneNumber,
        };
    }

    public static IEnumerable<PatientResponse> MapToResponse(this IEnumerable<Patient> patients)
    {
        return patients.Select(MapToResponse);
    }
}
