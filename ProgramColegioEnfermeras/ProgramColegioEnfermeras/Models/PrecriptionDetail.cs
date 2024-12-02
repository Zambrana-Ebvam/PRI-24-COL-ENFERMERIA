using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class PrecriptionDetail
{
    public int Id { get; set; }

    public string Dosage { get; set; } = null!;

    public string Frecuency { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int PrescriptionId { get; set; }

    public short MedicineId { get; set; }

    public DateTime RegisterDtae { get; set; }

    public DateTime LastUpdate { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;

    public virtual Prescription Prescription { get; set; } = null!;
}
