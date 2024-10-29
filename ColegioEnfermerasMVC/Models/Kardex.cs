using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("Kardex")]
public partial class Kardex
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("hearFrequency")]
    public byte HearFrequency { get; set; }

    [Column("temperature")]
    public double Temperature { get; set; }

    [Column("bloodPressure")]
    public byte BloodPressure { get; set; }

    [Column("bloodType")]
    [StringLength(5)]
    [Unicode(false)]
    public string BloodType { get; set; } = null!;

    [Column("breathingRate")]
    public byte BreathingRate { get; set; }

    [Column("spo2")]
    public byte Spo2 { get; set; }

    [Column("height", TypeName = "decimal(5, 2)")]
    public decimal Height { get; set; }

    [Column("weight", TypeName = "decimal(5, 2)")]
    public decimal Weight { get; set; }

    [Column("imc", TypeName = "decimal(10, 2)")]
    public decimal Imc { get; set; }

    [Column("priority")]
    [StringLength(20)]
    [Unicode(false)]
    public string Priority { get; set; } = null!;

    [Column("observations")]
    [StringLength(350)]
    [Unicode(false)]
    public string? Observations { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate")]
    [StringLength(10)]
    public string? LastUpdate { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("estudentId")]
    public int EstudentId { get; set; }

    [Column("madeBy")]
    public int MadeBy { get; set; }

    [ForeignKey("MadeBy")]
    [InverseProperty("Kardices")]
    public virtual Nurse MadeByNavigation { get; set; } = null!;

    [InverseProperty("Kardex")]
    public virtual ICollection<ModifyImc> ModifyImcs { get; set; } = new List<ModifyImc>();

    [ForeignKey("UserId")]
    [InverseProperty("Kardices")]
    public virtual Estudent User { get; set; } = null!;
}
