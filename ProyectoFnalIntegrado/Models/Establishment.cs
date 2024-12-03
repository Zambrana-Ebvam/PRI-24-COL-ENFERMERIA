using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("Establishment")]
public partial class Establishment
{
    [Key]
    [Column("id")]
    public byte Id { get; set; }

    [Column("name")]
    [StringLength(60)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("schedule")]
    [StringLength(30)]
    [Unicode(false)]
    public string Schedule { get; set; } = null!;

    [Column("zone")]
    [StringLength(50)]
    [Unicode(false)]
    public string Zone { get; set; } = null!;

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }

    [Column("phone")]
    [StringLength(10)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [Column("director")]
    [StringLength(60)]
    [Unicode(false)]
    public string Director { get; set; } = null!;

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("userId")]
    public int? UserId { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("provinceId")]
    public byte ProvinceId { get; set; }

    [InverseProperty("IdSchoolNavigation")]
    public virtual ICollection<Assignment>? Assignments { get; set; } = new List<Assignment>();

    [InverseProperty("School")]
    public virtual ICollection<FirstAidKit>? FirstAidKits { get; set; } = new List<FirstAidKit>();

    [ForeignKey("ProvinceId")]
    [InverseProperty("Establishments")]
    public virtual Province? Province { get; set; } = null!;

    [InverseProperty("IdEstablishmentNavigation")]
    public virtual ICollection<Student>? Students { get; set; } = new List<Student>();
}
