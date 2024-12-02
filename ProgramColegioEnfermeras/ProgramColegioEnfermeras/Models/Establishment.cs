using System;
using System.Collections.Generic;

namespace ProgramColegioEnfermeras.Models;

public partial class Establishment
{
    public byte Id { get; set; }

    public string Name { get; set; } = null!;

    public string Schedule { get; set; } = null!;

    public string? Zone { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Phone { get; set; } = null!;

    public string Director { get; set; } = null!;

    public DateTime RegisterDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public int UserId { get; set; }

    public byte Status { get; set; }

    public byte ProvinceId { get; set; }

    public virtual ICollection<FirstAidKit> FirstAidKits { get; set; } = new List<FirstAidKit>();

    public virtual Province Province { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
