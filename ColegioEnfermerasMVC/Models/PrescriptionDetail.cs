using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("PrescriptionDetail")]
public partial class PrescriptionDetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("dosage")]
    [StringLength(50)]
    [Unicode(false)]
    public string Dosage { get; set; } = null!;

    [Column("frequency")]
    [StringLength(50)]
    [Unicode(false)]
    public string Frequency { get; set; } = null!;

    [Column("instructions")]
    [StringLength(350)]
    [Unicode(false)]
    public string Instructions { get; set; } = null!;

    [Column("startDate", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("endDate", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column("prescriptionId")]
    public int PrescriptionId { get; set; }

    [Column("medicineId")]
    public short MedicineId { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [ForeignKey("MedicineId")]
    [InverseProperty("PrescriptionDetails")]
    public virtual Medicine Medicine { get; set; } = null!;

    [ForeignKey("PrescriptionId")]
    [InverseProperty("PrescriptionDetails")]
    public virtual Prescription Prescription { get; set; } = null!;
}
