using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("Supplier")]
public partial class Supplier
{
    [Key]
    [Column("id")]
    public byte Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("adress")]
    [StringLength(100)]
    [Unicode(false)]
    public string Adress { get; set; } = null!;

    [Column("phone")]
    [StringLength(50)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [Column("registraterDate", TypeName = "datetime")]
    public DateTime RegistraterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<Medicine>? Medicines { get; set; } = new List<Medicine>();
}
