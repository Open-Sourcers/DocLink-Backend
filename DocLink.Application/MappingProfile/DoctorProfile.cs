using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Application.MappingProfile
{
	internal class DoctorProfile : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<CreateDoctorDto, AppUser>();

			config.NewConfig<UpdateDoctorDto, Doctor>();

			config.NewConfig<string, LanguageSpoken>()
				 .Map(dest => dest.Name, src => src);

			config.NewConfig<string, Qualification>()
			 .Map(dest => dest.Name, src => src);

			config.NewConfig<Doctor, DoctorDto>()
				.Map(dest => dest.FirstName, src => src.user.FirstName)
				.Map(dest => dest.LastName, src => src.user.LastName)
				.Map(dest => dest.UserName, src => src.user.UserName)
				.Map(dest => dest.Email, src => src.user.Email)
				.Map(dest => dest.Specialty, src => src.Specialty.Name)
				.Map(dest => dest.image, src => src.user.ProfilePecture);

		}
	}
}
