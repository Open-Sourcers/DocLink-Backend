using DocLink.Application.Services;
using DocLink.Domain.DTOs.AppointmentDtos;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses.Genaric;
using Microsoft.AspNetCore.Mvc;

namespace DocLink.APIs.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        public async Task<IActionResult> SetAppointment(CreateAppointmentDto appointment)
        {
            var Response = await _appointmentService.CreateAppointment(appointment);

            if(Response.StatusCode != 200)
                return StatusCode(Response.StatusCode , Response);

            return Ok(Response);
        }

        [HttpGet("AppointmentDetails")]
        [ProducesResponseType<BaseResponse<AppointmentDetailsDTO>>(200)]
        public async Task<IActionResult> GetAppointmentDetails(int AppointmentID)
        {
            var Response = await _appointmentService.GetAppointmentDetails(AppointmentID);

            if (Response.StatusCode != 200)
                return StatusCode(Response.StatusCode, Response);

            return Ok(Response);
        }

        [HttpGet("Appointments")]
        [ProducesResponseType<BaseResponse<IReadOnlyList<AppointmentDetailsDTO>>>(200)]
        public async Task<IActionResult> GetAppointments(AppointmentFilterRequestDto appointmentFilterRequestDto)
        {
            var Response = await _appointmentService.GetAppointments(appointmentFilterRequestDto);

            if (Response.StatusCode != 200)
                return StatusCode(Response.StatusCode, Response);

            return Ok(Response);
        }

        [HttpPut]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        public async Task<IActionResult> ReScheduleAppointment(ScheduleAppointmentDto scheduleAppointmentDto)
        {
            var Response = await _appointmentService.RescheduleAppointment(scheduleAppointmentDto);

            if(Response.StatusCode != 200)
                return StatusCode(Response.StatusCode, Response);

            return Ok(Response);
        }

        [HttpGet("TimeSlotsStatus")]
        [ProducesResponseType<BaseResponse<int>>(200)]
        public async Task<IActionResult> GetTimeSlotsStatus(DoctorTimeSlotRequestDto slotRequestDto)
        {
            var Response = await _appointmentService.GetSlotsStatus(slotRequestDto);

            if (Response.StatusCode != 200)
                return StatusCode(Response.StatusCode, Response);

            return Ok(Response);

        }

        [HttpDelete]
        [ProducesResponseType<BaseResponse<bool>>(200)]
        public async Task<IActionResult> DeleteAppointment(int AppointmentID)
        {
            var Response = await _appointmentService.DeleteAppointment(AppointmentID);

            if (Response.StatusCode != 200)
                return StatusCode(Response.StatusCode, Response);

            return Ok(Response);
        }
    }
}
