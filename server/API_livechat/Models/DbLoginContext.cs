using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API_livechat.Models;

public partial class DbLoginContext : DbContext
{
    public DbLoginContext()
    {
    }

    public DbLoginContext(DbContextOptions<DbLoginContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=db_login;User ID=SA;Password=Password123;MultipleActiveResultSets=True;Encrypt=false;TrustServerCertificate=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserProf__CB9A1CDF14842A3B");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.Username, "UQ__UserProf__F3DBC572D459B791").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.ChatRoomsCode)
          .HasConversion(
              v => v != null ? string.Join(',', v) : null,
              v => v != null ? v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>()
          )
          .HasMaxLength(250)
          .IsUnicode(false)
          .HasColumnName("chatRoomsCode");


            entity.Property(e => e.Code)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Passwrd)
                .HasMaxLength(250)
                .HasColumnName("passwrd");
            entity.Property(e => e.UsImg)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("usImg");
            entity.Property(e => e.UsRole)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usRole");
            entity.Property(e => e.Username)
                .HasMaxLength(250)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
