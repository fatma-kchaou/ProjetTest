using Blazored.LocalStorage;
using metiers;
using System.Net.Http.Json;

namespace Front.Services
{
    public class BorrowRecordsServices
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public BorrowRecordsServices(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task<List<BorrowRecord>> GetBorrowRecordsAsync()
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await httpClient.GetFromJsonAsync<List<BorrowRecord>>("api/BorrowRecords");
        }

        public async Task<BorrowRecord> GetBorrowRecordAsync(int id)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await httpClient.GetFromJsonAsync<BorrowRecord>($"api/BorrowRecords/{id}");
        }

        public async Task<BorrowRecord> CreateBorrowRecord(BorrowRecord borrowRecord)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsJsonAsync<BorrowRecord>("api/BorrowRecords", borrowRecord);
            if (result.IsSuccessStatusCode)
                return await result.Content.ReadFromJsonAsync<BorrowRecord>();
            return null;
        }

        // Borrow with dates - sends LivreID, BorrowDate, ReturnDate, and UserId
        public async Task<bool> BorrowWithDates(int livreId, DateTime borrowDate, DateTime returnDate)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            string userId = "";
            
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                if (claim != null) userId = claim.Value;
            }

            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var payload = new 
            { 
                LivreID = livreId,
                UserId = userId,
                BorrowDate = borrowDate,
                ReturnDate = returnDate
            };
            
            var result = await httpClient.PostAsJsonAsync("api/BorrowRecords", payload);
            return result.IsSuccessStatusCode;
        }

        // Quick borrow - just send LivreID, API handles the rest
        public async Task<bool> QuickBorrow(int livreId)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var payload = new { LivreID = livreId };
            var result = await httpClient.PostAsJsonAsync("api/BorrowRecords", payload);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBorrowRecord(int id)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.DeleteAsync($"api/BorrowRecords/{id}");
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> UpdateBorrowRecord(BorrowRecord borrowRecord)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PutAsJsonAsync<BorrowRecord>("api/BorrowRecords", borrowRecord);
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> ReturnBookAsync(int id)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PutAsync($"api/BorrowRecords/{id}/return", null);
            return result.IsSuccessStatusCode;
        }
    }
}