using System;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Api
{
    public interface IEnovaAPiClient
    {
        Task<T> GetValueAsync<T>(string resourceName, Guid? objectGuid = null, string associationName = null, Guid? associationGuid = null);
    }
}
