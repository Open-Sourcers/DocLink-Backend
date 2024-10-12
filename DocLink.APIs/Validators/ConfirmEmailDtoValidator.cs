using DocLink.Domain.DTOs.AuthDtos;
using FluentValidation;

namespace DocLink.APIs.Validators
{
	public class ConfirmEmailDtoValidator:AbstractValidator<ConfirmEmailDto>
	{
        public ConfirmEmailDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Otp).NotEmpty();
        }
    }
}
