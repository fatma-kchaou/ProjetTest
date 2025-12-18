using API.DTOs;
using Blazored.LocalStorage;
using metiers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization; // Ajoutez ceci
namespace Front.Services
{
    public class AuthServices
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authStateProvider;
        public AuthServices(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.authStateProvider = authStateProvider;
        }

        public async Task<bool> login(LoginDTO loginDTO)
        {
            var response = await
            httpClient.PostAsJsonAsync("api/account/login", loginDTO);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null)
                {
                    await localStorage.SetItemAsync("token", loginResponse.token);
                    return true;
                }
            }
            return false;
        }

        // Ajoutez une méthode de logout si nécessaire
        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("token");
            ((CustomAuthStateProvider)authStateProvider).NotifyUserLogout();
        }
        public async Task<(bool success, string message)> register(NewUserDTO registerDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/Account/register", registerDTO);

            if (response.IsSuccessStatusCode)
                return (true, "Succès");

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }
        
    }


public class LoginResponse { public string token { get; set; } }
    public class RegisterDTO { public string Username { get; set; } public string Email { get; set; } public string Password { get; set; } }
}