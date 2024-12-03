using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("FirstAidKit")]
public partial class FirstAidKit
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(350)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("schoolId")]
    public byte SchoolId { get; set; }

    [Column("reponsibleId")]
    public int ReponsibleId { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [InverseProperty("IdMedicineNavigation")]
    public virtual ICollection<MedicineFirstAidKit>? MedicineFirstAidKits { get; set; } = new List<MedicineFirstAidKit>();

    [ForeignKey("SchoolId")]
    [InverseProperty("FirstAidKits")]
    public virtual Establishment? School { get; set; } = null!;
}
