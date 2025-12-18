using Blazored.LocalStorage;
using metiers;
using System.Net.Http.Json;

namespace Front.Services
{
    public class LivresServices
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public LivresServices(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task<List<Livre>> GetLivresAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Livre>>("api/Livres");
        }

        public async Task<Livre> GetLivreAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<Livre>($"api/Livres/{id}");
        }

        public async Task<Livre> CreateLivre(Livre livre)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsJsonAsync<Livre>("api/Livres", livre);
            if (result.IsSuccessStatusCode)
                return await result.Content.ReadFromJsonAsync<Livre>();
            return null;
        }

        public async Task<bool> DeleteLivre(int id)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.DeleteAsync($"api/Livres/{id}");
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> UpdateLivre(Livre livre)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PutAsJsonAsync<Livre>($"api/Livres/{livre.LivreID}", livre);
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }
    }
}