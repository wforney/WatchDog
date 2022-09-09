namespace WatchDog.src.Exceptions;

internal class WatchDogAuthenticationException : Exception
{
    internal WatchDogAuthenticationException() { }

    internal WatchDogAuthenticationException(string message)
        : base($"WatchDog Authentication Exception: {message}")
    {
    }
}
