using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("Userr")]
public partial class Userr
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [StringLength(30)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("password")]
    [MaxLength(50)]
    public byte[] Password { get; set; } = null!;

    [Column("rol")]
    [StringLength(30)]
    [Unicode(false)]
    public string Rol { get; set; } = null!;

    [Column("email")]
    [StringLength(60)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("adress")]
    [StringLength(200)]
    [Unicode(false)]
    public string Adress { get; set; } = null!;

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("Userr")]
    public virtual Person IdNavigation { get; set; } = null!;

    [InverseProperty("IdNavigation")]
    public virtual Nurse? Nurse { get; set; }
}
