using AbakTools.Core.Framework;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
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

        public async Task <T> GetValueAsync<T>(string resourceName, Guid? objectGuid = null, string associationName = null, Guid? associationGuid = null)
        {
            return await GetValueAsync<T>(CreateUri(resourceName, objectGuid, associationName, associationGuid));
        }

        public async Task<T> GetValueAsync<T>(string resource, string query)
        {
            return await GetValueAsync<T>(CreateUri(resource, query));
        }

        public async Task<T> GetValueAsync<T>(Uri uri)
        {
            try
            {
                var str = await _client.GetStringAsync(uri);
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<long> GetDbtsAsync()
        {
            return await GetValueAsync<long>("system", "getdbts");
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

        private Uri CreateUri(string resource, string query)
        {
            Guard.NotEmpty(resource, nameof(resource));

            var builder = new UriBuilder(_baseUrl);

            builder.Path = Path.Combine("api", resource, query);

            return builder.Uri;
        }
    }
}
