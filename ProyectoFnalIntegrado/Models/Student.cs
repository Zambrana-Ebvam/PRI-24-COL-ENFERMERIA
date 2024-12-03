using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("Student")]
public partial class Student
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

    [Column("code")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Code { get; set; }

    [Column("tutor")]
    [StringLength(100)]
    [Unicode(false)]
    public string Tutor { get; set; } = null!;

    [Column("tutorCellphone")]
    [StringLength(12)]
    [Unicode(false)]
    public string TutorCellphone { get; set; } = null!;

    [Column("bloodType")]
    [StringLength(3)]
    [Unicode(false)]
    public string? BloodType { get; set; }

    [Column("allergy")]
    [StringLength(255)]
    [Unicode(false)]
    public string Allergy { get; set; } = null!;

    [Column("registrationDate", TypeName = "smalldatetime")]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    // Validar y asignar la fecha de actualización
    [Column("updateDate", TypeName = "smalldatetime")]
    public DateTime? UpdateDate { get; set; }

    [Column("state")]
    public byte State { get; set; }

    [Column("idEstablishment")]
    public byte IdEstablishment { get; set; }

    [ForeignKey("IdEstablishment")]
    [InverseProperty("Students")]
    public virtual Establishment? IdEstablishmentNavigation { get; set; } = null!;

    [InverseProperty("IdStudentNavigation")]
    public virtual ICollection<Kardex>? Kardices { get; set; } = new List<Kardex>();

    [InverseProperty("Student")]
    public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();
}
