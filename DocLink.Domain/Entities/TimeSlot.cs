using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{
    public class TimeSlot:BaseEntity<int>
    {
        public DateTime Time { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
