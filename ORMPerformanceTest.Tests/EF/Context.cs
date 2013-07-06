using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Net.Configuration;

namespace ORMPerformanceTest.Tests.EF
{
    public class Context : DbContext
    {
        private static string connection;
        public Context(string connectionString)
            : base((new SqlConnectionStringBuilder(connectionString)).ConnectionString)
        {
            connection = connectionString;
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 3000;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }

        public Context(): base((new SqlConnectionStringBuilder(Context.connection)).ConnectionString)
        {
        }

        public DbSet<Home> Homes { get; set; }
        public DbSet<Province> Provinces { get; set; }
    }
}