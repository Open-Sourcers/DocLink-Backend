using DocLink.Domain.DTOs.AppointmentDtos;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Repositories;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses.Genaric;
using DocLink.Domain.Specifications;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public AppointmentService(IMapper mapper , IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<BaseResponse<bool>> CreateAppointment(CreateAppointmentDto AppointmentDetails)
        {
            Appointment appointment = _mapper.Map<Appointment>(AppointmentDetails);
            await _unitOfWork.Repository<Appointment,int>().AddAsync(appointment);
            var Result = await _unitOfWork.SaveAsync();
            
            if(Result <= 0)
                return new BaseResponse<bool>(false, "Falid to add Appointment",null,500);

            return new BaseResponse<bool>(true,"Sucessful add Appointment");
        }

        public async Task<BaseResponse<bool>> DeleteAppointment(int AppointmentID)
        {
            var Appointment = await _unitOfWork.Repository<Appointment,int>().GetByIdAsync(AppointmentID);

            if (Appointment is null)
                return new BaseResponse<bool>(false, "Appointment not exist", null, 500);

            _unitOfWork.Repository<Appointment,int>().Remove(Appointment);

            var Result = await _unitOfWork.SaveAsync();

            if (Result <= 0) 
                return new BaseResponse<bool>(false, "Falid cancel appointment", null, 500);

            return new BaseResponse<bool>(true, "appointment canceld Sucessfuly", null, 500);

        }

        public async Task<BaseResponse<AppointmentDetailsDTO>> GetAppointmentDetails(int AppointmentID)
        {
            var Appointment = await _unitOfWork.Repository<Appointment, int>().GetByIdAsync(AppointmentID);

            if (Appointment is null)
                return new BaseResponse<AppointmentDetailsDTO>(null, "Appointment not exist", null, 500);

            var AppointmentDto = _mapper.Map<AppointmentDetailsDTO>(Appointment);

            return new BaseResponse<AppointmentDetailsDTO>(AppointmentDto , "ahla data 3lek");
        }

        public async Task<BaseResponse<IReadOnlyList<AppointmentDetailsDTO>>> GetAppointments(AppointmentFilterRequestDto appointmentFilterRequestDto)
        {
            var User = await _userManager.FindByIdAsync(appointmentFilterRequestDto.ID);
            if (User is null)
            {
                return new BaseResponse<IReadOnlyList<AppointmentDetailsDTO>>(null, "User not Exist", null, 400);
            }
            var isPatient = await _userManager.IsInRoleAsync(User, "Patient");
            //TODO : Enum
            if(isPatient) {
                var PatientID = appointmentFilterRequestDto.ID;
                var status = appointmentFilterRequestDto.Status;
                var spec = new AppointmentsSpec(PatientID, status);
                var appointments = (await _unitOfWork.Repository<Appointment, int>().GetAllWithSpecAsync(spec));
                
                var ResultDto = _mapper.Map<IReadOnlyList<AppointmentDetailsDTO>>(appointments);

                return new BaseResponse<IReadOnlyList<AppointmentDetailsDTO>>(ResultDto);
            }
            return null;
        }

        public async Task<BaseResponse<int>> GetSlotsStatus(DoctorTimeSlotRequestDto doctorTimeSlotRequestDto)
        {
            string doctorId = doctorTimeSlotRequestDto.DoctorId;
            var status = doctorTimeSlotRequestDto.Date;
            var notAvilableSlots = (await _unitOfWork.Repository<Appointment, int>().GetAllAsync())
                                                     .Where(app => app.DoctorId == doctorId && app.Status != Domain.Enums.AppointmentStatus.Incoming)
                                                     .Select(app => app.TimeSlotID);
            int mask = (1 << 12) - 1;
            foreach (var id in notAvilableSlots)
                mask &= ~(1 << id);

            return new BaseResponse<int>(mask);
        }

        public async Task<BaseResponse<bool>> RescheduleAppointment(ScheduleAppointmentDto AppointmentDate)
        {
            var appointment = await _unitOfWork.Repository<Appointment,int>().GetByIdAsync(AppointmentDate.AppointmentId);

            if(appointment == null) 
                return new BaseResponse<bool>(false , "appointment not exist" , null , 500);

            appointment.Date = AppointmentDate.Date;
            appointment.TimeSlotID = AppointmentDate.TimeSlotId;

            //_unitOfWork.Repository<Appointment,int>().Update(appointment);

            var Result = await _unitOfWork.SaveAsync();

            if (Result <= 0)
                return new BaseResponse<bool>(false, "Appointment not updated", null, 500);

            return new BaseResponse<bool>(true, "Appointment updated sucessfuly");
        }
    }
}
