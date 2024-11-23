using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AppointmentDtos
{
    public class ScheduleAppointmentDto
    {
        public int AppointmentId { get; set; }
        public int TimeSlotId { get; set; }
        public DateTime Date { get; set; }
    }
}
