using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class Prescription
{
    public int Id { get; set; }

    public int IdNurse { get; set; }

    public int IdStudent { get; set; }

    public virtual Nurse IdNurseNavigation { get; set; } = null!;

    public virtual Student IdStudentNavigation { get; set; } = null!;

    public virtual ICollection<PrecriptionDetail> PrecriptionDetails { get; set; } = new List<PrecriptionDetail>();
}
