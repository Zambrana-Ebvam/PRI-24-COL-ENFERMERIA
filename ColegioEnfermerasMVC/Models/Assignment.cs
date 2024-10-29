using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("Assignment")]
public partial class Assignment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("startDate", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("endDate", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column("startTime")]
    public TimeOnly StartTime { get; set; }

    [Column("endTime")]
    public TimeOnly EndTime { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("idNurse")]
    public int IdNurse { get; set; }

    [Column("idSchool")]
    public byte IdSchool { get; set; }

    [ForeignKey("IdNurse")]
    [InverseProperty("Assignments")]
    public virtual Nurse? IdNurseNavigation { get; set; } = null!;

    [ForeignKey("IdSchool")]
    [InverseProperty("Assignments")]
    public virtual School? IdSchoolNavigation { get; set; } = null!;
}
