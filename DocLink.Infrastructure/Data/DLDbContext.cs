using DocLink.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Infrastructure.Data
{
    public class DLDbContext : IdentityDbContext<Account>
    {
        public DLDbContext(DbContextOptions<DLDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Doctor>()
                .HasOne(x => x.Account)
                .WithOne()
                .HasPrincipalKey<Account>(x => x.Id)
                .HasForeignKey<Doctor>(x => x.Id);

            builder.Entity<Patient>()
               .HasOne(x => x.Account)
               .WithOne()
               .HasPrincipalKey<Account>(x => x.Id)
               .HasForeignKey<Patient>(x => x.Id);

            builder.Entity<Patient>()
                .HasMany(a => a.Appointments)
                .WithOne(x => x.Patient)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<LanguageSpoken> LanguageSpokens { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }

    }
}
