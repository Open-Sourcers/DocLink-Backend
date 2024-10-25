using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.Enums;
using DocLink.Domain.Interfaces.Services.Exteranl_Logins;
using DocLink.Domain.Responses;
using DocLink.Domain.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocLink.APIs.Controllers
{
	public class DoctorController:BaseController
	{
		private readonly IDoctorService _doctor;

		public DoctorController(IDoctorService doctor)
		{
			_doctor = doctor;
		}

		[HttpGet("GetDoctorsWithSpec")]
		public async Task<ActionResult<BaseResponse>>GetDoctorsWithSpec(DoctorParams param)
		{
			return Ok(await _doctor.GetDoctorsWithSpec(param));
		}

		[HttpGet("GetDoctorWithSpec/{Id}")]
		public async Task<ActionResult<BaseResponse>> GetDoctorDetails(string Id)
		{
			return Ok(await _doctor.GetDoctorById(Id));
		}

		//[Authorize(Roles =nameof(Roles.Admin))]
		[HttpPost("CreateDoctorAccount")]
		public async Task<ActionResult<BaseResponse>> CreateDoctorAccount(CreateDoctorDto doctor)
		{
			return Ok(await _doctor.CreateDoctor(doctor));
		}

		[Authorize(Roles = nameof(Roles.Doctor))]
		[HttpDelete("DeleteDoctor/{id}")]
		public async Task<ActionResult<BaseResponse>> DeleteDoctor(string id)
		{
			return Ok(await _doctor.DeleteDoctor(id));
		}

		[Authorize(Roles = nameof(Roles.Doctor))]
		[HttpPut("UpdateDoctor/{id}")]
		public async Task<ActionResult<BaseResponse>> UpdateDoctorAccount(string id,UpdateDoctorDto doctor)
		{
			return Ok(await _doctor.UpdateDoctor(id, doctor));
		}

	}
}
