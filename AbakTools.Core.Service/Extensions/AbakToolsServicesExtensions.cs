using AbakTools.Core.Service.Category;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbakToolsServicesExtensions
    {
        public static void AddServiceComponent(this IServiceCollection services)
        {
            services.AddTransient<ICategoryService, CategoryService>();
        }
    }
}
