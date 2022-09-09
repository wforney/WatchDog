namespace WatchDog.src.Services;

using global::WatchDog.src.Enums;
using global::WatchDog.src.Interfaces;
using global::WatchDog.src.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class AutoLogClearerBackgroundService : BackgroundService
{
	private readonly ILogger<AutoLogClearerBackgroundService> logger;
	private readonly IServiceProvider serviceProvider;
	private bool isProcessing;

	/// <summary>
	/// Initializes a new instance of the <see cref="AutoLogClearerBackgroundService" /> class.
	/// </summary>
	/// <param name="logger">The logger.</param>
	/// <param name="serviceProvider">The service provider.</param>
	public AutoLogClearerBackgroundService(ILogger<AutoLogClearerBackgroundService> logger, IServiceProvider serviceProvider)
	{
		this.logger = logger;
		this.serviceProvider = serviceProvider;
	}

	/// <summary>
	/// Triggered when the application host is performing a graceful shutdown.
	/// </summary>
	/// <param name="cancellationToken">
	/// Indicates that the shutdown process should no longer be graceful.
	/// </param>
	/// <returns>A <see cref="Task" />.</returns>
	public override Task StopAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Log Clearer Background service is stopping");
		return base.StopAsync(cancellationToken);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			if (isProcessing)
			{
				return;
			}

			isProcessing = true;

			var schedule = AutoClearModel.ClearTimeSchedule;
			var minute = schedule switch
			{
				WatchDogAutoClearScheduleEnum.Daily => TimeSpan.FromDays(1),
				WatchDogAutoClearScheduleEnum.Weekly => TimeSpan.FromDays(7),
				WatchDogAutoClearScheduleEnum.Monthly => TimeSpan.FromDays(30),
				WatchDogAutoClearScheduleEnum.Quarterly => TimeSpan.FromDays(90),
				_ => TimeSpan.FromDays(7),
			};
			var start = DateTime.UtcNow;
			while (true)
			{
				var remaining = (minute - (DateTime.UtcNow - start)).TotalMilliseconds;
				if (remaining <= 0)
					break;
				if (remaining > short.MaxValue)
					remaining = short.MaxValue;
				await Task.Delay(TimeSpan.FromMilliseconds(remaining), stoppingToken);
			}

			await DoWorkAsync();
			isProcessing = false;
		}
	}

	private async Task DoWorkAsync()
	{
		await Task.Run(() =>
		{
			try
			{
				using var scope = serviceProvider.CreateScope();
				var loggerService = scope.ServiceProvider.GetRequiredService<ILoggerService>();
				try
				{
					logger.LogInformation("Log Clearer Background service is starting");
					logger.LogInformation("Log is clearing...");
					loggerService.ClearWatchLogs();
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "{Exception}", ex.Message);
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Log Clearer Background service error : {Exception}", ex.Message);
			}
		});
	}
}
