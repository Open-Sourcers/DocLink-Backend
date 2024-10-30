using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.DTOs.PatientDtos;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Repositories;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses.Genaric;
using DocLink.Domain.Specifications;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DocLink.Application.Services
{
    public class PatientServices : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMedia _media;

        public PatientServices(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMedia media)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _media = media;
        }

        public async Task<BaseResponse<bool>> AddRate(string DoctorId, float stars)
        {
            if (stars < 0 || stars > 5) return new BaseResponse<bool>("Rating isn't valid", StatusCodes.Status404NotFound, null);
            var doctor = await _unitOfWork.Repository<Doctor, string>().GetByIdAsync(DoctorId);
            if(doctor == null) return new BaseResponse<bool>("invalid patient id", StatusCodes.Status404NotFound, null);

            doctor.Rate = ((doctor.Rate * doctor.RatersCount) + stars) / (doctor.RatersCount + 1);
            doctor.RatersCount++;

            var Result = await _unitOfWork.SaveAsync();
            if (Result <= 0) return new BaseResponse<bool>("Error has been occured save Rating", StatusCodes.Status500InternalServerError, null);
            return new BaseResponse<bool>(true, "Rating has been successfully saved");
        }

        public async Task<BaseResponse<PatientToReturnDto>> GetPatientById(string patientId)
        {
            var spec = new PatientWithRelatedData(patientId, user: true);
            var patient = await _unitOfWork.Repository<Patient, string>().GetEntityWithSpecAsync(spec);
            if (patient is null) return new BaseResponse<PatientToReturnDto>("invalid patient id", StatusCodes.Status404NotFound, null);
            var mappedPatient = patient.Adapt<PatientToReturnDto>();
            return new BaseResponse<PatientToReturnDto>(mappedPatient);
        }

        public async Task<BaseResponse<PatientToReturnDto>> UpdatePatient(UpdatePatientDto updatePatientDto)
        {
            var user = await _userManager.FindByIdAsync(updatePatientDto.Id);
            if (user is null) return new BaseResponse<PatientToReturnDto>("invalid patient id", StatusCodes.Status404NotFound, null);

            user.FirstName = updatePatientDto.FirstName ?? user.FirstName;
            user.LastName = updatePatientDto.LastName ?? user.LastName;
            user.Email = updatePatientDto.Email ?? user.Email;
            user.ProfilePicture = updatePatientDto.image != null ? _media.UploadFile(updatePatientDto.image, nameof(Doctor)) : user.ProfilePicture;
          
            await _userManager.UpdateAsync(user);
                
            var patient = await _unitOfWork.Repository<Patient, string>().GetByIdAsync(updatePatientDto.Id);

            if (patient is null) return new BaseResponse<PatientToReturnDto>("patient id is not valid", StatusCodes.Status404NotFound, null);

            patient.BirthDay = updatePatientDto.BirthDay ?? patient.BirthDay;
            patient.Gender = updatePatientDto.Gender ?? patient.Gender;
            patient.EmergencyContact = updatePatientDto.EmergencyContact ?? patient.EmergencyContact;

            _unitOfWork.Repository<Patient, string>().Update(patient);

            var Updated = await _unitOfWork.SaveAsync();

            if (Updated <= 0) return new BaseResponse<PatientToReturnDto>("patient can't be updated", StatusCodes.Status400BadRequest, null);

            var UpdatedPatient = await GetPatientById(updatePatientDto.Id);

            return new BaseResponse<PatientToReturnDto>(UpdatedPatient.Data, "Account has been updated successfully");

        }   
    }
}
