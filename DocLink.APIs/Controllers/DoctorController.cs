using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.Entities;
using DocLink.Domain.Enums;
using DocLink.Domain.Interfaces.Services.Exteranl_Logins;
using DocLink.Domain.Responses.Genaric;
using DocLink.Domain.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocLink.APIs.Controllers
{
	public class DoctorController : BaseController
	{
		private readonly IDoctorService _doctor;

		public DoctorController(IDoctorService doctor)
		{
			_doctor = doctor;
		}

		[Authorize(Roles = nameof(Roles.Patient))]
		[HttpGet("GetDoctorsWithSpec")]
		public async Task<ActionResult<BaseResponse<IReadOnlyList<DoctorDto>>>> GetDoctorsWithSpec([FromQuery]DoctorParams param)
		{
			return Ok(await _doctor.GetDoctorsWithSpec(param));
		}

		[Authorize(Roles=nameof(Roles.Patient))]
		[HttpGet("GetDoctorDetails/{Id}")]
		public async Task<ActionResult<BaseResponse<DoctorDto>>> GetDoctorDetails(string Id)
		{
			return Ok(await _doctor.GetDoctorById(Id));
		}

		[Authorize(Roles =nameof(Roles.Admin))]
		[HttpPost("CreateDoctorAccount")]
		public async Task<ActionResult<BaseResponse<bool>>> CreateDoctorAccount(CreateDoctorDto doctor)
		{
			return Ok(await _doctor.CreateDoctor(doctor));
		}

		[Authorize(Roles = nameof(Roles.Admin))]
		[HttpDelete("DeleteDoctor/{id}")]
		public async Task<ActionResult<BaseResponse<bool>>> DeleteDoctor(string id)
		{
			return Ok(await _doctor.DeleteDoctor(id));
		}

		[Authorize(Roles = nameof(Roles.Doctor))]
		[HttpPut("UpdateDoctor")]
		public async Task<ActionResult<BaseResponse<bool>>> UpdateDoctorAccount(UpdateDoctorDto doctor)
		{
			return Ok(await _doctor.UpdateDoctor(doctor));
		}

		[HttpGet("GetDoctorLanguages")]
		public async Task<ActionResult<BaseResponse<IReadOnlyList<DoctorLanguageDto>>>> GetDoctorLanguages(string id)
		{
			return Ok(await _doctor.GetDoctorLanguages(id));
		}
		[HttpGet("GetDoctorQualifications")]
		public async Task<ActionResult<BaseResponse<IReadOnlyList<DoctorQualificationsDto>>>> GetDoctorQualifications(string id)
		{
			return Ok(await _doctor.GetDoctorQualifications(id));
		}
	} 
}
