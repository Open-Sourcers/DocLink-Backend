using DocLink.Domain.Responses;

namespace DocLink.Domain.DTOs.AuthDtos
{
    public class UserDto
    {
        public UserDto() { }

        public UserDto(string displayName, string email, string token)
        {
            DisplayName = displayName;
            Email = email;
            Token = token;
        }

        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
