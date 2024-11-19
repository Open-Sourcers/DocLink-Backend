using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLink.Domain.DTOs.PatientDtos;
using DocLink.Domain.Responses.Genaric;

namespace DocLink.Domain.Interfaces.Services
{
    public interface IPatientService
    {
        Task<BaseResponse<PatientToReturnDto>> GetPatientById(string patientId);
        Task<BaseResponse<PatientToReturnDto>> UpdatePatient(UpdatePatientDto updatePatientDto);
        Task<BaseResponse<bool>> AddRate(string DoctorId, float stars);
    }
}
