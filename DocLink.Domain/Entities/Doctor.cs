namespace DocLink.Domain.Entities
{
    public class Doctor:BaseEntity<string>
    {
        public AppUser user { get; set; }
        public int YearsOfExperience { get; set; }
        public float Rate { get; set; }
        public string? About { get; set; }
        public bool IsOnline { get; set; }
        public decimal ConsultationFee { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public ICollection<LanguageSpoken> Languages { get; set; } = new HashSet<LanguageSpoken>();
        public ICollection<Qualification> Qualifications { get; set; } = new HashSet<Qualification>();

        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

    }
}
