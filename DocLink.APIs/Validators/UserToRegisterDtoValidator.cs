﻿using DocLink.Domain.DTOs.AuthDtos;
using FluentValidation;

namespace DocLink.APIs.Validators
{
    public class UserToRegisterDtoValidator : AbstractValidator<UserToRegisterDto>
    {
        public UserToRegisterDtoValidator() { 
            RuleFor(User => User.FirstName).NotEmpty().WithMessage("First Name is Required");
            RuleFor(User => User.LastName).NotEmpty().WithMessage("Last Name is Required");
            RuleFor(User => User.Email).NotEmpty().EmailAddress();
            RuleFor(User => User.Password).NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"\d").WithMessage("Password must contain at least one digit")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character (e.g., @, #, $, etc.)");

        }
    }
}
