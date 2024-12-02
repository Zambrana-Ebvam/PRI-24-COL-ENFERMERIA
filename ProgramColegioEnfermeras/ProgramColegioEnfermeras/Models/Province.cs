using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class Province
{
    public byte Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Establishment> Establishments { get; set; } = new List<Establishment>();
}
