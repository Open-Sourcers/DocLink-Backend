using DocLink.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocLink.APIs.Extensions
{
	internal static class AutoUpdateDatabase
	{
		public static async Task ApplyMigrations(WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var service = scope.ServiceProvider;
			var loggerFactory = service.GetRequiredService<ILoggerFactory>();

			try
			{
				var _context = service.GetRequiredService<DocLinkDbContext>();
				var _roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

				var _unAppliedMigrations = await _context.Database.GetPendingMigrationsAsync();

				if (_unAppliedMigrations.Any()) await _context.Database.MigrateAsync();

				// TODO:Seeding
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex.Message);
			}
		}
	}
}
