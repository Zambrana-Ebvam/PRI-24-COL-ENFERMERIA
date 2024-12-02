using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class Supplier
{
    public byte Id { get; set; }

    public string Name { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime RegistraterDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public byte Status { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
