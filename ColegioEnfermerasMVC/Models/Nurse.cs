using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("Nurse")]
public partial class Nurse
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("codeSedes")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CodeSedes { get; set; }

    [Column("speciality")]
    [StringLength(60)]
    [Unicode(false)]
    public string? Speciality { get; set; }

    [Column("email")]
    [StringLength(60)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [InverseProperty("IdNurseNavigation")]
    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    [ForeignKey("Id")]
    [InverseProperty("Nurse")]
    public virtual Userr IdNavigation { get; set; } = null!;

    [InverseProperty("MadeByNavigation")]
    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    [InverseProperty("PrescribedByNavigation")]
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
