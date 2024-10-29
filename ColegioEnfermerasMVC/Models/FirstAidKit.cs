using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("FirstAidKit")]
public partial class FirstAidKit
{
    [Key]
    [Column("id")]
    public byte Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(350)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Column("responsibleId")]
    public int ResponsibleId { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("schoolId")]
    public byte SchoolId { get; set; }

    [InverseProperty("IdA")]
    public virtual ICollection<MedicineFirstAidKit>? MedicineFirstAidKits { get; set; } = new List<MedicineFirstAidKit>();

    [ForeignKey("SchoolId")]
    [InverseProperty("FirstAidKits")]
    public virtual School? School { get; set; } = null!;
}
