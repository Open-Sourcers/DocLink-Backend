using AutoMapper;
using DocLink.Domain.DTOs.AppointmentDtos;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses;
using DocLink.Domain.Responses.Genaric;
using Microsoft.AspNetCore.Mvc;

namespace DocLink.APIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAppointmentService _appointmentService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger , IAppointmentService appointmentService)
        {
            _logger = logger;
            this._appointmentService = appointmentService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            throw new Exception("Try throwing Exceptioin");
            
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

      
    }
}
