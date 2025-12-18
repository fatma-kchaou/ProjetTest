using Blazored.LocalStorage;
using metiers;
using System.Net.Http.Json;

namespace Front.Services
{
    public class SalleReservationsServices
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public SalleReservationsServices(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task<List<SalleReservation>> GetSalleReservationsAsync()
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await httpClient.GetFromJsonAsync<List<SalleReservation>>("api/SalleReservations");
        }

        public async Task<SalleReservation> GetSalleReservationAsync(int id)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await httpClient.GetFromJsonAsync<SalleReservation>($"api/SalleReservations/{id}");
        }

        public async Task<SalleReservation> CreateSalleReservation(SalleReservation salleR)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsJsonAsync<SalleReservation>("api/SalleReservations", salleR);
            if (result.IsSuccessStatusCode)
                return await result.Content.ReadFromJsonAsync<SalleReservation>();
            return null;
        }

        public async Task<bool> DeleteSalleReservation(int id)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.DeleteAsync($"api/SalleReservations/{id}");
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> UpdateSalleReservation(SalleReservation salleR)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PutAsJsonAsync<SalleReservation>("api/SalleReservations", salleR);
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }
      
    }
}