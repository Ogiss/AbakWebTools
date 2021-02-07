using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    public interface IEnovaImporter
    {
        Task RunImport(IServiceScope serviceScope);
    }
}
