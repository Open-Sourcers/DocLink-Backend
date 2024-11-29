using DocLink.Application.MappingProfile;
using DocLink.Application.Services;
using DocLink.Application.Utility;
using DocLink.Domain.DTOs;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Repositories;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Interfaces.Services.Exteranl_Logins;
using DocLink.Domain.Responses;
using DocLink.Domain.Specifications;
using DocLink.Infrastructure;
using DocLink.Infrastructure.Data;
using DocLink.Infrastructure.External_Services.Caching;
using DocLink.Infrastructure.External_Services.External_Logins.Facebook;
using DocLink.Infrastructure.External_Services.External_Logins.Google;
using DocLink.Infrastructure.Repositories;
using DocLink.Infrastructure.Services.Email;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DocLink.APIs.Extensions
{
	public static class ApplicationServices
	{

		public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration Configuration)
		{
			#region DbContext Registration

			Services.AddDbContext<DocLinkDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DataBaseConnection"));
				//options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			});

			#endregion

			#region Identity User
			Services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				options.SignIn.RequireConfirmedEmail = false;
				options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
			})
				 .AddEntityFrameworkStores<DocLinkDbContext>()
				 .AddDefaultTokenProviders();
			#endregion

			#region Fluent Validation

			Services.AddFluentValidation(fv => { fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()); });

			#endregion

			#region Validation Error standard Response
			Services.Configure<ApiBehaviorOptions>(Options =>
		   {
			   Options.InvalidModelStateResponseFactory = (actionContext) =>
			   {
				   var errors = actionContext.ModelState.Where(Parameter => Parameter.Value.Errors.Count() > 0)
														.SelectMany(Parameter => Parameter.Value.Errors)
														.Select(E => E.ErrorMessage).ToList();
				   var ValidationErrorModle = new BaseResponse(errors);

				   return new BadRequestObjectResult(ValidationErrorModle);

			   };
		   });
			#endregion

			#region General Services
			Services.AddScoped<IAccountService, AccountService>();
			Services.AddMemoryCache();
			Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
			Services.AddScoped<IFacebookAuthService, FacebookAuthService>();
			Services.AddScoped<ICacheService, CacheService>();
			Services.AddScoped<IEmailService, EmailService>();
			Services.AddScoped<IEmailSender, EmailSender>();
			Services.AddScoped<IUnitOfWork, UnitOfWork>();
			Services.AddScoped<IDoctorService, DoctorService>();
			Services.AddScoped<IPatientService, PatientServices>();
			Services.AddHttpContextAccessor();
			Services.AddScoped<IMedia, Media>();
			#endregion

			#region Mapster Registration
			Services.AddMapster();
			MapsterConfig.Configure();
			DoctorProfile.Configure(Configuration);
			PatientConfig.configure();
			#endregion

			#region Error Handling
			Services.AddExceptionHandler<GlobalErrorHandling>();
			Services.AddProblemDetails();
			#endregion

			return Services;
		}
	}
}
