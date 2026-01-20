using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GPWebAPI.Models;

public partial class GrillPizzeriaDbContext : DbContext
{
    public GrillPizzeriaDbContext()
    {
    }

    public GrillPizzeriaDbContext(DbContextOptions<GrillPizzeriaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alergen> Alergens { get; set; }

    public virtual DbSet<Hrana> Hranas { get; set; }

    public virtual DbSet<HranaAlergen> HranaAlergens { get; set; }

    public virtual DbSet<KategorijaHrane> KategorijaHranes { get; set; }

    public virtual DbSet<Korisnik> Korisniks { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Narudzba> Narudzbas { get; set; }

    public virtual DbSet<NarudzbaHrana> NarudzbaHranas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=.;Database=GrillPizzeriaDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alergen>(entity =>
        {
            entity.HasKey(e => e.Idalergen).HasName("PK__Alergen__87099776BD2C0061");

            entity.ToTable("Alergen");

            entity.Property(e => e.Naziv).HasMaxLength(100);
        });

        modelBuilder.Entity<Hrana>(entity =>
        {
            entity.HasKey(e => e.Idhrana).HasName("PK__Hrana__FB923E83D61E07A4");

            entity.ToTable("Hrana");

            entity.Property(e => e.Cijena).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Naslov).HasMaxLength(100);
            entity.Property(e => e.Opis).HasMaxLength(255);

            entity.HasOne(d => d.KategorijaHrane).WithMany(p => p.Hranas)
                .HasForeignKey(d => d.KategorijaHraneId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Hrana__Kategorij__5165187F");
        });

        modelBuilder.Entity<HranaAlergen>(entity =>
        {
            entity.HasKey(e => e.IdhranaAlergen).HasName("PK__HranaAle__6D1ED1441880C652");

            entity.ToTable("HranaAlergen");

            entity.HasOne(d => d.Alergen).WithMany(p => p.HranaAlergens)
                .HasForeignKey(d => d.AlergenId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__HranaAler__Alerg__5812160E");

            entity.HasOne(d => d.Hrana).WithMany(p => p.HranaAlergens)
                .HasForeignKey(d => d.HranaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__HranaAler__Hrana__571DF1D5");
        });

        modelBuilder.Entity<KategorijaHrane>(entity =>
        {
            entity.HasKey(e => e.IdkategorijaHrane).HasName("PK__Kategori__0B27D7DD88EF8005");

            entity.ToTable("KategorijaHrane");

            entity.Property(e => e.Naziv).HasMaxLength(100);
            entity.Property(e => e.Opis).HasMaxLength(255);
        });

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.HasKey(e => e.Idkorisnik).HasName("PK__Korisnik__EB961D9DF62C30A3");

            entity.ToTable("Korisnik");

            entity.HasIndex(e => e.Email, "UQ__Korisnik__A9D1053438AFC875").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Ime).HasMaxLength(100);
            entity.Property(e => e.Mobitel).HasMaxLength(255);
            entity.Property(e => e.Prezime).HasMaxLength(100);
            entity.Property(e => e.PwdHash).HasMaxLength(255);
            entity.Property(e => e.Salt).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Roles).WithMany(p => p.Korisniks)
                .HasForeignKey(d => d.RolesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Korisnik__RolesI__4CA06362");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3214EC07F4F59A6B");

            entity.ToTable("Log");

            entity.Property(e => e.Level).HasMaxLength(20);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Narudzba>(entity =>
        {
            entity.HasKey(e => e.Idnarudzba).HasName("PK__Narudzba__4069EE249A0392EF");

            entity.ToTable("Narudzba");

            entity.Property(e => e.Datum)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Narudzbas)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Narudzba__Korisn__5BE2A6F2");
        });

        modelBuilder.Entity<NarudzbaHrana>(entity =>
        {
            entity.HasKey(e => e.IdnarudzbaHrana).HasName("PK__Narudzba__AD29C39DEA2301F7");

            entity.ToTable("NarudzbaHrana");

            entity.HasOne(d => d.Hrana).WithMany(p => p.NarudzbaHranas)
                .HasForeignKey(d => d.HranaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__NarudzbaH__Hrana__5FB337D6");

            entity.HasOne(d => d.Narudzba).WithMany(p => p.NarudzbaHranas)
                .HasForeignKey(d => d.NarudzbaId)
                .HasConstraintName("FK__NarudzbaH__Narud__5EBF139D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolesId).HasName("PK__Roles__C4B2784032CBAE81");

            entity.Property(e => e.RolesName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
