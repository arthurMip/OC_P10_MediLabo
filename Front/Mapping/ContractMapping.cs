using Front.Models.Responses;
using Front.Models.Enums;
using Front.ViewModels;

namespace Front.Mapping;

public static class ContractMapping
{
    private static PatientListItemViewModel MapToListItemViewModel(this PatientResponse patient)
    {
        return new PatientListItemViewModel
        { 
            Id = patient.Id,
            Firstname = patient.Firstname,
            Lastname = patient.Lastname
        };
    }

    public static IEnumerable<PatientListItemViewModel> MapToViewModel(this IEnumerable<PatientResponse> patients)
    {
        return patients.Select(MapToListItemViewModel);
    }

    public static PatientDetailViewModel MapToViewModel(this PatientResponse patient, IEnumerable<NoteResponse> notes)
    {
        return new PatientDetailViewModel
        {
            PatientId = patient.Id,
            Firstname = patient.Firstname,
            Lastname = patient.Lastname,
            Gender = patient.Gender,
            BirthDate = patient.BirthDate,
            PostalAddress = patient.PostalAddress,
            PhoneNumber = patient.PhoneNumber,
            Notes = notes.Select(MapToViewModel)
        };
    }

    public static NoteViewModel MapToViewModel(this NoteResponse note)
    {
        return new NoteViewModel
        {
            Note = note.Note,
            CreatedAt = note.CreatedAt,
        };
    }

}
