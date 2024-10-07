using DocLink.Application.Services;
using DocLink.Domain.DTOs;
using DocLink.Domain.Entities;
using DocLink.Domain.Interfaces.Services;
using DocLink.Domain.Responses;
using DocLink.Infrastructure.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace DocLink.APIs.Extensions
{
    public static class ApplicationServices
    {
        
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            #region DbContext Registration
            Services.AddDbContext<DLDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("RemoteConnection")));
            #endregion

            #region Identity User
            Services.AddIdentity<AppUser, IdentityRole>()
                 .AddEntityFrameworkStores<DLDbContext>()
                 .AddDefaultTokenProviders();
            #endregion

            #region Fluent Validation

            Services.AddFluentValidation(fv =>{ fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());});

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
            #endregion
            return Services;
        }
    }
}
