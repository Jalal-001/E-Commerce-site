using CommerceApp.Data.Concrete.EfCore;
using CommerceAppWebUI.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommerceAppWebUI.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                {
                    try
                    {
                        applicationContext.Database.Migrate();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                using (var commerceAppContext = scope.ServiceProvider.GetRequiredService<CommerceAppContext>())
                {
                    try
                    {
                        commerceAppContext.Database.Migrate();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return host;
        }
    }
}
