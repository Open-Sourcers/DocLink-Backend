using DocLink.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AppointmentDtos
{
    public class AppointmentFilterRequestDto
    {
        public string ID { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
