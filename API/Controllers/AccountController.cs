using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        // Inscription
        [HttpPost("register")]
        public async Task<IActionResult> Register(NewUserDTO newUserDTO)
        {
            if (await userManager.FindByNameAsync(newUserDTO.Username) != null)
            {
                return BadRequest("User already exists");
            }

            var applicationUser = new ApplicationUser()
            {
                UserName = newUserDTO.Username,
                Email = newUserDTO.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(applicationUser, newUserDTO.Password);
            if (result.Succeeded)
            {
                // Assigner automatiquement le rôle "Client"
                await userManager.AddToRoleAsync(applicationUser, "Client");
                return Ok("User created successfully");
            }

           return BadRequest(new { 
    message = "Problem creating user", 
    errors = result.Errors.Select(e => e.Description) 
});
        }

        // Connexion
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var appUser = await userManager.FindByNameAsync(loginDTO.Username);
            if (appUser == null)
                return Unauthorized("Invalid credentials");

            if (!await userManager.CheckPasswordAsync(appUser, loginDTO.Password))
                return Unauthorized("Invalid credentials");

            // Récupérer les rôles de l'utilisateur
            var roles = await userManager.GetRolesAsync(appUser);

            // Création des claims pour le token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Ajouter les rôles aux claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Génération du token JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            // Retourner le token
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                username = appUser.UserName
            });
        }
    }
}
