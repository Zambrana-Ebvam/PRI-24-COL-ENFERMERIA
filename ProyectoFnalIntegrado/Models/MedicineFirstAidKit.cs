using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFnalIntegrado.Models;

[Table("MedicineFirstAidKit")]
public partial class MedicineFirstAidKit
{
    [Key]
    [Column("id")]
    public byte Id { get; set; }

    [Column("idAid")]
    public byte IdAid { get; set; }

    [Column("idMedicine")]
    public short IdMedicine { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("expirationDate", TypeName = "datetime")]
    public DateTime ExpirationDate { get; set; }

    [Column("lastUpdated", TypeName = "datetime")]
    public DateTime? LastUpdated { get; set; }

    [ForeignKey("IdMedicine")]
    [InverseProperty("MedicineFirstAidKits")]
    public virtual Medicine? IdMedicine1 { get; set; } = null!;

    [ForeignKey("IdMedicine")]
    [InverseProperty("MedicineFirstAidKits")]
    public virtual FirstAidKit? IdMedicineNavigation { get; set; } = null!;
}
