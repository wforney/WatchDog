﻿namespace WatchDog.src.Data;

using Dapper;
using global::WatchDog.src.Enums;
using global::WatchDog.src.Exceptions;
using global::WatchDog.src.Models;
using global::WatchDog.src.Utilities;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;

internal static class ExternalDbContext
{
    private static readonly string _connectionString = WatchDogExternalDbConfig.ConnectionString;

    /// <summary>
    /// Bootstraps the tables.
    /// </summary>
    /// <exception cref="WatchDogDatabaseException"></exception>
    public static async Task BootstrapTables()
    {
        var createWatchTablesQuery = GetSqlQueryString();

        using var connection = CreateConnection();
        try
        {
            switch (connection)
            {
                case SqlConnection sqlConnection:
                    await sqlConnection.OpenAsync();
                    break;
                case MySqlConnection mySqlConnection:
                    await mySqlConnection.OpenAsync();
                    break;
                case NpgsqlConnection postgreSqlConnection:
                    await postgreSqlConnection.OpenAsync();
                    break;
                default:
                    connection.Open();
                    break;
            }

            _ = await connection.QueryAsync(createWatchTablesQuery);

            connection.Close();
        }
        catch (SqlException ae)
        {
            Debug.WriteLine(ae.Message.ToString());
            throw ae;
        }
        catch (Exception ex)
        {
            throw new WatchDogDatabaseException(ex.Message);
        }
    }

    /// <summary>
    /// Creates the connection.
    /// </summary>
    /// <returns>IDbConnection.</returns>
    public static IDbConnection CreateConnection()
        => WatchDogSqlDriverOption.SqlDriverOption switch
        {
            WatchDogSqlDriverEnum.MSSQL => CreateMSSQLConnection(),
            WatchDogSqlDriverEnum.MySql => CreateMySQLConnection(),
            WatchDogSqlDriverEnum.PostgreSql => CreatePostgresConnection(),
            _ => throw new NotSupportedException()
        };

    /// <summary>
    /// Creates the MSSQL connection.
    /// </summary>
    /// <returns>SqlConnection.</returns>
    /// <exception cref="WatchDogDatabaseException"></exception>
    public static SqlConnection CreateMSSQLConnection()
    {
        try
        {
            return new SqlConnection(_connectionString);
        }
        catch (Exception ex)
        {
            throw new WatchDogDatabaseException(ex.Message);
        }
    }

    /// <summary>
    /// Creates my SQL connection.
    /// </summary>
    /// <returns>MySqlConnection.</returns>
    /// <exception cref="WatchDogDatabaseException"></exception>
    public static MySqlConnection CreateMySQLConnection()
    {
        try
        {
            return new MySqlConnection(_connectionString);
        }
        catch (Exception ex)
        {
            throw new WatchDogDatabaseException(ex.Message);
        }
    }

    /// <summary>
    /// Creates the postgres connection.
    /// </summary>
    /// <returns>NpgsqlConnection.</returns>
    /// <exception cref="WatchDogDatabaseException"></exception>
    public static NpgsqlConnection CreatePostgresConnection()
    {
        try
        {
            return new NpgsqlConnection(_connectionString);
        }
        catch (Exception ex)
        {
            throw new WatchDogDatabaseException(ex.Message);
        }
    }

    /// <summary>
    /// Gets the SQL query string.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string GetSqlQueryString() =>
        WatchDogSqlDriverOption.SqlDriverOption switch
        {
            WatchDogSqlDriverEnum.MSSQL => @$"
                                  IF OBJECT_ID('dbo.{Constants.WatchLogTableName}', 'U') IS NULL CREATE TABLE {Constants.WatchLogTableName} (
                                  id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                  responseBody    VARCHAR(max),
                                  responseStatus  int NOT NULL,
                                  requestBody     VARCHAR(30),
                                  queryString     VARCHAR(30),
                                  path            VARCHAR(30),
                                  requestHeaders  VARCHAR(max),
                                  responseHeaders VARCHAR(30),
                                  method          VARCHAR(30),
                                  host            VARCHAR(30),
                                  ipAddress       VARCHAR(30),
                                  timeSpent       VARCHAR(100),
                                  startTime       VARCHAR(100) NOT NULL,
                                  endTime         VARCHAR(100) NOT NULL
                            );
                                IF OBJECT_ID('dbo.{Constants.WatchLogExceptionTableName}', 'U') IS NULL CREATE TABLE {Constants.WatchLogExceptionTableName} (
                                id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                message       VARCHAR(max),
                                stackTrace    VARCHAR(max),
                                typeOf        VARCHAR(100),
                                source        VARCHAR(max),
                                path          VARCHAR(100),
                                method        VARCHAR(30),
                                queryString   VARCHAR(100),
                                requestBody   VARCHAR(max),
                                encounteredAt VARCHAR(100) NOT NULL
                             );
                                IF OBJECT_ID('dbo.{Constants.LogsTableName}', 'U') IS NULL CREATE TABLE {Constants.LogsTableName} (
                                id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                message       VARCHAR(max),
                                timestamp     VARCHAR(100) NOT NULL,
                                callingFrom   VARCHAR(max),
                                callingMethod VARCHAR(100),
                                lineNumber    INT
                             );
                        ",

            WatchDogSqlDriverEnum.MySql => @$"
                             CREATE TABLE IF NOT EXISTS {Constants.WatchLogTableName} (
                              id              INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
                              responseBody    TEXT(65535),
                              responseStatus  INT NOT NULL,
                              requestBody     VARCHAR(30),
                              queryString     VARCHAR(30),
                              path            VARCHAR(30),
                              requestHeaders  TEXT(65535),
                              responseHeaders VARCHAR(30),
                              method          VARCHAR(30),
                              host            VARCHAR(30),
                              ipAddress       VARCHAR(30),
                              timeSpent       VARCHAR(100),
                              startTime       VARCHAR(100) NOT NULL,
                              endTime         VARCHAR(100) NOT NULL
                            );
                           CREATE TABLE IF NOT EXISTS {Constants.WatchLogExceptionTableName} (
                                id            INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
                                message       TEXT(65535),
                                stackTrace    TEXT(65535),
                                typeOf        VARCHAR(100),
                                source        TEXT(65535),
                                path          VARCHAR(100),
                                method        VARCHAR(30),
                                queryString   VARCHAR(100),
                                requestBody   TEXT(65535),
                                encounteredAt VARCHAR(100) NOT NULL
                             );
                           CREATE TABLE IF NOT EXISTS {Constants.LogsTableName} (
                                id            INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
                                message       TEXT(65535),
                                timestamp     VARCHAR(100) NOT NULL,
                                callingFrom   VARCHAR(100),
                                callingMethod VARCHAR(100),
                                lineNumber    INT
                             );
                        ",

            WatchDogSqlDriverEnum.PostgreSql => @$"
                             CREATE TABLE IF NOT EXISTS {Constants.WatchLogTableName} (
                              id              SERIAL PRIMARY KEY,
                              responseBody    VARCHAR,
                              responseStatus  int NOT NULL,
                              requestBody     VARCHAR(30),
                              queryString     VARCHAR(30),
                              path            VARCHAR(30),
                              requestHeaders  VARCHAR,
                              responseHeaders VARCHAR(30),
                              method          VARCHAR(30),
                              host            VARCHAR(30),
                              ipAddress       VARCHAR(30),
                              timeSpent       VARCHAR,
                              startTime       TIMESTAMP with time zone NOT NULL,
                              endTime         TIMESTAMP with time zone NOT NULL
                            );
                           CREATE TABLE IF NOT EXISTS {Constants.WatchLogExceptionTableName} (
                                id            SERIAL PRIMARY KEY,
                                message       VARCHAR,
                                stackTrace    VARCHAR,
                                typeOf        VARCHAR(100),
                                source        VARCHAR,
                                path          VARCHAR(100),
                                method        VARCHAR(30),
                                queryString   VARCHAR(100),
                                requestBody   VARCHAR,
                                encounteredAt TIMESTAMP with time zone NOT NULL
                             );
                           CREATE TABLE IF NOT EXISTS {Constants.LogsTableName} (
                                id            SERIAL PRIMARY KEY,
                                message       VARCHAR,
                                timestamp     TIMESTAMP with time zone NOT NULL,
                                callingFrom   VARCHAR,
                                callingMethod VARCHAR(100),
                                lineNumber    INTEGER
                             );
                        ",
            _ => ""
        };

    /// <summary>
    /// Migrates this instance.
    /// </summary>
    public static void Migrate() => BootstrapTables().Wait();
}
