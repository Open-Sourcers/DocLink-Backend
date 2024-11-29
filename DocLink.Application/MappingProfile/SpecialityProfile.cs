using DocLink.Domain.DTOs.SpecialtyDto;
using DocLink.Domain.Entities;
using Mapster;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Application.MappingProfile
{
	public class SpecialtyProfile
	{
		public static void configure(IConfiguration config)
		{
			TypeAdapterConfig<Specialty, SpecialtyDto>.NewConfig()
				.Map(dest => dest.ImageUrl, src => $"{config["BaseUrl"]}{src.ImageUrl}");

		}
	}
}
