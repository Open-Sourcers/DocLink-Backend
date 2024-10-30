using DocLink.Domain.DTOs.PatientDtos;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses.Genaric;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocLink.APIs.Controllers
{
    public class PatientController : BaseController
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet("GetPatient")]
        public async Task<ActionResult> GetPatient(string patientId)
        {

            try
            {
                var patient = await _patientService.GetPatientById(patientId);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID: {PatientId} not found", patientId);
                    return NotFound("Patient not found");
                }

                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting patient with ID: {PatientId}", patientId);
                return StatusCode(500, "An error occurred while retrieving the patient");
            }
        }

        [HttpPut("UpdatePatient")]
        public async Task<ActionResult>UpdatePatient([FromForm]UpdatePatientDto updatePatientDto)
        {
            try
            {

                var result = await _patientService.UpdatePatient(updatePatientDto);

                if (result.StatusCode != 200)
                {
                    _logger.LogWarning("Failed to update patient with ID: {PatientId}. Error: {Error}", updatePatientDto.Id, result.Errors);
                    return StatusCode(result.StatusCode, result.Errors);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating patient with ID: {PatientId}", updatePatientDto.Id);
                return StatusCode(500, "An error occurred while updating the patient");
            }
        }

        [HttpPost("AddRate")]
        public async Task<ActionResult> AddRate(string DoctorId, float stars)
        {
            try
            {
                var result = await _patientService.AddRate(DoctorId, stars);
                if (result.StatusCode != 200)
                {
                    _logger.LogWarning("Failed to save the rate with doctor id {DoctorId}", DoctorId);
                    return StatusCode(result.StatusCode, result.Errors);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding rate to doctor with ID: {DoctorId}", DoctorId);
                return StatusCode(500, "An error occurred while adding rate to doctor");
            }
        }
    }
}
