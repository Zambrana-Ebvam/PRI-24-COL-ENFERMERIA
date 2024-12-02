using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("Province")]
public partial class Province
{
    [Key]
    [Column("id")]
    public byte Id { get; set; }

    [Column("name")]
    [StringLength(40)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Province")]
    public virtual ICollection<Establishment>? Establishments { get; set; } = new List<Establishment>();
}
