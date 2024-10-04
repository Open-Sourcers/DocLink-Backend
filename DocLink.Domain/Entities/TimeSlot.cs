using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{
    public class TimeSlot:BaseEntity<int>
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}
