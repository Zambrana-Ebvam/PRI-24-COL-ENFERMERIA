using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[PrimaryKey("Id", "KardexId")]
[Table("ModifyIMC")]
public partial class ModifyImc
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("kardexId")]
    public int KardexId { get; set; }

    [Column("previousIMC", TypeName = "decimal(10, 2)")]
    public decimal PreviousImc { get; set; }

    [Column("modficationDate", TypeName = "datetime")]
    public DateTime ModficationDate { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [ForeignKey("KardexId")]
    [InverseProperty("ModifyImcs")]
    public virtual Kardex Kardex { get; set; } = null!;
}
