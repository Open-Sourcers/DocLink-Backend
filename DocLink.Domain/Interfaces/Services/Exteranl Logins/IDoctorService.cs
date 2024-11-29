using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.DTOs.SpecialtyDto;
using DocLink.Domain.Responses.Genaric;
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
		Task<BaseResponse<bool>> CreateDoctor(CreateDoctorDto doctor);
		Task<BaseResponse<DoctorDto>> UpdateDoctor(UpdateDoctorDto doctor);
		Task<BaseResponse<bool>> DeleteDoctor(string id);
		Task<BaseResponse<IReadOnlyList<DoctorDto>>>GetDoctorsWithSpec(DoctorParams param);
		Task<BaseResponse<DoctorDto>> GetDoctorById(string id);
		Task<BaseResponse<IReadOnlyList<DoctorLanguageDto>>> GetDoctorLanguages(string id);
		Task<BaseResponse<IReadOnlyList<DoctorQualificationsDto>>> GetDoctorQualifications(string id);
		Task<BaseResponse<IReadOnlyList<SpecialtyDto>>> GetAllSpecialties();
	}
}
