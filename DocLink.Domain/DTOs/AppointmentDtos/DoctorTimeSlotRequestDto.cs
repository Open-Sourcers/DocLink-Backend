using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AppointmentDtos
{
    public class DoctorTimeSlotRequestDto
    {
        public string DoctorId { get; set; }
        public DateTime Date { get; set; }
    }
}
