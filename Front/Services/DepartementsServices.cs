using Blazored.LocalStorage;
using metiers;
using System.Net.Http.Json;

namespace Front.Services
{
    public class DepartementsServices
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public DepartementsServices(HttpClient httpClient,ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task<List<Departement>> GetDepartementsAsync()
        {
            var token= await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization=
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await httpClient.
                GetFromJsonAsync<List<Departement>>("api/Departement");
        }

        public async Task<Departement> GetDepartementAsync(int id)
        {
            return await httpClient.
                GetFromJsonAsync<Departement>($"api/Departement/{id}");
        }

        public async Task<Departement> CreateDepartement(Departement departement)
        {
            var result = await httpClient.
                PostAsJsonAsync<Departement>("api/Departement", departement);
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<Departement>();
        }

        public async Task<bool> DeleteDepartement(int id)
        {
            var result= await httpClient.DeleteAsync($"api/Departement/{id}");
            if(result.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> UpdateDepartement(Departement departement)
        {
            var result = await httpClient.
                PutAsJsonAsync<Departement>("api/departement", departement);
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }



    }
}
