using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.Entities;
using DocLink.Domain.Enums;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Repositories;
using DocLink.Domain.Interfaces.Services.Exteranl_Logins;
using DocLink.Domain.Responses.Genaric;
using DocLink.Domain.Specifications;
using Google.Apis.Util;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace DocLink.Application.Services
{
	public class DoctorService : IDoctorService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMedia _media;
		private readonly IEmailSender _sendEmail;
		private readonly IMapper _mapper;
		public DoctorService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMedia media, IEmailSender sendEmail, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_media = media;
			_sendEmail = sendEmail;
			_mapper = mapper;
		}

		public async Task<BaseResponse<bool>> CreateDoctor(CreateDoctorDto doctor)
		{
			var IsFound = await _userManager.FindByEmailAsync(doctor.Email);
			if (IsFound != null) return new BaseResponse<bool>(false, StatusCodes.Status400BadRequest, $"User with email{doctor.Email} already exist!");

			var user = doctor.Adapt<AppUser>();
			await _userManager.CreateAsync(user, doctor.Password);

			var identityResult = await _userManager.AddToRoleAsync(user!, Roles.Doctor.ToString());
			if (!identityResult.Succeeded)
			{
				return new BaseResponse<bool>(false, StatusCodes.Status500InternalServerError);
			}
			await _sendEmail.SendDoctorAccount(doctor.Email, doctor.Password);
			return new BaseResponse<bool>(true, StatusCodes.Status200OK, "Account has been created successfully.");
		}

		public async Task<BaseResponse<bool>> DeleteDoctor(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return new BaseResponse<bool>(false, StatusCodes.Status404NotFound, $"Invalid user id {id}");
			}
			await _userManager.DeleteAsync(user);
			return new BaseResponse<bool>(true, StatusCodes.Status200OK, "User has been deleted successfully.");
		}

		public async Task<BaseResponse<DoctorDto>> GetDoctorById(string id)
		{
			var spec = new DoctorWithRelatedData(id, user: true, specialty: true);
			var doctor = await _unitOfWork.Repository<Doctor, string>().GetEntityWithSpecAsync(spec);
			if (doctor is null) return new BaseResponse<DoctorDto>(null, StatusCodes.Status404NotFound, $"Invalid Id {id}.");

			return new BaseResponse<DoctorDto>(_mapper.Map<DoctorDto>(doctor));
		}

		public async Task<BaseResponse<IReadOnlyList<DoctorLanguageDto>>> GetDoctorLanguages(string id)
		{
			var spec = new DoctorWithRelatedData(id, language: true);
			var Doctor = await _unitOfWork.Repository<Doctor, string>().GetEntityWithSpecAsync(spec);
			return new BaseResponse<IReadOnlyList<DoctorLanguageDto>>(_mapper.Map<IReadOnlyList<DoctorLanguageDto>>(Doctor.Languages));
		}

		public async Task<BaseResponse<IReadOnlyList<DoctorQualificationsDto>>> GetDoctorQualifications(string id)
		{
			var spec = new DoctorWithRelatedData(id, qualification: true);
			var Doctor = await _unitOfWork.Repository<Doctor, string>().GetEntityWithSpecAsync(spec);
			return new BaseResponse<IReadOnlyList<DoctorQualificationsDto>>(_mapper.Map<IReadOnlyList<DoctorQualificationsDto>>(Doctor.Qualifications));
		}

		public async Task<BaseResponse<IReadOnlyList<DoctorDto>>> GetDoctorsWithSpec(DoctorParams param)
		{
			var spec = new DoctorWithSpec(param);
			var doctors = await _unitOfWork.Repository<Doctor, string>().GetAllWithSpecAsync(spec);
			return new BaseResponse<IReadOnlyList<DoctorDto>>(_mapper.Map<IReadOnlyList<DoctorDto>>(doctors));
		}

		public async Task<BaseResponse<DoctorDto>> UpdateDoctor(UpdateDoctorDto doctor)
		{
			var user = await _userManager.FindByIdAsync(doctor.Id);
			if (user == null)
			{
				return new BaseResponse<DoctorDto>(null, StatusCodes.Status404NotFound, $"InValid user id {doctor.Id}");
			}

			var IsFoundSpecialtyId = await _unitOfWork.Repository<Specialty, int>().GetByIdAsync(doctor.SpecialtyId);
			if (IsFoundSpecialtyId == null)
			{
				return new BaseResponse<DoctorDto>(null, StatusCodes.Status404NotFound, $"InValid SpecialtyName Id {doctor.SpecialtyId}");
			}

			user.FirstName = doctor.FirstName ?? user.FirstName;
			user.LastName = doctor.LastName ?? user.LastName;
			user.ProfilePicture = doctor.image != null ? _media.UploadFile(doctor.image, nameof(Doctor)) : user.ProfilePicture;
			await _userManager.UpdateAsync(user);

			var CreateOrUpdate = _mapper.Map<Doctor>(doctor);

			var spec = new DoctorWithRelatedData(CreateOrUpdate.Id, language: true, qualification: true);
			var isUpdated = await _unitOfWork.Repository<Doctor, string>().GetEntityWithSpecAsync(spec);

			if (isUpdated == null)
			{
				await _unitOfWork.Repository<Doctor, string>().AddAsync(CreateOrUpdate);
				await _unitOfWork.SaveAsync();
				return new BaseResponse<DoctorDto>(_mapper.Map<DoctorDto>(CreateOrUpdate), StatusCodes.Status200OK, "Account has been updated successfully.");
			}
			//_unitOfWork.Repository<Doctor, string>().Update(CreateOrUpdate);


			List<LanguageSpoken> languages = new List<LanguageSpoken>();
			foreach (var i in doctor.DoctorLanguages)
			{
				if (!isUpdated.Languages.Any(l => l.Id == i))
				{
					var language = await _unitOfWork.Repository<LanguageSpoken, int>().GetByIdAsync(i);
					if (language == null)
					{
						return new BaseResponse<DoctorDto>($"Invalid language id: {i}", StatusCodes.Status400BadRequest);
					}
					languages.Add(language);
				}

			}
			foreach (var lang in languages)
			{
				isUpdated.Languages.Add(lang);
			}

			foreach(var i in doctor.Qualifications)
			{
				isUpdated.Qualifications.Add(new Qualification { Name=i});
			}

			await _unitOfWork.SaveAsync();

			var specDock = new DoctorWithRelatedData(doctor.Id, user: true, specialty: true);
			var dock = await _unitOfWork.Repository<Doctor, string>().GetEntityWithSpecAsync(specDock);
			return new BaseResponse<DoctorDto>(_mapper.Map<DoctorDto>(dock), StatusCodes.Status200OK, "Account has been updated successfully.");
		}

	}
}
