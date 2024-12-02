using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Names { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Gender { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string? Code { get; set; }

    public string Tutor { get; set; } = null!;

    public string TutorCellphone { get; set; } = null!;

    public string? BloodType { get; set; }

    public string Allergy { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public byte State { get; set; }

    public byte IdEstablishment { get; set; }

    public virtual Establishment IdEstablishmentNavigation { get; set; } = null!;

    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
