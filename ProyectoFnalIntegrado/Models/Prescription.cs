using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("Prescription")]
public partial class Prescription
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("dosage")]
    [StringLength(50)]
    [Unicode(false)]
    public string Dosage { get; set; } = null!;

    [Column("frecuency")]
    [StringLength(50)]
    [Unicode(false)]
    public string Frecuency { get; set; } = null!;

    [Column("instructions")]
    [StringLength(350)]
    [Unicode(false)]
    public string Instructions { get; set; } = null!;

    [Column("starDate", TypeName = "datetime")]
    public DateTime StarDate { get; set; }

    [Column("endDate", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("medicineId")]
    public short MedicineId { get; set; }

    [Column("nurseId")]
    public int NurseId { get; set; }

    [Column("studentId")]
    public int StudentId { get; set; }

    [ForeignKey("MedicineId")]
    [InverseProperty("Prescriptions")]
    public virtual Medicine? Medicine { get; set; } = null!;

    [ForeignKey("NurseId")]
    [InverseProperty("Prescriptions")]
    public virtual Nurse? Nurse { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Prescriptions")]
    public virtual Student? Student { get; set; } = null!;
}
