using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{
    public class Specialty:BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int NumberOfDoctors { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}
