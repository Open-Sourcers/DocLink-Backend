using DocLink.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Entities
{

    public class PatientDetails
    {
        public string FullName { get; set; }
        public string AgeRange { get; set; }
        public Gender gender { get; set; }
        public string ProblemDescription { get; set; }

    }
}
