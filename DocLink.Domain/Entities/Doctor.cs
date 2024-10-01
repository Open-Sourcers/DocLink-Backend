namespace DocLink.Domain.Entities
{
    public class Doctor : Person
    {
        public int YearsOfExperience { get; set; }
        public float Rate { get; set; }
        public string About { get; set; }
        public bool IsOnline { get; set; }
        public decimal ConsultationFee { get; set; }
        public DateTime AppointmentDuration { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();

        public string SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

    }
}
