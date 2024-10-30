using DocLink.APIs.Extensions;
using DocLink.Domain.Interfaces.Interfaces;
using DocLink.Domain.Interfaces.Services;
using DocLink.Infrastructure.Extention;
using DocLink.Infrastructure.Services.Email;

namespace DocLink.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddFluentEmailServices(builder.Configuration);

            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfigurations();
            builder.Services.AddSwaggerGen();
            builder.Services.AddJwtService(builder.Configuration);
            var app = builder.Build();

            await AutoUpdateDatabase.ApplyMigrations(app);

            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
