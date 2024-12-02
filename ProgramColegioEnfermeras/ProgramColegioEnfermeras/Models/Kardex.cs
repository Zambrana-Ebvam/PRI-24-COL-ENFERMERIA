using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class Kardex
{
    public int Id { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public decimal? OxygenLevel { get; set; }

    public string Description { get; set; } = null!;

    public decimal? Temperature { get; set; }

    public short? RespiratoryRate { get; set; }

    public string? BloodPressure { get; set; }

    public string Derivation { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public byte State { get; set; }

    public int IdStudent { get; set; }

    public int? IdNurse { get; set; }

    public virtual Nurse? IdNurseNavigation { get; set; }

    public virtual Student IdStudentNavigation { get; set; } = null!;
}
