using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.DoctorDtos
{
	public class DoctorDto
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? image { get; set; }
		public float Rate { get; set; }
		public int YearsOfExperience { get; set; }
		public string? About { get; set; }
		public decimal ConsultationFee { get; set; }
		public string Specialty { get; set; }
	}
}
