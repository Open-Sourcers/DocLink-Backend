using DocLink.Application.Specification;
using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.Entities;
using DocLink.Domain.Enums;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Repositories;
using DocLink.Domain.Interfaces.Services.Exteranl_Logins;
using DocLink.Domain.Responses;
using DocLink.Domain.Specifications;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Application.Services
{
	public class DoctorService : IDoctorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMedia _media;
		public DoctorService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMedia media)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_media = media;
		}

		public async Task<BaseResponse> CreateDoctor(CreateDoctorDto doctor)
		{
			var IsFound = await _userManager.FindByEmailAsync(doctor.Email);
			if (IsFound != null) return new BaseResponse(StatusCodes.Status400BadRequest, $"User with email{doctor.Email} already exist!");

			var user = doctor.Adapt<AppUser>();
			await _userManager.CreateAsync(user, doctor.Password);

			var identityResult = await _userManager.AddToRoleAsync(user!, Roles.Doctor.ToString());
			if (!identityResult.Succeeded)
			{
				return new BaseResponse(StatusCodes.Status500InternalServerError);
			}

			return new BaseResponse(StatusCodes.Status200OK, "Account has been created successfully.");
		}

		public async Task<BaseResponse> DeleteDoctor(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return new BaseResponse(StatusCodes.Status404NotFound, "Invalid user id @{id}");
			}
			await _userManager.DeleteAsync(user);
			return new BaseResponse(StatusCodes.Status200OK, "User has been deleted successfully.");
		}

		public async Task<BaseResponse> GetDoctorById(string id)
		{
			var doctor = await _unitOfWork.Repository<Doctor, string>().GetByIdAsync(id);
			if (doctor is null) return new BaseResponse(StatusCodes.Status404NotFound, $"Invalid Id {id}.");
			return new BaseResponse(doctor.Adapt<DoctorDto>());
		}

		public async Task<BaseResponse> GetDoctorsWithSpec(DoctorParams param)
		{
			var spec = new DoctorWithSpec(param);
			var doctors = await _unitOfWork.Repository<Doctor, string>().GetAllWithSpecAsync(spec);
			return new BaseResponse(doctors.Adapt<List<DoctorDto>>());
		}

		public async Task<BaseResponse> UpdateDoctor(string id, UpdateDoctorDto doctor)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return new BaseResponse(StatusCodes.Status404NotFound, $"InValid user id {id}");
			}
			user.FirstName = doctor.FirstName ?? user.FirstName;
			user.LastName = doctor.LastName ?? user.LastName;
			user.ProfilePecture = doctor.image != null ? _media.UploadFile(doctor.image, nameof(Doctor)) : user.ProfilePecture;
			await _userManager.UpdateAsync(user);

			var mappedDoctor = doctor.Adapt<Doctor>();
			mappedDoctor.Id = id;

			var isUpdated = await _unitOfWork.Repository<Doctor, string>().GetByIdAsync(id);

			if (isUpdated == null)
			{
				await _unitOfWork.Repository<Doctor, string>().AddAsync(mappedDoctor);
			}
			else
			{
				_unitOfWork.Repository<Doctor, string>().Update(mappedDoctor);
			}
			await _unitOfWork.SaveAsync();
			return new BaseResponse(StatusCodes.Status200OK, "Account has been updated successfully.");
		}

	}
}
