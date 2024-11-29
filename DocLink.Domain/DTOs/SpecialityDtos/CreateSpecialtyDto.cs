using Microsoft.AspNetCore.Http;
namespace DocLink.Domain.DTOs.SpecialtyDto
{
	public class CreateSpecialtyDto
	{
		public string Name { get; set; }
		public IFormFile Image { get; set; }
	}
}
