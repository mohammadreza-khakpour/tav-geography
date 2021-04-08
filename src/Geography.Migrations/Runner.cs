using System.IO;
using Microsoft.Extensions.Configuration;
using FluentMigrator.Runner;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Geography.Migrations
{
    class Runner
    {
        static void Main(string[] args)
        {
            var options = GetCommandLineArgs(args);

            var connectionString = options.GetValue("connection-string", defaultValue: "Data Source=data.db");

            CreateDatabase(connectionString);

            var runner = CreateRunner(connectionString);

            runner.MigrateUp();
        }

        static void CreateDatabase(string connectionString)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder(connectionString);
            if (File.Exists(connectionStringBuilder.DataSource) == false)
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();
                connection.Close();
            }
        }

        static IMigrationRunner CreateRunner(string connectionString)
        {
            var container = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Runner).Assembly).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider();
            return container.GetRequiredService<IMigrationRunner>();
        }

        static IConfiguration GetCommandLineArgs(string[] args)
        {
            return new ConfigurationBuilder().AddCommandLine(args).Build();
        }
    }
}
