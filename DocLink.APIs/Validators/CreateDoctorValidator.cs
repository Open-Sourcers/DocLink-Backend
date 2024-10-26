using DocLink.Domain.DTOs.DoctorDtos;
using FluentValidation;

namespace DocLink.APIs.Validators
{
	public class CreateDoctorValidator:AbstractValidator<CreateDoctorDto>
	{
        public CreateDoctorValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().Length(3,20);
            RuleFor(x => x.LastName).NotEmpty().Length(3,20);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.UserName).NotEmpty().Length(3, 15);
			RuleFor(x => x.Password).NotEmpty().Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$");
        }
    }
}
