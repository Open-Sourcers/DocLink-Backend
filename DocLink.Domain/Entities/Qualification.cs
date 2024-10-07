using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{
    public class Qualification:BaseEntity<int>
    {
        public string Name { get; set; }
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
