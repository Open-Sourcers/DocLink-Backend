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
        public string Status { get; set; }
        public DateTime Duration { get; set; }
        public DateTime CreateAT { get; set; }

        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public string PatientId { get; set; }
        public Patient Patient { get; set; }
            
    }
}
