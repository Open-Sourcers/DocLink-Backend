using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.Responses;
using DocLink.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Interfaces.Services.Exteranl_Logins
{
	public interface IDoctorService
	{
		Task<BaseResponse> CreateDoctor(CreateDoctorDto doctor);
		Task<BaseResponse> UpdateDoctor(string id, UpdateDoctorDto doctor);
		Task<BaseResponse> DeleteDoctor(string id);
		Task<BaseResponse> GetDoctorsWithSpec(DoctorParams param);
		Task<BaseResponse> GetDoctorById(string id);
	}
}
