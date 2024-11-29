using DocLink.Domain.DTOs.DoctorDtos;
using DocLink.Domain.DTOs.SpecialtyDto;
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

		public async Task<BaseResponse<SpecialtyDto>> CreateSpecialty(CreateSpecialtyDto specialty)
		{
			var spec = new SpecialtyWithSpec(name: specialty.Name);
			var isFoundSpecialty = await _unitOfWork.Repository<Specialty, int>().GetEntityWithSpecAsync(spec);
			if (isFoundSpecialty != null)
			{
				return new BaseResponse<SpecialtyDto>("Specialty already exist", StatusCodes.Status400BadRequest);
			}
			var newSpecialty = new Specialty
			{
				Name = specialty.Name,
				ImageUrl = _media.UploadFile(specialty.Image, nameof(Specialty)),
			};
			await _unitOfWork.Repository<Specialty, int>().AddAsync(newSpecialty);
			await _unitOfWork.SaveAsync();
			return new BaseResponse<SpecialtyDto>(_mapper.Map<SpecialtyDto>(newSpecialty));
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
		public async Task<BaseResponse<IReadOnlyList<SpecialtyDto>>> GetAllSpecialties()
		{
			var specialties = await _unitOfWork.Repository<Specialty, int>().GetAllAsync();
			if (!specialties.Any())
			{
				return new BaseResponse<IReadOnlyList<SpecialtyDto>>("There is not specialty found!", StatusCodes.Status404NotFound);
			}

			return new BaseResponse<IReadOnlyList<SpecialtyDto>>(_mapper.Map<IReadOnlyList<SpecialtyDto>>(specialties));
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
		#region Update Doctor Implementation
		//public async Task<BaseResponse<DoctorDto>> UpdateDoctor(UpdateDoctorDto doctor)
		//{
		//	var user = await _userManager.FindByIdAsync(doctor.Id);
		//	if (user == null)
		//		return new BaseResponse<DoctorDto>(null, StatusCodes.Status404NotFound, $"InValid user id {doctor.Id}");

		//	//var IsFoundSpecialtyId = await _unitOfWork.Repository<Specialty, int>().GetByIdAsync(doctor.SpecialtyId);

		//	//if (IsFoundSpecialtyId == null)
		//	//{
		//	//	return new BaseResponse<DoctorDto>(null, StatusCodes.Status404NotFound, $"InValid SpecialtyName Id {doctor.SpecialtyId}");
		//	//}

		//	user.FirstName = doctor.FirstName ?? user.FirstName;
		//	user.LastName = doctor.LastName ?? user.LastName;
		//	user.ProfilePicture = doctor.image != null ? _media.UploadFile(doctor.image, nameof(Doctor)) : user.ProfilePicture;

		//	await _userManager.UpdateAsync(user);

		//	var CreateOrUpdate = _mapper.Map<Doctor>(doctor);

		//	var spec = new DoctorWithRelatedData(CreateOrUpdate.Id, language: true, qualification: true);
		//	var isUpdated = await _unitOfWork.Repository<Doctor, string>().GetEntityWithSpecAsync(spec);

		//	//if (isUpdated == null)
		//	//{
		//	//	await _unitOfWork.Repository<Doctor, string>().AddAsync(CreateOrUpdate);
		//	//	IsFoundSpecialtyId.NumberOfDoctors++;
		//	//	_unitOfWork.Repository<Specialty, int>().Update(IsFoundSpecialtyId);
		//	//	await _unitOfWork.SaveAsync();
		//	//	return new BaseResponse<DoctorDto>(_mapper.Map<DoctorDto>(CreateOrUpdate), StatusCodes.Status200OK, "Account has been updated successfully.");
		//	//}



		//	foreach (var i in doctor.DoctorLanguages)
		//	{

		//		var language = await _unitOfWork.Repository<LanguageSpoken, int>().GetByIdAsync(i);

		//		if (language == null)
		//		{
		//			return new BaseResponse<DoctorDto>($"Invalid language id: {i}", StatusCodes.Status400BadRequest);
		//		}
		//              var existingRelation = isUpdated.Languages.FirstOrDefault(lan => lan.Id == language.Id);
		//              if (existingRelation == null)
		//                  isUpdated.Languages.Add(language);


		//          }




		//	//await _unitOfWork.SaveAsync();

		//	foreach (var i in doctor.Qualifications)
		//	{
		//		isUpdated.Qualifications.Add(new Qualification { Name = i });
		//	}
		//	_unitOfWork.Repository<Doctor, string>().Update(isUpdated);
		//	try
		//	{
		//		await _unitOfWork.SaveAsync();// Exception throwen
		//	}
		//	catch (Exception ex)
		//	{
		//		return new BaseResponse<DoctorDto>(ex.Message, StatusCodes.Status400BadRequest);
		//	}
		//	var specDock = new DoctorWithRelatedData(doctor.Id, user: true, specialty: true);
		//	var dock = await _unitOfWork.Repository<Doctor, string>().GetEntityWithSpecAsync(specDock);
		//	return new BaseResponse<DoctorDto>(_mapper.Map<DoctorDto>(dock), StatusCodes.Status200OK, "Account has been updated successfully.");
		//} 
		#endregion
		public async Task<BaseResponse<DoctorDto>> UpdateDoctor(UpdateDoctorDto doctor)
		{
			// Step 1: Find the user
			var user = await _userManager.FindByIdAsync(doctor.Id);
			if (user == null)
				return new BaseResponse<DoctorDto>(null, StatusCodes.Status404NotFound, $"Invalid user ID: {doctor.Id}");

			user.FirstName = doctor.FirstName ?? user.FirstName;
			user.LastName = doctor.LastName ?? user.LastName;

			if (user.ProfilePicture != null && doctor.image != null)
			{
				var ImageName = user.ProfilePicture.Split('/')[^1];
				_media.DeleteFile(nameof(Doctor), ImageName);
			}
			user.ProfilePicture = doctor.image != null ? _media.UploadFile(doctor.image, nameof(Doctor)) : user.ProfilePicture;

			await _userManager.UpdateAsync(user);

			// Step 2: Retrieve the doctor with related data
			var spec = new DoctorWithRelatedData(doctor.Id, language: true, qualification: true);
			var existingDoctor = await _unitOfWork.Repository<Doctor, string>().GetEntityWithSpecAsync(spec);

			if (existingDoctor == null)
			{
				existingDoctor = _mapper.Map<Doctor>(doctor);
				await _unitOfWork.Repository<Doctor, string>().AddAsync(existingDoctor);
				await _unitOfWork.SaveAsync();
				return new BaseResponse<DoctorDto>(_mapper.Map<DoctorDto>(existingDoctor), StatusCodes.Status200OK, "Account has been updated successfully.");
			}

			// Step 3: Update Languages
			var existingLanguageIds = existingDoctor.Languages.Select(l => l.Id).ToHashSet();
			foreach (var languageId in doctor.DoctorLanguages)
			{
				// Check if the language is already associated with the doctor
				if (!existingLanguageIds.Contains(languageId))
				{
					var language = await _unitOfWork.Repository<LanguageSpoken, int>().GetByIdAsync(languageId);
					if (language == null)
						return new BaseResponse<DoctorDto>($"Invalid language ID: {languageId}", StatusCodes.Status400BadRequest);

					existingDoctor.Languages.Add(language);
				}
			}

			// Step 4: Update Qualifications
			foreach (var qualificationName in doctor.Qualifications)
			{
				if (!existingDoctor.Qualifications.Any(q => q.Name.Equals(qualificationName, StringComparison.OrdinalIgnoreCase)))
				{
					existingDoctor.Qualifications.Add(new Qualification { Name = qualificationName });
				}
			}

			// Step 5: Save changes
			_unitOfWork.Repository<Doctor, string>().Update(existingDoctor);
			await _unitOfWork.SaveAsync();

			// Step 6: Return updated doctor
			var updatedSpec = new DoctorWithRelatedData(doctor.Id, user: true, specialty: true);
			var updatedDoctor = await _unitOfWork.Repository<Doctor, string>().GetEntityWithSpecAsync(updatedSpec);

			return new BaseResponse<DoctorDto>(_mapper.Map<DoctorDto>(updatedDoctor), StatusCodes.Status200OK, "Doctor updated successfully.");
		}



	}
}
