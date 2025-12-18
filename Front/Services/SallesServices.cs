using Blazored.LocalStorage;
using metiers;
using System.Net.Http.Json;

namespace Front.Services
{
    public class SallesServices
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public SallesServices(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task<List<Salle>> GetSallesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Salle>>("api/Salles");
        }

        public async Task<Salle> GetSalleAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<Salle>($"api/Salles/{id}");
        }

        public async Task<Salle> CreateSalle(Salle salle)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsJsonAsync<Salle>("api/Salles", salle);
            if (result.IsSuccessStatusCode)
                return await result.Content.ReadFromJsonAsync<Salle>();
            return null;
        }

        public async Task<bool> DeleteSalle(int id)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.DeleteAsync($"api/Salles/{id}");
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> UpdateSalle(Salle salle)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PutAsJsonAsync<Salle>($"api/Salles/{salle.SalleID}", salle);
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }
       
       
        public async Task<List<Salle>> GetSallesDisponiblesAsync()
        {
            var salles = await GetSallesAsync();
            return salles.Where(s => s.IsAvailable).ToList();
        }

        public async Task<bool> ReserveSalle(int salleId)
        {
            var salle = await GetSalleAsync(salleId);
            if (salle == null) return false;

            salle.IsAvailable = false; // Marquer comme occupée
            await UpdateSalle(salle);
            return true;
        }

        public async Task<bool> ReleaseSalle(int salleId)
        {
            var salle = await GetSalleAsync(salleId);
            if (salle == null) return false;

            salle.IsAvailable = true; // Libérer la salle
            await UpdateSalle(salle);
            return true;
        }

    }
}