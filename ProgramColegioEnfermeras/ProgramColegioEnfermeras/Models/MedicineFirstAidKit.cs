using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class MedicineFirstAidKit
{
    public byte Id { get; set; }

    public byte IdAid { get; set; }

    public short IdMedicine { get; set; }

    public int Quantity { get; set; }

    public DateTime ExpirationDate { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Medicine IdMedicine1 { get; set; } = null!;

    public virtual FirstAidKit IdMedicineNavigation { get; set; } = null!;
}
