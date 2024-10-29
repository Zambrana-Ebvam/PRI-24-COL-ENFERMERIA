using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("Prescription")]
public partial class Prescription
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("prescribedBy")]
    public int PrescribedBy { get; set; }

    [Column("estudentId")]
    public int EstudentId { get; set; }

    [ForeignKey("EstudentId")]
    [InverseProperty("Prescriptions")]
    public virtual Estudent Estudent { get; set; } = null!;

    [ForeignKey("PrescribedBy")]
    [InverseProperty("Prescriptions")]
    public virtual Nurse PrescribedByNavigation { get; set; } = null!;

    [InverseProperty("Prescription")]
    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
}
