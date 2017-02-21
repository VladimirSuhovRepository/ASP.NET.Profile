using System.Data.Entity.Migrations;
using System.Reflection;
using log4net;
using Profile.DAL.Context;

namespace Profile.DAL.Migrations
{
    internal class Configuration : DbMigrationsConfiguration<ProfileContext>
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Configuration()
        {
        }

        protected override void Seed(ProfileContext context)
        {
            Logger.Debug("Running Seed() method in Migrations.Configuration");

            base.Seed(context);
        }
    }
}
