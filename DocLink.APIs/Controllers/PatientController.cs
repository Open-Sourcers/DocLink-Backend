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

            var patient = await _patientService.GetPatientById(patientId);

            if (patient.StatusCode != 200)
            {
                _logger.LogWarning("Patient with ID: {PatientId} not found", patientId);
                return StatusCode(patient.StatusCode,patient);
            }

            return Ok(patient);

        }

        [HttpPut("UpdatePatient")]
        public async Task<ActionResult>UpdatePatient([FromForm]UpdatePatientDto updatePatientDto)
        {

            var result = await _patientService.UpdatePatient(updatePatientDto);

            if (result.StatusCode != 200)
            {
                _logger.LogWarning("Failed to update patient with ID: {PatientId}. Error: {Error}", updatePatientDto.Id, result.Errors);
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        [HttpPost("AddRate")]
        public async Task<ActionResult> AddRate(string DoctorId, float stars)
        {
            
            var result = await _patientService.AddRate(DoctorId, stars);
            if (result.StatusCode != 200)
            {
                _logger.LogWarning("Failed to save the rate with doctor id {DoctorId}", DoctorId);
                return StatusCode(result.StatusCode, result);
            }
            return Ok(result);
             
        }
    }
}
