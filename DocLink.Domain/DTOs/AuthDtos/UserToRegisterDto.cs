using System.ComponentModel.DataAnnotations;

namespace DocLink.Domain.DTOs.AuthDtos
{
    public class UserToRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
