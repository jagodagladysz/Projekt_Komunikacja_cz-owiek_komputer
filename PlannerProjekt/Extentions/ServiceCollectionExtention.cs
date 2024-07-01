using PlannerProjekt.Services;

namespace PlannerProjekt.Extentions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<AdminService>();
            serviceCollection.AddTransient<CategoryService>();
            serviceCollection.AddTransient<PlannerService>();
            serviceCollection.AddTransient<SetTimeService>();
            serviceCollection.AddDbContext<DatabaseContext>();
            return serviceCollection;
        }
    }
}
