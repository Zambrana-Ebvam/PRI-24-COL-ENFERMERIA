using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("Nurse")]
public partial class Nurse
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("names")]
    [StringLength(60)]
    [Unicode(false)]
    public string Names { get; set; } = null!;

    [Column("firstName")]
    [StringLength(60)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("middleName")]
    [StringLength(60)]
    [Unicode(false)]
    public string? MiddleName { get; set; }

    [Column("gender")]
    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [Column("birthdate")]
    public DateOnly Birthdate { get; set; }

    [Column("ci")]
    [StringLength(14)]
    [Unicode(false)]
    public string Ci { get; set; } = null!;

    [Column("cellphone")]
    [StringLength(12)]
    [Unicode(false)]
    public string Cellphone { get; set; } = null!;

    [Column("email")]
    [StringLength(150)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("address")]
    [StringLength(200)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [Column("codeSedes")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CodeSedes { get; set; }

    [Column("specialty")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Specialty { get; set; }

    [Column("registrationDate", TypeName = "smalldatetime")]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    // Validar y asignar la fecha de actualización
    [Column("updateDate", TypeName = "smalldatetime")]
    public DateTime? UpdateDate { get; set; }

    [Column("state")]
    public byte State { get; set; }

    [InverseProperty("IdNurseNavigation")]
    public virtual ICollection<Assignment>? Assignments { get; set; } = new List<Assignment>();

    [InverseProperty("IdNurseNavigation")]
    public virtual ICollection<Kardex>? Kardices { get; set; } = new List<Kardex>();

    [InverseProperty("Nurse")]
    public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();

    [InverseProperty("IdNavigation")]
    public virtual Userr? Userr { get; set; }
}
