using System.Data.Entity.Migrations;

namespace ORMPerformanceTest.Tests.EF
{
    internal sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            //AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context context)
        {
        }
    }
}