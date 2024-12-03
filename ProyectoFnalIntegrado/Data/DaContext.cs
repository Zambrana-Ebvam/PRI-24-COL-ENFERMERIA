using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectoFnalIntegrado.Models;

namespace ProyectoFnalIntegrado.Data;

public partial class DaContext : DbContext
{
    public DaContext()
    {
    }

    public DaContext(DbContextOptions<DaContext> options)
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

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Userr> Userrs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9T9KDTI\\SQLEXPRESS; Database=Da; User=sa; Password=Univalle; Trusted_Connection=true; Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Assignme__3213E83FEC15E910");

            entity.Property(e => e.Status).HasDefaultValue((byte)1);

            entity.HasOne(d => d.IdNurseNavigation).WithMany(p => p.Assignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Nurse");

            entity.HasOne(d => d.IdSchoolNavigation).WithMany(p => p.Assignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Establishment1");
        });

        modelBuilder.Entity<Establishment>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Province).WithMany(p => p.Establishments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Establishment_Province");
        });

        modelBuilder.Entity<FirstAidKit>(entity =>
        {
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);

            entity.HasOne(d => d.School).WithMany(p => p.FirstAidKits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FirstAidKit_Establishment");
        });

        modelBuilder.Entity<Kardex>(entity =>
        {
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.State).HasDefaultValue((byte)1);

            entity.HasOne(d => d.IdNurseNavigation).WithMany(p => p.Kardices).HasConstraintName("FK_Kardex_Nurse");

            entity.HasOne(d => d.IdStudentNavigation).WithMany(p => p.Kardices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kardex_Student");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);

            entity.HasOne(d => d.Category).WithMany(p => p.Medicines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicine_Category");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Medicines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicine_Supplier");
        });

        modelBuilder.Entity<MedicineFirstAidKit>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdMedicineNavigation).WithMany(p => p.MedicineFirstAidKits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicineFirstAidKit_FirstAidKit");

            entity.HasOne(d => d.IdMedicine1).WithMany(p => p.MedicineFirstAidKits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicineFirstAidKit_Medicine1");
        });

        modelBuilder.Entity<Nurse>(entity =>
        {
            entity.Property(e => e.Gender).IsFixedLength();
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.State).HasDefaultValue((byte)1);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Prescription_1");

            entity.Property(e => e.RegisterDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);

            entity.HasOne(d => d.Medicine).WithMany(p => p.Prescriptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prescription_Medicine");

            entity.HasOne(d => d.Nurse).WithMany(p => p.Prescriptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prescription_Nurse");

            entity.HasOne(d => d.Student).WithMany(p => p.Prescriptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prescription_Student");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.BloodType).IsFixedLength();
            entity.Property(e => e.Gender).IsFixedLength();
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.State).HasDefaultValue((byte)1);

            entity.HasOne(d => d.IdEstablishmentNavigation).WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Establishment");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.RegistraterDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);
        });

        modelBuilder.Entity<Userr>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.State).HasDefaultValue((byte)1);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Userr)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Userr_Nurse");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    #region Consultas ala base de datos acerca de la Entidad Relacion

    public List<(int idNurse, byte idSchool, string FullName, string Speciality, string SchoolName, DateTime StartDate, DateTime? EndDate)> GetNurseSchoolAssignments()
    {
        var assignments = this.Assignments
            .Where(a => a.Status == 1) // Filtro principal
            .Select(a => new
            {
                IdNurse = a.IdNurse,
                IdSchool = a.IdSchool,
                FullName = (a.IdNurseNavigation != null
                    ? (a.IdNurseNavigation.Names ?? "") + " " +
                      (a.IdNurseNavigation.FirstName ?? "") + " " +
                      (a.IdNurseNavigation.MiddleName ?? "")
                    : "").Trim(),
                Speciality = a.IdNurseNavigation.Specialty, // Manejo de posibles nulos
                SchoolName = a.IdSchoolNavigation.Name,    // Manejo de posibles nulos
                StartDate = a.StartDate,
                EndDate = a.EndDate
            })
            .AsEnumerable()
            .Select(a => (
                idNurse: a.IdNurse,
                idSchool: (byte)(a.IdSchool), // Manejo de posibles nulos para IdSchool
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
        var schools = this.Establishments
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
            .Join(this.Establishments,
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
        var schools = this.Establishments
            .AsEnumerable()
            .Select(s => ((byte)s.Id, s.Name, s.Zone, s.Director))
            .ToList();

        return schools;
    }
    public List<(int Id, string FullName, string CodeSedes, string Speciality)> GetAllNurses()
    {
        var nurses = this.Nurses
            .Where(n => n.State == 1) 
            .Select(n => new
            {
                n.Id,
                FullName = $"{n.Names} {n.FirstName}",
                n.CodeSedes,
                n.Specialty
            })
            .AsEnumerable()
            .Select(n => (
                Id: n.Id,
                FullName: n.FullName,
                CodeSedes: n.CodeSedes,
                Speciality: n.Specialty
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
