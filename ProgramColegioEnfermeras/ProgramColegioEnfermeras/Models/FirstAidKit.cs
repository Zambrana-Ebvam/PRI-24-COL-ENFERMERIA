using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class FirstAidKit
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime RegisterDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public byte Status { get; set; }

    public byte SchoolId { get; set; }

    public int ReponsibleId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<MedicineFirstAidKit> MedicineFirstAidKits { get; set; } = new List<MedicineFirstAidKit>();

    public virtual Establishment School { get; set; } = null!;
}
