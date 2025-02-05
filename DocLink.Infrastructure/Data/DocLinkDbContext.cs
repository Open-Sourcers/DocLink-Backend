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
    public class DocLinkDbContext : IdentityDbContext<AppUser>
    {
        public DocLinkDbContext(DbContextOptions<DocLinkDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Doctor>()
                .HasOne(x => x.user)
                .WithOne()
                .HasPrincipalKey<AppUser>(x => x.Id)
                .HasForeignKey<Doctor>(x => x.Id);

            builder.Entity<Patient>()
               .HasOne(x => x.user)
               .WithOne()
               .HasPrincipalKey<AppUser>(x => x.Id)
               .HasForeignKey<Patient>(x => x.Id);

            builder.Entity<Patient>()
                .HasMany(a => a.Appointments)
                .WithOne(x => x.Patient)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Patient>()
                   .Property(patient => patient.Gender)
                   .HasConversion<string>();

            builder.Entity<Appointment>(Options =>
            {
                Options.HasOne(app => app.TimeSlot)
                       .WithMany(timeSlot => timeSlot.Appointments)
                       .OnDelete(DeleteBehavior.NoAction);

                Options.Property(app => app.Status)
                       .HasConversion<string>();


                Options.OwnsOne(app => app.PationDetails, details => { 
                    details.WithOwner(); 
                    details.Property(d => d.gender).HasConversion<string>();
                });

                

            });
                
            base.OnModelCreating(builder);
        }
        public DbSet<AppUser> Accounts { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LanguageSpoken> LanguageSpokens { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
