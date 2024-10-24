using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AppointmentDtos
{
    public class AppointmentDetailsDTO
    {
        public string DoctorName { get; set; }
        public string DoctorPictureUrl { get; set; }
        public string DoctorSpecialty { get; set;}
        public DateTime Date { get; set; }

    }
}
