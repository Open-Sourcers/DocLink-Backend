using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.Entities;
using Mapster;
using Microsoft.Extensions.Configuration;


namespace DocLink.Application.MappingProfile
{
	public static class DoctorProfile
	{
		public static void Configure(IConfiguration config)
		{
			TypeAdapterConfig<CreateDoctorDto, AppUser>.NewConfig();

			TypeAdapterConfig<UpdateDoctorDto, Doctor>.NewConfig()
				.Map(dest => dest.Qualifications, src => src.Qualifications.Select(name => new Qualification { Name = name }));

			TypeAdapterConfig<Doctor, DoctorDto>.NewConfig()
				.Map(dest => dest.FirstName, src => src.user.FirstName)
				.Map(dest => dest.LastName, src => src.user.LastName)
				.Map(dest => dest.UserName, src => src.user.UserName)
				.Map(dest => dest.Email, src => src.user.Email)
				.Map(dest => dest.Specialty, src => src.Specialty.Name)
				.Map(dest => dest.image, src => 
				    string.IsNullOrWhiteSpace(src.user.ProfilePicture) ? null 
				    : $"{config["BaseUrl"]}{src.user.ProfilePicture}");
		}
	}
}
