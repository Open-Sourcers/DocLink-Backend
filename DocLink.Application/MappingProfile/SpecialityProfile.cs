using DocLink.Domain.DTOs.SpecialtyDto;
using DocLink.Domain.Entities;
using Mapster;
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
		public static void configure()
		{
			TypeAdapterConfig<Specialty, SpecialtyDto>.NewConfig();
		}
	}
}
