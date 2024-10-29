using System;
using System.Collections.Generic;
using ColegioEnfermerasMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ColegioEnfermerasMVC.Data;

public partial class DbCollegeContext : DbContext
{
    public DbCollegeContext()
    {
    }

    public DbCollegeContext(DbContextOptions<DbCollegeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Estudent> Estudents { get; set; }

    public virtual DbSet<FirstAidKit> FirstAidKits { get; set; }

    public virtual DbSet<Kardex> Kardices { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<MedicineFirstAidKit> MedicineFirstAidKits { get; set; }

    public virtual DbSet<ModifyImc> ModifyImcs { get; set; }

    public virtual DbSet<Nurse> Nurses { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Userr> Userrs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9T9KDTI\\SQLEXPRESS; Database=dbCollege; User=sa; Password=Univalle; Trusted_Connection=true; Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasOne(d => d.IdNurseNavigation).WithMany(p => p.Assignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Nurse1");

            entity.HasOne(d => d.IdSchoolNavigation).WithMany(p => p.Assignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_School1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Category_1");
        });

        modelBuilder.Entity<Estudent>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Estudent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estudent_Person");

            entity.HasOne(d => d.School).WithMany(p => p.Estudents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estudent_School");
        });

        modelBuilder.Entity<FirstAidKit>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.School).WithMany(p => p.FirstAidKits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FirstAidKit_School");
        });

        modelBuilder.Entity<Kardex>(entity =>
        {
            entity.Property(e => e.LastUpdate).IsFixedLength();
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MadeByNavigation).WithMany(p => p.Kardices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kardex_Nurse");

            entity.HasOne(d => d.User).WithMany(p => p.Kardices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kardex_Estudent");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.Medicines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicine_Category1");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Medicines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicine_Supplier");
        });

        modelBuilder.Entity<MedicineFirstAidKit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MedicineFirstAidKit_1");

            // Configuración de Id como autoincremental
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdA).WithMany(p => p.MedicineFirstAidKits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicineFirstAidKit_FirstAidKit1");

            entity.HasOne(d => d.IdMedicineNavigation).WithMany(p => p.MedicineFirstAidKits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicineFirstAidKit_Medicine1");
        });


        modelBuilder.Entity<ModifyImc>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Kardex).WithMany(p => p.ModifyImcs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ModifyIMC_Kardex");
        });

        modelBuilder.Entity<Nurse>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Nurse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nurse_Userr");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasOne(d => d.Estudent).WithMany(p => p.Prescriptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prescription_Estudent");

            entity.HasOne(d => d.PrescribedByNavigation).WithMany(p => p.Prescriptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prescription_Nurse");
        });

        modelBuilder.Entity<PrescriptionDetail>(entity =>
        {
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Medicine).WithMany(p => p.PrescriptionDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrescriptionDetail_Medicine");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrescriptionDetail_Prescription");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Province).WithMany(p => p.Schools)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_School_Province");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Userr>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Userr)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Userr_Person");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    #region Consultas ala base de datos acerca de la Entidad Relacion

    public List<(int idNurse, byte idSchool, string FullName, string Speciality, string SchoolName, DateTime StartDate, DateTime? EndDate)> GetNurseSchoolAssignments()
    {
        var assignments = this.Assignments
            .Where(a => a.Status == 1 &&
                        a.IdNurseNavigation.Status == 1 &&
                        a.IdSchoolNavigation.Status == 1)
            .Select(a => new
            {
                IdNurse = a.IdNurse,
                IdSchool = a.IdSchool,
                FullName = a.IdNurseNavigation.IdNavigation.IdNavigation.Names + " " +
                           (a.IdNurseNavigation.IdNavigation.IdNavigation.MiddleName ?? "") + " " +
                           a.IdNurseNavigation.IdNavigation.IdNavigation.Surname + " " +
                           (a.IdNurseNavigation.IdNavigation.IdNavigation.SecondSurname ?? ""),
                Speciality = a.IdNurseNavigation.Speciality,
                SchoolName = a.IdSchoolNavigation.Name,
                StartDate = a.StartDate,
                EndDate = a.EndDate
            })
            .AsEnumerable() 
            .Select(a => (
                idNurse: a.IdNurse,
                idSchool: (byte)a.IdSchool, 
                FullName: a.FullName,
                Speciality: a.Speciality,
                SchoolName: a.SchoolName,
                StartDate: a.StartDate,
                EndDate: a.EndDate
            ))
            .ToList();

        return assignments;
    }
    public List<(byte Id, string Name)> GetSchoolsByZone(string zona = "Todas")
    {
        var schools = this.Schools
            .Where(s => zona == "Todas" || s.Zone == zona)
            .AsEnumerable() 
            .Select(s => ((byte)s.Id, s.Name)) 
            .ToList();

        return schools;
    }

    public List<(string NameSchool, DateTime StartDate, DateTime EndDate, string Turno, byte Status)> GetNurseHistoryAssignments(int idNurse)
    {
        var assignments = this.Assignments
            .Where(a => a.IdNurse == idNurse)
            .Join(this.Schools,
                  a => a.IdSchool,
                  s => s.Id,
                  (a, s) => new { s.Name, a.StartDate, a.EndDate, a.StartTime, a.EndTime, a.Status }) 
            .AsEnumerable() 
            .Select(a => (
                NameSchool: a.Name,
                StartDate: a.StartDate,
                EndDate: a.EndDate ?? DateTime.MinValue, 
                Turno: $"{a.StartTime} - {a.EndTime}",
                Status: (byte)a.Status 
            ))
            .OrderBy(a => a.Status) 
            .ThenBy(a => a.StartDate) 
            .ToList();

        return assignments;
    }




    public List<(byte Id, string Name, string Zone, string Director)> GetAllSchools()
    {
        var schools = this.Schools
            .AsEnumerable() 
            .Select(s => ((byte)s.Id, s.Name, s.Zone, s.Director)) 
            .ToList();

        return schools;
    }
    public List<(int Id, string FullName, string CodeSedes, string Speciality)> GetAllNurses()
    {
        var nurses = this.Nurses
            .Join(this.People,
                  n => n.Id,
                  p => p.Id,
                  (n, p) => new { n.Id, FullName = $"{p.Names} {p.Surname}", n.CodeSedes, n.Speciality }) 
            .AsEnumerable() 
            .Select(n => (
                Id: n.Id,
                FullName: n.FullName,
                CodeSedes: n.CodeSedes,
                Speciality: n.Speciality
            ))
            .ToList();

        return nurses;
    }



    public bool ReassignNurseToSchool(Assignment assignment, int currentSchoolId)
    {
       
        var previousAssignments = this.Assignments
            .Where(a => a.IdNurse == assignment.IdNurse && a.Status == 1 && a.IdSchool == currentSchoolId)
            .ToList();

        foreach (var prevAssignment in previousAssignments)
        {
            prevAssignment.Status = 0;  
            prevAssignment.EndDate = DateTime.Now;  
        }

        var newAssignment = new Assignment
        {
            IdNurse = assignment.IdNurse,
            IdSchool = assignment.IdSchool,  
            StartDate = assignment.StartDate,
            EndDate = assignment.EndDate,
            StartTime = assignment.StartTime,
            EndTime = assignment.EndTime,
            Status = 1  
        };

        this.Assignments.Add(newAssignment);
        return this.SaveChanges() > 0;  
    }

    #endregion
}
