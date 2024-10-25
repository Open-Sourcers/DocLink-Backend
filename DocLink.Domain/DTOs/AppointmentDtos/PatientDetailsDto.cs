using DocLink.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.AppointmentDtos
{
    public class PatientDetailsDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string AgeRange { get; set; }
        [Required]
        public Gender gender { get; set; }
        [Required]
        public string ProblemDescription { get; set; }
    }
}
