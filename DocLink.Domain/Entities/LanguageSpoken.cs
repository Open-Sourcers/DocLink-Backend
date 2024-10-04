using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{
    public class LanguageSpoken:BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}
