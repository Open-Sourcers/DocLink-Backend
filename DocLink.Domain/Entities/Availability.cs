using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{
    public class Availability:BaseEntity<int>
    {
        public string Day { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
