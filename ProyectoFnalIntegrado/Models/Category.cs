using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("Category")]
public partial class Category
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Medicine>? Medicines { get; set; } = new List<Medicine>();
}
