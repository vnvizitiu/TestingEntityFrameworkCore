namespace Test.TestUtilities
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using WebApplication.Data;

    public static class TestDatabaseContextFactory
    {
        public static ApplicationDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<ApplicationDbContext> contextOptionsBuilder = // declaring the options we are going to use for the context we will be using for tests
                new DbContextOptionsBuilder<ApplicationDbContext>();

            LoggerFactory loggerFactory = new LoggerFactory(); // this will allow us to add loggers so we can actually inspect what code and queries EntityFramework produces.
            loggerFactory
                .AddDebug() // this logger will log to the Debug output
                .AddConsole(); // this logger will output to the console

            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder { Mode = SqliteOpenMode.Memory }; // this is more syntax friendly approach to defining and InMemory connection string for our database, the alternative is to write it out as a string.
            SqliteConnection connection = new SqliteConnection(connectionStringBuilder.ConnectionString); // create a connection to the InMemory database.
            connection.Open(); // open the connection

            contextOptionsBuilder.UseLoggerFactory(loggerFactory); // register the loggers inside the context options builder, this way, entity framework logs the queries
            contextOptionsBuilder.UseSqlite(connection); // we're telling entity framework to use the SQLite connection we created.
            contextOptionsBuilder.EnableSensitiveDataLogging(); // this will give us more insight when something does go wrong. It's ok to use it here since it's a testing project, but be careful about enabling this in production.

            ApplicationDbContext context = new ApplicationDbContext(contextOptionsBuilder.Options); // creating the actual DbContext

            context.Database.EnsureCreated(); // this command will create the schema and apply configurations we have made in the context, like relations and constraints

            return context; // return the context to be further used in tests.
        }
    }
}