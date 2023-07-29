using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace PetPPP.Extensions
{
    public static class MigrationExtensions
    {
        public static WebApplication ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
            db.Database.Migrate();
            return app;
        }
    }
}
