using DocLink.Domain.DTOs.AuthDtos;
using FluentValidation;

namespace DocLink.APIs.Validators
{
	public class ConfirmEmailValidator:AbstractValidator<ConfirmEmailDto>
	{
        public ConfirmEmailValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Otp).NotEmpty();
        }
    }
}
