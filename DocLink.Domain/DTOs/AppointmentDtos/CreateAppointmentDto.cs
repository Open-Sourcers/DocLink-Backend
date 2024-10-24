using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        public string DoctorID { get; set; }
        public int TimeSlotID { get; set; }
        public DateTime Date { get; set; }
        public PationDetails PationDetails { get; set; }

    }
}
