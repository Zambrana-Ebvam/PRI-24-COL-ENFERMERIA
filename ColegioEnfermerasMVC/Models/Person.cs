using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("Person")]
public partial class Person
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("names")]
    [StringLength(60)]
    [Unicode(false)]
    public string Names { get; set; } = null!;

    [Column("middleName")]
    [StringLength(60)]
    [Unicode(false)]
    public string MiddleName { get; set; } = null!;

    [Column("surname")]
    [StringLength(60)]
    [Unicode(false)]
    public string Surname { get; set; } = null!;

    [Column("secondSurname")]
    [StringLength(60)]
    [Unicode(false)]
    public string SecondSurname { get; set; } = null!;

    [Column("ci")]
    [StringLength(9)]
    [Unicode(false)]
    public string? Ci { get; set; }

    [Column("cellphone")]
    [StringLength(10)]
    [Unicode(false)]
    public string Cellphone { get; set; } = null!;

    [Column("gender")]
    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [Column("birthDate")]
    public DateOnly BirthDate { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [InverseProperty("IdNavigation")]
    public virtual Estudent? Estudent { get; set; }

    [InverseProperty("IdNavigation")]
    public virtual Userr? Userr { get; set; }
}
