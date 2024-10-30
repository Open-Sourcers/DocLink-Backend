using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.DTOs.PatientDtos;
using DocLink.Domain.Entities;
using Mapster;

namespace DocLink.Application.MappingProfile
{
    public static class PatientConfig
    {
        public static void configure()
        {
            TypeAdapterConfig<Patient, PatientToReturnDto>.NewConfig().IgnoreNullValues(true)
                            .Map(dest => dest.Email, src => src.user.Email)
                            .Map(dest => dest.UserName, src => src.user.UserName)
                            .Map(dest => dest.FirstName, src => src.user.FirstName)
                            .Map(dest => dest.LastName, src => src.user.LastName)
                            .Map(dest => dest.image, src => src.user.ProfilePicture);
        }
    }
}
