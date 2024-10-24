using DocLink.Domain.DTOs.AppointmentDtos;
using DocLink.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{
    public class Appointment:BaseEntity<int>
    {
        public DateTime Date { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime CreateAT { get; set; } = DateTime.Now;
        public PationDetails PationDetails { get; set; }
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public string PatientId { get; set; }
        public Patient Patient { get; set; }

        public int TimeSlotID { get; set; }
        public TimeSlot TimeSlot { get; set; }

    }
}
