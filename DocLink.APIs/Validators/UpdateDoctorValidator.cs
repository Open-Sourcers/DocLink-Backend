using DocLink.Domain.DTOs.DoctorDtos;
using FluentValidation;

namespace DocLink.APIs.Validators
{
	public class UpdateDoctorValidator:AbstractValidator<UpdateDoctorDto>
	{
        public UpdateDoctorValidator()
        {
            RuleFor(x => x.YearsOfExperience).GreaterThan(-1).LessThan(50);
            RuleFor(x=>x.ConsultationFee).GreaterThan(-1);
            RuleFor(x => x.SpecialtyId).NotNull().GreaterThan(-1).LessThan(100000000);
            RuleFor(x => x.Qualifications).NotNull().Must(q => q.Any() == true);
            RuleFor(x => x.DoctorLanguages).NotNull().Must(l => l.Any() == true);
        }
    }
}
