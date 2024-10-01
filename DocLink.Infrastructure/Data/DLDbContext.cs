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
    public class DLDbContext:IdentityDbContext<Person>
    {
        public DLDbContext(DbContextOptions<DLDbContext> options):base(options)
        {
            
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<LanguageSpoken> LanguageSpokens { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Specialty> Specialties { get; set; }

    }
}
