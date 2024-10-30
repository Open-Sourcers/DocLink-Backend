using System.ComponentModel.DataAnnotations;
using DocLink.Domain.Enums;

namespace DocLink.Domain.DTOs.AuthDtos
{
    public class UserToRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }
        public string EmergencyContact { get; set; }
        public string Password { get; set; }
    }
}
