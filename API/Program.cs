
using API.Data;
using API.Repositories;
using API.Repositories.BorrowRecords;
using API.Repositories.Categories;
using API.Repositories.Livres;
using API.Repositories.Reservation;
using API.Repositories.SalleReservations;
using API.Repositories.Salles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var cnx= builder.Configuration.GetConnectionString("cnx");

            builder.Services.AddDbContext<ApplicationContext>
                (options =>options.UseSqlServer(cnx));
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IDepartementRepository,
                DepartementRepository>();
            builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            builder.Services.AddScoped<ILivresRepository, LivresRepository>();
            builder.Services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();
            builder.Services.AddScoped<ISallesRepository, SallesRepository>();
            builder.Services.AddScoped<ISalleReservationsRepository, SalleReservationsRepository>();
         






            builder.Services.AddIdentity<ApplicationUser,
                IdentityRole>().
                AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,

                    Description = "Entrez votre token JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
});
            });

            builder.Services.AddAuthentication(options =>
            {
                
                options.DefaultAuthenticateScheme =
JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey

(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            builder.Services.AddCors(options =>
            options.AddPolicy("AllowAll",
            policy => policy.AllowAnyOrigin()
            .AllowAnyHeader().AllowAnyMethod()
            ));
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                context.Database.Migrate();

                string[] roles = new string[] { "Admin", "Client" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                string adminEmail = "admin@example.com";
                string adminUserName = "admin";
                string adminPassword = "Admin123!";

                if (await userManager.FindByNameAsync(adminUserName) == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminUserName,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }



            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
