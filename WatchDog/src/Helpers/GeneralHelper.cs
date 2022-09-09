namespace WatchDog.src.Helpers;

using global::WatchDog.src.Models;
using System.IO;

internal static class GeneralHelper
{
	/// <summary>
	/// Reads the stream in chunks.
	/// </summary>
	/// <param name="stream">The stream.</param>
	/// <returns>System.String.</returns>
	public static string ReadStreamInChunks(Stream stream)
	{
		const int readChunkBufferLength = 4096;
		_ = stream.Seek(0, SeekOrigin.Begin);
		using var textWriter = new StringWriter();
		using var reader = new StreamReader(stream);
		var readChunk = new char[readChunkBufferLength];
		int readChunkLength;
		do
		{
			readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
			textWriter.Write(readChunk, 0, readChunkLength);
		} while (readChunkLength > 0);
		return textWriter.ToString();
	}

	/// <summary>
	/// Determines whether this instance is PostgreSQL.
	/// </summary>
	/// <returns><c>true</c> if this instance is postgres; otherwise, <c>false</c>.</returns>
	public static bool IsPostgres() => !string.IsNullOrEmpty(WatchDogExternalDbConfig.ConnectionString) && WatchDogSqlDriverOption.SqlDriverOption == Enums.WatchDogSqlDriverEnum.PostgreSql;
}
