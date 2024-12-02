using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class Assignment
{
    public int IdNurse { get; set; }

    public byte IdSchool { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public byte Status { get; set; }

    public virtual Nurse IdNurseNavigation { get; set; } = null!;

    public virtual Establishment IdSchoolNavigation { get; set; } = null!;
}
