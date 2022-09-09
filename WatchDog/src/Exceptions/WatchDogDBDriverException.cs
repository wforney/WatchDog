namespace WatchDog.src.Exceptions;

internal class WatchDogDBDriverException : Exception
{
	internal WatchDogDBDriverException(string message)
		: base($"WatchDog Database Exception: {message}")
	{
	}
}
