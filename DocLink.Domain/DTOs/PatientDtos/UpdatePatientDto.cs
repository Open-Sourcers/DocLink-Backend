using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace DocLink.Domain.DTOs.PatientDtos
{
    public class UpdatePatientDto
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile? image { get; set; }

        public DateTime? BirthDay { get; set; }
        public Gender? Gender { get; set; }
        public string? EmergencyContact { get; set; }
    }
}
