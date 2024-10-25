﻿using DocLink.Domain.DTOs.AppointmentDtos;
using DocLink.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain
{
    public static class MapsterConfig 
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreateAppointmentDto, Appointment>.NewConfig().TwoWays();
                       
           
            TypeAdapterConfig<Appointment, AppointmentDetailsDTO>.NewConfig()
                             .Map(dest => dest.DoctorName, src => src.Doctor.user.FirstName)
                             .Map(dest => dest.DoctorSpecialty, src => src.Doctor.Specialty.Name)
                             .Map(dest => dest.DoctorPictureUrl, src => src.Doctor.user.ProfilePicture);
                             

            TypeAdapterConfig<PatientDetailsDto, PatientDetails>.NewConfig();
                             
        }
    }
}
