using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    public interface IEnovaImporter
    {
        Task RunImport(IServiceScope serviceScope);
    }
}
