using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class Category
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
