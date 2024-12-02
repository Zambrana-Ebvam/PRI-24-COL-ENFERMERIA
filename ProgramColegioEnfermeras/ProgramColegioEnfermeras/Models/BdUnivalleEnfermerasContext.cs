using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProgramColegioEnfermeras.Models;

public partial class BdUnivalleEnfermerasContext : DbContext
{
    public BdUnivalleEnfermerasContext()
    {
    }

    public BdUnivalleEnfermerasContext(DbContextOptions<BdUnivalleEnfermerasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Establishment> Establishments { get; set; }

    public virtual DbSet<FirstAidKit> FirstAidKits { get; set; }

    public virtual DbSet<Kardex> Kardices { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<MedicineFirstAidKit> MedicineFirstAidKits { get; set; }

    public virtual DbSet<Nurse> Nurses { get; set; }

    public virtual DbSet<PrecriptionDetail> PrecriptionDetails { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Userr> Userrs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //optionsBuilder.UseSqlServer("server=MALONDY\\SQLEXPRESS; database=bdUnivalleEnfermeras; integrated security=true; TrustServerCertificate=True;");

        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Assignment");

            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.EndTime).HasColumnName("endTime");
            entity.Property(e => e.IdNurse).HasColumnName("idNurse");
            entity.Property(e => e.IdSchool).HasColumnName("idSchool");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.StartTime).HasColumnName("startTime");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");

            entity.HasOne(d => d.IdNurseNavigation).WithMany()
                .HasForeignKey(d => d.IdNurse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Nurse");

            entity.HasOne(d => d.IdSchoolNavigation).WithMany()
                .HasForeignKey(d => d.IdSchool)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Establishment1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Establishment>(entity =>
        {
            entity.ToTable("Establishment");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Director)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("director");
            entity.Property(e => e.LastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("lastUpdate");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.ProvinceId).HasColumnName("provinceId");
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registerDate");
            entity.Property(e => e.Schedule)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("schedule");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Zone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("zone");

            entity.HasOne(d => d.Province).WithMany(p => p.Establishments)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Establishment_Province");
        });

        modelBuilder.Entity<FirstAidKit>(entity =>
        {
            entity.ToTable("FirstAidKit");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.LastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("lastUpdate");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registerDate");
            entity.Property(e => e.ReponsibleId).HasColumnName("reponsibleId");
            entity.Property(e => e.SchoolId).HasColumnName("schoolId");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.School).WithMany(p => p.FirstAidKits)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FirstAidKit_Establishment");
        });

        modelBuilder.Entity<Kardex>(entity =>
        {
            entity.ToTable("Kardex");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BloodPressure)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("bloodPressure");
            entity.Property(e => e.Derivation)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("derivation");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Height)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("height");
            entity.Property(e => e.IdNurse).HasColumnName("idNurse");
            entity.Property(e => e.IdStudent).HasColumnName("idStudent");
            entity.Property(e => e.OxygenLevel)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("oxygenLevel");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("registrationDate");
            entity.Property(e => e.RespiratoryRate).HasColumnName("respiratoryRate");
            entity.Property(e => e.State)
                .HasDefaultValue((byte)1)
                .HasColumnName("state");
            entity.Property(e => e.Temperature)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("temperature");
            entity.Property(e => e.Weight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("weight");

            entity.HasOne(d => d.IdNurseNavigation).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.IdNurse)
                .HasConstraintName("FK_Kardex_Nurse");

            entity.HasOne(d => d.IdStudentNavigation).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.IdStudent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kardex_Student");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.ToTable("Medicine");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Description)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ExpirationDate).HasColumnName("expirationDate");
            entity.Property(e => e.LastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("lastUpdate");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registerDate");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.SupplierId).HasColumnName("supplierId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Category).WithMany(p => p.Medicines)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicine_Category");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Medicines)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicine_Supplier");
        });

        modelBuilder.Entity<MedicineFirstAidKit>(entity =>
        {
            entity.ToTable("MedicineFirstAidKit");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("expirationDate");
            entity.Property(e => e.IdAid).HasColumnName("idAid");
            entity.Property(e => e.IdMedicine).HasColumnName("idMedicine");
            entity.Property(e => e.LastUpdated)
                .HasColumnType("datetime")
                .HasColumnName("lastUpdated");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.IdMedicineNavigation).WithMany(p => p.MedicineFirstAidKits)
                .HasForeignKey(d => d.IdMedicine)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicineFirstAidKit_FirstAidKit");

            entity.HasOne(d => d.IdMedicine1).WithMany(p => p.MedicineFirstAidKits)
                .HasForeignKey(d => d.IdMedicine)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicineFirstAidKit_Medicine1");
        });

        modelBuilder.Entity<Nurse>(entity =>
        {
            entity.ToTable("Nurse");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Cellphone)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("cellphone");
            entity.Property(e => e.Ci)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("ci");
            entity.Property(e => e.CodeSedes)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codeSedes");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("middleName");
            entity.Property(e => e.Names)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("names");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("registrationDate");
            entity.Property(e => e.Specialty)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("specialty");
            entity.Property(e => e.State)
                .HasDefaultValue((byte)1)
                .HasColumnName("state");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<PrecriptionDetail>(entity =>
        {
            entity.ToTable("PrecriptionDetail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dosage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dosage");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.Frecuency)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("frecuency");
            entity.Property(e => e.Instructions)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("instructions");
            entity.Property(e => e.LastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("lastUpdate");
            entity.Property(e => e.MedicineId).HasColumnName("medicineId");
            entity.Property(e => e.PrescriptionId).HasColumnName("prescriptionId");
            entity.Property(e => e.RegisterDtae)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registerDtae");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");

            entity.HasOne(d => d.Medicine).WithMany(p => p.PrecriptionDetails)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrecriptionDetail_Medicine");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrecriptionDetails)
                .HasForeignKey(d => d.PrescriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrecriptionDetail_Prescription");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.ToTable("Prescription");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdNurse).HasColumnName("idNurse");
            entity.Property(e => e.IdStudent).HasColumnName("idStudent");

            entity.HasOne(d => d.IdNurseNavigation).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.IdNurse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prescription_Nurse1");

            entity.HasOne(d => d.IdStudentNavigation).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.IdStudent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prescription_Student");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.ToTable("Province");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Allergy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("allergy");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.BloodType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("bloodType");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.FirstName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.IdEstablishment).HasColumnName("idEstablishment");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("middleName");
            entity.Property(e => e.Names)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("names");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("registrationDate");
            entity.Property(e => e.State)
                .HasDefaultValue((byte)1)
                .HasColumnName("state");
            entity.Property(e => e.Tutor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tutor");
            entity.Property(e => e.TutorCellphone)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("tutorCellphone");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("updateDate");

            entity.HasOne(d => d.IdEstablishmentNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.IdEstablishment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Establishment");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("adress");
            entity.Property(e => e.LastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("lastUpdate");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RegistraterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registraterDate");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<Userr>(entity =>
        {
            entity.ToTable("Userr");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(40)
                .HasColumnName("password");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("registrationDate");
            entity.Property(e => e.Rol)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.State)
                .HasDefaultValue((byte)1)
                .HasColumnName("state");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("updateDate");
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("userName");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Userr)
                .HasForeignKey<Userr>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Userr_Nurse");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
