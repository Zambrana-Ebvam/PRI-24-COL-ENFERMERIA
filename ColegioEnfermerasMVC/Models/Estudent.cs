using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("Estudent")]
public partial class Estudent
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [Column("legalGuardian")]
    [StringLength(140)]
    [Unicode(false)]
    public string LegalGuardian { get; set; } = null!;

    [Column("legalGuardianCellphone")]
    [StringLength(10)]
    [Unicode(false)]
    public string LegalGuardianCellphone { get; set; } = null!;

    [Column("course")]
    [StringLength(30)]
    [Unicode(false)]
    public string Course { get; set; } = null!;

    [Column("nurseId")]
    public int? NurseId { get; set; }

    [Column("schoolId")]
    public byte SchoolId { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("Estudent")]
    public virtual Person IdNavigation { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    [InverseProperty("Estudent")]
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    [ForeignKey("SchoolId")]
    [InverseProperty("Estudents")]
    public virtual School School { get; set; } = null!;
}
