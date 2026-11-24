using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientManagementAPI.Models;

namespace PatientManagementAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
         public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patients");

            entity.HasKey(p => p.PatientId);

            entity.Property(p => p.PatientId)
                  .ValueGeneratedOnAdd();

            entity.Property(p => p.Pfirstname).IsRequired();
            entity.Property(p => p.Plastname).IsRequired();
            entity.Property(p => p.Email).IsRequired();
            entity.Property(p => p.Mobile).IsRequired();
            entity.Property(p => p.Address).IsRequired();
            entity.Property(p => p.Pincode).IsRequired();
        });
    }
    }
}