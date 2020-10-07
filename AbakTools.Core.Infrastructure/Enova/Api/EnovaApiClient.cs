using AbakTools.Core.Framework;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Api
{
    class EnovaApiClient : IEnovaAPiClient, IDisposable
    {
        private bool _disposed;
        private HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public EnovaApiClient(IConfiguration configuration)
        {
            _client = new HttpClient();
            _configuration = configuration;
            _baseUrl = configuration["Enova:BaseUrl"];
        }

        public async Task <T> GetValueAsync<T>(string resourceName, Guid? objectGuid, string associationName, Guid? associationGuid)
        {
            var uri = CreateUri(resourceName, objectGuid, associationName, associationGuid);
            var str = await _client.GetStringAsync(uri);

            return JsonConvert.DeserializeObject<T>(str);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                if(_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
            }
        }

        private Uri CreateUri(string resourceName, Guid? objectGuid, string associationName, Guid? associationGuid)
        {
            Guard.NotEmpty(resourceName, nameof(resourceName));

            var sb = new StringBuilder(_baseUrl.TrimEnd('/')).Append('/').Append(resourceName);

            if (objectGuid.HasValue)
            {
                sb.Append('/').Append(objectGuid);
            }

            if (!string.IsNullOrEmpty(associationName))
            {
                sb.Append('/').Append(associationName);
            }

            if (associationGuid.HasValue)
            {
                sb.Append('/').Append(associationGuid);
            }

            return new Uri(sb.ToString());
        }

    }
}
