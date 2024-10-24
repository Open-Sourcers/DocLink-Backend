using DocLink.Domain.DTOs.AppointmentDtos;
using DocLink.Domain.Responses.Genaric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocLink.Domain.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<BaseResponse<bool>> CreateAppointment(CreateAppointmentDto AppointmentDetails);
        Task<BaseResponse<bool>> DeleteAppointment(int AppointmentID);
        Task<BaseResponse<bool>> RescheduleAppointment(ScheduleAppointmentDto AppointmentDate);
        Task<BaseResponse<AppointmentDetailsDTO>> GetAppointmentDetails(int AppointmentID);
        Task<BaseResponse<int>> GetSlotsStatus(DoctorTimeSlotRequestDto doctorTimeSlotRequestDto); // mask
        Task<BaseResponse<IReadOnlyList<AppointmentDetailsDTO>>> GetAppointments(AppointmentFilterRequestDto appointmentFilterRequestDto);
    }
}
