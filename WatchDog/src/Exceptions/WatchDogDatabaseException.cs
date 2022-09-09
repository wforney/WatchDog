namespace WatchDog.src.Exceptions;

internal class WatchDogDatabaseException : Exception
{
	internal WatchDogDatabaseException() { }

	internal WatchDogDatabaseException(string message)
		: base($"WatchDog Database Exception: {message} Ensure you have passed the right SQLDriverOption at .AddWatchDogServices() as well as all required parameters for the database connection string")
	{
	}
}
