using DocLink.Domain.DTOs.AuthDtos;
using FluentValidation;

namespace DocLink.APIs.Validators
{
    public class UserToLogInDtoValidator : AbstractValidator<UserToLogInDto>
    {
        public UserToLogInDtoValidator()
        {
            RuleFor(User => User.Email).NotEmpty().EmailAddress();
            RuleFor(User => User.Password).NotEmpty().WithMessage("Password is required");

        }
    }
}
