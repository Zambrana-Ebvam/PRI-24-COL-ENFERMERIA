using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Models;

[Table("Medicine")]
public partial class Medicine
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("name")]
    [StringLength(150)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("stock")]
    public byte Stock { get; set; }

    [Column("description")]
    [StringLength(350)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Column("expirationDate")]
    public DateOnly ExpirationDate { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("status")]
    public byte Status { get; set; }

    [Column("categoryId")]
    public short CategoryId { get; set; }

    [Column("supplierId")]
    public byte SupplierId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Medicines")]
    public virtual Category? Category { get; set; } = null!;

    [InverseProperty("IdMedicineNavigation")]
    public virtual ICollection<MedicineFirstAidKit>?MedicineFirstAidKits { get; set; } = new List<MedicineFirstAidKit>();

    [InverseProperty("Medicine")]
    public virtual ICollection<PrescriptionDetail>? PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Medicines")]
    public virtual Supplier? Supplier { get; set; } = null!;
}
