using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Entities;

namespace DocLink.Domain.DTOs.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public int TimeSlotID { get; set; }
        public DateTime Date { get; set; }
        public PatientDetailsDto PationDetails { get; set; }

    }
}
