using Microsoft.Owin;
using Owin;
using PhotoGallery.Migrations;
using PhotoGallery.Models;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(PhotoGallery.Startup))]
namespace PhotoGallery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());

            ConfigureAuth(app);
        }
    }
}
