namespace WatchDog;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using WatchDog.src.Data;
using WatchDog.src.Exceptions;
using WatchDog.src.Helpers;
using WatchDog.src.Hubs;
using WatchDog.src.Interfaces;
using WatchDog.src.Models;
using WatchDog.src.Services;

/// <summary>
/// The watch dog extension methods class.
/// </summary>
public static class WatchDogExtension
{
	/// <summary>
	/// The embedded file provider
	/// </summary>
	public static readonly IFileProvider Provider = new EmbeddedFileProvider(
		typeof(WatchDogExtension).GetTypeInfo().Assembly,
		"WatchDog");

	/// <summary>
	/// Adds the watch dog services.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <param name="configureOptions">The configure options.</param>
	/// <returns>IServiceCollection.</returns>
	/// <exception cref="WatchDogDBDriverException">Missing DB Driver Option: SQLDriverOption is required at .AddWatchDogServices()</exception>
	/// <exception cref="WatchDogDatabaseException">Missing connection string.</exception>
	public static IServiceCollection AddWatchDogServices(this IServiceCollection services, [Optional] Action<WatchDogSettings> configureOptions)
	{
		var options = new WatchDogSettings();
		configureOptions?.Invoke(options);

		AutoClearModel.IsAutoClear = options.IsAutoClear;
		AutoClearModel.ClearTimeSchedule = options.ClearTimeSchedule;
		WatchDogExternalDbConfig.ConnectionString = options.SetExternalDbConnString;
		WatchDogSqlDriverOption.SqlDriverOption = options.SqlDriverOption;

		if (!string.IsNullOrEmpty(WatchDogExternalDbConfig.ConnectionString) && WatchDogSqlDriverOption.SqlDriverOption == 0)
		{
			throw new WatchDogDBDriverException("Missing DB Driver Option: SQLDriverOption is required at .AddWatchDogServices()");
		}

		if (WatchDogSqlDriverOption.SqlDriverOption != 0 && string.IsNullOrEmpty(WatchDogExternalDbConfig.ConnectionString))
		{
			throw new WatchDogDatabaseException("Missing connection string.");
		}

		_ = services.AddSignalR();
		_ = services
			.AddMvcCore(x => x.EnableEndpointRouting = false)
			.AddApplicationPart(typeof(WatchDogExtension).Assembly);

		_ = services
			.AddSingleton<IBroadcastHelper, BroadcastHelper>()
			.AddTransient<ILoggerService, LoggerService>();

		if (!string.IsNullOrEmpty(WatchDogExternalDbConfig.ConnectionString))
		{
			ExternalDbContext.Migrate();
		}

		if (AutoClearModel.IsAutoClear)
		{
			_ = services.AddHostedService<AutoLogClearerBackgroundService>();
		}

		return services;
	}

	/// <summary>
	/// Uses the watch dog exception logger.
	/// </summary>
	/// <param name="builder">The builder.</param>
	/// <returns>IApplicationBuilder.</returns>
	public static IApplicationBuilder UseWatchDogExceptionLogger(this IApplicationBuilder builder) => builder.UseMiddleware<src.WatchDogExceptionLogger>();

	/// <summary>
	/// Uses the watch dog.
	/// </summary>
	/// <param name="app">The application.</param>
	/// <param name="configureOptions">The configure options.</param>
	/// <returns>IApplicationBuilder.</returns>
	/// <exception cref="WatchDogAuthenticationException">Parameter Username is required on .UseWatchDog()</exception>
	/// <exception cref="WatchDogAuthenticationException">Parameter Password is required on .UseWatchDog()</exception>
	public static IApplicationBuilder UseWatchDog(this IApplicationBuilder app, Action<WatchDogOptionsModel> configureOptions)
	{
		ServiceProviderFactory.BroadcastHelper = app.ApplicationServices.GetRequiredService<IBroadcastHelper>();
		var options = new WatchDogOptionsModel();
		configureOptions(options);
		if (string.IsNullOrEmpty(options.WatchPageUsername))
		{
			throw new WatchDogAuthenticationException("Parameter Username is required on .UseWatchDog()");
		}
		else if (string.IsNullOrEmpty(options.WatchPagePassword))
		{
			throw new WatchDogAuthenticationException("Parameter Password is required on .UseWatchDog()");
		}

		_ = app
			.UseRouting()
			.UseMiddleware<src.WatchDog>(options)

			.UseStaticFiles(
				new StaticFileOptions
				{
					FileProvider = new EmbeddedFileProvider(
						typeof(WatchDogExtension).GetTypeInfo().Assembly,
						"WatchDog.src.WatchPage"),

					RequestPath = new PathString("/WTCHDGstatics")
				})

			.Build();

		_ = app.UseAuthorization();

		return app.UseEndpoints(
			 endpoints =>
			 {
				 _ = endpoints.MapHub<LoggerHub>("/wtchdlogger");
				 _ = endpoints.MapControllerRoute(
					 name: "WTCHDwatchpage",
					 pattern: "WTCHDwatchpage/{action}",
					 defaults: new { controller = "WatchPage", action = "Index" });
				 _ = endpoints.MapControllerRoute(
					 name: "default",
					 pattern: "{controller=Home}/{action=Index}/{id?}");
				 _ = endpoints.MapGet("watchdog", async context =>
				 {
					 context.Response.ContentType = "text/html";
					 await context.Response.SendFileAsync(WatchDogExtension.GetFile());
				 });
			 });
	}

	/// <summary>
	/// Gets the index.html file.
	/// </summary>
	/// <returns>The <see cref="IFileInfo"/>.</returns>
	public static IFileInfo GetFile() => Provider.GetFileInfo("src.WatchPage.index.html");

	/// <summary>
	/// Gets the bin folder where this application is located.
	/// </summary>
	/// <returns>The path to the bin folder where this application is located.</returns>
	public static string GetFolder() => Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
}
