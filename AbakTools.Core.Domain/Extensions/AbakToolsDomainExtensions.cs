using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Services.Implementations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbakToolsDomainExtensions
    {
        public static void AddDomainComponent(this IServiceCollection service)
        {
            service.AddTransient<ISynchronizeStampService, SynchronizeStampService>();
        }
    }
}
