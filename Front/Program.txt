using Blazored.LocalStorage;
using Front.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Front
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.
                AddScoped(sp => new HttpClient 
                { BaseAddress = new 
                Uri("http://localhost:5213/") });

            builder.Services.AddScoped<DepartementsServices>();
            builder.Services.AddScoped<BorrowRecordsServices>();
            builder.Services.AddScoped<LivresServices>();
            builder.Services.AddScoped<CategoriesServices>();
            builder.Services.AddScoped<SalleReservationsServices>();
            builder.Services.AddScoped<SallesServices>();
            //  builder.Services
            builder.Services.AddScoped<AuthServices>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(); // Enregistrez le provider personnalisé
            await builder.Build().RunAsync();
        }
    }
}
