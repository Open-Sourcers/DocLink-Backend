using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Enums;

namespace DocLink.Domain.DTOs.PatientDtos
{
    public class PatientToReturnDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? image { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }
        public string EmergencyContact { get; set; }
    }
}
