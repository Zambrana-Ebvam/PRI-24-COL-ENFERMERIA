using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("Kardex")]
public partial class Kardex
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("height", TypeName = "decimal(5, 2)")]
    public decimal? Height { get; set; }

    [Column("weight", TypeName = "decimal(5, 2)")]
    public decimal? Weight { get; set; }

    [Column("oxygenLevel", TypeName = "decimal(5, 2)")]
    public decimal? OxygenLevel { get; set; }

    [Column("description")]
    [StringLength(1000)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Column("temperature", TypeName = "decimal(5, 2)")]
    public decimal? Temperature { get; set; }

    [Column("respiratoryRate")]
    public short? RespiratoryRate { get; set; }

    [Column("bloodPressure")]
    [StringLength(7)]
    [Unicode(false)]
    public string? BloodPressure { get; set; }

    [Column("derivation")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Derivation { get; set; }

    [Column("registrationDate", TypeName = "smalldatetime")]
    public DateTime RegistrationDate { get; set; }

    [Column("state")]
    public byte State { get; set; }

    [Column("idStudent")]
    public int IdStudent { get; set; }

    [Column("idNurse")]
    public int? IdNurse { get; set; }

    [ForeignKey("IdNurse")]
    [InverseProperty("Kardices")]
    public virtual Nurse? IdNurseNavigation { get; set; }

    [ForeignKey("IdStudent")]
    [InverseProperty("Kardices")]
    public virtual Student? IdStudentNavigation { get; set; } = null!;
}
