using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("Userr")]
public partial class Userr
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userName")]
    [StringLength(30)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [Column("password")]
    [StringLength(40)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("rol")]
    [StringLength(30)]
    [Unicode(false)]
    public string Rol { get; set; } = null!;

    [Column("registrationDate", TypeName = "smalldatetime")]
    public DateTime RegistrationDate { get; set; }

    [Column("updateDate", TypeName = "smalldatetime")]
    public DateTime? UpdateDate { get; set; }

    [Column("state")]
    public byte State { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("Userr")]
    public virtual Nurse? IdNavigation { get; set; } = null!;
}
