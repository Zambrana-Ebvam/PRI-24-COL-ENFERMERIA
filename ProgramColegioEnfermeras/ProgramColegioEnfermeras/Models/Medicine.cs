using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class Medicine
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public byte Stock { get; set; }

    public string Description { get; set; } = null!;

    public DateOnly ExpirationDate { get; set; }

    public DateTime RegisterDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public byte Status { get; set; }

    public int UserId { get; set; }

    public short CategoryId { get; set; }

    public byte SupplierId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<MedicineFirstAidKit> MedicineFirstAidKits { get; set; } = new List<MedicineFirstAidKit>();

    public virtual ICollection<PrecriptionDetail> PrecriptionDetails { get; set; } = new List<PrecriptionDetail>();

    public virtual Supplier Supplier { get; set; } = null!;
}
