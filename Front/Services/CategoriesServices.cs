using Blazored.LocalStorage;
using metiers;
using System.Net.Http.Json;

namespace Front.Services
{
    public class CategoriesServices
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public CategoriesServices(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Category>>("api/Categories");
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<Category>($"api/Categories/{id}");
        }

        public async Task<Category> CreateCategory(Category category)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsJsonAsync<Category>("api/Categories", category);
            if (result.IsSuccessStatusCode)
                return await result.Content.ReadFromJsonAsync<Category>();
            return null;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.DeleteAsync($"api/Categories/{id}");
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PutAsJsonAsync($"api/Categories/{category.CategoryID}", category);
            return result.IsSuccessStatusCode;
        }
    }
}