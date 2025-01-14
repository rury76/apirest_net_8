using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backendWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace backendWebApi.Controllers
{
    public class TokenController(WebApplication app, string? llave, string validIssuer, string validAudience)
    {
        public void Incializar()
        {
            app.MapPost("/api/token", ([FromBody] LoginModel usuario) =>
            {
                if (usuario.UserName != "admin" || usuario.Password != "1234") return Results.Unauthorized();
                var token = GenerarJwtToken(usuario.UserName);
                return Results.Ok(new { Token = token });
            });
        }

        private string GenerarJwtToken(string nombreUsuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nombreUsuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(llave));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}