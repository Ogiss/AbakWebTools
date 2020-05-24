using AbakTools.Core.Dto.Category;
using AbakTools.Web.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AbakTools.Web.Framework
{
    public class WebApiClient
    {
        private const string categoryPath = "/api/Category/";
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpClient Client { get; }
        public WebApiClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://localhost:5001/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "AbakToolsWeb-Client");

            Client = client;
        }

        public async Task<IEnumerable<CategoryItemDto>> GetCategoriesByParentIdAsync(int? parentId)
        {
            var response = await Client.GetAsync($"{categoryPath}GetAll?parentId={parentId}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var categories = await JsonSerializer.DeserializeAsync<IEnumerable<CategoryItemDto>>(responseStream, options);

            return await Task.FromResult(categories);
        }

        public async Task<CategoryEditModel> GetCategoryAsync(int id)
        {
            var response = await Client.GetAsync($"{categoryPath}Get?id={id}");

            response.EnsureSuccessStatusCode();
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var categories = await JsonSerializer.DeserializeAsync<CategoryEditModel>(responseStream, options);

            return await Task.FromResult(categories);
        }
    }
}
