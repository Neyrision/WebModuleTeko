using Microsoft.EntityFrameworkCore;
using WebModuleTeko.Database;

namespace WebModuleTeko.Extensions
{
    public static class DbExtension
    {

        public static void EnsureDatabaseMigrated(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WmtContext>();
            context.Database.Migrate();
        }

    }
}
