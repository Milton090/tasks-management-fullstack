using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TaskManagement.Data
{
   public class DbInitializer
   {
      private readonly string _connectionString;

      public DbInitializer(IConfiguration config)
      {
         _connectionString = config.GetConnectionString("SQLConnection");
      }

      public void Initialize()
      {
         using (var connection = new SqlConnection(_connectionString))
         {
            connection.Open();

            var dbExists = connection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM sys.databases WHERE name = 'TaskManagement'");

            if (dbExists == 0)
            {
               connection.Execute("CREATE DATABASE TaskManagement");
            }

            connection.ChangeDatabase("TaskManagement");

            var createAuthTable = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Auth]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[Auth] (
                    [Id] INT IDENTITY(1,1) NOT NULL,
                    [Email] VARCHAR(255) NULL,
                    [Password] VARCHAR(255) NULL,
                    PRIMARY KEY CLUSTERED ([Id] ASC)
                );
            END";

            var createStateTable = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[State]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[State] (
                    [Id] INT IDENTITY(1,1) NOT NULL,
                    [Name] VARCHAR(50) NULL,
                    PRIMARY KEY CLUSTERED ([Id] ASC)
                );
            END";

            var createTaskTable = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[Task] (
                    [Id] INT IDENTITY(1,1) NOT NULL,
                    [Title] VARCHAR(50) NULL,
                    [StateId] INT NULL,
                    [Created] DATETIME NULL,
                    [Updated] DATETIME NULL,
                    [UserId] INT NULL,
                    PRIMARY KEY CLUSTERED ([Id] ASC),
                    CONSTRAINT [FK_Task_State] FOREIGN KEY ([StateId]) REFERENCES [dbo].[State] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE
                );
            END";

            connection.Execute(createAuthTable);
            connection.Execute(createStateTable);
            connection.Execute(createTaskTable);
         }
      }
   }
}