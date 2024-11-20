using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.DTOs.DoctorDtos
{
	public class UpdateDoctorDto
	{
		public string Id { get; set; }
		public string? FirstName { get; set; }
		public string?LastName { get; set; }
		public IFormFile? image { get; set; }
		public int YearsOfExperience { get; set; }
		public string? About { get; set; }
		public decimal ConsultationFee { get; set; }
		public int SpecialtyId { get; set; }
		public List<int> DoctorLanguages { get; set; } = new List<int>();
		public List<string> Qualifications { get; set; }= new List<string>();	

	}
}
